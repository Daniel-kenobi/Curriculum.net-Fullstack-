import { Injectable } from '@angular/core';
import { CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { authService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class GuardServiceGuard implements CanLoad {
  constructor(private authService: authService, private router: Router) {

  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    if (!this.authService.fnc_logado()) {
      this.router.navigateByUrl('home');
      return false;
    }
    return true;
  }
}
