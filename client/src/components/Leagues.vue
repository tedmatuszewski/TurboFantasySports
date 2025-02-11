<template>
  <div class="next-steps">
    <h2 class="my-5 text-center">Available leagues</h2>

    <div v-for="(pair, index) in leaguePairs" :key="index" class="row mb-4">
      <div class="row">
        <div v-for="league in pair" :key="league.Id" class="col-md-6 mb-4">
          <h6 class="mb-3">{{ league.Name }}</h6>
          <p>{{ league.Description }}</p>
          <router-link v-if="league.isMember(auth0.user.value.sub)" :to="{ name: 'league', params: { id: league.Id } }" class="btn btn-primary">View</router-link>
          <button v-else @click="joinLeague(league)" class="btn btn-primary">Join</button>
        </div>
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
      console.log(league.isMember(auth0.user.value.sub));
    });
    console.log(leagues.value);
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
      LeagueGuid: league.Id,
      UserGuid: auth0.user.value.sub,
      TeamName: "Team " + auth0.user.value.name
    });
    
    league.Members.push(response);
  }
</script>
