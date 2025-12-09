<template>
  <div>
    <hero />

    <div class="mb-5">
      <BulletCallout />
    </div>
    
    <div class="container">
      <div class="row">
        <div class="col-md-7 mb-5">
          <Checklist />
        </div>

        <div class="col-md-5 mb-5 d-flex align-items-center justify-content-center">
          <Donate />
        </div>
      </div>
    </div>

    <CreateLeague />
    
    <div class="container">
      <div class="py-5">
        <leagues />
      </div>
    </div>
  </div>
</template>

<script setup>
  import Hero from "../components/Hero.vue";
  import Leagues from "../components/Leagues.vue"
  import BulletCallout from "../components/BulletCallout.vue";
  import Checklist from "../components/Checklist.vue";
  import CreateLeague from "../components/CreateLeagueBanner.vue";
  import Donate from "../components/Donate.vue";
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
