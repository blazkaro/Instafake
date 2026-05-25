import { inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'compactNumber',
})
export class CompactNumberPipe implements PipeTransform {
  private localeId = inject(LOCALE_ID);

  transform(value: number, ...args: unknown[]): string {
    if (value === null || value === undefined || isNaN(value)) {
      return '';
    }

    const formatter = new Intl.NumberFormat(this.localeId, {
      notation: 'compact',
      compactDisplay: 'short'
    });

    return formatter.format(value);
  }
}
