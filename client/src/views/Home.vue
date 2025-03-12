<template>
  <div>
    <hero />
    
    <div class="container py-3">
      <Checklist />
    </div>

    <BulletCallout />

    <div class="container">
      <!-- Home Page Ad -->
      <ins class="adsbygoogle"
          style="display:block"
          data-ad-client="ca-pub-1641621719114473"
          data-ad-slot="1623933238"
          data-ad-format="auto"
          data-full-width-responsive="true"></ins>
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
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
