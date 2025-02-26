<template>
    <div>
        <vueper-slides
        class="no-shadow"
        :visible-slides="3"
        slide-multiple
        :gap="3"
        :slide-ratio="1 / 4"
        :dragging-distance="200"
        :initSlide=initSlide
        :arrows="false"
        fixed-height="200px"
        :breakpoints="{ 800: { visibleSlides: 2, slideMultiple: 2 } }">
            <vueper-slide v-for="(race, i) in races" :key="i">
                <template #content>
                    <div class="card" style="width: 18rem;">
                        <div class="card-body">
                            <h5 class="card-title">{{ race.name }} <span v-if="isNextUpcomingRace(race)" class="badge rounded-pill text-bg-warning">Next Race</span></h5>
                            <h6 class="card-subtitle mb-2 text-body-secondary">{{ formatDate(race.date) }}</h6>
                            <p class="card-text">{{ race.class }}</p>
                            <a target="_blank" :href="getRaceLink(race)" class="card-link mr-3">Info</a>
                            <router-link v-if="showResultLink(race)" :to="{ name: 'result', params: { race: race.key } }">Results</router-link>
                        </div>
                    </div>
                </template>
            </vueper-slide>
        </vueper-slides>
    </div>
</template>

<script setup>
    // https://antoniandre.github.io/vueper-slides/?ref=madewithvuejs.com
    import { defineProps } from 'vue';
    import { VueperSlides, VueperSlide } from 'vueperslides'
    import 'vueperslides/dist/vueperslides.css'
    import races from '../data/races.json';
    import { reactive  } from "vue";
    import config from '../data/config.json';

    const props = defineProps({
        race: { type: String }
    });

    let initSlide = reactive(getNextUpcomingRaceIndex());

    if(props.race !== undefined) {
        initSlide = 1;
        let race = races.find(r => r.key === props.race);
        let index = races.indexOf(race);

        initSlide = index;
    }

    function formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
        return new Date(dateString).toLocaleDateString(undefined, options);
    }

    function isNextUpcomingRace(race) {
        const now = new Date();
        const raceDate = new Date(race.date);

        if (raceDate <= now) return false;

        const upcomingRace = races.filter(r => new Date(r.date) > now)[0];
        
        return race.key === upcomingRace.key;
    }

    function getNextUpcomingRaceIndex() {
        for(let i = 0; i<races.length; i++) {
            if (isNextUpcomingRace(races[i])) {
                return (i+1);
            }
        }
    }

    function getRaceLink(race) {
        // https://racerxonline.com/sx/2025/daytona
        const year = new Date().getFullYear();
        return config.racerx + "/sx/" + year + "/" + race.key;
    }

    function showResultLink(race) {
        const now = new Date();
        const raceDate = new Date(race.date);
        
        if (raceDate <= now) {
            return true;
        } else {
            return false;
        }
    }
</script>

<style>
.text-bg-warning {
    color: #000 !important;
    background-color: RGBA(255, 193, 7, 1) !important;
}
</style>