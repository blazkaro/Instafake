import { Component } from '@angular/core';
import { TuiHint, TuiIcon, TuiAppearance, TuiButton } from '@taiga-ui/core';

@Component({
  selector: 'app-signin-component',
  imports: [TuiIcon, TuiHint, TuiButton],
  templateUrl: './signin-component.html',
  styleUrl: './signin-component.scss'
})
export class SigninComponent {}
