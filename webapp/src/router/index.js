import Vue from 'vue'
import Router from 'vue-router'
import Main from '@/components/Main'
import Backend from '@/components/Backend'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Main',
      component: Main
    },
    {
      path: '/backend',
      name: 'Backend',
      component: Backend
    }
  ]
})
