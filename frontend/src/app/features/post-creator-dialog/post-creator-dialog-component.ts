import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  TuiAppearance,
  TuiButton,
  TuiDialogContext,
  TuiGroup,
  TuiInputDirective,
  TuiLink,
  TuiNotificationService,
  TuiTextfieldComponent,
  TuiTextfieldMultiComponent,
} from '@taiga-ui/core';
import {
  TuiAvatar,
  TuiButtonLoading,
  TuiFile,
  TuiFiles,
  TuiInputChipComponent,
  TuiInputChipDirective,
  TuiInputFiles,
  TuiTextareaComponent,
} from '@taiga-ui/kit';
import { POLYMORPHEUS_CONTEXT, PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { map, switchMap, tap } from 'rxjs';
import { UserService } from '../../core/services/user-service';
import { MultimediaPurpose } from '../../shared/multimedia/multimedia-purpose';
import { MultimediaService } from '../../shared/multimedia/multimedia-service';
import { PostEventsService } from '../../shared/post/services/post-events-service';
import { CreatePostResponse, PostsService } from '../../shared/post/services/posts-service';
import { ToastComponent } from '../../shared/toasts/toast-component/toast-component';

@Component({
  selector: 'app-post-creator-dialog-component',
  imports: [
    TuiInputFiles,
    TuiAvatar,
    TuiButtonLoading,
    TuiLink,
    TuiFiles,
    FormsModule,
    TuiFile,
    TuiTextfieldComponent,
    TuiTextfieldMultiComponent,
    TuiInputChipComponent,
    TuiInputChipDirective,
    TuiInputDirective,
    TuiTextareaComponent,
    TuiButton,
    TuiAppearance,
    TuiGroup,
  ],
  templateUrl: './post-creator-dialog-component.html',
  styleUrl: './post-creator-dialog-component.scss',
})
export class PostCreatorDialogComponent {
  private readonly context = inject<TuiDialogContext<boolean, void>>(POLYMORPHEUS_CONTEXT);
  private readonly notificationsService = inject(TuiNotificationService);
  private readonly multimediaService = inject(MultimediaService);
  private readonly postsService = inject(PostsService);
  private readonly postEventsService = inject(PostEventsService);
  private readonly userService = inject(UserService);

  files = signal<File[]>([]);
  tags = signal<string[]>([]);
  description: string = '';

  processing = signal<boolean>(false);

  onTagsChange(newValue: string[]): void {
    const validTags = newValue.filter((item) => item.startsWith('#') && item.length > 1);
    this.tags.set(validTags);
  }

  onCancel(): void {
    this.context.completeWith(false);
  }

  onCreate(): void {
    this.processing.set(true);

    const description = this.description;
    const files = this.files();
    const tags = this.tags().map((tag) => tag.substring(1));
    let urls: URL[] = [];

    this.multimediaService
      .createUploadUrls(MultimediaPurpose.Post, files)
      .pipe(
        switchMap((uploadMetadata) => {
          const targets = files.map((file, index) => ({
            file,
            uploadUrl: uploadMetadata[index].uploadUrl,
          }));

          return this.multimediaService
            .upload(targets)
            .pipe(map(() => uploadMetadata.map((m) => m.publicUrl)));
        }),
        tap((publicUrls) => urls.push(...publicUrls)), // capture public urls
        switchMap((publicUrls) => this.postsService.createPost(publicUrls, this.description, tags)),
      )
      .subscribe({
        error: (err: HttpErrorResponse) => {
          this.notificationsService
            .open(new PolymorpheusComponent(ToastComponent), {
              label: 'Failure.',
              autoClose: 3000,
              size: 'l',
              appearance: 'negative',
              data: { message: 'Try again later' },
            })
            .subscribe();

          this.processing.set(false);
        },
        next: (response: CreatePostResponse) => {
          const user = this.userService.user()!;
          this.postEventsService.emitPostCreated({
            post: {
              id: response.id,
              author: {
                id: user.id,
                name: user.userName,
                avatarUrl: user.avatarUrl,
              },
              description: description,
              commentsCount: 0,
              likesCount: 0,
              likedByUser: false,
              multimediaUrls: urls.map((url) => url.href),
              tags: tags,
              createdAt: new Date(),
            },
          });
        },
        complete: () => {
          this.notificationsService
            .open(new PolymorpheusComponent(ToastComponent), {
              label: 'Success.',
              autoClose: 3000,
              size: 'l',
              appearance: 'positive',
              data: { message: 'Post created successfully' },
            })
            .subscribe();

          this.processing.set(false);
          this.context.completeWith(true);
        },
      });
  }
}
