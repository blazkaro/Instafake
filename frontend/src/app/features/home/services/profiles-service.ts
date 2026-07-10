import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiConfig, ApiPaths } from '../../../shared/api-config';

export interface Profile {
  userId: string;
  userName: string;
  avatarUrl: string;
  postsCount: number;
  followersCount: number;
  description: string;
  followedByUser: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class ProfilesService {
  private readonly http = inject(HttpClient);

  getProfile(profileName: string): Observable<Profile> {
    return this.http.get<Profile>(`${apiConfig.baseUrl}${ApiPaths.Profiles}/${profileName}`, { withCredentials: true });
  }

  follow(profileId: string): Observable<{}> {
    return this.updateFollowStatus(profileId, true);
  }

  unfollow(profileId: string): Observable<{}> {
    return this.updateFollowStatus(profileId, false);
  }

  private updateFollowStatus(profileId: string, follow: boolean): Observable<{}> {
    return this.http.put<{}>(`${apiConfig.baseUrl}${ApiPaths.Follows}`, { profileId, follow }, { withCredentials: true });
  }
}
