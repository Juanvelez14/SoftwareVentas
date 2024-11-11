using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;

namespace SoftwareVentas.BLL
{
    public interface IUsersService
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public Task<IdentityResult> ConfirmEmail(User user, string token);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        public Task<User> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(string email, string password);
        public Task LogoutAsync();
        Task ConfirmEmailAsync(User user, string token);
    }
    public class UserService : IUsersService
    {
        private readonly DataContext _context;
        private readonly SignInManager<User> _SignInManager;
        private readonly UserManager<User> _UserManager;

        public UserService(DataContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmail(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _context.Users.Include(u => u.Role)
                                              .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
