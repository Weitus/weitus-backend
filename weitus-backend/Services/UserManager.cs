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

    public UserManager(IWeitusRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<WeitusUser?> GetUserAsync(int id)
    {
        return await _userRepository.GetUserAsync(id);
    }

    public async Task<WeitusUser?> GetUserAsync(string username)
    {
        return await _userRepository.GetUserAsync(username);
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
            Email = registerUser.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

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

        return Result<AuthenticationResponse>.Ok(_jwtService.CreateToken(user));
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
            errors.Add("Username \"" + user.UserName + "\" is already taken");
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
