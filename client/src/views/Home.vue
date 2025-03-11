<template>
  <div>
    <hero />
    
    <div class="container py-3">
      <Checklist />
    </div>

    <BulletCallout />

    <div class="container">
      <amp-ad width="100vw" height="320"
          type="adsense"
          data-ad-client="ca-pub-1641621719114473"
          data-ad-slot="1623933238"
          data-auto-format="rspv"
          data-full-width="">
        <div overflow=""></div>
      </amp-ad>
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
