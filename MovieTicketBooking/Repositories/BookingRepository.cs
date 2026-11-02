using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(ApplicationDbContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<Booking?> GetByIdWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Screen)
                        .ThenInclude(sc => sc.Theatre)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<ShowSeat>> GetShowSeatsByIdsAsync(int showId, IEnumerable<int> seatIds)
        {
            // Fetch ShowSeats that belong to the given show AND match the requested seat IDs
            return await _context.ShowSeats
                .Where(ss => ss.ShowId == showId && seatIds.Contains(ss.SeatId))
                .ToListAsync(); // Tracked — we need to update these records
        }

        public async Task<Booking> CreateAsync(
            Booking booking,
            IEnumerable<BookingSeat> bookingSeats,
            IEnumerable<ShowSeat> showSeatsToUpdate)
        {
            // All operations within a single database transaction.
            // If any step fails, the entire transaction is rolled back.
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Save the booking
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Step 2: Save BookingSeat records (link booking to each seat)
                var seats = bookingSeats.ToList();
                foreach (var bs in seats)
                {
                    bs.BookingId = booking.BookingId;
                }
                await _context.BookingSeats.AddRangeAsync(seats);

                // Step 3: Update ShowSeat status from Available → Booked
                foreach (var showSeat in showSeatsToUpdate)
                {
                    showSeat.Status = ShowSeatStatus.Booked;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Created booking {BookingId} for show {ShowId}", booking.BookingId, booking.ShowId);
                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                _logger.LogError("Booking creation failed for show {ShowId} — transaction rolled back", booking.ShowId);
                throw;
            }
        }

        public async Task CancelAsync(Booking booking, IEnumerable<ShowSeat> showSeatsToRestore)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Update booking status to Cancelled
                booking.BookingStatus = BookingStatusConstants.Cancelled;
                _context.Bookings.Update(booking);

                // Step 2: Restore ShowSeat status from Booked → Available
                foreach (var showSeat in showSeatsToRestore)
                {
                    showSeat.Status = ShowSeatStatus.Available;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Cancelled booking {BookingId}", booking.BookingId);
            }
            catch
            {
                await transaction.RollbackAsync();
                _logger.LogError("Cancellation failed for booking {BookingId} — transaction rolled back", booking.BookingId);
                throw;
            }
        }
    }
}
