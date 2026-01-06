import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import {AdminApiAuthApiClient, AuthenticatedResult, LoginRequest} from '../../../api/admin-api.service.generated'
import { AlertService } from '../../../shared/services/alert.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private authApiClient: AdminApiAuthApiClient, private alertService: AlertService, private router: Router) {
    this.loginForm = this.formBuilder.group({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  login() {
    let request: LoginRequest = new LoginRequest({
      userName: this.loginForm.controls['username'].value,
      password: this.loginForm.controls['password'].value,
    })

    this.authApiClient.login(request).subscribe({
      next: (res: AuthenticatedResult) => {
        console.log('Login successful', res);
        //Save token and refresh token to localStorage

        //redirect to dashboard
        this.router.navigate(['/dashboard'])
      },
      error: (err: any) => {
        console.error('Login failed: ', err);
        this.alertService.showError("Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin đăng nhập.");
      }
    });
  }
}
