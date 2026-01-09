import { Component, OnDestroy, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import {
  AddPostSeriesRequest,
  AdminApiSeriesApiClient,
  PostInListDto,
} from '../../../api/admin-api.service.generated';
import { MessageConstants } from '../../../shared/constants/messages.constant';
import { AlertService } from '../../../shared/services/alert.service';

@Component({
  templateUrl: 'series-posts.component.html',
})
export class SeriesPostsComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  public blockedPanel: boolean = false;
  public title!: string;
  public posts: PostInListDto[] = [];

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private seriesApiClient: AdminApiSeriesApiClient,
    private alertService: AlertService
  ) {}
  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.initData();
  }
  initData() {
    this.toggleBlockUI(true);
    this.loadData(this.config.data?.id);
  }

  loadData(id: string) {
    this.toggleBlockUI(true);

    this.seriesApiClient
      .getPostsInSeries(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PostInListDto[]) => {
          this.posts = response;
          this.toggleBlockUI(false);
        },
        error: (error) => {
          this.toggleBlockUI(false);
        },
      });
  }
  removePost(id: string) {
    let body: AddPostSeriesRequest = new AddPostSeriesRequest({
      postId: id,
      seriesId: this.config.data.id,
    });
    this.seriesApiClient
      .deletePostToSeries(body)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.alertService.showSuccess(MessageConstants.DELETED_OK_MSG);
          this.loadData(this.config.data?.id);
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  private toggleBlockUI(enabled: boolean) {
    if (enabled) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }
}
