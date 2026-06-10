import { Component } from '@angular/core';
import { TuiPortalContext } from '@taiga-ui/cdk/portals';
import { TuiNotificationOptions } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';
import { ToastData } from '../toast-data';

@Component({
  selector: 'app-error-toast-component',
  imports: [],
  templateUrl: './toast-component.html',
  styleUrl: './toast-component.scss',
})
export class ToastComponent {
  private readonly context = injectContext<TuiPortalContext<TuiNotificationOptions<ToastData>>>();
  toast: ToastData = this.context.data;
}
