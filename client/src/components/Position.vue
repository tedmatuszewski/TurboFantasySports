<template>
    <div class="pricing card-deck flex-column flex-md-row my-3">
        <div class="card card-pricing text-center px-3 mb-4">
            <span class="h6 w-60 mx-auto px-4 py-1 rounded-bottom bg-primary text-white shadow-sm">My Team</span>
            <div class="bg-transparent card-header pt-4 border-0">
                <h1 class="h1 font-weight-normal text-primary text-center mb-0"><span class="price">{{ pointsDisplay }}</span><span class="h6 text-muted ml-2">place</span></h1>
            </div>
            <div class="card-body pt-0">
                <ul class="list-unstyled mb-4">
                    <li>{{ member?.TeamName }}</li>
                </ul>
                
                <ul class="list-unstyled mb-4">
                    <li>You were <span class="badge rounded-pill bg-danger text-white">{{ getOrdinalSuffix(member.DraftPosition) }}</span> in draft order</li>
                    <li>You have {{ numOf250Riders }} 250 riders</li>
                    <li>You have {{ numOf450Riders }} 450 riders</li>
                    <li>You have {{ (numOf250Riders + numOf450Riders) }} of {{ Config.maxRiders }} spots filled</li>
                    <li>Your current score is {{ totalPoints }}</li>
                </ul>
                <router-link :to="{ name: 'standings', params: { id: route.params.id } }" class="btn btn-secondary btn-block mb-2">Points Standings</router-link>
                <router-link :to="{ name: 'matchup', params: { id: route.params.id } }" class="btn btn-secondary btn-block mb-2">Current Matchup</router-link>
                <button v-on:click="btnChangeTeamNameClick" class="btn btn-secondary btn-block">Change Team Name</button>
            </div>
        </div>
    </div> 
</template>

<script setup>
    import { useStorage } from '../storage/StorageContext';
    import { ref,onMounted,computed, reactive  } from "vue";
    import { useRoute } from 'vue-router';
    import Config from "../config.json";
    import { useAuth0 } from '@auth0/auth0-vue';

    const storage = useStorage();
    const auth0 = useAuth0();
    const route = useRoute();

    let numOf250Riders = ref(0);
    let numOf450Riders = ref(0);
    let totalPoints = ref(0);
    let place = ref(0);
    let member = null;

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

    async function btnChangeTeamNameClick() {
        let newName = prompt("Enter new team name", member?.TeamName);
        let oldName = member?.TeamName;

        if(newName !== null) {
            member.TeamName = newName;
            await storage.Members.update(member);
            await storage.Feeds.create({
                League: route.params.id,
                Member: member.rowKey,
                Action: `Changed team name from ${oldName} to ${newName}`
            });
        }
    }

    onMounted(async () => {
        member = storage.Members.getByLeagueAndEmail2(route.params.id, auth0.user.value.email);
        
        let team = storage.Teams.getByLeagueAndMember2(route.params.id, member.rowKey);
        let results = storage.Results.getByLeague2(route.params.id);
        let riders = storage.Riders.data;
        let ids = team.map(t => t.Rider);
        let totals = {};
        let myRiders = riders.filter(rider => {
            return ids.indexOf(rider.rowKey) !== -1;
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
        
        place.value = standings.findIndex(s => s.member === member.rowKey);
        totalPoints.value = results.filter(r => r.Member === member.rowKey).reduce((acc, result) => acc + result.Points, 0);
        numOf250Riders.value = myRiders.filter(t => t.Class.indexOf("250") !== -1).length;
        numOf450Riders.value = myRiders.filter(t => t.Class.indexOf("450") !== -1).length;
    });

    function getOrdinalSuffix(num) {
        if (num == null || num === undefined) {
            return "-";
        }

        const j = num % 10;
        const k = num % 100;
        if (j === 1 && k !== 11) {
            return num + "st";
        }
        if (j === 2 && k !== 12) {
            return num + "nd";
        }
        if (j === 3 && k !== 13) {
            return num + "rd";
        }
        return num + "th";
    }
</script>

<style scoped>

</style>