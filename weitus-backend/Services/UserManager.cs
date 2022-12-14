using System.Security.Cryptography;
using System.Text;
using weitus_backend.Data;
using weitus_backend.Data.Dto;
using weitus_backend.Data.Models;
using System.Security.Claims;

namespace weitus_backend.Services;

public class UserManager
{
    private readonly IWeitusRepository _userRepository;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _config;

    public UserManager(IWeitusRepository userRepository, JwtService jwtService, IConfiguration config)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _config = config;
    }

    public async Task<WeitusUser?> GetUserAsync(int id)
    {
        return await _userRepository.GetUserAsync(id);
    }

    public async Task<WeitusUser?> GetUserAsync(string username)
    {
        return await _userRepository.GetUserAsync(username);
    }

    public async Task<WeitusUser?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task<WeitusUser?> GetUserAsync(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return null;
        }

        return await GetUserAsync(int.Parse(userId));
    }

    public async Task<Result<WeitusUser>> RegisterUserAsync(RegisterUser registerUser)
    {
        var valid = await EnsureUserIsValid(registerUser);

        if (!valid.Success)
        {
            return Result<WeitusUser>.Err(valid.Errors);
        }

        var (passwordHash, passwordSalt) = CreatePasswordHashAndSalt(registerUser.Password);

        var user = new WeitusUser
        {
            UserName = registerUser.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        user.SetEncryptedEmail(registerUser.Email, Convert.FromHexString(_config["Encryption:Key"]), Convert.FromHexString(_config["Encryption:IV"]));

        await _userRepository.Add(user);

        return Result<WeitusUser>.Ok(user);
    }

    public async Task<Result<AuthenticationResponse>> LoginUserAsync(LoginUser loginUser)
    {
        var user = await _userRepository.GetUserAsync(loginUser.UserName);

        if (user == null)
        {
            return Result<AuthenticationResponse>.Err("Bad username/password combination");
        }

        var passwordHash = CreatePasswordHash(loginUser.Password, user.PasswordSalt);

        if (passwordHash != user.PasswordHash)
        {
            return Result<AuthenticationResponse>.Err("Bad username/password combination");
        }

        var response = _jwtService.CreateToken(user);

        response.ChatIdentifier = GetChatSessionId(user);

        return Result<AuthenticationResponse>.Ok(response);
    }

    private string GetChatSessionId(WeitusUser user)
    {
        var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.UserName + user.UserId + _config["Chat:Secret"]));

        return Convert.ToBase64String(hash);
    }

    private async Task<Result> EnsureUserIsValid(RegisterUser user)
    {
        List<string> errors = new List<string>();
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            errors.Add("Username is required");
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            errors.Add("Password is required");
        }

        if (user.Password.Length < 4)
        {
            errors.Add("Password must be at least 6 characters");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            errors.Add("Email is required");
        }

        if (await _userRepository.GetUserAsync(user.UserName) != null)
        {
            errors.Add("Username is already taken");
        }

        if (await _userRepository.GetUserByEmailAsync(user.Email) != null)
        {
            errors.Add("Email is already in use");
        }

        if (errors.Count > 0)
        {
            return Result.Err(errors.ToArray());
        }
        else return Result.Ok();
    }

    private (string, string) CreatePasswordHashAndSalt(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(WeitusUser.PasswordHashLength);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 10000, HashAlgorithmName.SHA512, WeitusUser.PasswordHashLength);

        return (Convert.ToHexString(hash), Convert.ToHexString(salt));
    }

    private string CreatePasswordHash(string password, string salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), Convert.FromHexString(salt), 10000, HashAlgorithmName.SHA512, WeitusUser.PasswordHashLength);

        return Convert.ToHexString(hash);
    }
}
