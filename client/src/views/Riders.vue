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
          <li class="list-group-item">RPI = Races Positioned In</li>
          <li class="list-group-item">E = Number of Races Entered</li>
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

    <input type="text" class="form-control mb-3" placeholder="Search for a rider" v-model="searchValue">

    <Vue3EasyDataTable
        :headers="headers"
        :items="riders"
        :search-field="searchField"
        :search-value="searchValue"
        :rows-per-page="100"
        v-model:items-selected="itemsSelected"
        @click-row="rowClick"
        :hide-footer="true">
      <template #item-link="{ RowKey }">
        <RacerLink :id="RowKey"></RacerLink>
      </template>
      <template #item-headshot="{ ImageUrl, Injury }">
        <Headshot :rider="{ ImageUrl, Injury }"></Headshot>
      </template>
    </Vue3EasyDataTable>
  </div>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, reactive  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import Config from "../config.json";
  import RacerLink from "../components/RacerLink.vue";
  import Vue3EasyDataTable from 'vue3-easy-data-table';
  import Headshot from "../components/Headshot.vue";
  import router from "../router";

  const headers = [
    { text: "Number", value: "Number", sortable: true },
    { text: "", value: "headshot", width: 50 },
    { text: "Rider", value: "Name", sortable: true },
    { text: "", value: "link"},
    { text: "Class", value: "Class", sortable: true },
    { text: "E", value: "Entries", sortable: true },
    { text: "AFP/R", value: "Afpr", sortable: true },
    { text: "ARP/R", value: "Arpr", sortable: true },
    { text: "RPI", value: "Rpi", sortable: true }
  ];

  const storage = useStorage();
  const itemsSelected = ref([]);
  const searchField = ref("Name");
  const searchValue = ref("");
  const auth0 = useAuth0();
  const route = useRoute();
  const teams = ref([]);
  const team = ref([]);
  
  let member = ref(null);
  let riders = ref([]);
  let outcomes = ref([]);
  let riderModal = null;

  const availableSpots = computed(() => {
    return Config.maxRiders - team.value.length;
  });

  const canClickAdd = computed(() => {
    return availableSpots.value === 0;
  });

  onMounted(() => {
    riderModal = new bootstrap.Modal(document.getElementById('riderModal'), {});

    member.value = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
    teams.value = storage.Teams.getByLeague2(route.params.id);
    team.value = storage.Teams.getByLeagueAndMember2(route.params.id, member.value.RowKey);
    outcomes.value = storage.Outcomes.data;

    storage.Riders.data.forEach(rider => {
      let all = teams.value.map(a => a.Rider).indexOf(rider.RowKey);
      let totalPoints = outcomes.value.filter(o => o.Rider == rider.RowKey).reduce((a, b) => a + b.Points, 0);
      let totalPosition = outcomes.value.filter(o => o.Rider == rider.RowKey).reduce((a, b) => a + b.Place, 0);
      let races = outcomes.value.filter(o => o.Rider == rider.RowKey).length;
      
      if(all === -1) {
        rider.Afpr = (totalPoints/rider.Entries).toFixed(2);
        rider.Arpr = (totalPosition/rider.Entries).toFixed(2);
        rider.Rpi = races;
        riders.value.push(rider);
      }
    });
  });

  function rowClick(row, e) {
    e.srcElement.closest("tr").querySelector("input[type=checkbox]").click();
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

    itemsSelected.value.map(i => i.RowKey).forEach(async RowKey => {
      let sel = riders.value.find(r => r.RowKey == RowKey);
      let index = riders.value.indexOf(sel);

      riders.value.splice(index, 1);
      team.value.push(sel);

      await  storage.Teams.create({
        League: route.params.id,
        Member: member.value.RowKey,
        Rider: sel.RowKey
      });

      await storage.Feeds.create({
        League: route.params.id,
        Member: member.value.RowKey,
        Action: `Added rider ${sel.Name} to their team`
      });
    });
    
    //router.push({ name: 'league', params: { id: route.params.id } });
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
