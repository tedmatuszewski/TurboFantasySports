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
        :breakpoints="{ 800: { visibleSlides: 2, slideMultiple: 2 }, 600: { visibleSlides: 1, slideMultiple: 1 } }">
            <vueper-slide v-for="(race, i) in races" :key="i">
                <template #content>
                    <div class="card" style="width: 18rem;">
                        <div class="card-body">
                            <h5 class="card-title">{{ race.Name }} <span v-if="isNextUpcomingRace(race)" class="badge rounded-pill text-bg-warning">Next Race</span></h5>
                            <h6 class="card-subtitle mb-2 text-body-secondary">{{ formatDate(race.Date) }}</h6>
                            <p class="card-text">{{ race.Lites }}</p>
                            <a target="_blank" :href="getRaceLink(race)" class="card-link mr-3">Info</a>
                            <router-link v-if="showResultLink(race)" :to="{ name: 'result', params: { race: race.rowKey } }">Results</router-link>
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
    import config from '../config.json';
    import { useStorage } from '../storage/StorageContext';

    const props = defineProps({
        race: { type: String }
    });

    let storage = useStorage();
    let races = storage.Races.data;
    let initSlide = storage.Races.getNextUpcomingRaceIndex();

    if(props.race !== undefined) {
        let race = races.find(r => r.rowKey === props.race);
        let index = races.indexOf(race);

        initSlide = index;
    }

    function isNextUpcomingRace(race) {
        return storage.Races.isNextUpcomingRace(race);
    }

    function formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
        return new Date(dateString).toLocaleDateString(undefined, options);
    }

    function getRaceLink(race) {
        // https://racerxonline.com/sx/2025/daytona
        const year = new Date().getFullYear();
        return config.racerx + "/" + race.Racerx;
    }

    function showResultLink(race) {
        const now = new Date();
        const raceDate = new Date(race.Date);
        
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