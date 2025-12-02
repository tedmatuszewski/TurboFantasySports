<template>
  <div class="container my-4">
    <div class="row">
      <div class="col">
        <h3>{{ race.Name }} Race Results</h3>
      </div>
      <div class="col text-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
      </div>
    </div>

    <races :race="race.rowKey"></races>
    
    <div class="row">
      <div class="col-md-6" v-for="(table, i) in tables">
        <div class="search-list mb-3">
          <h4>{{ (i+1) }}. {{table.TeamName}}</h4>

          <table class="table table-sm">
            <thead>
              <tr>
                <th>Rider</th>
                <th>Place</th>
                <th>Points</th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="result in table.Results">
                <td>{{ getRiderName(result.Rider) }}</td>
                <td>{{ result.Place }}</td>
                <td>{{ result.Points }}</td>
              </tr>
              <tr>
                <td><b>Total</b></td>
                <td></td>
                <td>{{ table.Points }}</td>
              </tr>
            </tbody>
          </table>
      </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import Races from "../components/Races.vue";
  import { useStorage } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { onMounted, watch, ref } from "vue";
  
  const storage = useStorage();
  const route = useRoute();
  const tables = ref([]);
  const race = ref(null);

  watch(() => route.params.race, async (newRace) => {
    await loadPage(newRace);
  });
  
  onMounted(async () => {
    await loadPage(route.params.race);
  });
  
  async function loadPage(raceKey){
    race.value = storage.Races.getRaceByKey(raceKey);
    tables.value = storage.getTeamWithPoints(route.params.id, raceKey);
  }

  function getRiderName(id) {
    let rider = storage.Riders.data.find(r => r.rowKey === id);
    return rider ? rider.Name : '';
  }
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}

.search-list{
    background: #fff;
    border: 1px solid #ababab;
}
.search-list h4{
    background: #eee;
    padding: 3%;
    margin-bottom: 0%;
}
table {
  margin-bottom: 0;
}
</style>
