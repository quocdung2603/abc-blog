import { Component, OnDestroy } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import {
  AdminApiAuthApiClient,
  AuthenticatedResult,
  LoginRequest,
} from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { Router } from '@angular/router';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy {
  loginForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  loading = false;

  constructor(
    private formBuilder: FormBuilder,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private tokenStorageService: TokenStorageService
  ) {
    this.loginForm = this.formBuilder.group({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  login() {
    this.loading = true;
    let request: LoginRequest = new LoginRequest({
      userName: this.loginForm.controls['username'].value,
      password: this.loginForm.controls['password'].value,
    });

    this.authApiClient
      .login(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: AuthenticatedResult) => {
          console.log('Login successful', res);
          //Save token and refresh token to localStorage
          this.tokenStorageService.saveToken(res.token || '');
          this.tokenStorageService.saveRefreshToken(res.refreshToken || '');
          this.tokenStorageService.saveUser(res);
          //redirect to dashboard
          this.router.navigate([UrlConstants.HOME]);
        },
        error: (err: any) => {
          console.error('Login failed: ', err);
          this.alertService.showError(
            'Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin đăng nhập.'
          );
          this.loading = false;
        },
      });
  }
}
