import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { TuiAppearance, TuiButton, TuiDialogContext, TuiGroup, TuiIcon } from '@taiga-ui/core';
import { TuiAvatar, TuiAvatarLabeled } from "@taiga-ui/kit";
import { TuiElasticContainer, TuiSlides } from "@taiga-ui/layout";
import { POLYMORPHEUS_CONTEXT } from '@taiga-ui/polymorpheus';
import { CompactNumberPipe } from '../../pipes/compact-number-pipe';
import { Post } from '../models/post';

@Component({
  selector: 'app-post-dialog-component',
  imports: [TuiSlides, TuiElasticContainer, TuiGroup, TuiAppearance, TuiButton, TuiIcon, CompactNumberPipe, TuiAvatarLabeled, TuiAvatar, DatePipe],
  templateUrl: './post-dialog-component.html',
  styleUrl: './post-dialog-component.scss',
})
export class PostDialogComponent {
  private readonly context = inject<TuiDialogContext<boolean, Post>>(POLYMORPHEUS_CONTEXT);
  public readonly post: Post = this.context.data;

  multimedia_index = signal<number>(0);

  toggleLike() {
    this.post.likedByUser = !this.post.likedByUser;
    this.post.likesCount += this.post.likedByUser ? 1 : -1;
  }

  prev() {
    if (this.multimedia_index() - 1 < 0)
      return;

    this.multimedia_index.update(cur => cur - 1);
  }

  next() {
    if (this.multimedia_index() + 1 > this.post.multimediaUrls.length)
      return;

    this.multimedia_index.update(cur => cur + 1);
  }
}
