import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { TokenStorageService } from '../shared/services/token-storage.service';
import { UrlConstants } from '../shared/constants/url.constants';

@Injectable()
export class AuthGuard {
  constructor(
    private router: Router,
    private tokenStorageService: TokenStorageService
  ) {}

  canActivate(
    activateRoute: ActivatedRouteSnapshot,
    routerState: RouterStateSnapshot
  ): boolean {
    let requiredPolicy = activateRoute.data['requiredPolicy'] as string;
    let loggedInUser = this.tokenStorageService.getUser();

    if (loggedInUser) {
      let listPermission = JSON.parse(loggedInUser.permissions);
      if (
        listPermission != null &&
        listPermission != '' &&
        listPermission.filter((x: string) => x == requiredPolicy)
      ) {
        return true;
      } else {
        this.router.navigate([UrlConstants.ACCESS_DENIED], {
          queryParams: {
            returnUrl: routerState.url,
          },
        });
        return false;
      }
    } else {
      this.router.navigate([UrlConstants.LOGIN], {
        queryParams: {
          returnUrl: routerState.url,
        },
      });
      return false;
    }
  }
}
