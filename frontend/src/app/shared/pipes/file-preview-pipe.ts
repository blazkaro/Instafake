import { inject, Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Pipe({
  name: 'filePreview'
})
export class FilePreviewPipe implements PipeTransform {
  private sanitizer = inject(DomSanitizer);
  private cache = new WeakMap<File, SafeUrl>();

  transform(file: File | null | undefined): SafeUrl | string {
    if (!file) return '';

    if (this.cache.has(file)) {
      return this.cache.get(file)!;
    }

    const objectUrl = URL.createObjectURL(file);
    const safeUrl = this.sanitizer.bypassSecurityTrustUrl(objectUrl);
    this.cache.set(file, safeUrl);

    return safeUrl;
  }
}
