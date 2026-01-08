import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { BadgeModule } from 'primeng/badge';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { ContentRoutingModule } from './content-routing.module';
import { PostComponent } from './posts/post.component';
import { PostCategoryComponent } from './posts-categories/post-category.component';
import { PostCategoryDetailComponent } from './posts-categories/post-category-detail.component';
import { SeriesComponent } from './series/series.component';
import { RoyaltyComponent } from './royalties/royalty.component';
import { PaginatorModule } from 'primeng/paginator';
import { AbcSharedModule } from '../../shared/modules/abc-shared.module';
import { KeyFilterModule } from 'primeng/keyfilter';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';

@NgModule({
  imports: [
    ContentRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    BadgeModule,
    PaginatorModule,
    BlockUIModule,
    ProgressSpinnerModule,
    PanelModule,
    CheckboxModule,
    AbcSharedModule,
    KeyFilterModule,
    InputTextModule,
    ButtonModule,
  ],
  declarations: [
    PostComponent,
    PostCategoryComponent,
    PostCategoryDetailComponent,
    SeriesComponent,
    RoyaltyComponent,
  ],
})
export class ContentModule {}
