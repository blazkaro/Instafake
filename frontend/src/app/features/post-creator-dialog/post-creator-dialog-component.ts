import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiAppearance, TuiButton, TuiDialogContext, TuiGroup, TuiIcon, TuiInputDirective, TuiLink, TuiNotificationService, TuiTextfieldComponent, TuiTextfieldMultiComponent } from "@taiga-ui/core";
import { TuiAvatar, TuiBadge, TuiButtonLoading, TuiFile, TuiFiles, TuiInputChipComponent, TuiInputChipDirective, TuiInputFiles, TuiTextareaComponent, TuiToastService } from "@taiga-ui/kit";
import { POLYMORPHEUS_CONTEXT, PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { map, switchMap } from 'rxjs';
import { MultimediaPurpose } from '../../shared/multimedia/multimedia-purpose';
import { MultimediaService } from '../../shared/multimedia/multimedia-service';
import { FilePreviewPipe } from "../../shared/pipes/file-preview-pipe";
import { PostsService } from '../../shared/post/services/posts-service';
import { ToastComponent } from '../../shared/toasts/error-toast-component/toast-component';

@Component({
  selector: 'app-post-creator-dialog-component',
  imports: [TuiInputFiles, TuiAvatar, TuiIcon, TuiButtonLoading, TuiLink, TuiFiles, FormsModule, TuiFile, FilePreviewPipe, TuiTextfieldComponent, TuiTextfieldMultiComponent, TuiInputChipComponent, TuiInputChipDirective, TuiInputDirective, TuiTextareaComponent, TuiButton, TuiAppearance, TuiGroup, TuiBadge],
  templateUrl: './post-creator-dialog-component.html',
  styleUrl: './post-creator-dialog-component.scss',
})
export class PostCreatorDialogComponent {
  private readonly context = inject<TuiDialogContext<boolean, void>>(POLYMORPHEUS_CONTEXT);
  private readonly notificationsService = inject(TuiNotificationService);
  private readonly multimediaService = inject(MultimediaService);
  private readonly postsService = inject(PostsService);

  files = signal<File[]>([]);
  tags = signal<string[]>([]);
  description: string = "";

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

    const files = this.files();
    const tags = this.tags().map(tag => tag.substring(1));

    this.multimediaService.createUploadUrls(MultimediaPurpose.Post, files).pipe(
      switchMap(uploadMetadata => {
        const targets = files.map((file, index) => ({
          file,
          uploadUrl: uploadMetadata[index].uploadUrl
        }));

        return this.multimediaService.upload(targets).pipe(
          map(() => uploadMetadata.map(m => m.publicUrl))
        );
      }),
      switchMap(publicUrls =>
        this.postsService.createPost(publicUrls, this.description, tags)
      )
    ).subscribe({
      error: (err: HttpErrorResponse) => {
        this.notificationsService.open(new PolymorpheusComponent(ToastComponent), {
          label: 'Failure.',
          autoClose: 3000,
          size: 'l',
          appearance: 'negative',
          data: { message: 'Try again later' }
        }).subscribe();

        this.processing.set(false);
      },
      complete: () => {
        this.notificationsService.open(new PolymorpheusComponent(ToastComponent), {
          label: 'Success.',
          autoClose: 3000,
          size: 'l',
          appearance: 'positive',
          data: { message: 'Post created successfully' }
        }).subscribe();

        this.processing.set(false);
        this.context.completeWith(false);
      }
    });
  }
}
