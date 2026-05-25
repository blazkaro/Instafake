import { Component, inject, Input } from '@angular/core';
import { TuiAppBarComponent, TuiAppBarSizeDirective, TuiSlides, TuiCardLarge, TuiElasticContainer, TuiAppBarDirective } from "@taiga-ui/layout";
import { TuiLike, TuiProgressBar, TuiBadgedContentComponent, TuiBadgeNotification, TuiBadge, TuiButtonCopy, TuiAccordionDirective, TuiAvatarLabeled, TuiAvatar, TuiLineClamp, TuiFade } from "@taiga-ui/kit";
import { Post } from '../models/post';
import { TuiDialogContext, TuiGroup, TuiAppearance, TuiButton, TuiIcon, TuiNotificationDirective, TuiLink } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT } from '@taiga-ui/polymorpheus';
import { CompactNumberPipe } from '../../pipes/compact-number-pipe';
import { DatePipe } from '@angular/common';

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
