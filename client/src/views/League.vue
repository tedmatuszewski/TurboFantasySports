<template>
  <div class="container my-5">
    <h1>{{league?.Name}}</h1>
    <p>{{league?.Description}}</p>

    <h3>Races</h3>
    <races></races>

    <div class="alert alert-primary" role="alert">
      You can have up to {{ Config.maxRiders }} riders on your team. You can edit hour lineup up to the start of the current weeks race. Once the race begins, you will no longer see the edit roster buttons. The roster editing ability will become unlock as soon as the admin of the website uploads the race rasults.
    </div>
    
    <div class="row">
      <div class="col-md-8">
        <h3>My Roster</h3>
        <p>Countdown to the next race. When this runs out, your roster will be locked until the race is over and results are posted.</p>

        <div class="text-center my-2">
          <Countdown></Countdown>
        </div>

        <table class="table table-sm">
          <thead>
            <tr>
              <th>Number</th>
              <th>Rider</th>
              <th></th>
              <th>Class</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="rider in myRidersList" :value="rider.number">
              <td>{{rider.number}}</td>
              <td>{{rider.name}}</td>
              <td><RacerLink :id="rider.id"></RacerLink></td>
              <td>{{rider.class}}</td>
              <td><button class="btn btn-sm btn-danger" :disabled="!isRosterEditable" v-on:click="removeRiderClick(rider)">Remove</button></td>
            </tr>
          </tbody>
        </table>

        <button class="btn btn-primary btn-block" v-on:click="showModalClick()" :disabled="isAddRiderDisabled">Add Rider</button>
      </div>
      
      <div class="col-md-4">
        <Position></Position>
      </div> 
    </div>

    <div class="modal" tabindex="-1" id="riderModal">
      <div class="modal-dialog modal-dialog-scrollable">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Riders</h5>
          </div>

          <div class="modal-body">
            <input type="text" class="form-control mb-3" placeholder="Search for a rider" v-model="searchValue">

            <Vue3EasyDataTable
                :headers="headers"
                :items="addRidersList"
                :search-field="searchField"
                :search-value="searchValue"
                :rows-per-page="100"
                v-model:items-selected="itemsSelected"
                @click-row="rowClick"
                :hide-footer="true">
              <template #item-link="{ id }">
                <RacerLink :id="id"></RacerLink>
              </template>
            </Vue3EasyDataTable>
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
  import riderBank from '../data/riders.json';
  import Races from "../components/Races.vue";
  import Position from "../components/Position.vue";
  import Countdown from "../components/Countdown.vue";
  import Config from "../config.json";
  import RacerLink from "../components/RacerLink.vue";
  import Vue3EasyDataTable from 'vue3-easy-data-table';
  import { getPreviousRace } from '../models/RaceNavigator';

  const headers = [
    { text: "Number", value: "number", sortable: true },
    { text: "Rider", value: "name", sortable: true},
    { text: "", value: "link"},
    { text: "Class", value: "class", sortable: true}
  ];

  let itemsSelected = ref([]);
  const searchField = ref("name");
  const searchValue = ref("");
  const auth0 = useAuth0();
  const route = useRoute();
  
  let addRidersList = reactive([]);
  let myRidersList = reactive([]);
  let league = ref(null);
  let members = ref(null);
  let context = null;
  let riderModal = null;
  let confirmModal = null;
  let prev = getPreviousRace();
  let isRosterEditable = ref(false);
  let member;

  const isAddRiderDisabled = computed(() => {
    if ((isRosterEditable.value == false) || (myRidersList.length >= Config.maxRiders)) {
      return true;
    } else {
      return false;
    }
  });

  onMounted(async () => {
    context = StorageContext();
    riderModal = new bootstrap.Modal(document.getElementById('riderModal'), {});
    confirmModal = new bootstrap.Modal(document.getElementById('confirmModal'), {});

    league.value = context.Leagues.getSingle(route.params.id);
    members.value = context.Members.getByLeague2(route.params.id);
    isRosterEditable.value = context.Results.hasResults2(route.params.id, prev.key);
    member = context.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);

    let allTeams = context.Teams.getByLeague2(route.params.id);
    let myTeam = allTeams.filter(t => t.Member == member.RowKey);
    
    riderBank.sort((a, b) => a.name.localeCompare(b.name));
    riderBank.forEach(rider => {
      let all = allTeams.map(a => a.Rider).indexOf(rider.id);
      let mine = myTeam.map(a => a.Rider).indexOf(rider.id);
      
      if(all === -1) {
        addRidersList.push(rider);
      }
      
      if (mine > -1) {
        myRidersList.push(rider);
      }
    });
  });

  function rowClick(row, e) {
    e.srcElement.closest("tr").querySelector("input[type=checkbox]").click();
  }

  function showModalClick() {
    riderModal.show();
  }

  function closeRiderModalClick() {
    riderModal.hide();
  }

  async function addRiderModalClick() {
    if(myRidersList.length >= Config.maxRiders && itemsSelected.value.length > 0) {
      alert("You can only have " + Config.maxRiders + " riders on your team.");
      return;
    }

    itemsSelected.value.map(i => i.id).forEach(async id => {
      let sel = addRidersList.find(r => r.id == id);
      let index = addRidersList.indexOf(sel);

      myRidersList.push(sel);
      addRidersList.splice(index, 1);

      await context.Teams.create({
        League: route.params.id,
        Member: member.RowKey,
        Rider: sel.id
      });
    });

    riderModal.hide();
  }

  async function removeRiderClick(rider){
    if(confirm("Are you sure that you want to remove this rider from your team? Removing this rider will put them back in the pool of available riders for anyone else to scoop up.") == false) {
      return;
    }

    let sel = myRidersList.find(r => r.id == rider.id);
    let index = myRidersList.indexOf(sel);
    
    addRidersList.push(sel);
    myRidersList.splice(index, 1);

    let toDelete = await context.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.RowKey, sel.id);

    toDelete.forEach(async d => {
      await context.Teams.remove(d.RowKey);
    })
  }
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}

.card-pricing.popular {
    z-index: 1;
    border: 3px solid #007bff;
}
.card-pricing .list-unstyled li {
    padding: .5rem 0;
    color: #6c757d;
}
</style>
