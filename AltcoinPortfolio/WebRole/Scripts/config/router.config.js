'use strict';

const routes = [
    { path: '/', name: "GettingStarted", component: GettingStartedView },
    { path: '/login', name: "Login", component: LoginView },
    { path: '/register', name: "Register", component: RegisterView },
    { path: '/portfolio', name: "Portfolio", component: PortfolioView },
    { path: '*', redirect: '/'}
];

const router = new VueRouter({
    routes
});