import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5105/api/auth';

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, { username, password });
  }

  signup(username: string, email: string, password: string) {
    return this.http.post(`${this.apiUrl}/signup`, { username, email, password });
  }
}
