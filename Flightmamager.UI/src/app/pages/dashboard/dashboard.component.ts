import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent {
  searchId!: number;
  FlightObj: any = {
    id: '',
    flightNumber: '',
    departureTime: new Date(),
    departure: '',
    destination: '',
    airplaneType: ''
  };
  newFlight = {
    flightNumber: '',
    departure: '',
    destination: '',
    departureTime: '',
    arrivalTime: '',
    airplaneType: ''
  };

  selectedFlight: any;
  constructor(private http: HttpClient) { }
  deleteFlight(id: number) {
    const token = localStorage.getItem('angular17TokenData'); // get the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.delete(`http://localhost:5000/flight/delateFlight?id=${id}`, { headers }).subscribe({
      next: (res: any) => {
        console.log(res);
        this.ngOnInit();
      },
      error: (err: any) => {
        this.getRefreshToken()
        if (err.status === 404) {
          alert('Flight not found');
        } else if (err.status === 401) {
          console.log("Not authorized only admins can delete flights");
        } else {
          console.log("Api error occured");

        }
      }
    });
  }
  ngOnInit(): void {
    const token = localStorage.getItem('angular17TokenData'); // get the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get('http://localhost:5000/flight/getAllFlight', { headers }).subscribe({
      next: (res: any) => {
        this.FlightObj = Object.keys(res.value).map(key => res.value[key]);
        this.FlightObj.sort((a: any, b: any) => a.id - b.id);
      },
      error: (err: any) => {
        this.getRefreshToken()
        console.log("Api error occured");
      }
    });
  }
  searchFlight(): void {
    if (this.searchId) {
      this.getFlight(this.searchId);
    } else {
      this.ngOnInit();
    }
  }
  getFlight(id: number): void {
    const token = localStorage.getItem('angular17TokenData'); // get the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get(`http://localhost:5000/flight/getFlight?id=${id}`, { headers }).subscribe({
      next: (res: any) => {
        console.log(res);
        this.FlightObj = [res.value];
      },
      error: (err: any) => {
        this.getRefreshToken()
        console.log("Api error occured");
      }
    }
    );
  }
  addFlight(): void {
    const token = localStorage.getItem('angular17TokenData');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.post('http://localhost:5000/flight/addFlight', this.newFlight, { headers }).subscribe({
      next: (res: any) => {
        console.log(res);
        this.ngOnInit();
      },
      error: (err: any) => {
        this.getRefreshToken()
        if (err.status === 400) {
          alert('Invalid flight date');
        } else if (err.status === 409) {
          alert('Flight already exists');
        } else if (err.status === 401) {
          console.log('Not authorized only admins can add flights');
        } else {
          console.log("Api error occured");
        }
      }
    });
  }
  getRefreshToken() {


  }
}