<template>
  <div>
    <hero />
    
    <div class="container py-3">
      <Checklist />
    </div>

    <BulletCallout />

    <div class="container adHomePage">
      <Adsense adStyle="display:inline-block;height:300px"
           clientId="ca-pub-1641621719114473"
           slotId="1623933238"
           fullWidthResponsive="true">
      </Adsense>
    </div>

    <div class="container">
      <leagues />
    </div>
  </div>
</template>

<script setup>
  import Hero from "../components/Hero.vue";
  import Leagues from "../components/Leagues.vue"
  import BulletCallout from "../components/BulletCallout.vue";
  import Checklist from "../components/Checklist.vue";
  import { Adsense } from 'vue3-google-adsense';
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
