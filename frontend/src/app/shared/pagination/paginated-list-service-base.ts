import { effect, ResourceRef, Signal, signal } from "@angular/core";
import { rxResource, RxResourceOptions } from "@angular/core/rxjs-interop";
import { map } from "rxjs";
import { CursorPagination, PaginatedResponse } from "./cursor-pagination";

interface RxResourcePaginatedResponseParams {
  cursor: CursorPagination | null;
}

export abstract class PaginatedListServiceBase<TData, TParams> {
  private _items = signal<TData[]>([]);
  public items = this._items.asReadonly();

  private _cursor = signal<CursorPagination | null>(null);
  private _nextCursor: CursorPagination | null = null;
  protected cursor = this._cursor.asReadonly();

  private _resource: ResourceRef<{ response: PaginatedResponse<TData>, params: TParams & RxResourcePaginatedResponseParams } | undefined>;
  public isLoading: Signal<boolean>;

  constructor(resourceOptions: RxResourceOptions<PaginatedResponse<TData>, TParams>) {
    const { stream, params, equal, defaultValue, injector } = resourceOptions;

    const opts: RxResourceOptions<{ response: PaginatedResponse<TData>, params: TParams & RxResourcePaginatedResponseParams },
      TParams & RxResourcePaginatedResponseParams> = {
      stream: (ctx) => stream(ctx).pipe(
        map(response => ({ response, params: ctx.params })) // propagate params locally to avoid mixing up values
      ),
      injector,
      ...(params && { params: () => ({ ...params(), cursor: this._cursor() }) }),
      ...(equal && { equal: (a, b) => equal(a.response, b.response) }),
      ...(defaultValue && {
        defaultValue: {
          response: defaultValue,
          params: { cursor: null } as TParams & RxResourcePaginatedResponseParams,
        },
      }),
    };

    this._resource = rxResource(opts);
    this.isLoading = this._resource.isLoading;
    effect(() => this.handleResourceLoad());
  }

  /**
   * Loads more elements
   * @param checked If true, load is performed only if resource is not currently in loading state and there are more elements to load
   */
  public loadMore(checked: boolean = true): void {
    if (!checked || (!this.isLoading() && this.hasMore())) {
      this._cursor.set(this._nextCursor);
    }
  }

  public hasMore(): boolean {
    return this._nextCursor?.lastItemCreatedAt != undefined;
  }

  protected resetPagination(): void {
    this._nextCursor = null;
    this._cursor.set(null);
  }

  /**
   * Use only to satisfy UI needs (real-time addition of just created items)
   * @param item Item to prepend
   */
  protected prependItem(item: TData) {
    this._items.update((current) => [item, ...current]);
  }

  private handleResourceLoad(): void {
    const resourceState = this._resource.value();
    if (!resourceState) return;

    const { response, params } = resourceState;
    this._nextCursor = response.pagination.nextCursor;

    if (params.cursor === null) {
      // Params changed (or initial load) -> Clear and set fresh items
      this._items.set(response.items);
    } else {
      // Only cursor changed -> append new items
      this._items.update((current) => [...current, ...response.items]);
    }

    this.onResourceLoaded(response, params);
  }

  protected onResourceLoaded(response: PaginatedResponse<TData>, params: TParams & RxResourcePaginatedResponseParams) {
    // Optional implementation for subclasses
  }
}
