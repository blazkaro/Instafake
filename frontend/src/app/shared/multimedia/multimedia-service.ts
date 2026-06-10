import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { forkJoin, map, Observable } from 'rxjs';
import { ApiPaths } from '../api-config';
import { MultimediaPurpose } from './multimedia-purpose';

interface FileMetadata {
  extension: string;
  contentType: string;
  sizeBytes: number;
}

export interface UploadMetadata {
  uploadUrl: URL;
  publicUrl: URL;
}

@Injectable({
  providedIn: 'root',
})
export class MultimediaService {
  private readonly http = inject(HttpClient);

  createUploadUrls(purpose: MultimediaPurpose, files: File[]): Observable<UploadMetadata[]> {
    const path = (() => {
      switch (purpose) {
        case MultimediaPurpose.Post: return "posts"
        default: throw new Error("Multimedia type not supported")
      }
    })();

    const body: FileMetadata[] = files.map<FileMetadata>((file) => ({
      extension: file.name.split('.').pop()!,
      contentType: file.type,
      sizeBytes: file.size
    }));

    return this.http.post<UploadMetadata[]>(`api${ApiPaths.Multimedia}/${path}`, body, { withCredentials: true }).pipe(
      map(result => result.map<UploadMetadata>(metadata => ({ uploadUrl: URL.parse(metadata.uploadUrl)!, publicUrl: URL.parse(metadata.publicUrl)! })))
    );
  }

  upload(targets: { file: File, uploadUrl: URL }[]): Observable<{}[]> {
    console.log(targets);
    const uploads = targets.map(target => {
      const headers = new HttpHeaders({ 'Content-Type': target.file.type });
      return this.http.put<{}>(target.uploadUrl.href, target.file, { headers: headers });
    });

    return forkJoin(uploads);
  }
}
