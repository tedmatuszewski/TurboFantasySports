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
      You can have up to {{ Config.maxRiders }} riders on your team. The entry list for the next race will process on Thursday at 12:00 PM UTC. You can edit 
      your lineup up to the start of the current week's race, but once the race begins, roster editing will be temporarily locked until the processing of 
      race results. Results will be processed on Mondays at 12:00 PM UTC and the site will reflect the results with updated points and standings.
    </div>
    
    <RiderTableKey></RiderTableKey>
    
    <h3>My Roster</h3>

    <ag-grid-vue :rowData="myRidersList" :columnDefs="colDefs" style="height: 420px;" :autoSizeStrategy="{ type: 'fitCellContents' }" :getRowStyle="getRowStyle"></ag-grid-vue>
    
    <router-link :to="{ name: 'riders', params: { id: route.params.id } }" class="btn btn-primary btn-block mt-3">View Riders</router-link>

    <button class="btn btn-secondary btn-block mt-3" v-on:click="showBenchModal">Edit Bench</button>

    <div class="row">
      <div class="col-md-6 py-5">
        <Feed :league="route.params.id"></Feed>
      </div>
      
      <div class="col-md-6 py-5">
         <Donate />
      </div>
    </div> 
    
    <Rider ref="riderModal" :league="route.params.id"></Rider>
    
    <div id="benchModal" class="modal">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Roster</h5>
          </div>
          <div class="modal-body">
            <div class="alert alert-info" role="alert">
              Choose a rider from your active roster, then a rider from your bench and press the "Swap Riders" button to swap their positions.
            </div>

            <h6>Bench</h6>
            <ag-grid-vue ref="benchRidersTable" :rowSelection="{ mode: 'singleRow', enableClickSelection: true }" :rowData="myBenchRidersList" :columnDefs="activeColDefs" style="height: 150px;"></ag-grid-vue>
            
            <div class="mt-3"></div>
            
            <h6>Active</h6>
            <ag-grid-vue ref="activeRidersTable" :rowSelection="{ mode: 'singleRow', enableClickSelection: true }" :rowData="myActiveRidersList" :columnDefs="benchColDefs" style="height: 350px;"></ag-grid-vue>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary" v-on:click="switchBenchRiders">Swap Riders</button>
            <button type="button" class="btn btn-secondary" v-on:click="closeBenchModal">Close</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, reactive, nextTick  } from "vue";
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
  import TableRemove from "../components/TableRemove.vue";
  import RiderTableKey from "../components/RiderTableKey.vue";

  const benchColDefs = ref([
    { field: "Number", headerName: "#", width: 60 },
    { field: "Name", headerName: "Name" },
    {
      field: 'Name',
      headerName: 'Toggle', 
      width: 100,
      cellRenderer: params => {
        return '<button class="btn btn-sm btn-success">Bench</button>';
      },
      onCellClicked: async params => {
          let benchCount = myBenchRidersList.value.length;

          if(benchCount >= Config.maxBench) {
            alert("You cannot have more than " + Config.maxBench + " bench riders on your roster.");

            return;
          }
   
          let rider = myRidersList.value.find(r => r.rowKey == params.data.rowKey);
          let team = storage.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.rowKey, rider.rowKey)[0];
          
          rider.IsBench = true;
          team.IsBench = true;
          
          await storage.Teams.update(team);
          await storage.Feeds.create({ League: route.params.id, Member: member.rowKey, Action: `Moved ${rider.Name} to the bench` });
      }
    }
  ]);

  const activeColDefs = ref([
    { field: "Number", headerName: "#", width: 60 },
    { field: "Name", headerName: "Name" },
    {
        field: 'Name',
        headerName: 'Toggle', 
        width: 100,
        cellRenderer: params => {
            return '<button class="btn btn-sm btn-success">Activate</button>';
        },
        onCellClicked: async params => {
          let activeCount = myActiveRidersList.value.length;

          if(activeCount >= Config.maxRiders) {
            alert("You cannot have more than " + Config.maxRiders + " active riders on your roster. Please bench an active rider before activating another one.");

            return;
          }
   
          let benchRider = myRidersList.value.find(r => r.rowKey == params.data.rowKey);
          let benchTeam = storage.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.rowKey, benchRider.rowKey)[0];
          
          benchRider.IsBench = false;
          benchTeam.IsBench = false;
          
          await storage.Teams.update(benchTeam);
          await storage.Feeds.create({ League: route.params.id, Member: member.rowKey, Action: `Moved ${benchRider.Name} to active roster` });
        }
    }
  ]);

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
    { 
      headerName: "Actions",
      field: "Name",
      pinned: 'left',
      cellRenderer: TableRemove,
      cellRendererParams: {
        click: removeRiderClick
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
  const benchRidersTable = ref(null);
  const activeRidersTable = ref(null);

  let benchModal2 = null;
  let member = null;
  let myRidersList = ref([]);
  let league = ref(null);

  const myActiveRidersList = computed(() => {
    return myRidersList.value.filter(rider => rider.IsBench !== true);
  });

  const myBenchRidersList = computed(() => {
    return myRidersList.value.filter(rider => rider.IsBench === true); 
  });

  onMounted(async () => {
    league.value = storage.Leagues.getSingle(route.params.id);
    member = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
    myRidersList.value = storage.getTeam(route.params.id, member.rowKey).sort((a, b) => {
      return (a.IsBench === true) - (b.IsBench === true); // Sort by IsBench: false (active) riders first, then true (bench) riders
    });

    await nextTick();
    
    benchModal2 = new bootstrap.Modal(document.getElementById('benchModal'), {});
  });

  function buttonClicked() {
    console.log(1);
  }

  async function removeRiderClick(data){
    if(confirm("Are you sure that you want to remove this rider from your team? Removing this rider will put them back in the pool of available riders for anyone else to scoop up.") == false) {
      return;
    }
    
    let sel = myRidersList.value.find(r => r.rowKey == data.rowKey);
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

  function showBenchModal(){
    benchModal2.show();
  }

  function closeBenchModal(){
    benchModal2.hide();
  }

  function getRowStyle(params) {
    if(params.data.IsBench) {
      return { background: '#f0f0f0' };
    }
    return null;
  }

  async function switchBenchRiders() {
    let benchSelected = benchRidersTable.value.api.getSelectedRows();
    let activeSelected = activeRidersTable.value.api.getSelectedRows();

    if(benchSelected.length !== 1 || activeSelected.length !== 1) {
      alert("You must select one rider from your bench and one rider from your active roster to swap their positions.");

      return;
    }

    let benchRider = myRidersList.value.find(r => r.rowKey == benchSelected[0].rowKey);
    let benchTeam = storage.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.rowKey, benchRider.rowKey)[0];
    let activeRider = myRidersList.value.find(r => r.rowKey == activeSelected[0].rowKey);
    let activeTeam = storage.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.rowKey, activeRider.rowKey)[0];

    benchRider.IsBench = false;
    activeRider.IsBench = true;
    benchTeam.IsBench = false;
    activeTeam.IsBench = true;
    
    await storage.Teams.update(benchTeam);
    await storage.Teams.update(activeTeam);
    await storage.Feeds.create({ League: route.params.id, Member: member.rowKey, Action: `Moved ${benchRider.Name} to active roster and ${activeRider.Name} to bench` });
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
