import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import {
  AuthSession,
  Booking,
  MovieDetails,
  MovieSummary,
  Seat,
  Show
} from './models';

type AuthMode = 'login' | 'register' | null;

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  movies: MovieSummary[] = [];
  featuredMovie: MovieDetails | null = null;
  page = 1;
  pageSize = 12;
  totalPages = 1;
  totalMovies = 0;
  searchTerm = '';
  isSearching = false;
  isLoadingMovies = true;
  moviesError = '';
  featuredError = '';

  bookingOpen = false;
  bookingLoading = false;
  bookingError = '';
  showsLoading = false;
  seatsLoading = false;
  selectedMovie: MovieSummary | null = null;
  shows: Show[] = [];
  selectedShow: Show | null = null;
  seats: Seat[] = [];
  selectedSeats: Seat[] = [];
  customerName = '';
  customerEmail = '';
  customerPhone = '';
  bookingResult: Booking | null = null;

  authMode: AuthMode = null;
  authLoading = false;
  authError = '';
  authName = '';
  authEmail = '';
  authPassword = '';
  session: AuthSession | null = null;

  lookupOpen = false;
  lookupId: number | null = null;
  lookupLoading = false;
  lookupError = '';
  lookupResult: Booking | null = null;

  readonly posterStyles = ['poster-one', 'poster-two', 'poster-three', 'poster-four'];

  constructor(
    private readonly api: ApiService,
    private readonly auth: AuthService
  ) {}

  ngOnInit(): void {
    this.auth.session$.subscribe(session => {
      this.session = session;
    });
    this.loadMovies();
  }

  loadMovies(page = 1): void {
    this.isLoadingMovies = true;
    this.moviesError = '';
    this.api.getMovies(page, this.pageSize).subscribe({
      next: result => {
        this.movies = result.movies.filter(movie => movie.isActive);
        this.page = result.page;
        this.totalPages = Math.max(1, result.totalPages);
        this.totalMovies = result.totalCount;
        this.isSearching = false;
        this.isLoadingMovies = false;
        this.loadFeaturedMovie(this.movies[0]);
      },
      error: error => {
        this.isLoadingMovies = false;
        this.moviesError = this.errorMessage(error, 'Could not load movies. Check that the API is running.');
      }
    });
  }

  searchMovies(): void {
    const query = this.searchTerm.trim();
    if (!query) {
      this.loadMovies();
      return;
    }

    this.isLoadingMovies = true;
    this.moviesError = '';
    this.api.searchMovies(query).subscribe({
      next: results => {
        this.movies = results.filter(movie => movie.isActive);
        this.totalMovies = this.movies.length;
        this.totalPages = 1;
        this.page = 1;
        this.isSearching = true;
        this.isLoadingMovies = false;
        this.loadFeaturedMovie(this.movies[0]);
      },
      error: error => {
        this.isLoadingMovies = false;
        this.moviesError = this.errorMessage(error, 'Movie search failed. Please try again.');
      }
    });
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.loadMovies();
  }

  private loadFeaturedMovie(movie: MovieSummary | undefined): void {
    this.featuredMovie = null;
    this.featuredError = '';
    if (!movie) {
      return;
    }

    this.api.getMovie(movie.movieId).subscribe({
      next: details => {
        this.featuredMovie = details;
      },
      error: error => {
        this.featuredMovie = null;
        this.featuredError = this.errorMessage(error, 'Could not load featured movie details.');
      }
    });
  }

  openBooking(movie: MovieSummary): void {
    this.selectedMovie = movie;
    this.bookingOpen = true;
    this.bookingResult = null;
    this.selectedShow = null;
    this.selectedSeats = [];
    this.seats = [];
    this.bookingError = '';
    this.showsLoading = true;

    const currentSession = this.auth.session;
    this.customerName = currentSession?.fullName ?? '';
    this.customerEmail = currentSession?.email ?? '';
    this.customerPhone = '';

    this.api.getShowsForMovie(movie.movieId).subscribe({
      next: shows => {
        this.shows = shows
          .filter(show => show.isActive && this.isUpcoming(show))
          .sort((a, b) => this.showDateTime(a).getTime() - this.showDateTime(b).getTime());
        this.showsLoading = false;
      },
      error: error => {
        this.showsLoading = false;
        this.bookingError = this.errorMessage(error, 'Could not load showtimes. Please try again.');
      }
    });
  }

  closeBooking(): void {
    this.bookingOpen = false;
    this.bookingResult = null;
    this.bookingError = '';
  }

  chooseShow(show: Show): void {
    this.selectedShow = show;
    this.selectedSeats = [];
    this.seats = [];
    this.bookingError = '';
    this.seatsLoading = true;
    this.api.getSeatsForScreen(show.screenId).subscribe({
      next: seats => {
        this.seats = seats
          .filter(seat => seat.isActive)
          .sort((a, b) => a.rowLetter.localeCompare(b.rowLetter) || a.columnNumber - b.columnNumber);
        this.seatsLoading = false;
      },
      error: error => {
        this.seatsLoading = false;
        this.bookingError = this.errorMessage(error, 'Could not load this screen’s seats.');
      }
    });
  }

  get seatRows(): Seat[][] {
    const rows = new Map<string, Seat[]>();
    for (const seat of this.seats) {
      const row = rows.get(seat.rowLetter) ?? [];
      row.push(seat);
      rows.set(seat.rowLetter, row);
    }
    return Array.from(rows.values());
  }

  isSeatSelected(seat: Seat): boolean {
    return this.selectedSeats.some(selected => selected.seatId === seat.seatId);
  }

  toggleSeat(seat: Seat): void {
    this.bookingError = '';
    if (this.isSeatSelected(seat)) {
      this.selectedSeats = this.selectedSeats.filter(selected => selected.seatId !== seat.seatId);
      return;
    }
    this.selectedSeats = [...this.selectedSeats, seat];
  }

  get estimatedTotal(): number {
    return this.selectedSeats.reduce(
      (total, seat) => total + (seat.price > 0 ? seat.price : this.selectedShow?.basePrice ?? 0),
      0
    );
  }

  submitBooking(): void {
    if (!this.selectedShow || !this.selectedSeats.length) {
      this.bookingError = 'Choose a showtime and at least one seat to continue.';
      return;
    }

    this.bookingLoading = true;
    this.bookingError = '';
    this.api.createBooking({
      showId: this.selectedShow.showId,
      seatIds: this.selectedSeats.map(seat => seat.seatId),
      customerName: this.customerName.trim(),
      customerEmail: this.customerEmail.trim(),
      customerPhone: this.customerPhone.trim()
    }).subscribe({
      next: booking => {
        this.bookingResult = booking;
        this.bookingLoading = false;
      },
      error: error => {
        this.bookingLoading = false;
        this.bookingError = this.errorMessage(
          error,
          'Booking could not be completed. The selected seats may have just been taken.'
        );
      }
    });
  }

  openAuth(mode: Exclude<AuthMode, null>): void {
    this.authMode = mode;
    this.authError = '';
    this.authName = '';
    this.authEmail = '';
    this.authPassword = '';
  }

  closeAuth(): void {
    this.authMode = null;
    this.authError = '';
  }

  submitAuth(): void {
    this.authLoading = true;
    this.authError = '';
    const request = this.authMode === 'register'
      ? this.auth.register(this.authName.trim(), this.authEmail.trim(), this.authPassword)
      : this.auth.login(this.authEmail.trim(), this.authPassword);

    request.subscribe({
      next: () => {
        this.authLoading = false;
        this.closeAuth();
      },
      error: error => {
        this.authLoading = false;
        this.authError = this.errorMessage(error, 'Authentication failed. Please try again.');
      }
    });
  }

  logout(): void {
    this.auth.logout();
  }

  openLookup(): void {
    this.lookupOpen = true;
    this.lookupId = null;
    this.lookupResult = null;
    this.lookupError = '';
  }

  closeLookup(): void {
    this.lookupOpen = false;
    this.lookupResult = null;
  }

  findBooking(): void {
    if (!this.lookupId || this.lookupId < 1) {
      this.lookupError = 'Enter a valid booking reference number.';
      return;
    }

    this.lookupLoading = true;
    this.lookupError = '';
    this.lookupResult = null;
    this.api.getBooking(this.lookupId).subscribe({
      next: booking => {
        this.lookupResult = booking;
        this.lookupLoading = false;
      },
      error: error => {
        this.lookupLoading = false;
        this.lookupError = this.errorMessage(error, 'Could not find that booking.');
      }
    });
  }

  posterClass(index: number): string {
    return this.posterStyles[index % this.posterStyles.length];
  }

  timeLabel(time: string): string {
    return time.slice(0, 5);
  }

  bookingSeatList(booking: Booking): string {
    return booking.bookedSeats.map(seat => seat.seatNumber).join(', ');
  }

  private isUpcoming(show: Show): boolean {
    return this.showDateTime(show).getTime() >= Date.now();
  }

  private showDateTime(show: Show): Date {
    return new Date(`${show.showDate.slice(0, 10)}T${this.timeLabel(show.startTime)}:00`);
  }

  private errorMessage(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      const body = error.error as { message?: string; title?: string } | null;
      if (body?.message) {
        return body.message;
      }
      if (body?.title) {
        return body.title;
      }
      if (error.status === 0) {
        return 'Could not reach the booking API. Make sure the C# backend is running.';
      }
    }
    return fallback;
  }
}
