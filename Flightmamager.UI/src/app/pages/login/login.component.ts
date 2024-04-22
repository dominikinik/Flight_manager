import { Component, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginObj: any = {
    "username": "",
    "passwordHash": ""
  }

  constructor(private userSrv: UserService, private router: Router) { }

  login() {
  this.userSrv.onLogin(this.loginObj).subscribe({
    next: (res: any) => {
      localStorage.setItem('angular17TokenData', res.token);
      this.router.navigateByUrl('/dashboard'); 
    },
    error: error => {
      if (error.status === 404) {
        alert('User not found');
        this.router.navigateByUrl('/register'); 
      } else if (error.status === 400) {
        alert('Incorrect password');
      } else {
        alert("Wrong Credentials")
      }
    },
  })
}
}
