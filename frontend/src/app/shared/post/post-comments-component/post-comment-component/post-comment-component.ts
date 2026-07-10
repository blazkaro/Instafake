import { DatePipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { TuiAvatar, TuiAvatarLabeled } from '@taiga-ui/kit';
import { PostComment } from '../../models/post-comment';

@Component({
  selector: 'app-post-comment-component',
  imports: [TuiAvatarLabeled, TuiAvatar, DatePipe],
  templateUrl: './post-comment-component.html',
  styleUrl: './post-comment-component.scss',
})
export class PostCommentComponent {
  comment = input.required<PostComment>();
}
