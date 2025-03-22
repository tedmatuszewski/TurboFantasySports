<template>
  <div class="container my-5">
    <h3>Races</h3>
    <races></races>

    <div class="row">
      <div class="col-md-8">
        <LeagueHeader></LeagueHeader>
      </div>
      
      <div class="col-md-4">
        <Position></Position>
      </div> 
    </div>
        
    <div class="alert alert-primary" role="alert">
      You can have up to {{ Config.maxRiders }} riders on your team. You can edit hour lineup up to the start of the current weeks race. Once the race begins, you will no longer see the edit roster buttons. The roster editing ability will become unlock as soon as the admin of the website uploads the race rasults.
    </div>
    
    <h3>My Roster</h3>

    <Vue3EasyDataTable :headers="headers" :items="myRidersList" :rows-per-page="6" :hide-footer="true">
      <template #item-remove="{ RowKey }">
        <button class="btn btn-sm btn-danger" :disabled="!isRosterEditable" v-on:click="removeRiderClick(RowKey)">Remove</button>
      </template>
      <template #item-link="{ RowKey }">
        <RacerLink :id="RowKey"></RacerLink>
      </template>
      <template #item-headshot="{ ImageUrl, Injury }">
        <Headshot :rider="{ ImageUrl, Injury }"></Headshot>
      </template>
    </Vue3EasyDataTable>

    <router-link :to="{ name: 'riders', params: { id: route.params.id } }" class="btn btn-primary btn-block mt-3">View Available Riders</router-link>

    <div class="row">
      <div class="col-md-6 py-5">
        <Feed></Feed>
      </div>
      
      <div class="col-md-6 py-5">
        <!-- <Trades v-if="Config.showTrades"></Trades>-->
      </div>
    </div>  
  </div>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, reactive  } from "vue";
  import { useRoute } from 'vue-router';
  import { useAuth0 } from '@auth0/auth0-vue';
  import Races from "../components/Races.vue";
  import Position from "../components/Position.vue";
  import Countdown from "../components/Countdown.vue";
  import Config from "../config.json";
  import RacerLink from "../components/RacerLink.vue";
  import Feed from "../components/Feed.vue";
  import Vue3EasyDataTable from 'vue3-easy-data-table';
  import Headshot from "../components/Headshot.vue";
  import LeagueHeader from "../components/LeagueHeader.vue";
  import Trades from "../components/Trades.vue";

  const storage = useStorage();
  const auth0 = useAuth0();
  const route = useRoute();
  const headers = [
    { text: "", value: "remove"},
    { text: "Number", value: "Number", sortable: true },
    { text: "", value: "headshot", width: 50 },
    { text: "Rider", value: "Name", sortable: true },
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

  let member = null;
  let myRidersList = reactive([]);
  let league = ref(null);
  let prev = storage.Races.getPreviousRace();
  let isRosterEditable = ref(false);

  onMounted(async () => {
    league.value = storage.Leagues.getSingle(route.params.id);
    isRosterEditable.value = storage.Results.hasResults2(route.params.id, prev.RowKey);
    
    member = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
    let team = storage.Teams.getByLeagueAndMember2(route.params.id, member.RowKey);
    
    storage.Riders.data.forEach(rider => {
      let mine = team.map(a => a.Rider).indexOf(rider.RowKey);
      
      if (mine > -1) {
        myRidersList.push(rider);
      }
    });
  });

  async function removeRiderClick(key){
    if(confirm("Are you sure that you want to remove this rider from your team? Removing this rider will put them back in the pool of available riders for anyone else to scoop up.") == false) {
      return;
    }
    let sel = myRidersList.find(r => r.RowKey == key);
    let index = myRidersList.indexOf(sel);
    
    myRidersList.splice(index, 1);
    
    console.log(route.params.id, member.RowKey,sel.RowKey);
    let toDelete = await  storage.Teams.getByLeagueAndOwnerAndNumber2(route.params.id, member.RowKey, sel.RowKey);

    toDelete.forEach(async d => {
      await storage.Teams.remove(d.RowKey);
    });

    await storage.Feeds.create({ League: route.params.id, Member: member.RowKey, Action: `Removed rider ${sel.Name} from their team` });
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
