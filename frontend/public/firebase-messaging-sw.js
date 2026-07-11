importScripts('https://www.gstatic.com/firebasejs/12.16.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/12.16.0/firebase-messaging-compat.js');

firebase.initializeApp({
    apiKey: "AIzaSyA0oAN5K8aChzju90lAS_huuLuwPVpripM",
    authDomain: "instafake-notifications.firebaseapp.com",
    projectId: "instafake-notifications",
    storageBucket: "instafake-notifications.firebasestorage.app",
    messagingSenderId: "302581154160",
    appId: "1:302581154160:web:58d5de7327b3773f5d8fb5"
});

const messaging = firebase.messaging();
