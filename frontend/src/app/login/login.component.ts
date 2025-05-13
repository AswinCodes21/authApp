import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    this.authService.login(this.username, this.password).subscribe(
      (response: any) => {
        localStorage.setItem('jwtToken', response.token);
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        console.error('Login failed', error);
      }
    );
  }
}
