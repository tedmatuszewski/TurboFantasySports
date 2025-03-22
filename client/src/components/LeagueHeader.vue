<template>
    <div class="pricing card-deck flex-column flex-md-row my-3">
        <div class="card card-pricing px-3">
            <span class="h6 w-60 mx-auto px-4 py-1 rounded-bottom bg-primary text-white shadow-sm">League</span>
            <!-- <div class="bg-transparent card-header pt-4 border-0">
                <h1 class="h1 font-weight-normal text-primary text-center mb-0"><span class="price">{{ pointsDisplay }}</span><span class="h6 text-muted ml-2">place</span></h1>
            </div> -->
            <div class="card-body mt-3 pt-0">
                <h1>{{league?.Name}}</h1>
                <p>{{league?.Description}}</p>    
                
                <hr class="my-5" />
                
                <p>Countdown to the next race. When this runs out, your roster will be locked until the race is over and results are posted.</p>

                <div class="text-center my-2">
                <Countdown></Countdown>
                </div>
                <!-- <ul class="list-unstyled mb-4">
                    <li>{{ member?.TeamName }}</li>
                </ul>
                
                <ul class="list-unstyled mb-4">
                    <li>You have {{ numOf250Riders }} 250 riders</li>
                    <li>You have {{ numOf450Riders }} 450 riders</li>
                    <li>You have {{ (numOf250Riders + numOf450Riders) }} of {{ Config.maxRiders }} spots filled</li>
                    <li>Your current score is {{ totalPoints }}</li>
                </ul>
                <router-link :to="{ name: 'standings', params: { id: route.params.id } }" class="btn btn-secondary btn-block mb-2">Points Standings</router-link>
                <router-link :to="{ name: 'matchup', params: { id: route.params.id } }" class="btn btn-secondary btn-block mb-2">Current Matchup</router-link>
                <button v-on:click="btnChangeTeamNameClick" class="btn btn-secondary btn-block">Change Team Name</button> -->
            </div>
        </div>
    </div> 
</template>

<script setup>
    import { useStorage } from '../storage/StorageContext';
    import { ref,onMounted,computed, reactive  } from "vue";
    import { useRoute } from 'vue-router';
    import Countdown from "../components/Countdown.vue";
    import Config from "../config.json";
    import { useAuth0 } from '@auth0/auth0-vue';

    const storage = useStorage();
    const route = useRoute();
    const league = ref(null);

    onMounted(async () => {
        league.value = storage.Leagues.getSingle(route.params.id);
    });
</script>

<style scoped>

</style>