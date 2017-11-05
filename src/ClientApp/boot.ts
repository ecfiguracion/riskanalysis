import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/counter', component: require('./components/counter/counter.vue.html') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue.html') },
    { path: '/login', component: require('./components/login/login.vue.html') },
    {
        path: '/backend', component: require('./components/backend/main/main.vue.html'),
        children: [
            { path: '/assessments', component: require('./components/backend/assessments/assessments.vue.html') },
            { path: '/barangays', component: require('./components/backend/barangays/index.vue.html') },
            { path: '/barangays/:id', component: require('./components/backend/barangays/form.vue.html') },
            { path: '/categories', component: require('./components/backend/category/index.vue.html') },
            { path: '/categories/:id', component: require('./components/backend/category/form.vue.html') },
            { path: '/lookups', component: require('./components/backend/lookups/index.vue.html') },
            { path: '/lookups/:id', component: require('./components/backend/lookups/form.vue.html') }
        ]
    }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});
