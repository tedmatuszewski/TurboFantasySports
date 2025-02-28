<template>
    <div class="pricing card-deck flex-column flex-md-row my-3">
        <div class="card card-pricing text-center px-3 mb-4">
            <span class="h6 w-60 mx-auto px-4 py-1 rounded-bottom bg-primary text-white shadow-sm">Standings</span>
            <div class="bg-transparent card-header pt-4 border-0">
                <h1 class="h1 font-weight-normal text-primary text-center mb-0"><span class="price">{{ pointsDisplay }}</span><span class="h6 text-muted ml-2">place</span></h1>
            </div>
            <div class="card-body pt-0">
                <ul class="list-unstyled mb-4">
                    <li>You have {{ numOf250Riders }} 250 riders</li>
                    <li>You have {{ numOf450Riders }} 450 riders</li>
                    <li>You have {{ (numOf250Riders + numOf450Riders) }} of {{ Config.maxRiders }} spots filled</li>
                    <li>Your current score is {{ totalPoints }}</li>
                </ul>
                <router-link :to="{ name: 'standings', params: { id: route.params.id } }" class="btn btn-secondary mb-3">Points Standings</router-link>
                <router-link :to="{ name: 'matchup', params: { id: route.params.id } }" class="btn btn-secondary mb-3">Current Matchup</router-link>
            </div>
        </div>
    </div> 
</template>

<script setup>
    import { StorageContext } from '../storage/StorageContext';
    import { ref,onMounted,computed, reactive  } from "vue";
    import { useRoute } from 'vue-router';
    import riderBank from '../data/riders.json';
    import Config from "../config.json";
    import { useAuth0 } from '@auth0/auth0-vue';

    const auth0 = useAuth0();
    const route = useRoute();
    let numOf250Riders = ref(0);
    let numOf450Riders = ref(0);
    let totalPoints = ref(0);
    let place = ref(0);
    let context = null;

    const pointsDisplay = computed(() => {
        switch (place.value) {
            case 0: return "1st";
            case 1: return "2nd";
            case 2: return "3rd";
            case 3: return "4th";
            case 4: return "5th";
            case 5: return "6th";
            case 6: return "7th";
            case 7: return "8th";
            case 8: return "9th";
            case 9: return "10th";
            case 10: return "11th";
        }
    });

    onMounted(async () => {
        context = await StorageContext();

        let member = await context.Members.getByLeagueAndEmail(route.params.id, auth0.user.value.email);
        let team = await context.Teams.getByLeagueAndMember(route.params.id, member.RowKey);
        let results = await context.Results.getByLeague(route.params.id);
        let ids = team.map(t => t.Rider);
        let totals = {};
        let myRiders = riderBank.filter(rider => {
            return ids.indexOf(rider.id) !== -1;
        });

        results.forEach(r => {
            if(totals[r.Member] === undefined) {
                totals[r.Member] = 0;
            }
            
            totals[r.Member] += r.Points;
        });
        
        let standings = Object.entries(totals)
            .map(([member, points]) => ({ member, points }))
            .sort((a, b) => b.points - a.points);
        
        place.value = standings.findIndex(s => s.member === member.RowKey);
        totalPoints.value = results.filter(r => r.Member === member.RowKey).reduce((acc, result) => acc + result.Points, 0);
        numOf250Riders.value = myRiders.filter(t => t.class === 250).length;
        numOf450Riders.value = myRiders.filter(t => t.class === 450).length;
    });
</script>

<style scoped>

</style>