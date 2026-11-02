
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int userId);

    Task<User> AddAsync(User user);

    Task<bool> EmailExistsAsync(string email);

    Task SaveChangesAsync();
}
