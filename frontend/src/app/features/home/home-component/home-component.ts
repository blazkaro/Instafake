import { Component, effect, inject, OnInit, Signal, signal } from '@angular/core';
import { TuiIcon, TuiInput } from '@taiga-ui/core';
import { UserService } from '../../../core/services/user-service';
import { TuiAvatar, TuiAvatarOutline } from '@taiga-ui/kit';
import { PostOverviewComponent } from "../../../shared/post/post-overview-component";
import { Post } from '../../../shared/post/models/post';
import { PostsService } from '../posts-service';
import { rxResource } from '@angular/core/rxjs-interop';
import { distinctUntilKeyChanged, map } from 'rxjs';
import { CursorPagination } from '../../../shared/pagination/cursor-pagination';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
})
export class HomeComponent {
  private readonly PAGE_SIZE: number = 20;

  userService = inject(UserService)
  postsService = inject(PostsService);
  /*
    posts: Array<Post> = [{
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 1149,
      createdAt: new Date(),
      description: "alalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemanoalalalal siemano",
      likedByUser: false,
      likesCount: 9324342,
      multimediaUrls: ['https://wallpapercat.com/w/full/a/8/7/5815535-3840x2160-desktop-hd-4k-wallpaper-image.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg', 'https://plus.unsplash.com/premium_photo-1675826908169-ac0de41a213a?fm=jpg&q=60&w=3000&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8bG9uZyUyMHBhdGh8ZW58MHx8MHx8fDA%3D'],
      tags: ['dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa', 'dupa', 'marmolada', 'glebogryzarka', 'grzymislawa',]
    },
    {
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 9,
      createdAt: new Date(),
      description: "alalalal siemano",
      likedByUser: false,
      likesCount: 40,
      multimediaUrls: ['https://fwcdn.pl/cpo/11/41/1141/361_2.4.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg'],
      tags: ['dupa']
    },
    {
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 9,
      createdAt: new Date(),
      description: "alalalal siemano",
      likedByUser: false,
      likesCount: 40,
      multimediaUrls: ['https://fwcdn.pl/cpo/11/41/1141/361_2.4.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg'],
      tags: ['dupa']
    }, {
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 9,
      createdAt: new Date(),
      description: "alalalal siemano",
      likedByUser: false,
      likesCount: 40,
      multimediaUrls: ['https://fwcdn.pl/cpo/11/41/1141/361_2.4.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg'],
      tags: ['dupa']
    }, {
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 9,
      createdAt: new Date(),
      description: "alalalal siemano",
      likedByUser: false,
      likesCount: 40,
      multimediaUrls: ['https://fwcdn.pl/cpo/11/41/1141/361_2.4.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg'],
      tags: ['dupa']
    }, {
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 9,
      createdAt: new Date(),
      description: "alalalal siemano",
      likedByUser: false,
      likesCount: 40,
      multimediaUrls: ['https://fwcdn.pl/cpo/11/41/1141/361_2.4.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg'],
      tags: ['dupa']
    }, {
      id: "asdsa",
      author: { id: "dsa", avatarUrl: "https://avatars.githubusercontent.com/u/108865369?v:4", name: "blazkaro" },
      commentsCount: 9,
      createdAt: new Date(),
      description: "alalalal siemano",
      likedByUser: false,
      likesCount: 40,
      multimediaUrls: ['https://fwcdn.pl/cpo/11/41/1141/361_2.4.jpg', 'https://www.filmy-animowane.pl/wp-content/uploads/2022/03/shrek-postac.jpg'],
      tags: ['dupa']
    }
    ]
    */

  tags = signal<string[]>([]);
  userName = signal<string | null>(null);
  cursor = signal<CursorPagination | null>(null);

  posts = signal<Post[]>([]);
  nextCursor: CursorPagination | null = null;

  postsResource = rxResource({
    params: () => ({
      tags: this.tags(),
      authorId: this.userName(),
      cursor: this.cursor()
    }),
    stream: ({ params }) => this.postsService.getPosts(params.tags, params.authorId, params.cursor).pipe(
      map((response) => ({ response, params }))
    )
  });

  constructor() {
    effect(() => {
      const resourceState = this.postsResource.value();
      if (!resourceState) return;

      const { response, params } = resourceState;
      this.nextCursor = response.nextCursor;

      if (params.cursor === null) {
        // Tags or username changed (or initial load) -> Clear and set fresh posts
        this.posts.set(response.posts);
      } else {
        // Only cursor changed -> append new posts
        this.posts.update((current) => [...current, ...response.posts]);
      }
    });
  }

  loadMorePosts() {
    this.cursor.set(this.nextCursor);
  }
}
