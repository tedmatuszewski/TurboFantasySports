<template>
  <div class="container my-4">
    <div class="row my-2">
      <div class="col-md">
        <h3 class="text-center text-md-left">Current Standings</h3>
      </div>
      <div class="col-md text-center text-md-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
          <router-link :to="{ name: 'matchup', params: { id: route.params.id } }" class="btn btn-primary ml-3">Matchup</router-link>
      </div>
    </div>

    <table class="table table-sm">
      <thead>
        <tr>
          <th>Place</th>
          <th>Team</th>
          <th>Points</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="row in table" :value="row.RowKey">
          <td>{{ row.Place }}</td>
          <td>{{ row.TeamName }}</td>
          <td>{{ row.Points }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { ref,onMounted,computed, reactive, watch } from "vue";
  
  const storage = useStorage();
  const route = useRoute();
  let table = reactive([]);

  onMounted(async () => {
    let results = storage.Results.getByLeague2(route.params.id);
    let members = storage.Members.getByLeague2(route.params.id);

    members.forEach(member => {
      let rr = results.filter(result => result.Member === member.RowKey);

      table.push({
        TeamName: member.TeamName,
        Points: rr.map(r => r.Points).reduce((a, b) => a + b, 0)
      });
    });

    table.sort((a, b) => b.Points - a.Points);
    table.forEach((t, i) => t.Place = (i+1));
  });
</script>