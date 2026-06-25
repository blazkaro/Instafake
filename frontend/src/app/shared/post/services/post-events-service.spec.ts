import { TestBed } from '@angular/core/testing';

import { PostEventsService } from './post-events-service';

describe('PostEventsService', () => {
  let service: PostEventsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostEventsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
