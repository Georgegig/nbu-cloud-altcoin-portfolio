'use strict';

let GettingStartedView = {
    template:`
    <section>
        <v-parallax src="Content/Assets/hero.jpeg" height="300">
            <v-card class="elevation-0 blue">
                <v-card-title primary-title class="layout justify-center white--text">
                    <div class="headline text-xs-center">The best way to monitor your portfolio</div>
                </v-card-title>
                <v-card-text class="layout justify-center white--text">
                    Multifunctional and easy. 
                </v-card-text>
            </v-card>
        </v-parallax>
        <v-layout column wrap class="my-5" align-center>
            <v-flex xs12>
                <v-container grid-list-xl>
                    <v-layout row wrap align-center>
                        <v-flex xs12 md4>
                            <v-card class="elevation-0 transparent">
                            <v-card-text class="text-xs-center">
                                <v-icon x-large class="blue--text text--lighten-2">color_lens</v-icon>
                            </v-card-text>
                            <v-card-title primary-title class="layout justify-center">
                                <div class="headline text-xs-center">Material Design</div>
                            </v-card-title>
                            <v-card-text>
                                Platform based on material design and flex box containers. Responsive and fully customizable. 
                            </v-card-text>
                            </v-card>
                        </v-flex>
                        <v-flex xs12 md4>
                            <v-card class="elevation-0 transparent">
                            <v-card-text class="text-xs-center">
                                <v-icon x-large class="blue--text text--lighten-2">flash_on</v-icon>
                            </v-card-text>
                            <v-card-title primary-title class="layout justify-center">
                                <div class="headline">Easy to use</div>
                            </v-card-title>
                            <v-card-text>
                                Get started under 5 minutes
                            </v-card-text>
                            </v-card>
                        </v-flex>
                        <v-flex xs12 md4>
                            <v-card class="elevation-0 transparent">
                            <v-card-text class="text-xs-center">
                                <v-icon x-large class="blue--text text--lighten-2">build</v-icon>
                            </v-card-text>
                            <v-card-title primary-title class="layout justify-center">
                                <div class="headline text-xs-center">Completely Open Sourced</div>
                            </v-card-title>
                            <v-card-text>
                                Developed using only open source frameworks. Code could be found on <a href="https://github.com/Georgegig/nbu-altcoin-portfolio">Altcoin Portfolio</a> 
                            </v-card-text>
                            </v-card>
                        </v-flex>
                    </v-layout>
                </v-container>
            </v-flex>
        </v-layout>
        <v-parallax src="Content/Assets/section.jpg" height="380">
            <v-layout column align-center justify-center>
                <v-layout column align-center justify-center class="white--text"> 
                    <!-- <v-dialog v-model="dialog" persistent max-width="600">
                        <v-btn color="blue" dark slot="activator">Get started right away</v-btn>
                        <login-component></login-component>
                    </v-dialog> -->
                </v-layout>
                </v-layout>
        </v-parallax>
    </section>
    `,
    data () {
        return {
            dialog: false
        }
    },
    mounted () {
        this.$eventHub.$on('closeLoginDialog', () => {
            this.dialog = false;
        });
    },
};
