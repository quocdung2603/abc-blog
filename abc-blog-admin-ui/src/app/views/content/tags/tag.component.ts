import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  AdminApiPostCategoryApiClient,
  AdminApiTagApiClient,
  PostCategoryDto,
  PostCategoryDtoPageResult,
  TagDto,
  TagDtoPageResult,
} from '../../../api/admin-api.service.generated';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../../shared/services/alert.service';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { MessageConstants } from '../../../shared/constants/messages.constant';
import { TagDetailComponent } from './tag-detail.component';

@Component({
  selector: 'app-tag',
  templateUrl: './tag.component.html',
})
export class TagComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number = 0;

  //business variables
  public items: TagDto[] = [];
  public selectedItems: TagDto[] = [];
  public keyword: string = '';

  constructor(
    private alertService: AlertService,
    private confirmationService: ConfirmationService,
    private dialogService: DialogService,
    private tagService: AdminApiTagApiClient,
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.loadData();
  }

  private toggleBlockUI(enable: boolean) {
    if (enable) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }

  pageChanged(event: any) {
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
  }

  loadData() {
    this.toggleBlockUI(true);
    this.tagService
      .getAllTagsPaging(this.keyword, this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: TagDtoPageResult) => {
          this.items = res.results || [];
          this.totalCount = res.rowCount || 0;

          this.toggleBlockUI(false);
        },
        error: (err: any) => {
          this.toggleBlockUI(false);
          this.alertService.showError(
            'Đã có lỗi xảy ra khi tải dữ liệu. Vui lòng thử lại sau.',
          );
        },
      });
  }

  showAddModal() {
    const ref = this.dialogService.open(TagDetailComponent, {
      header: 'Thêm mới danh mục',
      width: '70%',
    });

    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;

    ref.onClose.subscribe((data: any) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.CREATED_OK_MSG);
        this.selectedItems = [];
        this.loadData();
      }
    });
  }

  deleteItems() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }

    let ids: any[] = [];
    this.selectedItems.forEach((item) => {
      ids.push(item.id);
    });

    this.confirmationService.confirm({
      message: MessageConstants.CONFIRM_DELETE_MSG,
      accept: () => {
        this.deleteItemsConfirm(ids);
      },
    });
  }

  deleteItemsConfirm(ids: any[]) {
    this.toggleBlockUI(true);

    this.tagService
      .deleteTags(ids)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.alertService.showSuccess(MessageConstants.DELETED_OK_MSG);
          this.selectedItems = [];
          this.loadData();
          this.toggleBlockUI(false);
        },
        error: (err: any) => {
          this.alertService.showError(
            'Đã có lỗi xảy ra khi xóa danh mục. Vui lòng thử lại sau.',
          );
          this.toggleBlockUI(false);
        },
      });
  }

  showEditModal() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }

    let id = this.selectedItems[0].id;

    const ref = this.dialogService.open(TagDetailComponent, {
      header: 'Chỉnh sửa danh mục',
      width: '70%',
      data: {
        id: id,
      },
    });

    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;

    ref.onClose.subscribe((data: any) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData();
      }
    });
  }
}
