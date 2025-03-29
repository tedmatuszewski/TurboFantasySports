<template>
<div id="riderModal" class="modal" tabindex="-1">
  <div class="modal-dialog modal-xl">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Rider Profile</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
          <div class="text-center">
            <div>
              <Headshot :rider="rider" :width="100"></Headshot>
              <h4>{{ rider.Name }}</h4>
              <p class="text-muted mb-0">{{ teamDisplay }}</p>
            </div>

            <p class="mt-2 card-text">{{ rider.Injury }}</p>

            <div class="pt-3">
              <div class="row">
                <div class="col-4">
                  <h6>{{ rider.Entries }}</h6>
                  <p>Entries</p>
                </div>
                <div class="col-4">
                  <h6>{{ rider.AveragePlace }}</h6>
                  <p>ARP/R</p>
                </div>
                <div class="col-4">
                  <h6>{{ rider.AveragePoints }}</h6>
                  <p>AFP/R</p>
                </div>
              </div>
            </div>
          </div>

          <h4>Race Results</h4>
        <Vue3EasyDataTable :headers="headers" :items="historyTable" :rows-per-page="6">
         <template #item-date="{ Date }">
            <span>{{ Date.toLocaleDateString() }}</span>
          </template>
          <!-- <template #item-stats="{ RowKey, Name }">
            <a href="#" v-on:click.prevent="showRiderModal(RowKey)">{{ Name }}</a>
          </template>
          <template #item-link="{ RowKey }">
            <RacerLink :id="RowKey"></RacerLink>
          </template>
          <template #item-headshot="{ ImageUrl, Injury }">
            <Headshot :rider="{ ImageUrl, Injury }"></Headshot>
          </template> -->
        </Vue3EasyDataTable>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" v-on:click="close">Close</button>
      </div>
    </div>
  </div>
</div>
</template>

<script setup>
import { onMounted, defineExpose, ref, defineProps, computed } from 'vue';
import { useStorage } from '../storage/StorageContext';
import Vue3EasyDataTable from 'vue3-easy-data-table';
import Headshot from "../components/Headshot.vue";

const props = defineProps([ "league" ]);
const storage = useStorage();
const outcomes = ref([]);
const rider = ref({});
const team = ref({});
const member = ref(null);
const historyTable = ref([]);
const headers = [
  { text: "Place", value: "Place" },
  { text: "Race", value: "Race" },
  { text: "Date", value: "date" },
  { text: "Points", value: "Points" },
  // { text: "", value: "remove"},
  // { text: "Number", value: "Number", sortable: true },
  // { text: "", value: "headshot", width: 50 },
  // { text: "", value: "stats" },
  // { text: "", value: "link"},
  // { text: "Class", value: "Class", sortable: true },
  // { text: "E", value: "Entries", sortable: true },
  // { text: "MEQF", value: "TotalOutcomes", sortable: true },
  // { text: "TP", value: "TotalPoints", sortable: true },
  // { text: "AFP/R", value: "AveragePoints", sortable: true },
  // { text: "ARP/R", value: "AveragePlace", sortable: true },
  // { text: "Wins", value: "Wins", sortable: true },
  // { text: "Top3", value: "Podiums", sortable: true },
  // { text: "Top5", value: "TopFives", sortable: true },
  // { text: "Top10", value: "TopTens", sortable: true },
];

let modal = null;
let rowKey = null;

onMounted(async () => {
  modal = new bootstrap.Modal(document.getElementById('riderModal'), {});
});

const teamDisplay = computed(() => {
  if (member.value && member.value.TeamName) {
    return member.value.TeamName;
  }
  return "Available";
});

function open(id) {
  modal.show();
  outcomes.value = storage.Outcomes.getByRider(id);
  rider.value = storage.Riders.getSingle(id);
  team.value = storage.Teams.getByLeagueAndRider(props.league, id);
  member.value = storage.Members.get(team.value.Member);
  historyTable.value.length = 0;

  outcomes.value.forEach(outcome => {
    let race = storage.Races.getRaceByKey(outcome.Race);
    
    historyTable.value.push({
        Race: race.Name,
        Date: new Date(race.Date),
        Points: outcome.Points,
        Place: outcome.Place
    });
  });
  
  historyTable.value.sort((a, b) => a.Date - b.Date);
}

function close() {
  modal.hide();
}

defineExpose({ open });
</script>

<style>
/* Profile container */
.profile {
  margin: 20px 0;
}

/* Profile sidebar */
.profile-sidebar {
  padding: 20px 0 10px 0;
  background: #fff;
}

.profile-userpic img {
  float: none;
  margin: 0 auto;
  width: 50%;
  height: 50%;
  -webkit-border-radius: 50% !important;
  -moz-border-radius: 50% !important;
  border-radius: 50% !important;
}

.profile-usertitle {
  text-align: center;
  margin-top: 20px;
}

.profile-usertitle-name {
  color: #5a7391;
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 7px;
}

.profile-usertitle-job {
  text-transform: uppercase;
  color: #5b9bd1;
  font-size: 12px;
  font-weight: 600;
  margin-bottom: 15px;
}

.profile-userbuttons {
  text-align: center;
  margin-top: 10px;
}

.profile-userbuttons .btn {
  text-transform: uppercase;
  font-size: 11px;
  font-weight: 600;
  padding: 6px 15px;
  margin-right: 5px;
}

.profile-userbuttons .btn:last-child {
  margin-right: 0px;
}
    
.profile-usermenu {
  margin-top: 30px;
}

.profile-usermenu ul li {
  border-bottom: 1px solid #f0f4f7;
}

.profile-usermenu ul li:last-child {
  border-bottom: none;
}

.profile-usermenu ul li a {
  color: #93a3b5;
  font-size: 14px;
  font-weight: 400;
}

.profile-usermenu ul li a i {
  margin-right: 8px;
  font-size: 14px;
}

.profile-usermenu ul li a:hover {
  background-color: #fafcfd;
  color: #5b9bd1;
}

.profile-usermenu ul li.active {
  border-bottom: none;
}

.profile-usermenu ul li.active a {
  color: #5b9bd1;
  background-color: #f6f9fb;
  border-left: 2px solid #5b9bd1;
  margin-left: -2px;
}

/* Profile Content */
.profile-content {
  padding: 20px;
  background: #fff;
  min-height: 460px;
}
</style>