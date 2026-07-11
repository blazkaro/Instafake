import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { getMessaging, onMessage, onRegistered, register } from 'firebase/messaging';
import { apiConfig, ApiPaths } from '../../shared/api-config';
import { NotificationService } from './notification-service';

const firebaseConfig = {
    apiKey: "AIzaSyA0oAN5K8aChzju90lAS_huuLuwPVpripM",
    authDomain: "instafake-notifications.firebaseapp.com",
    projectId: "instafake-notifications",
    storageBucket: "instafake-notifications.firebasestorage.app",
    messagingSenderId: "302581154160",
    appId: "1:302581154160:web:58d5de7327b3773f5d8fb5"
};

@Service()
export class FcmService {
    private app = initializeApp(firebaseConfig);
    private messaging = getMessaging(this.app);
    private http = inject(HttpClient);
    private notificationService = inject(NotificationService);

    constructor() {
        // Listen for the Installation ID (FID) (old device token) when registration completes
        onRegistered(this.messaging, (installationId) => {
            this.http.post(`${apiConfig.baseUrl}${ApiPaths.Notifications}/devices`, { deviceToken: installationId }, { withCredentials: true }).subscribe();
            this.listenToMessages();
        });

        if (Notification.permission === 'granted') {
            this.askForNotifications(); // registers silently without asking on startup, permission already granted
        }
    }

    async askForNotifications() {
        try {
            const permission = await Notification.requestPermission();
            if (permission === 'granted') {
                await register(this.messaging, {
                    vapidKey: 'BA3vI7q8YPerZQWfH6JMnPgFhhYthcpcVs4DWkKUjEfwtBiXlOsNbaLHI-axkncNS1ACw9TdOq_ycygIFiFDj1U'
                });
            }
        } catch (err) {
            console.error('Error registering with FCM:', err);
        }
    }

    listenToMessages() {
        if (Notification.permission === 'granted') {
            onMessage(this.messaging, (payload) => this.notificationService.handleNotification(payload));
        }
    }
}
