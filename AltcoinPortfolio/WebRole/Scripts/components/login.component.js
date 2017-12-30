'use strict';

let LoginComponent = Vue.component('login-component', {
    template:`
    <v-card>
    <v-card-title class="headline">Do you have an account?</v-card-title>
    <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" flat @click.native="register()">Register</v-btn>
        <v-btn color="blue darken-1" flat @click.native="login()">Log in</v-btn>
    </v-card-actions>
    </v-card>`,
    props:['dialog'],
    methods: {
        register(){
            this.$eventHub.$emit('closeLoginDialog');
            this.$router.push('/register');
        },
        login (){
            this.$eventHub.$emit('closeLoginDialog');
            this.$router.push('/login');
        }
    }
});