import { Injectable } from '@angular/core';
import { UserModel } from '../models//user.model';

const TOKEN_KEY = 'auth-token';
const REFRESH_TOKEN_KEY = 'auth-refresh-token';
const USER_KEY = 'auth-user';
@Injectable()
export class TokenStorageService {
  constructor() {}

  signOut(): void {
    globalThis.localStorage.clear();
  }

  public saveToken(token: string): void {
    globalThis.localStorage.removeItem(TOKEN_KEY);
    globalThis.localStorage.setItem(TOKEN_KEY, token);

    const user = this.getUser();
    if (user?.id) {
      this.saveUser({ ...user, accessToken: token });
    }
  }

  public getToken(): string | null {
    return globalThis.localStorage.getItem(TOKEN_KEY);
  }

  public saveRefreshToken(refreshToken: string): void {
    globalThis.localStorage.removeItem(REFRESH_TOKEN_KEY);
    globalThis.localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
  }

  public getRefreshToken(): string | null {
    return globalThis.localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  public saveUser(user: any): void {
    globalThis.localStorage.removeItem(USER_KEY);
    globalThis.localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): UserModel | null {
    const token = globalThis.localStorage.getItem(USER_KEY);
    if (!token) {
      return null;
    }

    const base64url = token.split('.')[1];
    const base64 = base64url.replace('-', '+').replace('_', '/');
    const user: UserModel = JSON.parse(this.b64DecodeUnicode(base64));
    return user;
  }

  b64DecodeUnicode(str: string) {
    return decodeURIComponent(
      Array.prototype.map
        .call(atob(str), function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );
  }
}
