<template>
  <div class="container my-4">
    <div class="row my-2">
      <div class="col-sm">
        <h3 class="text-center text-sm-left">Current Matchup</h3>
      </div>
      <div class="col-sm text-center text-sm-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
          <router-link :to="{ name: 'standings', params: { id: route.params.id } }" class="btn btn-primary ml-3">Standings</router-link>
      </div>
    </div>

    <div class="alert alert-primary" role="alert">
      This page shows the current roster of all the teams in the league. What is shown on this page will be the roster for the next upcoming race.
    </div>

    <div class="row">
      <div class="col-md-6" v-for="(table, i) in tables" :value="table.TeamName">
        <div class="search-list mb-3">
          <h4>{{table.TeamName}}</h4>

          <table class="table table-sm">
            <thead>
              <tr>
                <th>Rider</th>
                <th>Number</th>
                <th>Class</th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="team in table.Team" :value="team.RowKey">
                <td><a href="#" v-on:click.prevent="showRiderModal(team.Rider);">{{ getRiderName(team.Rider) }}</a></td>
                <td>{{ getRiderNumber(team.Rider) }}</td>
                <td>{{ getRiderClass(team.Rider) }}</td>
              </tr>
            </tbody>
          </table>
      </div>
      </div>
    </div>
  </div>
  
  <Rider ref="riderModal" :league="route.params.id"></Rider>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { ref,onMounted,computed, reactive, watch } from "vue";
  import Rider from "../components/Rider.vue";
  
  const storage = useStorage();
  const route = useRoute();
  const riderModal = ref(null);
  let tables = reactive([]);

  onMounted(async () => {
      let teams =  storage.Teams.getByLeague2(route.params.id);
      let members =  storage.Members.getByLeague2(route.params.id);

      members.forEach((member, i) => {
        let rr = teams.filter(t => t.Member === member.RowKey);
        let table = {
          TeamName: member.TeamName,
          Team: rr
        };
  
        tables.push(table);
      });
  });

  function showRiderModal(id) {
    riderModal.value.open(id);
  }

function getRiderName(id) {
  let rider = storage.Riders.data.find(r => r.RowKey === id);
  return rider ? rider.Name : id;
}

function getRiderNumber(id) {
  let rider = storage.Riders.data.find(r => r.RowKey === id);
  return rider ? rider.Number : id;
}

function getRiderClass(id) {
  let rider = storage.Riders.data.find(r => r.RowKey === id);
  return rider ? rider.Class : id;
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
