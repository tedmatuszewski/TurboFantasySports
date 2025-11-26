<template>
    <div class="py-5 text-center track-background">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <h2 class="mb-3">Ready to start your own league?</h2>
          <p class="lead mb-4">Create your own fantasy sports SMX league with your friends and family. Set your own team, optimize your lineup, and compete for bragging rights!</p>
          <router-link v-if="isAuthenticated" :to="{ name: 'CreateLeague' }" class="btn btn-secondary btn-lg">Create New League</router-link>
          <span v-else>Please log in to create a new league.</span>
        </div>
      </div>
    </div>
</template>

<script setup>
  import { useAuth0 } from '@auth0/auth0-vue';
  import { ref,onMounted,computed,watch } from "vue";
  
  const auth0 = useAuth0();
  
  const isAuthenticated = computed(() => {
      return auth0.isAuthenticated.value;
  });
</script>

<style scoped>
    .track-background {
        color: white;
        height: 35vh;
        background-image: url('../assets/track.png');
        background-size: cover;
        background-position: center center;
        background-repeat: no-repeat;
        background-color: #2a2a45;
    }
</style>