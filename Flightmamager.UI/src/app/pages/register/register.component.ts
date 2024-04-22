import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  registerObj: any = {
    "username": "",
    "passwordHash": ""
  }

  constructor(private http: HttpClient, private router: Router) { }
  register() {
    this.http.post('http://localhost:5015/api/Usercontroler/register', this.registerObj).subscribe({
      next: (res: any) => {
        this.router.navigate(['/login']);
      },
      error: (err: any) => {
        alert("Registration This user exists!")
      }
    });
  }
}