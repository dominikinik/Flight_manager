import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public $refreshToken = new Subject<boolean>;
  public $refreshTokenReceived = new Subject<boolean>;

  constructor(private http: HttpClient) {

    this.$refreshToken.subscribe((res: any) => {
      this.getRefreshToken()
    })
  }
  onLogin(loginObj: any): Observable<any> {
    return this.http.post('http://localhost:5015/api/Usercontroler/login', loginObj, { withCredentials: true });
  }



  getRefreshToken() {
    this.http.get("http://localhost:5015/api/Usercontroler/ref_refresh_token", { withCredentials: true }).subscribe(
      (Res: any) => {
        console.log(Res.token);
        localStorage.setItem('angular17TokenData', Res.token);
      },
      (err: any) => {
        console.error('Error refreshing token:', err);
      }
    );
  }


}
