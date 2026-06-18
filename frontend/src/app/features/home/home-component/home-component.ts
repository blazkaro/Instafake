import { Component, effect, ElementRef, inject, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiItem } from "@taiga-ui/cdk/directives/item";
import { TuiButton, TuiDialogService, TuiDropdownDirective, TuiDropdownHover, TuiGroup, TuiInput, TuiLoader, TuiTextfieldMultiComponent } from '@taiga-ui/core';
import { TuiAvatar, TuiAvatarOutline, TuiChip, TuiInputChipComponent, TuiInputChipDirective } from '@taiga-ui/kit';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { UserService } from '../../../core/services/user-service';
import { PostOverviewComponent } from '../../../shared/post/post-overview-component/post-overview-component';
import { PostCreatorDialogComponent } from '../../post-creator-dialog/post-creator-dialog-component';
import { PostListService } from './post-list-service';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent, TuiLoader, TuiTextfieldMultiComponent, TuiChip, TuiInputChipComponent, TuiItem, FormsModule, TuiInputChipDirective, TuiDropdownHover,
    TuiDropdownDirective, TuiButton, TuiGroup],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
  providers: [PostListService]
})
export class HomeComponent {
  private dialogs = inject(TuiDialogService);

  protected searchInput: string[] = [];
  private sentinel = viewChild<ElementRef<HTMLElement>>('sentinel');

  protected readonly userService = inject(UserService);
  protected readonly postListService = inject(PostListService);

  constructor() {
    effect(() => {
      this.postListService.observeSentinel(this.sentinel());
    });
  }

  onSearchInputChange(newValue: string[]) {
    this.searchInput = this.postListService.onSearchQueryChange(newValue);
  }

  openPostCreator() {
    this.dialogs.open(new PolymorpheusComponent(PostCreatorDialogComponent), {
      size: 'l'
    }).subscribe();
  }
}
