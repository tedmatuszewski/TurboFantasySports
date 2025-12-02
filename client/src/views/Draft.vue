<template>
  <div class="container my-3">

    <div v-if="hasUnsetDraftPositions" class="my-3">
      <p>Draft has not been started. Click the 'Setup Draft' button to randomize the order of the league members. Once clicked, you will 
        see the draft order appear. After confirming the order, click the 'Start Draft' button to begin drafting riders.
      </p>
      
      <button v-on:click="randomizeOrder" class="btn btn-primary my-3">Setup Draft</button>
      <router-link :to="{ name: 'ManageLeague', params: { id: route.params.id } }" class="btn btn-secondary ml-2">Back to Manage Page</router-link>
    </div>

    <div v-else>
      <div class="row my-2">
        <div class="col-md-4">
          <h3 class="text-center text-md-left">Choosing Team</h3>
        </div>

        <div class="col-md-8 text-center text-md-right">
          <div class="d-flex">
            <div class="flex-grow-1 mx-2">
              <select class ="form-control" v-model="selectionStyle">
                <option :value="automatic">Automatically move to next team</option>
                <option :value="manual">Manually move to next team</option>
              </select>
            </div>

            <div class="text-center text-sm-right">
              <router-link :to="{ name: 'ManageLeague', params: { id: route.params.id } }" class="btn btn-primary mr-2">Manage League</router-link>
              <router-link :to="{ name: 'matchup', params: { id: route.params.id } }" class="btn btn-secondary">View Teams</router-link>
            </div>
          </div>
        </div>
      </div>

      <vueper-slides ref="slides" class="no-shadow" :visible-slides="3" slide-multiple :arrows="false" fixed-height="120px" :breakpoints="{ 800: { visibleSlides: 2, slideMultiple: 1 } }">
        <vueper-slide v-for="(member, i) in members" :key="i">
          <template #content>
            <div class="card position-relative" :class="{ 'border-warning': selecting == member.rowKey }" style="width: 20rem;">
              <div class="card-body">
                <h5 class="text-nowrap card-title">
                  <span class="badge text-bg-secondary">{{member.teamCount}}/{{ Config.maxRiders }}</span>
                  {{ member.TeamName }}
                </h5>
                <p class="card-text text-nowrap fs-6">{{ member.Email }}</p>
              </div>
              <span v-if="selecting == member.rowKey" class="position-absolute top-0 start-100 translate-middle badge text-bg-warning">Choosing</span>
            </div>
          </template>
        </vueper-slide>
      </vueper-slides>

      <div class="card text-bg-light mb-3" v-if="draftCompleteMessage != null">
        <div class="card-body">
          <h5 class="card-title">The draft is complete!</h5>
          <p class="card-text">{{ draftCompleteMessage }}</p>
            
          <button class="btn btn-secondary" v-if="league.DraftComplete == false || league.DraftComplete == null" v-on:click="completeDraftClick">Complete Draft</button>
        </div>
      </div>

      <div class="row">
        <div class="col-md-8 mb-3">
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
        
        <div class="col-md-4">
          <Feed></Feed>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref,onMounted,computed, reactive  } from "vue";
  import { AgGridVue } from "ag-grid-vue3";
  import { useStorage } from '../storage/StorageContext';
  import { useRoute } from 'vue-router';
  import { VueperSlides, VueperSlide } from 'vueperslides'
  import TableChoose from "../components/TableChoose.vue";
  import 'vueperslides/dist/vueperslides.css'
  import Config from "../config.json";
  import Feed from "../components/Feed.vue";

  const automatic = "automatic";
  const manual = "manual";
  const storage = useStorage();
  const route = useRoute();
  const members = ref([]);
  const riders = ref([]);
  const teams = ref([]);
  const selecting = ref(null);
  const selectionStyle = ref(automatic);
  const slides = ref(null);
  const league = ref({});

  const colDefs = ref([
    { 
      headerName: "Actions",
      field: "Name",
      pinned: 'left',
      cellRenderer: TableChoose,
      cellRendererParams: {
        click: chooseRider,
      }
    },
    { field: "Number", headerName: "Number" },
    { field: "Name", headerName: "Name" },
    { field: "Class", headerName: "LY Class" },
    { field: "TotalPoints", headerName: "LY Points", sort: "desc" }
  ]);
  
  const hasUnsetDraftPositions = computed(() => {
    return members.value.some(member => !member.DraftPosition);
  });

  const isDraftComplete = computed(() => {
    return members.value.every(member => member.teamCount >= Config.maxRiders);
  });

  const draftCompleteMessage = computed(() => {
    let message = null;

    if (isDraftComplete.value == true && league.value.DraftComplete == true) {
      message = "All teams have selected the maximum number of riders and the 'Complete Draft' button has been clicked. The league is now open for regular season management.";
    }

    if(isDraftComplete.value == true && (league.value.DraftComplete == false || league.value.DraftComplete == null)) {
      message = `Every team has selected ${ Config.maxRiders } riders. Click the 'Complete Draft' button to finalize the draft. This will open 
            up the league for regular season management`;
    }

    return message;
  });
  
  onMounted(() => {
    members.value = storage.Members.getByLeague2(route.params.id);
    riders.value = storage.getAvailableRiders(route.params.id);
    teams.value = storage.Teams.getByLeague2(route.params.id);
    league.value = storage.Leagues.getSingle(route.params.id);
    
    members.value.sort((a, b) => a.DraftPosition - b.DraftPosition);
    selecting.value = members.value[0].rowKey;

    members.value.forEach(member => {
      member.teamCount = teams.value.filter(t => t.Member === member.rowKey).length;
    });

    console.log(league.value);
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

    selecting.value = members.value[0].rowKey;
  }

  function next() {
    const currentIndex = members.value.findIndex(m => m.rowKey === selecting.value);
    const nextIndex = (currentIndex + 1) % members.value.length;

    slides.value.goToSlide(nextIndex);

    selecting.value = members.value[nextIndex].rowKey;
  }

  function previous() {
    const currentIndex = members.value.findIndex(m => m.rowKey === selecting.value);
    const previousIndex = (currentIndex - 1 + members.value.length) % members.value.length;

    slides.value.goToSlide(previousIndex);
    
    selecting.value = members.value[previousIndex].rowKey;
  }

  function chooseRider(rider) {
    const currentMember = members.value.find(m => m.rowKey === selecting.value);

    if(currentMember.teamCount >= Config.maxRiders) {
      alert("This team has already selected the maximum number of riders.");
      return;
    }

    storage.Teams.create({
      League: route.params.id,
      Member: selecting.value,
      Rider: rider.rowKey
    });
    
    storage.Feeds.create({ League: route.params.id, Member: selecting.value, Action: `Added rider ${rider.Name} to team` });
    
    if (currentMember) {
      currentMember.teamCount++;
    }

    riders.value = riders.value.filter(r => r.rowKey !== rider.rowKey);

    if (selectionStyle.value === automatic) {
      next();
    }
  }

  async function completeDraftClick() {
    if(confirm("Are you sure you want to complete the draft?") == false) {
      return;
    }

    league.value.DraftComplete = true;
    await storage.Leagues.update(league.value);
  }
</script>

<style lang="css" scoped>
  .vueperslides--fixed-height.vueperslides--bullets-outside {
    margin-bottom: 4em;
  }
</style>
