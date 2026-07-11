import { Component, effect, ElementRef, inject, signal, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiItem } from "@taiga-ui/cdk/directives/item";
import { TuiButton, TuiDialogService, TuiDropdownDirective, TuiDropdownHover, TuiGroup, TuiInput, TuiLoader, TuiTextfieldMultiComponent } from '@taiga-ui/core';
import { TuiAvatar, TuiAvatarOutline, TuiChip, TuiInputChipComponent, TuiInputChipDirective } from '@taiga-ui/kit';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { UserService } from '../../../core/services/user-service';
import { InfiniteScrollService } from '../../../shared/infinite-scroll/infinite-scroll-service';
import { PostOverviewComponent } from '../../../shared/post/post-overview-component/post-overview-component';
import { PostCreatorDialogComponent } from '../../post-creator-dialog/post-creator-dialog-component';
import { PostListService } from '../services/post-list-service';
import { Profile, ProfilesService } from '../services/profiles-service';
import { CompactNumberPipe } from "../../../shared/pipes/compact-number-pipe";
import { catchError, debounceTime, Observable, of, Subject, switchMap } from 'rxjs';
import { rxResource, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FcmService } from '../../../core/services/fcm-service';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent, TuiLoader, TuiTextfieldMultiComponent, TuiChip, TuiInputChipComponent, TuiItem, FormsModule, TuiInputChipDirective, TuiDropdownHover,
    TuiDropdownDirective, TuiButton, TuiGroup, CompactNumberPipe],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
  providers: [PostListService, InfiniteScrollService]
})
export class HomeComponent {
  private readonly dialogs = inject(TuiDialogService);
  private readonly infiniteScrollService = inject(InfiniteScrollService);
  private readonly profileService = inject(ProfilesService);
  private readonly fcmService = inject(FcmService);

  protected searchInput: string[] = [];
  private sentinel = viewChild<ElementRef<HTMLElement>>('sentinel');

  protected readonly userService = inject(UserService);
  protected readonly postListService = inject(PostListService);

  private toggleFollow$ = new Subject<{ profileId: string; isFollowing: boolean }>();

  constructor() {
    effect(() => {
      this.infiniteScrollService.observeSentinel(this.sentinel(), () => this.postListService.loadMore());
    });
  }

  private toggleFollowSub = this.toggleFollow$.pipe(
    debounceTime(1000),
    switchMap(({ profileId, isFollowing }) => {
      if (isFollowing) {
        return this.profileService.unfollow(profileId);
      } else {
        return this.profileService.follow(profileId);
      }
    }),
    takeUntilDestroyed()
  ).subscribe();

  private _profileName = signal<string | null>(null);

  protected profileResource = rxResource({
    params: () => ({
      profileName: this._profileName()
    }),
    stream: ({ params }) => {
      if (params.profileName) {
        return this.profileService.getProfile(params.profileName.substring(1))
      }

      return of(null);
    }
  })

  protected profile = this.profileResource.value.asReadonly();

  onSearchInputChange(newValue: string[]) {
    this.searchInput = this.postListService.onSearchQueryChange(newValue);

    const newProfileName = this.searchInput.find(val => val.startsWith('@'));
    if (newProfileName === undefined) {
      this._profileName.set(null);
      return;
    }

    if (this._profileName() != newProfileName) {
      this._profileName.set(newProfileName);
    }
  }

  openPostCreator() {
    this.dialogs.open<boolean>(new PolymorpheusComponent(PostCreatorDialogComponent), {
      size: 'l'
    }).subscribe(created => {
      if (created) {
        this.onSearchInputChange([`@${this.userService.user()?.userName}`]);
      }
    });
  }


  follow() {
    const profileId = this.profile()?.userId;
    if (!profileId) {
      return;
    }

    this.toggleFollow$.next({ profileId, isFollowing: this.profile()!.followedByUser });

    // update locally (optimistic, may be reverted in event handler), do not reload resource
    if (this.profile()?.followedByUser) {
      this.profileResource.update((val) => ({ ...val!, followedByUser: !val!.followedByUser, followersCount: val!.followersCount - 1 }));
    } else {
      this.profileResource.update((val) => ({ ...val!, followedByUser: !val!.followedByUser, followersCount: val!.followersCount + 1 }));
      this.fcmService.askForNotifications();
    }
  }
}
