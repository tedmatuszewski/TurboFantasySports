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
            <tr v-for="rider in myRidersList" :value="rider.Number">
              <td>{{rider.Number}}</td>
              <td>{{rider.Name}}</td>
              <td><RacerLink :id="rider.RowKey"></RacerLink></td>
              <td>{{rider.Class}}</td>
              <td><button class="btn btn-sm btn-danger" :disabled="!isRosterEditable" v-on:click="removeRiderClick(rider)">Remove</button></td>
            </tr>
          </tbody>
        </table>

        <router-link :to="{ name: 'riders', params: { id: route.params.id } }" class="btn btn-primary btn-block">View Available Riders</router-link>
      </div>
      
      <div class="col-md-4">
        <Position></Position>
      </div> 
    </div>

    <div class="row">
      <div class="col-md-6 py-5">
        <Feed></Feed>
      </div>
      
      <div class="col-md-6 py-5">
        <Trades v-if="Config.showTrades"></Trades>
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
  import Trades from "../components/Trades.vue";

  const storage = useStorage();
  const auth0 = useAuth0();
  const route = useRoute();

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

  async function removeRiderClick(rider){
    if(confirm("Are you sure that you want to remove this rider from your team? Removing this rider will put them back in the pool of available riders for anyone else to scoop up.") == false) {
      return;
    }
    let sel = myRidersList.find(r => r.RowKey == rider.RowKey);
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
