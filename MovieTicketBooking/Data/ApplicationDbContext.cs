
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowSeat> ShowSeats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }

        // New DbSet for JWT Authentication
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MOVIE
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);
                entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
                entity.Property(m => m.Description).HasMaxLength(1000);
                entity.Property(m => m.Genre).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Language).IsRequired().HasMaxLength(50);
                entity.Property(m => m.DurationMinutes).IsRequired();
                entity.Property(m => m.Rating).HasColumnType("decimal(3,1)");
                entity.Property(m => m.PosterUrl).HasMaxLength(500);
                entity.Property(m => m.IsActive).HasDefaultValue(true);
                entity.Property(m => m.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(m => m.Title)
                      .HasDatabaseName("IX_Movie_Title");
            });

            // THEATRE
            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.HasKey(t => t.TheatreId);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Address).IsRequired().HasMaxLength(500);
                entity.Property(t => t.City).IsRequired().HasMaxLength(100);
                entity.Property(t => t.State).HasMaxLength(100);
                entity.Property(t => t.Phone).HasMaxLength(20);
                entity.Property(t => t.Email).HasMaxLength(100);
                entity.Property(t => t.IsActive).HasDefaultValue(true);
                entity.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // SCREEN
            modelBuilder.Entity<Screen>(entity =>
            {
                entity.HasKey(s => s.ScreenId);
                entity.Property(s => s.ScreenName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.ScreenType)
                      .HasMaxLength(50)
                      .HasDefaultValue("Regular");
                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(s => s.Theatre)
                      .WithMany(t => t.Screens)
                      .HasForeignKey(s => s.TheatreId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // SEAT
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.SeatId);
                entity.Property(s => s.RowLetter).IsRequired().HasMaxLength(5);
                entity.Property(s => s.SeatType)
                      .HasMaxLength(50)
                      .HasDefaultValue("Regular");
                entity.Property(s => s.Price).HasColumnType("decimal(10,2)");
                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(s => new
                {
                    s.ScreenId,
                    s.RowLetter,
                    s.ColumnNumber
                })
                .IsUnique()
                .HasDatabaseName("IX_Seat_Screen_Row_Column_Unique");

                entity.HasOne(s => s.Screen)
                      .WithMany(sc => sc.Seats)
                      .HasForeignKey(s => s.ScreenId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // SHOW
            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.ShowId);
                entity.Property(s => s.BasePrice).HasColumnType("decimal(10,2)");
                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(s => new { s.ScreenId, s.ShowDate })
                      .HasDatabaseName("IX_Show_Screen_Date");

                entity.HasOne(s => s.Movie)
                      .WithMany(m => m.Shows)
                      .HasForeignKey(s => s.MovieId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Screen)
                      .WithMany(sc => sc.Shows)
                      .HasForeignKey(s => s.ScreenId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // SHOW SEAT
            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.HasKey(ss => ss.ShowSeatId);
                entity.Property(ss => ss.Status)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Available");
                entity.Property(ss => ss.Price).HasColumnType("decimal(10,2)");

                entity.HasIndex(ss => new { ss.ShowId, ss.SeatId })
                      .IsUnique()
                      .HasDatabaseName("IX_ShowSeat_Show_Seat_Unique");

                entity.HasIndex(ss => ss.Status)
                      .HasDatabaseName("IX_ShowSeat_Status");

                entity.HasOne(ss => ss.Show)
                      .WithMany(s => s.ShowSeats)
                      .HasForeignKey(ss => ss.ShowId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.Seat)
                      .WithMany(s => s.ShowSeats)
                      .HasForeignKey(ss => ss.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // BOOKING
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.BookingId);
                entity.Property(b => b.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(b => b.CustomerEmail).IsRequired().HasMaxLength(200);
                entity.Property(b => b.CustomerPhone).HasMaxLength(20);
                entity.Property(b => b.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(b => b.BookingStatus)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Confirmed");
                entity.Property(b => b.BookingDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(b => b.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(b => b.BookingStatus)
                      .HasDatabaseName("IX_Booking_Status");

                entity.HasOne(b => b.Show)
                      .WithMany(s => s.Bookings)
                      .HasForeignKey(b => b.ShowId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // BOOKING SEAT
            modelBuilder.Entity<BookingSeat>(entity =>
            {
                entity.HasKey(bs => bs.BookingSeatId);
                entity.Property(bs => bs.Price).HasColumnType("decimal(10,2)");

                entity.HasOne(bs => bs.Booking)
                      .WithMany(b => b.BookingSeats)
                      .HasForeignKey(bs => bs.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bs => bs.Seat)
                      .WithMany(s => s.BookingSeats)
                      .HasForeignKey(bs => bs.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bs => bs.ShowSeat)
                      .WithMany(ss => ss.BookingSeats)
                      .HasForeignKey(bs => bs.ShowSeatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // USER - JWT AUTHENTICATION
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasIndex(u => u.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_User_Email_Unique");

                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(u => u.Role)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Customer");

                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(u => u.IsActive)
                      .HasDefaultValue(true);
            });
        }
    }
}
