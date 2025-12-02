<template>
  <div>
    <hero />

    <BulletCallout />
    
    <div class="container py-3">
      <Checklist />
    </div>

    <!-- <div class="container adHomePage">
      <Adsense adStyle="display:inline-block;height:200px" clientId="ca-pub-1641621719114473" slotId="1623933238" fullWidthResponsive="true"></Adsense>
    </div> -->
    
    <CreateLeague />
    
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
  import CreateLeague from "../components/CreateLeagueBanner.vue";
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
