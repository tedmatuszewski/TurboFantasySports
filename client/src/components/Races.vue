<template>
    <div>
        <vueper-slides
        class="no-shadow"
        :visible-slides="3"
        slide-multiple
        :gap="3"
        :slide-ratio="1 / 4"
        :dragging-distance="200"
        :initSlide="5"
        fixed-height="130px"
        :breakpoints="{ 800: { visibleSlides: 2, slideMultiple: 2 } }">
            <vueper-slide v-for="(race, i) in races" :key="i">
                <template #content>
                    <div class="card" style="width: 18rem;">
                        <div class="card-body">
                            <h5 class="card-title">{{ race.name }}</h5>
                            <h6 class="card-subtitle mb-2 text-body-secondary">{{ formatDate(race.date) }}</h6>
                            <p class="card-text">{{ race.class }}</p>
                            <!-- <a href="#" class="card-link">Card link</a>
                            <a href="#" class="card-link">Another link</a> -->
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

    function formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
        return new Date(dateString).toLocaleDateString(undefined, options);
    }
</script>

<style>

</style>