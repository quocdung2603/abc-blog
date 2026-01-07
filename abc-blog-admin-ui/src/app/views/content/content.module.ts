import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import {
  AvatarModule,
  ButtonGroupModule,
  ButtonModule,
  CardModule,
  FormModule,
  GridModule,
  NavModule,
  ProgressModule,
  TableModule,
  TabsModule,
} from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { ContentRoutingModule } from './content-routing.module';
import { PostComponent } from './posts/post.component';
import { PostCategoryComponent } from './posts-categories/postCategory.component';
import { SeriesComponent } from './series/series.component';
import { RoyaltyComponent } from './royalties/royalty.component';

@NgModule({
  imports: [
    ContentRoutingModule,
    CardModule,
    NavModule,
    IconModule,
    TabsModule,
    CommonModule,
    GridModule,
    ProgressModule,
    ReactiveFormsModule,
    ButtonModule,
    FormModule,
    ButtonModule,
    ButtonGroupModule,
    ChartjsModule,
    AvatarModule,
    TableModule,
  ],
  declarations: [
    PostComponent,
    PostCategoryComponent,
    SeriesComponent,
    RoyaltyComponent,
  ],
})
export class ContentModule {}
