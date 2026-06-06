import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostCreatorDialogComponent } from './post-creator-dialog-component';

describe('PostCreatorDialogComponent', () => {
  let component: PostCreatorDialogComponent;
  let fixture: ComponentFixture<PostCreatorDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostCreatorDialogComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PostCreatorDialogComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
