import { Component, effect, inject, OnInit, Signal, signal } from '@angular/core';
import { TuiIcon, TuiInput, TuiLoader } from '@taiga-ui/core';
import { UserService } from '../../../core/services/user-service';
import { TuiAvatar, TuiAvatarOutline } from '@taiga-ui/kit';
import { PostOverviewComponent } from "../../../shared/post/post-overview-component";
import { Post } from '../../../shared/post/models/post';
import { PostsService } from '../posts-service';
import { rxResource } from '@angular/core/rxjs-interop';
import { distinctUntilKeyChanged, map } from 'rxjs';
import { CursorPagination, Pagination } from '../../../shared/pagination/cursor-pagination';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent, TuiLoader],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
})
export class HomeComponent {
  private readonly PAGE_SIZE: number = 20;

  userService = inject(UserService)
  postsService = inject(PostsService);

  tags = signal<string[]>([]);
  userName = signal<string | null>(null);
  cursor = signal<CursorPagination | null>(null);

  posts = signal<Post[]>([]);
  nextCursor: CursorPagination | null = null;

  postsResource = rxResource({
    params: () => ({
      tags: this.tags(),
      authorId: this.userName(),
      cursor: this.cursor()
    }),
    stream: ({ params }) => this.postsService.getPosts(params.tags, params.authorId, { pageSize: this.PAGE_SIZE, cursor: params.cursor }).pipe(
      map((response) => ({ response, params }))
    )
  });

  constructor() {
    effect(() => {
      const resourceState = this.postsResource.value();
      if (!resourceState) return;

      const { response, params } = resourceState;
      this.nextCursor = response.pagination.cursor;

      if (params.cursor === null) {
        // Tags or username changed (or initial load) -> Clear and set fresh posts
        this.posts.set(response.posts);
      } else {
        // Only cursor changed -> append new posts
        this.posts.update((current) => [...current, ...response.posts]);
      }
    });
  }

  loadMorePosts() {
    this.cursor.set(this.nextCursor);
  }
}
