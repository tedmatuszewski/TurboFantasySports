<template>
  <div class="py-5">
    <div v-if="isAuthenticated">
      <h2 class="text-center">Current leagues</h2>

      <div v-for="(pair, index) in leaguePairs" :key="index" class="row my-5">
          <div v-for="league in pair" :key="league.Id" class="col-md-6 mb-4">
            <div class="card">
              <div class="row g-0">
                <div class="col-8">
                  <div class="card-body">
                    <h5 class="card-title">{{ league.Name }}</h5>
                    <p class="card-text">{{ league.Description }}</p>
                  </div>
                </div>
                <div class="col-4">
                  <div class="card-body d-flex justify-content-end align-items-center h-100">
                    <router-link v-if="isMember(league)" :to="{ name: 'league', params: { id: league.RowKey } }" class="btn btn-primary">View</router-link>
                  </div>
                </div>
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
  import { useStorage } from "../storage/StorageContext";
  import { useAuth0 } from '@auth0/auth0-vue';
  
  const auth0 = useAuth0();
  const storage = useStorage();
  
  let leagues = ref([]);
  let members = ref([]);

  const isAuthenticated = computed(() => {
      return auth0.isAuthenticated.value;
  });

  onMounted(async () => {
    if(isAuthenticated) {
      members.value =  storage.Members.data;
      leagues.value =  storage.Leagues.data;
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
</script>
