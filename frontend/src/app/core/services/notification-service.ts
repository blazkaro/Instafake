import { inject, Service } from '@angular/core';
import { TuiNotificationService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { MessagePayload } from 'firebase/messaging';
import { ToastComponent } from '../../shared/toasts/toast-component/toast-component';

@Service()
export class NotificationService {
    private tuiNotificationService = inject(TuiNotificationService);

    handleNotification(payload: MessagePayload) {
        this.tuiNotificationService
            .open(new PolymorpheusComponent(ToastComponent), {
                label: payload.notification?.title,
                autoClose: 5000,
                size: 'l',
                appearance: 'info'
            }).subscribe();
    }
}
