<template>
  <div class="nav-container">
    <nav class="navbar navbar-expand-md navbar-light bg-light">
      <div class="container">
        <div class="navbar-brand logo"></div>
        <button
          class="navbar-toggler"
          type="button"
          data-toggle="collapse"
          data-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav mr-auto">
            <li class="nav-item">
              <router-link to="/" class="nav-link">Home</router-link>
            </li>
            <li class="nav-item">
              <a href="https://live.supermotocross.com/" class="nav-link">Live Timing</a>
            </li>
            <li class="nav-item">
              <a href="https://cash.app/$tmaski45/" class="nav-link">Buy me a Beer!</a>
            </li>
            <li class="nav-item">
              <router-link :to="{ name: 'privacy' } " class="nav-link">Privacy</router-link>
            </li>
            <li class="nav-item">
              <router-link :to="{ name: 'contact' } " class="nav-link">Contact</router-link>
            </li>
          </ul>
          <ul class="navbar-nav d-none d-md-block">
            <li v-if="!isAuthenticated && !isLoading" class="nav-item">
              <button
                id="qsLoginBtn"
                class="btn btn-primary btn-margin"
                @click.prevent="login"
              >Login</button>
            </li>

            <li class="nav-item dropdown" v-if="isAuthenticated">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="profileDropDown"
                data-toggle="dropdown"
              >
                <img
                  :src="user.picture"
                  alt="User's profile picture"
                  class="nav-user-profile rounded-circle"
                  width="50"
                />
              </a>
              <div class="dropdown-menu dropdown-menu-right">
                <div class="dropdown-header">{{ user.name }}</div>
                <router-link to="/profile" class="dropdown-item dropdown-profile">
                  <i class="mr-3 fa fa-user"></i> Profile
                </router-link>
                <a id="qsLogoutBtn" href="#" class="dropdown-item" @click.prevent="logout">
                  <i class="mr-3 fa fa-power-off"></i> Log out
                </a>
              </div>
            </li>
          </ul>

          <ul class="navbar-nav d-md-none" v-if="!isAuthenticated && !isLoading">
            <button id="qsLoginBtn" class="btn btn-primary btn-block" @click="login">Log in</button>
          </ul>

          <ul
            id="mobileAuthNavBar"
            class="navbar-nav d-md-none d-flex"
            v-if="isAuthenticated"
          >
            <li class="nav-item">
              <span class="user-info">
                <img
                  :src="user.picture"
                  alt="User's profile picture"
                  class="nav-user-profile d-inline-block rounded-circle mr-3"
                  width="50"
                />
                <h6 class="d-inline-block">{{ user.name }}</h6>
              </span>
            </li>
            <li>
              <i class="fa fa-user mr-3"></i>
              <router-link to="/profile">Profile</router-link>
            </li>

            <li>
              <i class="mr-3 fa fa-power-off" />
              <a id="qsLogoutBtn" href="#" class @click.prevent="logout">Log out</a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </div>
</template>

<script lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';

export default {
  name: "NavBar",
  setup() {
    const auth0 = useAuth0();
    
    return {
      isAuthenticated: auth0.isAuthenticated,
      isLoading: auth0.isLoading,
      user: auth0.user,
      login() {
        auth0.loginWithRedirect();
      },
      logout() {
        auth0.logout({
          logoutParams: {
            returnTo: window.location.origin
          }
        });
      }
    }
  }
};
</script>

<style>
#mobileAuthNavBar {
  min-height: 125px;
  justify-content: space-between;
}
</style>
