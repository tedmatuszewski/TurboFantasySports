<template>
    <div>
        <b-navbar toggleable="lg">
            <b-navbar-brand href="#">NavBar</b-navbar-brand>
            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav class="ml-auto">
                    <b-nav-item-dropdown :text="name" right>
                        <b-dropdown-item href="#">EN</b-dropdown-item>
                        <b-dropdown-item href="#">ES</b-dropdown-item>
                        <b-dropdown-item href="#">RU</b-dropdown-item>
                        <b-dropdown-item href="#">FA</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>

        <main class="d-flex">
            <aside>
                <sidebar-menu :menu="menu" :relative="true" theme='white-theme' />
            </aside>

            <content class="container-fluid">
                <router-view />
            </content>
        </main>
    </div>
</template>

<script>
    import { AuthenticationContext } from 'vue-adal';

    export default {
        mounted() {
            // const profile = AuthenticationContext.user.profile;

            // this.name = profile.name;
        },
        data() {
            return {
                name: "",
                menu: [
                    {
                        href: '/',
                        title: 'Home',
                        icon: 'fa fa-user'
                    },
                    {
                        href: '/users',
                        title: 'Users',
                        icon: 'fa fa-chart-area'
                    },
                    {
                        href: '/login',
                        title: 'Login',
                        icon: 'fa fa-sign-on'
                    }
                ]
            }
        }
    }
</script>

<style>
    .navbar {
        background-color: #dc0202;
    }

    .navbar a:not(.dropdown-item) {
        color: white !important;
        font-size: .85rem;
        line-height: 1.8em;
    }

    .v-sidebar-menu {
        height: 94vh!important;
        background-color: #f8f9fa !important;
        border-right:1px solid #dee2e6 !important;
        width:225px!important;
    }

    aside {
        min-height: 94vh;
    }

    content {
        min-width: 0;
        width: calc(100% - 300px);
        height:94vh;
        overflow: auto;
    }

    aside, content {
        display:inline-flex !important;
    }

    .v-sidebar-menu .vsm--link_level-1 .vsm--icon {
        background-color: #d6dce1!important;
    }

    .v-sidebar-menu.vsm_white-theme .vsm--link_level-1.vsm--link_exact-active .vsm--icon, .v-sidebar-menu.vsm_white-theme .vsm--link_level-1.vsm--link_active .vsm--icon {
        color: #fff!important;
        background-color: #262626!important;
    }
</style>