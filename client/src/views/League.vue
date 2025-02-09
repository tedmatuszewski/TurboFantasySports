<template>
  <div>
    <h1>{{league?.Name}}</h1>
    <p>{{league?.Description}}</p>

    <h1>My Team</h1>
  </div>
</template>

<script setup>
  import { StorageContext } from '../storage/StorageContext';
  import { ref,onMounted,computed  } from "vue";
  import { useRoute } from 'vue-router';

  const route = useRoute();
  let league = ref(null);

  onMounted(async () => {
    let cont = await StorageContext();

    league.value = await cont.Leagues.get(route.params.id);

    console.log(league.value);
  });
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
