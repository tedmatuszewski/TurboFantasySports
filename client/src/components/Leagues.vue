<template>
  <div class="py-5">
    <div v-if="isAuthenticated">
      <h2 class="text-center">Available leagues</h2>
      <p class="text-center text-muted h5">Click the join button to join one of the available leagues. After clicking join you will see a view button. Click that to navigate to the league home page.</p>

      <div v-for="(pair, index) in leaguePairs" :key="index" class="row my-5">
          <div v-for="league in pair" :key="league.Id" class="col-md-6 mb-4">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">{{ league.Name }}</h5>
                <p class="card-text">{{ league.Description }}</p>
                <router-link v-if="isMember(league)" :to="{ name: 'league', params: { id: league.RowKey } }" class="btn btn-primary">View</router-link>
                <button v-else @click="joinLeague(league)" class="btn btn-primary">Join</button>
              </div>
            </div>
          </div>
      </div>
    </div>

    <div v-else>
      <h2 class="text-center text-muted">Please click login in the navbar to view available leagues</h2>
    </div>
  </div>
</template>

<script setup>
  import { ref,onMounted,computed,watch } from "vue";
  import { StorageContext } from "../storage/StorageContext";
  import { useAuth0 } from '@auth0/auth0-vue';
  
  const auth0 = useAuth0();
  
  let leagues = ref([]);
  let members = ref([]);
  let context;

  const isAuthenticated = computed(() => {
      return auth0.isAuthenticated.value;
  });

  onMounted(async () => {
    context = await StorageContext();

    if(isAuthenticated) {
      members.value = await context.Members.getAll();
      leagues.value = await context.Leagues.getAll();
    }
  });

  const leaguePairs = computed(() => {
    if (!leagues.value) return [];
    const pairs = [];
    for (let i = 0; i < leagues.value.length; i += 2) {
      pairs.push(leagues.value.slice(i, i + 2));
    }
    return pairs;
  });

  function isMember(league) {
    var member = members.value.find(m => m.League === league.RowKey && m.Email === auth0.user.value.email);

    return member !== undefined;
  }

  async function joinLeague(league) {
    let response = await context.Members.create({
      League: league.RowKey,
      Email: auth0.user.value.email,
      TeamName: "Team " + auth0.user.value.nickname
    });
    
    members.value.push(response);
  }
</script>
