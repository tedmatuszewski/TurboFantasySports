<template>
  <div class="container my-5">
    <div class="row">
      <div class="col-md-4">
        <h3>Account Information</h3>

        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ user?.name }}</h5>
            <p class="card-text">{{ user?.sub }}</p>
          </div>
          
          <ul class="list-group list-group-flush">
            <li class="list-group-item">Name: {{ user?.name }}</li>
            <li class="list-group-item">Email: {{ user?.email }}</li> 
            <li class="list-group-item">Nickname: {{ user?.nickname }}</li>
            <li class="list-group-item">Email Verified: {{ user?.email_verified }}</li>
          </ul>

          <div class="card-footer">
            <small class="text-body-secondary">Last updated {{ user?.updated_at }}</small>
          </div>
        </div>
      </div>

      <div class="col-md-8">
        <div class="row">
          <div class="col-sm-8">
            <h3 class="text-center text-sm-left">Prior Leagues</h3>
          </div>

          <div class="col-sm-4 text-center text-sm-right">
            <select class="form-control" v-model="partition">
              <option v-for="season in seasons" :key="season.Partition" :value="season.Partition">{{ season.Season }}</option>
            </select>
          </div>
        </div>

        <ag-grid-vue :rowData="data" style="height: 320px;" :columnDefs="colDefs" :autoSizeStrategy="{ type: 'fitCellContents' }"></ag-grid-vue>
      </div>
    </div>

    <div style="display:none;">
      {{ JSON.stringify(user, null, 2) }}
    </div>
  </div>
</template>

<script setup>
  import { useAuth0 } from '@auth0/auth0-vue';
  import { AgGridVue } from "ag-grid-vue3";
  import { onMounted, ref, watch } from 'vue';
  import { useStorage } from '../storage/StorageContext';
  import TableViewPastLeague from '../components/TableViewPastLeague.vue'

  const auth0 = useAuth0();
  const user = auth0.user;
  const data = ref([]);
  const storage = useStorage();
  const seasons = ref([]);
  const partition = ref(1);
  const colDefs = ref([
    { field: "TeamName", headerName: "Team Name" },
    { field: "LeagueName", headerName: "League Name" },
    // { 
    //   headerName: "",
    //   field: "LeagueName",
    //   cellRenderer: TableViewPastLeague,
    // }
  ]);

  onMounted(() => {
    seasons.value = storage.getSeasonPartitionHistory();
  });

  watch(partition, async (newPartition) => {
    if (newPartition) {
      data.value = await storage.getSeasonHistory(user.value.email, newPartition);
    }
  }, { immediate: true });
    
</script>

