import {
  Component,
  effect,
  ElementRef,
  inject,
  signal,
  viewChild
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiSheetDialogOptions } from '@taiga-ui/addon-mobile';
import {
  TuiButton,
  TuiDialogContext,
  TuiHintDirective,
  TuiInput,
  TuiLabel,
  TuiLoader,
  TuiNotificationService,
} from '@taiga-ui/core';
import { TuiTextareaComponent } from '@taiga-ui/kit';
import { injectContext, PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { UserService } from '../../../core/services/user-service';
import { InfiniteScrollService } from '../../infinite-scroll/infinite-scroll-service';
import { ToastComponent } from '../../toasts/toast-component/toast-component';
import { Post } from '../models/post';
import { PostCommentsService } from '../services/post-comments-service';
import { PostEventsService } from '../services/post-events-service';
import { PostCommentComponent } from './post-comment-component/post-comment-component';
import { COMMENTS_POST_ID, PostCommentsListService } from './post-comments-list-service';

@Component({
  selector: 'app-post-comments-component',
  imports: [
    PostCommentComponent,
    TuiInput,
    TuiTextareaComponent,
    TuiButton,
    TuiLabel,
    TuiHintDirective,
    TuiLoader,
    FormsModule,
  ],
  templateUrl: './post-comments-component.html',
  styleUrl: './post-comments-component.scss',
  providers: [
    {
      provide: COMMENTS_POST_ID,
      useFactory: () => {
        const context = injectContext<TuiDialogContext<boolean, Post>>();
        return context.data.id;
      },
    },
    PostCommentsListService,
    InfiniteScrollService,
  ],
})
export class PostCommentsComponent {
  public static readonly defaultOptions: Partial<TuiSheetDialogOptions> = {
    closable: true,
    appearance: 'fullscreen',
  };

  private readonly context = injectContext<TuiDialogContext<boolean, Post>>();
  protected readonly post = this.context.data;

  private sentinel = viewChild<ElementRef<HTMLElement>>('sentinel');
  private readonly infiniteScrollService = inject(InfiniteScrollService);

  private readonly commentsService = inject(PostCommentsService);
  protected readonly commentsListService = inject(PostCommentsListService);
  private readonly notificationsService = inject(TuiNotificationService);
  private readonly postEventsService = inject(PostEventsService);
  private readonly userService = inject(UserService);

  constructor() {
    effect(() => {
      this.infiniteScrollService.observeSentinel(this.sentinel(), () =>
        this.commentsListService.loadMore(),
      );
    });
  }

  message = signal<string>('');

  createComment() {
    const msg = this.message().trim();
    if (msg.length == 0) return;

    this.commentsService.createComment(this.post.id, msg).subscribe((response) => {
      this.notificationsService
        .open(new PolymorpheusComponent(ToastComponent), {
          label: 'Success.',
          autoClose: 3000,
          size: 'l',
          appearance: 'positive',
          data: { message: 'Comment added successfully' },
        })
        .subscribe();

      this.message.set('');

      const user = this.userService.user()!;
      this.postEventsService.emitCommentAdded({
        postId: this.post.id,
        comment: {
          id: response.id,
          author: {
            id: user.id,
            name: user.userName,
            avatarUrl: user.avatarUrl,
          },
          content: msg,
          createdAt: new Date(), // doesn't need to be accurate (we dont get it from server)
        },
      });
    });
  }
}
