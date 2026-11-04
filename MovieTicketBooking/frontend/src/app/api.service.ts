import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  AuthSession,
  Booking,
  BookingRequest,
  MovieDetails,
  MovieSummary,
  PagedMovies,
  Seat,
  Show
} from './models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly baseUrl = '/api';

  constructor(private readonly http: HttpClient) {}

  getMovies(page: number, pageSize: number): Observable<PagedMovies> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PagedMovies>(`${this.baseUrl}/movies`, { params });
  }

  searchMovies(title: string): Observable<MovieSummary[]> {
    const params = new HttpParams().set('title', title);
    return this.http.get<MovieSummary[]>(`${this.baseUrl}/movies/search`, { params });
  }

  getMovie(movieId: number): Observable<MovieDetails> {
    return this.http.get<MovieDetails>(`${this.baseUrl}/movies/${movieId}`);
  }

  getShowsForMovie(movieId: number): Observable<Show[]> {
    return this.http.get<Show[]>(`${this.baseUrl}/shows/movie/${movieId}`);
  }

  getSeatsForScreen(screenId: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(`${this.baseUrl}/seats/screen/${screenId}`);
  }

  login(email: string, password: string): Observable<AuthSession> {
    return this.http.post<AuthSession>(`${this.baseUrl}/auth/login`, { email, password });
  }

  register(fullName: string, email: string, password: string): Observable<AuthSession> {
    return this.http.post<AuthSession>(`${this.baseUrl}/auth/register`, {
      fullName,
      email,
      password
    });
  }

  createBooking(booking: BookingRequest): Observable<Booking> {
    return this.http.post<Booking>(`${this.baseUrl}/bookings`, booking);
  }

  getBooking(bookingId: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.baseUrl}/bookings/${bookingId}`);
  }
}
