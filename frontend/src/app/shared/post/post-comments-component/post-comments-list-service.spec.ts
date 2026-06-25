import { TestBed } from '@angular/core/testing';

import { PostCommentsListService } from './post-comments-list-service';

describe('PostCommentsListService', () => {
  let service: PostCommentsListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostCommentsListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
