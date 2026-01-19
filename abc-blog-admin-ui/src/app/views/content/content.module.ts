import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { ChartjsModule } from '@coreui/angular-chartjs';
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
import { PaginatorModule } from 'primeng/paginator';
import { AbcSharedModule } from '../../shared/modules/abc-shared.module';
import { KeyFilterModule } from 'primeng/keyfilter';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ImageModule } from 'primeng/image';
import { SeriesPostsComponent } from './series/series-posts.component';
import { SeriesDetailComponent } from './series/series-detail.component';
import { PostActivityLogsComponent } from './posts/post-activity-logs.component';
import { PostDetailComponent } from './posts/post-detail.component';
import { PostReturnReasonComponent } from './posts/post-return-reason.component';
import { PostSeriesComponent } from './posts/post-series.component';
import { DropdownModule } from 'primeng/dropdown';
import { EditorModule } from 'primeng/editor';
import { InputNumberModule } from 'primeng/inputnumber';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { TagComponent } from './tags/tag.component';
import { TagDetailComponent } from './tags/tag-detail.component';
import { MultiSelectModule } from 'primeng/multiselect';

@NgModule({
  imports: [
    ContentRoutingModule,
    IconModule,
    CommonModule,
    ReactiveFormsModule,
    ChartjsModule,
    ProgressSpinnerModule,
    PanelModule,
    BlockUIModule,
    PaginatorModule,
    BadgeModule,
    CheckboxModule,
    TableModule,
    KeyFilterModule,
    AbcSharedModule,
    ButtonModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    EditorModule,
    InputNumberModule,
    ImageModule,
    AutoCompleteModule,
    DynamicDialogModule,
    MultiSelectModule,  
  ],
  declarations: [
    PostComponent,
    PostDetailComponent,
    PostCategoryComponent,
    PostCategoryDetailComponent,
    SeriesComponent,
    SeriesDetailComponent,
    PostReturnReasonComponent,
    PostSeriesComponent,
    SeriesPostsComponent,
    PostActivityLogsComponent,
    TagComponent,
    TagDetailComponent,
  ],
})
export class ContentModule {}
