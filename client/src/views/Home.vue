<template>
  <div>
    <hero />
    
    <leagues />
  </div>
</template>

<script setup>
  import Hero from "../components/Hero.vue";
  import Leagues from "../components/Leagues.vue"

  // let riders = await cont.Riders.getAll();
  // let races = await cont.Races.getAll();
  // let members = await cont.Members.getAll();

  // console.log(riders);
  // console.log(leagues);
  // console.log(races);
  // console.log(members);
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
