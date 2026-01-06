import { Component } from '@angular/core';
import {
  AdminApiTestApiClient,
} from '../../../api/admin-api.service.generated';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
})
export class PostComponent {
  constructor(private apiClient: AdminApiTestApiClient) {}

  TestFunction() {
    this.apiClient.testAuthen().subscribe({
      next: () => {
        console.log('API call successful');
      },
      error: (err) => {
        console.error('Error occurred:', err);
      },
    });
  }
}
