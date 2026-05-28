import { Component } from '@angular/core';
import { TuiButton, TuiHint, TuiIcon } from '@taiga-ui/core';
import { apiConfig, ApiPaths } from '../../shared/api-config';

@Component({
  selector: 'app-signin-component',
  imports: [TuiIcon, TuiHint, TuiButton],
  templateUrl: './signin-component.html',
  styleUrl: './signin-component.scss'
})
export class SigninComponent {
  redirectToIdentityProvider() {
    window.location.href = `${apiConfig.baseUrl.replace('/api', '')}${ApiPaths.Auth}/signin`
  }
}
