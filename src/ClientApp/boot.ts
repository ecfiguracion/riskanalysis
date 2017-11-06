import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
import moment from 'moment';

Vue.use(VueRouter);

Vue.filter('formatDate', function (value: any) {
    if (value) {
        return moment(String(value)).format('MM/DD/YYYY')
    }
});

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
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
            { path: '/lookups/:categoryid/:id', component: require('./components/backend/lookups/form.vue.html') },
            { path: '/typhoons', component: require('./components/backend/typhoons/index.vue.html') },
            { path: '/typhoons/:id', component: require('./components/backend/typhoons/form.vue.html') }
        ]
    }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});
