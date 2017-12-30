'use strict';

let FooterComponent = Vue.component('footer-component',{
    template:`<v-footer class="blue darken-2" absolute bottom>
    <v-layout row wrap align-center>
      <v-flex xs12>
        <div class="white--text ml-3">
          Made with <a class="white--text" href="https://vuetifyjs.com" target="_blank">Vuetify</a>
        </div>
      </v-flex>
    </v-layout>
  </v-footer>`
});