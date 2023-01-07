using Microsoft.EntityFrameworkCore;
using weitus_backend.Data.Models;

namespace weitus_backend.Data
{
    public interface IWeitusRepository
    {
        Task<T> Add<T>(T entity) where T : class;

        Task<T> Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<IEnumerable<ChatMessage>> GetMessageLog(WeitusUser user);

        Task<ChatBot?> GetChatBot(int id);

        Task<WeitusUser?> GetUserAsync(int id);

        Task<WeitusUser?> GetUserAsync(string username);
        
        Task<WeitusUser?> GetUserByEmailAsync(string email);
    }

    public class WeitusRepository : IWeitusRepository
    {
        private readonly WeitusDbContext _context;

        public WeitusRepository(WeitusDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add<T>(T entity) where T : class
        {
            var newEntity = (await _context.AddAsync<T>(entity)).Entity;

            await _context.SaveChangesAsync();

            return newEntity;
        }

        public async Task<T> Update<T>(T entity) where T : class
        {
            var newEntity = _context.Update(entity).Entity;

            await _context.SaveChangesAsync();

            return newEntity;
        }

        public async void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetMessageLog(WeitusUser user)
        {
            if (user == null)
            {
                return Enumerable.Empty<ChatMessage>();
            }

            return await _context.ChatMessages.Where(m => m.ChatterId == user.UserId).OrderBy(m => m.TimeStamp).ToListAsync();
        }

        public async Task<ChatBot?> GetChatBot(int id)
        {
            return await _context.ChatBots.FirstOrDefaultAsync(b => b.ChatBotId == id);
        }

        public async Task<WeitusUser?> GetUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<WeitusUser?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<WeitusUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
