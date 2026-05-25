import { Component, inject, Input, OnInit } from '@angular/core';
import { TuiCardLarge, TuiHeader, TuiCardMedium } from '@taiga-ui/layout';
import { TuiIcon, TuiTitle, TuiAppearance, TuiNotificationService, TuiDialogService } from "@taiga-ui/core";
import { Post } from './models/post';
import { TuiAvatar, TuiBadge } from '@taiga-ui/kit';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { switchMap } from 'rxjs';
import { PostDialogComponent } from './post-dialog-component/post-dialog-component';

@Component({
  selector: 'app-post-component',
  imports: [TuiCardLarge, TuiHeader, TuiTitle, TuiBadge, TuiAvatar, TuiAppearance],
  templateUrl: './post-overview-component.html',
  styleUrl: './post-overview-component.scss',
})
export class PostOverviewComponent {
  @Input()
  post!: Post;

  private readonly dialogs = inject(TuiDialogService);

  openPost() {
    this.dialogs.open(new PolymorpheusComponent(PostDialogComponent), {
      size: 'l',
      data: this.post
    }).subscribe()
  }
}
