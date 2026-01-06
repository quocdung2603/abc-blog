import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../shared/services/token-storage.service';
import { UrlConstants } from '../../shared/constants/url.constants';

import { navItems } from './_nav';
import { Router } from '@angular/router';
import { INavData } from '@coreui/angular';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
})
export class DefaultLayoutComponent implements OnInit {
  public navItems: INavData[] = [];

  constructor(
    private tokenStorageService: TokenStorageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = this.tokenStorageService.getUser();
    if (user == null) {
      this.router.navigate([UrlConstants.LOGIN]);
      return;
    }
    const permissions: string[] = user.permissions
      ? JSON.parse(user.permissions)
      : [];
    for (const item of navItems) {
      const children = item.children;
      if (children) {
        for (const child of children) {
          if (
            child.attributes &&
            permissions.filter(
              (x: string) => x === child.attributes!['policyName']
            ).length === 0
          ) {
            child.class = 'hidden';
          }
        }
      }
    }
    this.navItems = navItems;
  }
}
