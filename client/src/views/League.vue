<template>
  <div>
    <h1>{{league?.Name}}</h1>
    <p>{{league?.Description}}</p>

    <h3>Races</h3>
    <races></races>

    <div class="row">
      <div class="col-md-8">
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
            <tr v-for="rider in myRidersList" :value="rider.number">
              <td>{{rider.number}}</td>
              <td>{{rider.name}}</td>
              <td>{{rider.class}}</td>
              <td><button class="btn btn-sm" v-on:click="removeRiderClick(rider)">Remove</button></td>
            </tr>
          </tbody>
        </table>

        <button class="btn btn-primary btn-block" v-on:click="showModalClick()">Add Rider</button>
      </div>
      
      <div class="col-md-4">
        <!-- <div class="row align-items-center">
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
              <th>Number</th>
              <th>Rider</th>
              <th>Class</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="rider in myRidersList" :value="rider.number">
              <td>{{rider.number}}</td>
              <td>{{rider.name}}</td>
              <td>{{rider.class}}</td>
            </tr>
          </tbody>
        </table>-->
      </div> 
    </div>

    <div class="modal" tabindex="-1" id="riderModal">
      <div class="modal-dialog modal-xl modal-dialog-scrollable">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Riders</h5>
          </div>

          <div class="modal-body">
            <table class="table table-sm table-hover">
              <thead>
                <tr>
                  <th>Number</th>
                  <th>Rider</th>
                  <th>Class</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="rider in addRidersList" :value="rider.number" v-on:click="selectRider(rider)" :class="{ 'table-active': rider.selected }">
                  <td>{{rider.number}}</td>
                  <td>{{rider.name}}</td>
                  <td>{{ rider.class }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div class="modal-footer">
            <button type="button" class="btn btn-primary" v-on:click="addRiderModalClick">Add Rider</button>
            <button type="button" class="btn btn-secondary" v-on:click="closeRiderModalClick">Close</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { StorageContext } from '../storage/StorageContext';
  import { ref,onMounted,computed, reactive  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import results from '../data/results.json';
  import riderBank from '../data/riders.json';
  import Races from "../components/Races.vue";

  const showModal = ref(false)
  const auth0 = useAuth0();
  const route = useRoute();
  
  let addRidersList = reactive([]);
  let myRidersList = reactive([]);
  let selectedTeam = ref(null);
  let league = ref(null);
  let members = ref(null);
  let teams = ref(null);
  let context = null;
  let riderModal = null;
  let confirmModal = null;

  onMounted(async () => {
    context = await StorageContext();
    riderModal = new bootstrap.Modal(document.getElementById('riderModal'), {});
    confirmModal = new bootstrap.Modal(document.getElementById('confirmModal'), {});

    league.value = await context.Leagues.get(route.params.id);
    members.value = await context.Members.getByLeagueGuid(route.params.id);

    let allTeams = await context.Teams.getByLeague(route.params.id);
    let myTeam = await context.Teams.getByLeagueAndOwner(route.params.id, auth0.user.value.sub);

    riderBank.sort((a, b) => a.name.localeCompare(b.name));
    riderBank.forEach(rider => {
      let all = allTeams.map(a => a.Rider).indexOf(rider.number);
      let mine = myTeam.map(a => a.Rider).indexOf(rider.number);

      if(all === -1) {
        addRidersList.push(rider);
      }

      if (mine > -1) {
        myRidersList.push(rider);
      }
    });
  });

  async function onTeamChange() {
    myTeam.value = await context.Teams.getByLeagueAndOwner(route.params.id, selectedTeam.value);
    console.log(route.params.id, selectedTeam.value, teams.value);
  }

  function showModalClick() {
    riderModal.show();
  }

  function closeRiderModalClick() {
    riderModal.hide();
  }

  async function addRiderModalClick() {
    let sel = addRidersList.find(r => r.selected);
    let index = addRidersList.indexOf(sel);

    sel.selected = false;
    riderModal.hide();
    myRidersList.push(sel);
    addRidersList.splice(index, 1);

    await context.Teams.create({
      League: route.params.id,
      Owner: auth0.user.value.sub,
      Rider: sel.number
    });
  }

  function selectRider(rider) {
    addRidersList.forEach(r => r.selected = false);
    rider.selected = true;
  }

  async function removeRiderClick(rider){
    if(confirm("Are you sure that you want to remove this rider from your team? Removing this rider will put them back in the pool of available riders for anyone else to scoop up.") == false) {
      return;
    }

    let sel = myRidersList.find(r => r.number == rider.number);
    let index = myRidersList.indexOf(sel);
    
    addRidersList.push(sel);
    myRidersList.splice(index, 1);

    let toDelete = await context.Teams.getByLeagueAndOwnerAndNumber(route.params.id, auth0.user.value.sub, sel.number);

    toDelete.forEach(async d => {
      await context.Teams.remove(d.Id);
    })
  }
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
