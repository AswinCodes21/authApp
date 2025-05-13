import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  imports: [],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  username: string = '';
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSignup() {
    this.authService.signup(this.username, this.email, this.password).subscribe(
      (response: any) => {
        console.log('Signup successful', response);
        this.router.navigate(['/login']);
      },
      (error) => {
        console.error('Signup failed', error);
      }
    );
  }
}
