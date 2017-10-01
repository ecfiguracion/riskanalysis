import Vue from 'vue'
import Router from 'vue-router'
import { store } from '../core/config/store'
import auth from '../core/authenticate'

import Main from '@/components/Main'
import Login from '@/components/Login'
import BackEnd from '@/components/backend/BackEnd'
import Assessments from '@/components/backend/Assessments'
import Barangay from '@/components/backend/config/Barangay'
import Agriculture from '@/components/backend/config/Agriculture'
import Lifelines from '@/components/backend/config/Lifelines'
import Structures from '@/components/backend/config/Structures'

Vue.use(Router)

export const router = new Router({
  routes: [
    {
      path: '/',
      name: 'Main',
      component: Main
    },
    {
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/logout',
      name: 'Logout',
      component: Main
    },    
    { path: '/backend',
      component: BackEnd,
      children: [
        { path: 'assessments', component: Assessments },
        { path: 'barangay', component: Barangay },
        { path: 'agriculture', component: Agriculture },
        { path: 'lifelines', component: Lifelines },
        { path: 'structures', component: Structures }
      ]
    },
    {
      path: '*',
      redirect: '/'
    }
  ]
})

router.beforeEach((to,from,next) => 
{
  switch (to.path) {
    case '/login': {
      if (store.state.isLoggedIn)
      {
        next('/backend/assessments')
      } else {
        next()
      }
      break;
    }
    case '/logout': {
      auth.logout()
      next()
      break;
    }
    default: {
      next()      
    }
  }

})