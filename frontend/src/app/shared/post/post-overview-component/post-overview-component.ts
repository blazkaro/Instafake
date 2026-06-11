import { Component, inject, Input } from '@angular/core';
import { TuiAppearance, TuiDialogService, TuiTitle } from "@taiga-ui/core";
import { TuiAvatar, TuiBadge } from '@taiga-ui/kit';
import { TuiCardLarge, TuiHeader } from '@taiga-ui/layout';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { Post } from '../models/post';
import { PostDialogComponent } from '../post-dialog-component/post-dialog-component';

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
      data: this.post,
      size: 'l'
    }).subscribe()
  }
}
