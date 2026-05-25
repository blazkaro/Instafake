import { Component, inject, OnInit } from '@angular/core';
import { TuiIcon, TuiInput } from '@taiga-ui/core';
import { UserService } from '../../../core/services/user-service';
import { TuiAvatar, TuiAvatarOutline } from '@taiga-ui/kit';
import { PostOverviewComponent } from "../../../shared/post/post-overview-component";
import { Post } from '../../../shared/post/models/post';

@Component({
  selector: 'app-home-component',
  imports: [TuiInput, TuiAvatar, TuiAvatarOutline, PostOverviewComponent],
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
})
export class HomeComponent {
  userService = inject(UserService)

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
}
