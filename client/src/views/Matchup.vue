<template>
  <div class="container my-4">
    <div class="row">
      <div class="col">
        <h3>Current Matchup</h3>
      </div>
      <div class="col text-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
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
                <td>{{ getRiderName(team.Rider) }}</td>
                <td>{{ getRiderNumber(team.Rider) }}</td>
                <td>{{ getRiderClass(team.Rider) }}</td>
              </tr>
            </tbody>
          </table>
      </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import Riders from '../data/riders.json';
  import { StorageContext } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { ref,onMounted,computed, reactive, watch } from "vue";
  
  const route = useRoute();
  let tables = reactive([]);
  let context;

  onMounted(async () => {
    context = await StorageContext();
    
      //let results = await context.Results.getByLeague(route.params.id);
      let teams = await context.Teams.getByLeague(route.params.id);
      let members = await context.Members.getByLeagueGuid(route.params.id);

      members.forEach((member, i) => {
        let rr = teams.filter(t => t.Member === member.UserGuid);
        let table = {
          TeamName: member.TeamName,
          Team: rr
        };
  
        tables.push(table);
      });
  });

function getRiderName(id) {
  let rider = Riders.find(r => r.id === id);
  return rider ? rider.name : id;
}

function getRiderNumber(id) {
  let rider = Riders.find(r => r.id === id);
  return rider ? rider.number : id;
}

function getRiderClass(id) {
  let rider = Riders.find(r => r.id === id);
  return rider ? rider.class : id;
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
