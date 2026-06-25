import { Component, input } from '@angular/core';
import { PostComment } from '../../models/post-comment';
import { TuiAvatar, TuiAvatarLabeled } from "@taiga-ui/kit";
import { ɵɵDir } from "@angular/cdk/scrolling";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-post-comment-component',
  imports: [TuiAvatarLabeled, TuiAvatar, DatePipe],
  templateUrl: './post-comment-component.html',
  styleUrl: './post-comment-component.scss',
})
export class PostCommentComponent {
  comment = input.required<PostComment>();
}
