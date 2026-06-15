import { DatePipe } from '@angular/common';
import { Component, inject, OnDestroy, signal } from '@angular/core';
import { TuiAppearance, TuiButton, TuiDialogContext, TuiGroup, TuiIcon } from '@taiga-ui/core';
import { TuiAvatar, TuiAvatarLabeled } from "@taiga-ui/kit";
import { TuiElasticContainer, TuiSlides } from "@taiga-ui/layout";
import { POLYMORPHEUS_CONTEXT } from '@taiga-ui/polymorpheus';
import { CompactNumberPipe } from '../../pipes/compact-number-pipe';
import { Post } from '../models/post';
import { LikesService } from '../services/likes-service';

@Component({
  selector: 'app-post-dialog-component',
  imports: [TuiSlides, TuiElasticContainer, TuiGroup, TuiAppearance, TuiButton, TuiIcon, CompactNumberPipe, TuiAvatarLabeled, TuiAvatar, DatePipe],
  templateUrl: './post-dialog-component.html',
  styleUrl: './post-dialog-component.scss',
})
export class PostDialogComponent implements OnDestroy {
  private readonly likesService = inject(LikesService);

  private readonly context = inject<TuiDialogContext<boolean, Post>>(POLYMORPHEUS_CONTEXT);
  public readonly post: Post = this.context.data;

  private readonly initLikeStatus = this.post.likedByUser;

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

  ngOnDestroy(): void {
    if (this.initLikeStatus === this.post.likedByUser)
      return;

    (this.post.likedByUser ?
      this.likesService.like(this.post.id) : this.likesService.dislike(this.post.id))
      .subscribe(); // dont handle errors, likes arent that important and explicit notifications AFTER closing post are not very user friendly. 
    // TODO: silent, client side retries OR real time requests with debounce time?
  }
}
