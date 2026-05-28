import { DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { TuiAppearance, TuiButton, TuiDialogContext, TuiGroup, TuiIcon } from '@taiga-ui/core';
import { TuiAvatar, TuiAvatarLabeled } from "@taiga-ui/kit";
import { TuiElasticContainer, TuiSlides } from "@taiga-ui/layout";
import { POLYMORPHEUS_CONTEXT } from '@taiga-ui/polymorpheus';
import { CompactNumberPipe } from '../../pipes/compact-number-pipe';
import { Post } from '../models/post';

@Component({
  selector: 'app-post-component',
  imports: [TuiSlides, TuiElasticContainer, TuiGroup, TuiAppearance, TuiButton, TuiIcon, CompactNumberPipe, TuiAvatarLabeled, TuiAvatar, DatePipe],
  templateUrl: './post-dialog-component.html',
  styleUrl: './post-dialog-component.scss',
})
export class PostDialogComponent {
  private readonly context = inject<TuiDialogContext<boolean, Post>>(POLYMORPHEUS_CONTEXT);
  public readonly post: Post = this.context.data;

  multimedia_index = 0;

  toggleLike() {
    this.post.likedByUser = !this.post.likedByUser;
    this.post.likesCount += this.post.likedByUser ? 1 : -1;
  }
}
