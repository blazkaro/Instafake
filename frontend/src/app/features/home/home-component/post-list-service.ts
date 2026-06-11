import { DestroyRef, effect, ElementRef, inject, Injectable, Signal, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import { CursorPagination } from '../../../shared/pagination/cursor-pagination';
import { Post } from '../../../shared/post/models/post';
import { PostsService } from '../../../shared/post/services/posts-service';

@Injectable()
export class PostListService {
  private readonly PAGE_SIZE: number = 2;

  private postsService = inject(PostsService);

  private observer!: IntersectionObserver;
  private destroyRef = inject(DestroyRef);

  private tags = signal<string[]>([]);
  private userName = signal<string | null>(null);
  private cursor = signal<CursorPagination | null>(null);

  private _posts = signal<Post[]>([]);
  public posts = this._posts.asReadonly();
  private nextCursor: CursorPagination | null = null;

  private postsResource = rxResource({
    params: () => ({
      tags: this.tags(),
      authorName: this.userName(),
      cursor: this.cursor()
    }),
    stream: ({ params }) => this.postsService.getPosts(params.tags, params.authorName, { pageSize: this.PAGE_SIZE, cursor: params.cursor }).pipe(
      map((response) => ({ response, params })) // propagate params locally to avoid race conditions
    )
  });

  public isLoading: Signal<boolean> = this.postsResource.isLoading;

  constructor() {
    effect(() => this.onPostsLoaded());
  }

  /**
   * Handles change of search query params.
   * @param newValue new search query params
   * @returns array of validated query params
   */
  onSearchQueryChange(newValue: string[]): string[] {
    // delete duplicates
    newValue = Array.from(new Set(newValue));

    const validUsernames = newValue.filter((item) => item.startsWith('@') && item.length > 1);
    const validTags = newValue.filter((item) => item.startsWith('#') && item.length > 1);

    const newestUsername = validUsernames.at(-1);
    const validSearchQuery = newestUsername ? [newestUsername, ...validTags] : validTags;

    // clear cursors (search filters changed)
    this.nextCursor = null;
    this.cursor.set(null);

    // update username if changed
    if (newestUsername) {
      this.userName.set(newestUsername.substring(1));
    }

    // update tags if changed
    const validTagsSet = new Set(validTags);
    if (this.tags().length != validTagsSet.size || !this.tags().every((tag) => validTagsSet.has(tag))) {
      this.tags.set(validTags.map((val) => val.substring(1)));
    }

    return validSearchQuery;
  }

  /**
   * Applies infinite scroll with sentinel pattern. Has to be called AFTER sentinel initialization
   * @param sentinel the sentinel to observe
   */
  observeSentinel(sentinel?: ElementRef<HTMLElement>) {
    if (!sentinel)
      return;

    this.observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting && !this.postsResource.isLoading() && this.nextCursor?.lastItemCreatedAt) {
        this.loadMorePosts();
      }
    }, {
      root: null, // uses viewport,
      rootMargin: '200px', // trigger 100px before user reaches end,
      threshold: 0
    });

    this.observer.observe(sentinel.nativeElement);
    this.destroyRef.onDestroy(() => this.observer.disconnect());
  }

  private loadMorePosts() {
    this.cursor.set(this.nextCursor);
  }

  private onPostsLoaded() {
    const resourceState = this.postsResource.value();
    if (!resourceState) return;

    const { response, params } = resourceState;
    this.nextCursor = response.pagination.nextCursor;

    if (params.cursor === null) {
      // Tags or username changed (or initial load) -> Clear and set fresh posts
      this._posts.set(response.posts);
    } else {
      // Only cursor changed -> append new posts
      this._posts.update((current) => [...current, ...response.posts]);
    }
  }
}
