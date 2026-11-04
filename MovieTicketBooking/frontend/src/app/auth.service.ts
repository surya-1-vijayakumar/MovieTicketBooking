import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiService } from './api.service';
import { AuthSession } from './models';

const TOKEN_KEY = 'movie-ticket-booking.token';
const SESSION_KEY = 'movie-ticket-booking.session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly sessionSubject = new BehaviorSubject<AuthSession | null>(
    this.readSession()
  );
  readonly session$ = this.sessionSubject.asObservable();

  constructor(private readonly api: ApiService) {}

  get session(): AuthSession | null {
    return this.sessionSubject.value;
  }

  login(email: string, password: string): Observable<AuthSession> {
    return this.api.login(email, password).pipe(tap(session => this.save(session)));
  }

  register(fullName: string, email: string, password: string): Observable<AuthSession> {
    return this.api.register(fullName, email, password).pipe(tap(session => this.save(session)));
  }

  logout(): void {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(SESSION_KEY);
    this.sessionSubject.next(null);
  }

  private save(session: AuthSession): void {
    sessionStorage.setItem(TOKEN_KEY, session.token);
    sessionStorage.setItem(SESSION_KEY, JSON.stringify(session));
    this.sessionSubject.next(session);
  }

  private readSession(): AuthSession | null {
    const saved = sessionStorage.getItem(SESSION_KEY);
    if (!saved) {
      return null;
    }

    try {
      const session = JSON.parse(saved) as AuthSession;
      if (session.expiresAt && new Date(session.expiresAt).getTime() <= Date.now()) {
        sessionStorage.removeItem(TOKEN_KEY);
        sessionStorage.removeItem(SESSION_KEY);
        return null;
      }
      return session;
    } catch {
      sessionStorage.removeItem(TOKEN_KEY);
      sessionStorage.removeItem(SESSION_KEY);
      return null;
    }
  }
}
