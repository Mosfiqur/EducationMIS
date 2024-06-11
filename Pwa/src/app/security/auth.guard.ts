import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { UserDB } from '../localdb/UserDB';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private userDb: UserDB) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : Observable<boolean> | Promise<boolean> | boolean {

    return new Promise((resolve, reject) => {
      this.userDb.getUser().subscribe(data => {

        if (data[0]) {
          resolve(true);
        }
        else {
          this.router.navigate(['unicefpwa/authentication/login'],
            { queryParams: { returnUrl: state.url } });
          resolve(false);
        }

      })
    });
  }
}
