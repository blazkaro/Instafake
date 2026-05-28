import { Component, effect, inject, OnInit, Signal, signal } from '@angular/core';
import { TuiIcon, TuiInput, TuiLoader, TuiTextfieldMultiComponent, TuiHintOverflow, TuiScrollbar, TuiScrollbarDirective } from '@taiga-ui/core';
import { UserService } from '../../../core/services/user-service';
import { TuiAvatar, TuiAvatarOutline, TuiChevron, TuiChip, TuiInputChipComponent, TuiInputChipDirective } from '@taiga-ui/kit';
import { PostOverviewComponent } from "../../../shared/post/post-overview-component";
import { Post } from '../../../shared/post/models/post';
import { PostsService } from '../posts-service';
import { rxResource } from '@angular/core/rxjs-interop';
import { distinctUntilKeyChanged, map } from 'rxjs';
import { CursorPagination, Pagination } from '../../../shared/pagination/cursor-pagination';
import { TuiItem } from "@taiga-ui/cdk/directives/item";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent, TuiLoader, TuiChevron, TuiTextfieldMultiComponent, TuiChip, TuiInputChipComponent, TuiItem, FormsModule, TuiInputChipDirective, TuiHintOverflow, TuiScrollbar, TuiScrollbarDirective],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
})
export class HomeComponent {
  private readonly PAGE_SIZE: number = 20;

  userService = inject(UserService)
  postsService = inject(PostsService);

  protected searchInput: string[] = [];

  tags = signal<string[]>([]);
  userName = signal<string | null>(null);
  cursor = signal<CursorPagination | null>(null);

  posts = signal<Post[]>([]);
  nextCursor: CursorPagination | null = null;

  postsResource = rxResource({
    params: () => ({
      tags: this.tags(),
      authorName: this.userName(),
      cursor: this.cursor()
    }),
    stream: ({ params }) => this.postsService.getPosts(params.tags, params.authorName, { pageSize: this.PAGE_SIZE, cursor: params.cursor }).pipe(
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

  onSearchInputChange(newValue: string[]) {
    // no duplicates in both usernames and tags (handled by tui-chip)
    const validUsernames = newValue.filter((item) => item.startsWith('@') && item.length > 1);
    const validTags = newValue.filter((item) => item.startsWith('#') && item.length > 1);

    const newestUsername = validUsernames.at(-1);
    console.log(newestUsername);
    this.searchInput = newestUsername ? [newestUsername, ...validTags] : validTags;

    if (newestUsername) {
      console.log(newestUsername);
      this.userName.set(newestUsername.substring(1));
    }

    const validTagsSet = new Set(validTags);
    if (this.tags().length != validTagsSet.size || !this.tags().every((tag) => validTagsSet.has(tag)))
      this.tags.set(validTags.map((val) => val.substring(1)));
  }
}
