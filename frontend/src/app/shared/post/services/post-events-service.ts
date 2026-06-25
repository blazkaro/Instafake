import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { PostComment } from '../models/post-comment';
import { Post } from '../models/post';

export interface CommentAddedEvent {
  postId: string;
  comment: PostComment;
}

export interface PostCreatedEvent {
  post: Post;
}

@Injectable({
  providedIn: 'root',
})
export class PostEventsService {
  private _commentAdded = new Subject<CommentAddedEvent>();
  public commentAdded$ = this._commentAdded.asObservable();

  private _postCreated = new Subject<PostCreatedEvent>();
  public postCreated$ = this._postCreated.asObservable();

  emitCommentAdded(event: CommentAddedEvent) {
    this._commentAdded.next(event);
  }

  emitPostCreated(event: PostCreatedEvent) {
    this._postCreated.next(event);
  }
}
