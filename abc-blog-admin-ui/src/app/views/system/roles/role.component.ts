import { Component, OnDestroy, OnInit, Signal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiRoleApiClient,
  RoleDto,
  RoleDtoPageResult,
} from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { ConfirmationService } from 'primeng/api';
import { RolesDetailComponent } from './role-detail.component';
import { MessageConstants } from '../../../shared/constants/messages.constant';
import { PermissionGrantComponent } from './permission-grant.component';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
})
export class RoleComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //pagination variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number = 0;

  //business variables
  public items: RoleDto[] = [];
  public selectedItems: RoleDto[] = [];
  public keyword: string = '';
  constructor(
    private roleService: AdminApiRoleApiClient,
    private dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService
  ) {}
  ngOnInit(): void {
    this.loadData();
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  loadData() {
    this.toggleBlockUI(true);

    this.roleService
      .getRolesPaging(this.keyword, this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: RoleDtoPageResult) => {
          this.items = res.results || [];
          this.totalCount = res.rowCount || 0;

          this.toggleBlockUI(false);
        },
        error: (err: any) => {
          this.toggleBlockUI(false);
          this.alertService.showError(
            'Đã có lỗi xảy ra khi tải dữ liệu. Vui lòng thử lại sau.'
          );
        },
      });
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

  showAddModal() {
    const ref = this.dialogService.open(RolesDetailComponent, {
      header: 'Thêm mới quyền',
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

  showEditModal() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }

    var id = this.selectedItems[0].id;
    const ref = this.dialogService.open(RolesDetailComponent, {
      header: 'Cập nhật quyền',
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

  showPermissionModal(rowId: number, rowName: string) {
    const ref = this.dialogService.open(PermissionGrantComponent, {
      header: `Phân quyền - ${rowName}`,
      width: '70%',
      data: {
        id: rowId,
      },
    });

    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;

    ref.onClose.subscribe((data: RoleDto) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
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

    var ids: any[] = [];
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

    this.roleService
      .deleteRoles(ids)
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
            'Đã có lỗi xảy ra khi xóa bản ghi. Vui lòng thử lại sau.'
          );
          this.toggleBlockUI(false);
        },
      });
  }
}
