export interface MovieSummary {
  movieId: number;
  title: string;
  genre: string;
  language: string;
  durationMinutes: number;
  rating: number;
  isActive: boolean;
}

export interface MovieDetails extends MovieSummary {
  description: string | null;
  releaseDate: string;
  posterUrl: string | null;
  createdAt: string;
}

export interface PagedMovies {
  movies: MovieSummary[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface Show {
  showId: number;
  movieId: number;
  movieTitle: string;
  durationMinutes: number;
  screenId: number;
  screenName: string;
  theatreId: number;
  theatreName: string;
  showDate: string;
  startTime: string;
  endTime: string;
  basePrice: number;
  isActive: boolean;
  createdAt: string;
  totalSeats: number;
  availableSeats: number;
}

export interface Seat {
  seatId: number;
  screenId: number;
  screenName: string;
  rowLetter: string;
  columnNumber: number;
  seatNumber: string;
  seatType: string;
  price: number;
  isActive: boolean;
  createdAt: string;
}

export interface AuthSession {
  token: string;
  expiresAt: string;
  userId: number;
  fullName: string;
  email: string;
  role: string;
}

export interface BookingRequest {
  showId: number;
  seatIds: number[];
  customerName: string;
  customerEmail: string;
  customerPhone: string;
}

export interface Booking {
  bookingId: number;
  showId: number;
  movieTitle: string;
  theatreName: string;
  screenName: string;
  showDate: string;
  startTime: string;
  customerName: string;
  customerEmail: string;
  customerPhone: string | null;
  totalAmount: number;
  bookingStatus: string;
  bookingDate: string;
  bookedSeats: BookedSeat[];
}

export interface BookedSeat {
  seatId: number;
  seatNumber: string;
  seatType: string;
  price: number;
}
