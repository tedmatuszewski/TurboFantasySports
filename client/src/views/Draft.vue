<template>
  <div class="container my-5">
    <div class="row my-2">
      <div class="col-md">
        <h3 class="text-center text-md-left">Choosing Team</h3>
      </div>
      <div class="col-md text-center text-md-right">
          <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary mr-2">League Home</router-link>
      </div>
    </div>

    <div v-if="hasUnsetDraftPositions" class="my-3">
      <p>Draft has not been started. Click the 'Setup Draft' button below to randomize the order of the league members. Once clicked, you will 
        see the draft order appear below. After confirming the order, click the 'Start Draft' button to begin drafting riders.
      </p>
      
      <button v-on:click="randomizeOrder" class="btn btn-primary my-3">Setup Draft</button>
    </div>
    <div v-else>
      <vueper-slides
      class="no-shadow"
      :visible-slides="3"
      slide-multiple
      :arrows="false"
      fixed-height="120px"
      :breakpoints="{ 800: { visibleSlides: 2, slideMultiple: 1 } }">
          <vueper-slide v-for="(member, i) in members" :key="i">
              <template #content>
                  <div class="card" :class="{ 'border-warning': selecting == member.RowKey }" style="width: 20rem;">
                    <div class="card-body">
                      <h5 class="card-title">
                        {{ member.TeamName }} 
                        <span v-if="selecting == member.RowKey" class="badge rounded-pill text-bg-warning">Choosing</span>
                      </h5>
                      <p class="card-text text-nowrap">{{ member.Email }}</p>
                    </div>
                  </div>
              </template>
          </vueper-slide>
      </vueper-slides>
    </div>
        
    <div class="row my-2">
      <div class="col-md">
        <h3 class="text-center text-md-left">Available Riders</h3>
      </div>

      <div class="col-md text-center text-md-right">
          <button class="btn btn-secondary mr-2" v-on:click="previous">⇦ Previous Team</button>
          <button class="btn btn-secondary mr-2" v-on:click="next">Next Team ⇨</button>
      </div>
    </div>

    <ag-grid-vue :rowData="riders" :columnDefs="colDefs" style="height: 320px;" :autoSizeStrategy="{ type: 'fitCellContents' }"></ag-grid-vue>
  </div>
</template>

<script setup>
  import { ref,onMounted,computed, reactive  } from "vue";
  import { AgGridVue } from "ag-grid-vue3";
  import { useStorage } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { VueperSlides, VueperSlide } from 'vueperslides'
  import 'vueperslides/dist/vueperslides.css'
  // import { useAuth0 } from '@auth0/auth0-vue';

  const storage = useStorage();
  const route = useRoute();
  const members = ref([]);
  const riders = ref([]);
  const teams = ref([]);
  const selecting = ref(null);
  // const league = ref({});
  // const auth0 = useAuth0();
  // const member = ref({});

  const colDefs = ref([
    { field: "Number", headerName: "Number" },
    { field: "Name", headerName: "Name" },
    { field: "Class", headerName: "LY Class" },
    { field: "TotalPoints", headerName: "LY Points" }
  ]);
  
  const hasUnsetDraftPositions = computed(() => {
    return members.value.some(member => !member.DraftPosition);
  });

  onMounted(async () => {
    members.value = await storage.Members.getByLeague2(route.params.id);
    riders.value = await storage.getAvailableRiders(route.params.id);
    teams.value = await storage.Teams.getByLeague2(route.params.id);
    
    members.value.sort((a, b) => a.DraftPosition - b.DraftPosition);
    selecting.value = members.value[0].RowKey;

    console.log(riders.value);
  });

  async function randomizeOrder() {
    members.value = members.value
      .map(value => ({ value, sort: Math.random() }))
      .sort((a, b) => a.sort - b.sort)
      .map(({ value }) => value);

    members.value.forEach(async (member, index) => {
      member.DraftPosition = (index + 1);
      await storage.Members.update(member);
    });
  }

  function next() {
    const currentIndex = members.value.findIndex(m => m.RowKey === selecting.value);
    const nextIndex = (currentIndex + 1) % members.value.length;

    selecting.value = members.value[nextIndex].RowKey;
  }

  function previous() {
    const currentIndex = members.value.findIndex(m => m.RowKey === selecting.value);
    const previousIndex = (currentIndex - 1 + members.value.length) % members.value.length;

    selecting.value = members.value[previousIndex].RowKey;
  }
</script>

<style lang="css" scoped>
  .vueperslides--fixed-height.vueperslides--bullets-outside {
    margin-bottom: 4em;
  }
</style>
