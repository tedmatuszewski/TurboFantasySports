<template>
  <div class="container my-4">
    <div class="row">
      <div class="col">
        <h3>{{ race.name }} Race Results</h3>
      </div>
      <div class="col text-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
      </div>
    </div>

    <races :race="race.key"></races>
    
    <div class="row">
      <div class="col-md-6" v-for="(table, i) in tables" :value="table.member.UserGuid">
        <div class="search-list mb-3">
          <h4>{{ (i+1) }}. {{table.member.TeamName}}</h4>

          <table class="table table-sm">
            <thead>
              <tr>
                <th>Rider</th>
                <th>Place</th>
                <th>Points</th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="result in table.results" :value="result.RowKey">
                <td>{{ getRiderName(result.Rider) }}</td>
                <td>{{ result.Place }}</td>
                <td>{{ result.Points }}</td>
              </tr>
              <tr>
                <td><b>Total</b></td>
                <td></td>
                <td>{{ table.total }}</td>
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
  import races from '../data/races.json';
  import Riders from '../data/riders.json';
  import { useStorage } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { ref,onMounted,computed, reactive, watch } from "vue";
  
  const storage = useStorage();
  const route = useRoute();
  let tables = reactive([]);
  let race = races.find(r => r.key === route.params.race);

  watch(() => route.params.race, async (newRace) => {
    race = races.find(r => r.key === newRace);
    
    await loadPage();
  });

  onMounted(async () => {
    await loadPage();
  });

  async function loadPage(){
    tables.length = 0;
    
    let results =  storage.Results.getByLeagueAndRace2(route.params.id, route.params.race);
    let members =  storage.Members.getByLeague2(route.params.id);
    
    members.forEach((member, i) => {
      let rr = results.filter(result => result.Member === member.RowKey);
      let table = {
        place: i,
        member: member,
        results: rr,
        total: rr.map(r => r.Points).reduce((a, b) => a + b, 0)
      };

      tables.push(table);
    });

    tables.sort((a, b) => b.total - a.total);
  }

  function getRiderName(id) {
    let rider = Riders.find(r => r.id === id);
    return rider ? rider.name : '';
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
