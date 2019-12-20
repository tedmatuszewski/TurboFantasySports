import HomeComponent from '@/components/HomeComponent';
import UsersComponent from '@/components/UsersComponent';
import LoginComponent from '@/components/LoginComponent';

const routes = [
    {
        path: '/',
        name: 'Home',
        component: HomeComponent,
        meta: {
            requiresAuthentication: true
        }
    },
    {
        path: '/users',
        name: 'Users',
        component: UsersComponent,
        meta: {
            requiresAuthentication: true
        }
    },
    {
        path: '/login',
        name: 'Login',
        component: LoginComponent
    }
];

export default routes;