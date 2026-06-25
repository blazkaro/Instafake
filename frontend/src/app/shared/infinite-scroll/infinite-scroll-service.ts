import { DestroyRef, ElementRef, inject, Injectable } from '@angular/core';

@Injectable()
export class InfiniteScrollService {
  private observer!: IntersectionObserver;
  private destroyRef = inject(DestroyRef);

  /**
 * Applies infinite scroll with sentinel pattern. Has to be called AFTER sentinel initialization
 * @param sentinel the sentinel to observe
 */
  observeSentinel(sentinel: ElementRef<HTMLElement> | undefined, loadMoreCb: () => void, options?: IntersectionObserverInit) {
    if (!sentinel)
      return;

    this.observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting) {
        loadMoreCb();
      }
    }, {
      root: null, // uses viewport,
      rootMargin: '200px', // trigger before user reaches end,
      threshold: 0,
      ...options
    });

    this.observer.observe(sentinel.nativeElement);
    this.destroyRef.onDestroy(() => this.observer.disconnect());
  }
}
