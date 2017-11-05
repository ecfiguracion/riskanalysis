// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import { router } from './router'
import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.css'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
// import 'material-icons/css/material-icons.css'
//import 'bulma/css/bulma.css'
//import Buefy from 'buefy'
//import 'buefy/lib/buefy.css'
//import Vuetify from 'vuetify'
// import VueBlu from 'vue-blu'
// import 'vue-blu/dist/css/vue-blu.min.css'
//import 'vuetify/dist/vuetify.min.css'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VeeValidate from 'vee-validate'
import { store } from './core/config/store'

Vue.config.productionTip = false
//Vue.use(VueBlu)
//Vue.use(Vuetify)
//Vue.use(Buefy)
Vue.use(VueMaterial)
Vue.use(VueAxios, axios)
Vue.use(VeeValidate)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  template: '<App/>',
  components: { App }
})
