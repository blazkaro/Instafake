import { inject, Injectable, InjectionToken } from '@angular/core';
import { PaginatedListServiceBase } from '../../pagination/paginated-list-service-base';
import { PostComment } from '../models/post-comment';
import { PostCommentsService } from '../services/post-comments-service';
import { PostEventsService } from '../services/post-events-service';
import { filter, pipe } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export const COMMENTS_POST_ID = new InjectionToken<string>('COMMENTS_POST_ID');

@Injectable()
export class PostCommentsListService extends PaginatedListServiceBase<PostComment, {}> {
  private readonly PAGE_SIZE: number = 10;

  private readonly postCommentsService = inject(PostCommentsService);
  private readonly postEventsService = inject(PostEventsService);

  private postId: string = inject(COMMENTS_POST_ID);

  constructor() {
    super({
      params: () => ({}),
      stream: () => this.postCommentsService.getComments(this.postId, { pageSize: this.PAGE_SIZE, cursor: this.cursor() })
    });
  }

  private commentAddedSub = this.postEventsService.commentAdded$.pipe(
    filter((ev) => ev.postId == this.postId),
    takeUntilDestroyed()
  ).subscribe((ev) => {
    this.prependItem(ev.comment);
  });
}
