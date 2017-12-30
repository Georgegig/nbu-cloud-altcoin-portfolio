'use strict';

let NavigationComponent = Vue.component('nav-component', {
    template: `
    <v-toolbar class="white">                  
        <span v-on:click="homePage()"><h1 style="cursor:pointer;" class="title" v-text="title"></h1></span>
        <v-spacer></v-spacer>
        <v-toolbar-side-icon class="hidden-md-and-up"></v-toolbar-side-icon>
        <span><h1 v-text="loggedUser"></h1></span>
        <v-toolbar-items class="hidden-sm-and-down">
            <v-btn flat v-show="userLoggedIn" @click.native="portfolio()">Portfolio</v-btn>
            <v-btn flat v-show="userLoggedIn" @click.native="logout()">Log out</v-btn>
            <v-btn flat v-show="!userLoggedIn" @click.native="login()">Log in</v-btn>
            <v-btn flat v-show="!userLoggedIn" @click.native="register()">Register</v-btn>
        </v-toolbar-items>
    </v-toolbar>`,
    props:['title'],
    data(){
        return {
            userLoggedIn: false,
            loggedUser: ''
        }
    },
    mounted() {
        this.loginStatus();
        this.$eventHub.$on('loginChange', () => {
            this.loginStatus();
        });
    },
    methods: {
        homePage() {
            this.$router.push('/');
        },
        login(){
            this.$router.push('/login');
        },
        logout(){
            localStorage.removeItem('user');
            this.$eventHub.$emit('loginChange');
            this.$router.push('/');
        },
        register(){
            this.$router.push('/register');
        },
        portfolio() {
            this.$router.push('/portfolio');
        },
        loginStatus(){
            this.userLoggedIn = UsersTable.userLoggedIn();
            if(this.userLoggedIn){
                this.loggedUser = JSON.parse(localStorage.getItem('user')).name;
            }
            else{
                this.loggedUser = '';
            }
        }
    }
});
