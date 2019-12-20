import Vue from 'vue';
import VueRouter from 'vue-router';
import App from './App';
import routes from './routes';
import BootstrapVue from 'bootstrap-vue';
import VueSidebarMenu from 'vue-sidebar-menu';
import axios from 'axios';
import VueAxios from 'vue-axios';

const router = new VueRouter({
    routes,
    linkActiveClass: 'active',
    mode: 'history'
});

Vue.use(VueAxios, axios);
Vue.use(VueRouter);
Vue.use(BootstrapVue);
Vue.use(VueSidebarMenu);

Vue.axios.defaults.baseURL = 'https://localhost:44325/api';

import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import 'vue-sidebar-menu/dist/vue-sidebar-menu.css';

new Vue({
    el: '#app',
    render: h => h(App),
    router
});