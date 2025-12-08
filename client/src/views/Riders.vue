<template>
  <div class="container my-5">
    <div v-if="availableSpots > 0" class="alert alert-primary" role="alert">
      You currently have ({{ availableSpots }}) open slots on your team. Select up to ({{ availableSpots }}) riders, then press the add button to add them to your team.
    </div>
    <div v-else class="alert alert-primary" role="alert">
      You currently have zero open slots on your team. You must first remove riders from your team on the league home page before you can use this page.
    </div>

    <div class="row">
      <div class="col-md-6">
        <ul class="list-group list-group-horizontal mb-3">
          <li class="list-group-item">AFP/R = Average Fantasy Points per Race</li>
          <li class="list-group-item">ARP/R = Average Rider Position per Race</li>
        </ul>
      </div>
        <div class="col-md-6">

        <ul class="list-group list-group-horizontal mb-3">
          <li class="list-group-item">MEQF = Main Events Qualified For</li>
          <li class="list-group-item">E = Number of Races Entered</li>
        </ul>
      </div>
    </div>

    <div class="row">
      <div class="col-md-6">
        <ul class="list-group list-group-horizontal mb-3">
          <li class="list-group-item">TP = Total Points</li>
        </ul>
      </div>
    </div>

    <div class="row my-2">
      <div class="col-md">
        <h3 class="text-center text-md-left">Riders</h3>
      </div>

      <div class="col-md text-center text-md-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
          <button :disabled="canClickAdd" v-on:click="addRidersClick" class="btn btn-danger ml-3">Add Rider(s)</button>
      </div>
    </div>

    <input id="filter-text-box" type="text" class="form-control mb-3" placeholder="Search for a rider" v-on:input="onFilterTextBoxChanged()" />

    <ag-grid-vue ref="grid" @selection-changed="onSelectionChanged" :rowData="riders" :columnDefs="colDefs" style="height: 320px;" :selectionColumnDef="{pinned: 'left', width: 50}" :rowSelection="{ mode: 'multiRow' }" :autoSizeStrategy="{ type: 'fitCellContents' }"></ag-grid-vue>
  </div> 
    
  <Rider ref="riderModal" :league="props.league"></Rider>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, defineProps  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import Config from "../config.json";
  import RacerName from "../components/RacerName.vue";
  import Rider from "../components/Rider.vue";
  import { AgGridVue } from "ag-grid-vue3";

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

  const props = defineProps([ "league" ]);
  const storage = useStorage();
  const itemsSelected = ref([]);
  const auth0 = useAuth0();
  const route = useRoute();
  const teams = ref([]);
  const team = ref([]);
  const grid = ref(null);
  
  let member = ref(null);
  let riders = ref([]);
  let riderModal = ref(null);

  const availableSpots = computed(() => {
    return Config.maxRiders - team.value.length;
  });

  const canClickAdd = computed(() => {
    return availableSpots.value === 0;
  });

  onMounted(() => {
    member.value = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
    team.value = storage.Teams.getByLeagueAndMember2(route.params.id, member.value.rowKey);
    riders.value = storage.getAvailableRiders(route.params.id);
  });

  function onFilterTextBoxChanged() {
    grid.value.api.setGridOption("quickFilterText", document.getElementById("filter-text-box").value);
  }

  function onSelectionChanged() {
    itemsSelected.value = grid.value.api.getSelectedRows();
  }

  async function addRidersClick() {
    if(itemsSelected.value.length === 0) {
      alert("You must select at least one rider to add to your team.");

      return;
    }

    if(team.value.length >= Config.maxRiders) {
      alert("You already have " + Config.maxRiders + " riders on your team. You must drop riders before you can add more.");

      return;
    }
    
    if((team.value.length + itemsSelected.value.length) > Config.maxRiders) {
      alert("You have selected to many riders. You can only have " + Config.maxRiders + " riders on your team.");

      return;
    }

    itemsSelected.value.map(i => i.rowKey).forEach(async rowKey => {
      let sel = riders.value.find(r => r.rowKey == rowKey);
      let index = riders.value.indexOf(sel);

      riders.value.splice(index, 1);
      team.value.push(sel);

      await  storage.Teams.create({
        League: route.params.id,
        Member: member.value.rowKey,
        Rider: sel.rowKey
      });

      await storage.Feeds.create({
        League: route.params.id,
        Member: member.value.rowKey,
        Action: `Added rider ${sel.Name} to their team`
      });
    });
  }

function showRiderModal(key){
  riderModal.value.open(key);
}
</script>

<style lang="css" scoped>
.headshot {
  display: inline-block;
  position: relative;
  width: 35px;
  height: 35px;
  vertical-align: top;
    margin: -5px 10px -5px 0;
    border-radius: 50%;
    object-fit: cover;
}
</style>
