import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TuiRoot } from '@taiga-ui/core';
import { FcmService } from './core/services/fcm-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TuiRoot],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('Instafake');

  private readonly fcmService = inject(FcmService);

  constructor() {
    this.fcmService.listenToMessages();
  }
}
