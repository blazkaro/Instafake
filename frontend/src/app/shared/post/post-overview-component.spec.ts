import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostOverviewComponent } from './post-overview-component';

describe('PostOverviewComponent', () => {
  let component: PostOverviewComponent;
  let fixture: ComponentFixture<PostOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostOverviewComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PostOverviewComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
