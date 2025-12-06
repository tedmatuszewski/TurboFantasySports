<template>
  <div class="container my-5">
    <h3>Races</h3>
    <races></races>

    <div class="alert alert-danger" role="alert" v-if="league.DraftComplete != true">
      The draft for your league has not yet taken place. Once the draft is complete, you will be able to manage your riders for the season.
    </div>

    <div class="row">
      <div class="col-md-8">
        <LeagueHeader></LeagueHeader>

        <router-link v-if="member?.IsAdmin" :to="{ name: 'ManageLeague', params: { id: route.params.id } }" class="btn btn-primary btn-block mb-2">Manage League</router-link>
      </div>
      
      <div class="col-md-4">
        <Position></Position>
      </div> 
    </div>
        
    <div class="alert alert-primary" role="alert">
      You can have up to {{ Config.maxRiders }} riders on your team. You can edit hour lineup up to the start of the current weeks race. Once the race begins, you will no longer see the edit roster buttons. The roster editing ability will become unlock as soon as the admin of the website uploads the race rasults.
    </div>
    
    <h3>My Roster</h3>

    <ag-grid-vue :rowData="myRidersList" :columnDefs="colDefs" style="height: 320px;" :autoSizeStrategy="{ type: 'fitCellContents' }"></ag-grid-vue>

    <router-link v-if="isRosterEditable" :to="{ name: 'riders', params: { id: route.params.id } }" class="btn btn-primary btn-block mt-3">Edit Roster</router-link>

    <div class="row">
      <div class="col-md-6 py-5">
        <Feed :league="route.params.id"></Feed>
      </div>
      
      <div class="col-md-6 py-5">
         <Donate />
      </div>
    </div> 
    
    <Rider ref="riderModal" :league="route.params.id"></Rider>
  </div>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, reactive  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import Races from "../components/Races.vue";
  import Position from "../components/Position.vue";
  import Config from "../config.json";
  import Feed from "../components/Feed.vue";
  import RacerName from "../components/RacerName.vue";
  import Rider from "../components/Rider.vue";
  import LeagueHeader from "../components/LeagueHeader.vue";
  import { AgGridVue } from "ag-grid-vue3";
  import Donate from '../components/Donate.vue';

  const colDefs = ref([
    { 
      headerName: "Rider",
      field: "Name",
      pinned: 'left',
      cellRenderer: RacerName,
      cellRendererParams: {
        click: showRiderModal
      }
    },
    { field: "Number", headerName: "#" },
      { field: "Class" },
      { headerName: "E", field: "Entries" },
      { headerName: "MEQF", field: "TotalOutcomes" },
      { headerName: "TP", field: "TotalPoints" },
      { headerName: "AFP/R", field: "AveragePoints" },
      { headerName: "ARP/R", field: "AveragePlace" },
      { headerName: "Wins", field: "Wins" },
      { headerName: "Top3", field: "Podiums" },
      { headerName: "Top5", field: "TopFives" },
      { headerName: "Top10", field: "TopTens" }
  ]);

  const storage = useStorage();
  const auth0 = useAuth0();
  const route = useRoute();
  const riderModal = ref(null);
  // const headers = [
  //   { text: "", value: "remove"},
  //   { text: "Number", value: "Number", sortable: true },
  //   { text: "", value: "headshot", width: 50 },
  //   { text: "", value: "stats" },
  //   { text: "", value: "link"},
  //   { 
  //     headerName: "Rider",
  //     field: "rowKey",
  //     cellRenderer: RiderLink,
  //   },
  //   { text: "Class", value: "Class", sortable: true },
  //   { text: "E", value: "Entries", sortable: true },
  //   { text: "MEQF", value: "TotalOutcomes", sortable: true },
  //   { text: "TP", value: "TotalPoints", sortable: true },
  //   { text: "AFP/R", value: "AveragePoints", sortable: true },
  //   { text: "ARP/R", value: "AveragePlace", sortable: true },
  //   { text: "Wins", value: "Wins", sortable: true },
  //   { text: "Top3", value: "Podiums", sortable: true },
  //   { text: "Top5", value: "TopFives", sortable: true },
  //   { text: "Top10", value: "TopTens", sortable: true },
  // ];

  let member = null;
  let myRidersList = ref([]);
  let league = ref(null);
  let prev = storage.Races.getPreviousRace();
  let isRosterEditable = ref(false);

  onMounted(async () => {
    league.value = storage.Leagues.getSingle(route.params.id);

    console.log(league.value);
    
    // Previous will be null the first race of the year.
    if(prev != null) {
      console.log("1");
      isRosterEditable.value = league.value.DraftComplete == true && storage.Results.hasResults2(route.params.id, prev.rowKey);
    } else {
      console.log("2");
      isRosterEditable.value = league.value.DraftComplete == true;
    }

    member = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
    myRidersList.value = storage.getTeam(route.params.id, member.rowKey);
  });

  async function removeRiderClick(key){
    if(confirm("Are you sure that you want to remove this rider from your team? Removing this rider will put them back in the pool of available riders for anyone else to scoop up.") == false) {
      return;
    }
    
    let sel = myRidersList.value.find(r => r.rowKey == key);
    let index = myRidersList.value.indexOf(sel);
    let toDelete = await  storage.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.rowKey, sel.rowKey);
    
    myRidersList.value.splice(index, 1);

    toDelete.forEach(async d => {
      await storage.Teams.remove(d.rowKey);
    });

    await storage.Feeds.create({ League: route.params.id, Member: member.rowKey, Action: `Removed rider ${sel.Name} from their team` });
  }

  function showRiderModal(key){
    riderModal.value.open(key);
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
