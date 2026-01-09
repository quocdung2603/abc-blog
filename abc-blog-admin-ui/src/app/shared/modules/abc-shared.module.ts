import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ValidationMessageComponent } from './validation-message/validation-message.component';
import { PermissionDirectiveModule } from './permission-directive.module';

@NgModule({
  imports: [CommonModule],
  declarations: [ValidationMessageComponent, PermissionDirectiveModule],
  exports: [ValidationMessageComponent],
})
export class AbcSharedModule {}
