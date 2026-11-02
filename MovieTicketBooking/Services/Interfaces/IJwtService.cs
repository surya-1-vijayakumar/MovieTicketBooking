
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);

    DateTime GetTokenExpiration();
}
