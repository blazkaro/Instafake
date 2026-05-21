import { Component, inject } from '@angular/core';
import { TuiHint, TuiIcon, TuiButton } from '@taiga-ui/core';
import { apiConfig, ApiPaths } from '../../shared/api-config';
import { Router } from '@angular/router';

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
