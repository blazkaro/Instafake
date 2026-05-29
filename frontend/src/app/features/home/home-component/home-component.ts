import { Component, DestroyRef, effect, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { TuiItem } from "@taiga-ui/cdk/directives/item";
import { TuiInput, TuiLoader, TuiTextfieldMultiComponent } from '@taiga-ui/core';
import { TuiAvatar, TuiAvatarOutline, TuiChip, TuiInputChipComponent, TuiInputChipDirective } from '@taiga-ui/kit';
import { map } from 'rxjs';
import { UserService } from '../../../core/services/user-service';
import { CursorPagination } from '../../../shared/pagination/cursor-pagination';
import { Post } from '../../../shared/post/models/post';
import { PostOverviewComponent } from "../../../shared/post/post-overview-component";
import { PostsService } from '../posts-service';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent, TuiLoader, TuiTextfieldMultiComponent, TuiChip, TuiInputChipComponent, TuiItem, FormsModule, TuiInputChipDirective],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
})
export class HomeComponent implements OnInit {
  private readonly PAGE_SIZE: number = 2;

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

  private sentinel = viewChild<ElementRef<HTMLElement>>('sentinel');
  private destroyRef = inject(DestroyRef);
  private observer!: IntersectionObserver;

  constructor() {
    effect(() => {
      const resourceState = this.postsResource.value();
      if (!resourceState) return;

      const { response, params } = resourceState;
      this.nextCursor = response.pagination.nextCursor;

      if (params.cursor === null) {
        // Tags or username changed (or initial load) -> Clear and set fresh posts
        this.posts.set(response.posts);
      } else {
        // Only cursor changed -> append new posts
        this.posts.update((current) => [...current, ...response.posts]);
      }
    });
  }

  ngOnInit(): void {
    this.observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting && !this.postsResource.isLoading() && this.nextCursor?.lastItemCreatedAt) {
        this.loadMorePosts();
      }
    }, {
      root: null, // uses viewport,
      rootMargin: '100px', // trigger 100px before user reaches end,
      threshold: 0.1
    });

    const element = this.sentinel()?.nativeElement;
    if (element) {
      this.observer.observe(element);
    }

    this.destroyRef.onDestroy(() => this.observer.disconnect());
  }

  loadMorePosts() {
    this.cursor.set(this.nextCursor);
  }

  onSearchInputChange(newValue: string[]) {
    // no duplicates in both usernames and tags (handled by tui-chip)
    const validUsernames = newValue.filter((item) => item.startsWith('@') && item.length > 1);
    const validTags = newValue.filter((item) => item.startsWith('#') && item.length > 1);

    const newestUsername = validUsernames.at(-1);
    this.searchInput = newestUsername ? [newestUsername, ...validTags] : validTags;

    // clear cursors (search filters changed)
    this.nextCursor = null;
    this.cursor.set(null);

    if (newestUsername) {
      this.userName.set(newestUsername.substring(1));
    }

    const validTagsSet = new Set(validTags);
    if (this.tags().length != validTagsSet.size || !this.tags().every((tag) => validTagsSet.has(tag))) {
      this.tags.set(validTags.map((val) => val.substring(1)));
    }
  }
}
