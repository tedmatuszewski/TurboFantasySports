<template>
    <div class="py-5 text-center track-background">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <h2 class="mb-3">Ready to start your own league?</h2>
          <p class="lead mb-4">Create your own fantasy sports SMX league with your friends and family. Set your own team, optimize your lineup, and compete for bragging rights!</p>
          <router-link :to="{ name: 'CreateLeague' }" class="btn btn-secondary btn-lg">Create New League</router-link>
        </div>
      </div>
    </div>
</template>

<script setup></script>

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