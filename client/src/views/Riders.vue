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
      <template #item-stats="{ RowKey, Name }">
        <a href="#" v-on:click.prevent="showRiderModal(RowKey)">{{ Name }}</a>
      </template>
    </Vue3EasyDataTable>
  </div>
    
  <Rider ref="riderModal" :league="props.league"></Rider>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, defineProps  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import Config from "../config.json";
  import RacerLink from "../components/RacerLink.vue";
  import Vue3EasyDataTable from 'vue3-easy-data-table';
  import Headshot from "../components/Headshot.vue";
  import Rider from "../components/Rider.vue";
  import router from "../router";

  const props = defineProps([ "league" ]);
  const headers = [
    { text: "Number", value: "Number", sortable: true },
    { text: "", value: "headshot", width: 50 },
    { text: "", value: "stats", sortable: true },
    { text: "", value: "link"},
    { text: "Class", value: "Class", sortable: true },
    { text: "E", value: "Entries", sortable: true },
    { text: "MEQF", value: "TotalOutcomes", sortable: true },
    { text: "TP", value: "TotalPoints", sortable: true },
    { text: "AFP/R", value: "AveragePoints", sortable: true },
    { text: "ARP/R", value: "AveragePlace", sortable: true },
    { text: "Wins", value: "Wins", sortable: true },
    { text: "Top3", value: "Podiums", sortable: true },
    { text: "Top5", value: "TopFives", sortable: true },
    { text: "Top10", value: "TopTens", sortable: true },
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
  let riderModal = ref(null);

  const availableSpots = computed(() => {
    return Config.maxRiders - team.value.length;
  });

  const canClickAdd = computed(() => {
    return availableSpots.value === 0;
  });

  onMounted(() => {
    member.value = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
    team.value = storage.Teams.getByLeagueAndMember2(route.params.id, member.value.RowKey);
    riders.value = storage.getAvailableRiders(route.params.id);
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
