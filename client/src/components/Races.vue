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
                            <a v-bind:style="{ visibility: showResultLink(race) ? 'hidden' : 'visible' }" href="#" class="card-link">Results</a>
                            <!-- <a href="#" class="card-link">Another link</a> -->
                        </div>
                    </div>
                </template>
            </vueper-slide>
        </vueper-slides>
    </div>
</template>

<script setup>
    // https://antoniandre.github.io/vueper-slides/?ref=madewithvuejs.com
    import { VueperSlides, VueperSlide } from 'vueperslides'
    import 'vueperslides/dist/vueperslides.css'
    import races from '../data/races.json';
    import { reactive  } from "vue";

    const initSlide = reactive(getNextUpcomingRaceIndex());

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

    function showResultLink(race) {
        const now = new Date();
        const raceDate = new Date(race.date);
        
        if (raceDate <= now) {
            return false;
        } else {
            return true;
        }
    }
</script>

<style>
.text-bg-warning {
    color: #000 !important;
    background-color: RGBA(255, 193, 7, 1) !important;
}
</style>