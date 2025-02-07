<template>
  <div>
    <hero />
    <hr />
    <home-content />
  </div>
</template>

<script lang="ts">
import Hero from "../components/Hero.vue";
import HomeContent from "../components/HomeContent.vue";
import { StorageContext } from "../storage/StorageContext";

let cont = await StorageContext();
let riders = await cont.Riders.getAll();
let leagues = await cont.Leagues.getAll();
let races = await cont.Races.getAll();

console.log(riders);
console.log(leagues);
console.log(races);

export default {
  name: "home-view",
  components: {
    Hero,
    HomeContent,
  },
};
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
