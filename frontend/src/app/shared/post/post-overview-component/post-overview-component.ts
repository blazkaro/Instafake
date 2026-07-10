import { Component, inject, input, Input } from '@angular/core';
import { TuiAppearance, TuiDialogService, TuiTitle } from '@taiga-ui/core';
import { TuiAvatar, TuiBadge } from '@taiga-ui/kit';
import { TuiCardLarge, TuiHeader } from '@taiga-ui/layout';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { Post } from '../models/post';
import { PostDialogComponent } from '../post-dialog-component/post-dialog-component';

@Component({
  selector: 'app-post-overview-component',
  imports: [TuiCardLarge, TuiHeader, TuiTitle, TuiBadge, TuiAvatar, TuiAppearance],
  templateUrl: './post-overview-component.html',
  styleUrl: './post-overview-component.scss',
})
export class PostOverviewComponent {
  post = input.required<Post>();

  private readonly dialogs = inject(TuiDialogService);

  openPost() {
    this.dialogs
      .open(new PolymorpheusComponent(PostDialogComponent), {
        data: this.post(),
        size: 'l',
      })
      .subscribe();
  }
}
