<template>
  <div>
    <h1>{{league?.Name}}</h1>
    <p>{{league?.Description}}</p>

    <h1>Teams</h1>
    <select v-model="selectedTeam">
      <option v-for="member in members" :value="member.Id">{{member.TeamName}}</option>
    </select>
  </div>
</template>

<script setup>
  import { StorageContext } from '../storage/StorageContext';
  import { ref,onMounted,computed  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  
  const auth0 = useAuth0();
  const route = useRoute();
  
  let selectedTeam = ref(null);
  let league = ref(null);
  let members = ref(null);
  let riders = ref(null);
  let races = ref(null);
  let teams = ref(null);

  onMounted(async () => {
    let cont = await StorageContext();

    league.value = await cont.Leagues.get(route.params.id);
    members.value = await cont.Members.getByLeagueGuid(route.params.id);
    riders.value = await cont.Riders.getAll();
    races.value = await cont.Races.getAll();
    teams.value = await cont.Teams.getAll();
  });
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
