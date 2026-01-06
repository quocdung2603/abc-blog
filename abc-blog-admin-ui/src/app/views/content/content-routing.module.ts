import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostComponent } from './posts/post.component';
import { PostCategoryComponent } from './posts-categories/postCategory.component';
import { SeriesComponent } from './series/series.component';
import { RoyaltyComponent } from './royalties/royalty.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'posts',
    pathMatch: 'full',
  },
  {
    path: 'posts',
    component: PostComponent,
    data: {
      title: 'Posts',
    },
  },
  {
    path: 'post-categories',
    component: PostCategoryComponent,
    data: {
      title: 'Post Categories',
    },
  },
  {
    path: 'series',
    component: SeriesComponent,
    data: {
      title: 'Series',
    },
  },
  {
    path: 'post-categories',
    component: PostCategoryComponent,
    data: {
      title: 'Post Categories',
    },
  },
  {
    path: 'royalties',
    component: RoyaltyComponent,
    data: {
      title: 'Royalties',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ContentRoutingModule {}
