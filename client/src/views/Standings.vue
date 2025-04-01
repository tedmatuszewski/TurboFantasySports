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
  import { ref, onMounted } from "vue";
  
  const storage = useStorage();
  const route = useRoute();
  const table = ref([]);

  onMounted(async () => {
    table.value = storage.getTeamWithPoints(route.params.id);
  });
</script>