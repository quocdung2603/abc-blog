import { Component, OnDestroy, OnInit, Signal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiRoleApiClient,
  RoleDto,
  RoleDtoPageResult,
} from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { DialogService } from 'primeng/dynamicdialog';
import { ConfirmationService } from 'primeng/api';

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
    this.pageIndex = event.page;
    this.pageSize = event.rows;
    this.loadData();
  }

  deleteItems() {}

  showAddModal() {}

  showEditModal() {}

  showPermissionModal(rowId: number, rowName: string) {}
}
