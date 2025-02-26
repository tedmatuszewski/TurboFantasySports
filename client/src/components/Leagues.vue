<template>
  <div class="py-5">
    <h2 class="text-center">Available leagues</h2>
    <p class="text-center text-muted h5">Click the join button to join one of the available leagues. After clicking join you will see a view button. Click that to navigate to the league home page.</p>

    <div v-for="(pair, index) in leaguePairs" :key="index" class="row my-5">
        <div v-for="league in pair" :key="league.Id" class="col-md-6 mb-4">
          <h6 class="mb-3">{{ league.Name }}</h6>
          <p>{{ league.Description }}</p>
          <router-link v-if="league.isMember(auth0.user.value.sub)" :to="{ name: 'league', params: { id: league.RowKey } }" class="btn btn-primary">View</router-link>
          <button v-else @click="joinLeague(league)" class="btn btn-primary">Join</button>
        </div>
    </div>
  </div>
</template>

<script setup>
  import { ref,onMounted,computed  } from "vue";
  import { StorageContext } from "../storage/StorageContext";
  import { useAuth0 } from '@auth0/auth0-vue';
  
  const auth0 = useAuth0();
  
  let leagues = ref(null);
  let members = ref(null);
  let context;

  onMounted(async () => {
    context = await StorageContext();

    members.value = await context.Members.getAll();
    leagues.value = await context.Leagues.getAll();

    leagues.value.forEach(league => {
      league.setMembers(members.value);
    });
  });

  const leaguePairs = computed(() => {
    if (!leagues.value) return [];
    const pairs = [];
    for (let i = 0; i < leagues.value.length; i += 2) {
      pairs.push(leagues.value.slice(i, i + 2));
    }
    return pairs;
  });

  async function joinLeague(league) {
    let response = await context.Members.create({
      LeagueGuid: league.RowKey,
      UserGuid: auth0.user.value.sub,
      TeamName: "Team " + auth0.user.value.name
    });
    
    league.Members.push(response);
  }
</script>
