<template>
  <div>
    <h1>{{league?.Name}}</h1>
    <p>{{league?.Description}}</p>

    <h3>Races</h3>
    <races></races>

    <div class="row">
      <div class="col-md-6">
        <h3>My Roster</h3>

        <table class="table table-sm">
          <thead>
            <tr>
              <th>Rider</th>
              <th>Number</th>
              <th>Class</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="my in myTeam" :value="my.Id">
              <td>{{my.Id}}</td>
              <td>{{my.Rider}}</td>
              <td></td>
              <td><button class="btn btn-sm">Trade</button></td>
            </tr>
          </tbody>
        </table>

        <button class="btn btn-primary btn-block" v-on:click="showModalClick()">Add Rider</button>
      </div>
      
      <div class="col-md-6">
        <div class="row align-items-center">
          <div class="col-md-6">
            <h3>Other Teams</h3>
          </div>
          
          <div class="col-md-6">
            <select v-model="selectedTeam" class="form-control" v-on:change="onTeamChange">
              <option v-for="member in members" :value="member.UserGuid">{{member.TeamName}}</option>
            </select>
          </div>
        </div>

        <table class="table table-sm">
          <thead>
            <tr>
              <th>Rider</th>
              <th>Number</th>
              <th>Class</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="my in myTeam" :value="my.Id">
              <td>{{my.Id}}</td>
              <td>{{my.Rider}}</td>
              <td></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="modal" tabindex="-1" id="riderModal">
      <div class="modal-dialog modal-xl">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Riders</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <table class="table table-sm">
          <thead>
            <tr>
              <th>Rider</th>
              <th>Number</th>
              <th>Class</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="rider in riderBank" :value="rider.number">
              <td>{{rider.number}}</td>
              <td>{{rider.name}}</td>
              <td>{{ rider.class }}</td>
            </tr>
          </tbody>
        </table>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { StorageContext } from '../storage/StorageContext';
  import { ref,onMounted,computed  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import results from '../data/results.json';
  import riderBank from '../data/riders.json';
  import Races from "../components/Races.vue";

  const showModal = ref(false)
  const auth0 = useAuth0();
  const route = useRoute();
  
  let isRiderModalVisible = ref(false);
  let selectedTeam = ref(null);
  let league = ref(null);
  let members = ref(null);
  let riders = ref(null);
  let teams = ref(null);
  let context = null;
  let myTeam = ref(null);

  let myModal = null;

  onMounted(async () => {
    context = await StorageContext();
    myModal = new bootstrap.Modal(document.getElementById('riderModal'), {})

    league.value = await context.Leagues.get(route.params.id);
    members.value = await context.Members.getByLeagueGuid(route.params.id);
    riders.value = await context.Riders.getAll();
    myTeam.value = await context.Teams.getByLeagueAndOwner(route.params.id, auth0.user.value.sub);

    console.log(route.params.id, auth0.user.value.sub, teams.value);
  });

  async function onTeamChange() {
    myTeam.value = await context.Teams.getByLeagueAndOwner(route.params.id, selectedTeam.value);
    console.log(route.params.id, selectedTeam.value, teams.value);
  }

  function showModalClick() {
    myModal.show();
  }
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
