<template>
    <div>
        <vueper-slides
        class="no-shadow"
        :visible-slides="3"
        slide-multiple
        :gap="3"
        :slide-ratio="1 / 4"
        :dragging-distance="200"
        :breakpoints="{ 800: { visibleSlides: 2, slideMultiple: 2 } }">
            <vueper-slide v-for="(slide, i) in slides" :key="i" :title="slide.title" :content="slide.content">
                <template #content>
                    <div class="vueperslide__content-wrapper" :style="'background-color: ' + ['#ff5252', '#42b983'][i % 2]">
                        <div class="vueperslide__title">{{ i.toString() }}</div>
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

    // export default {
    //     components: { VueperSlides, VueperSlide },
    // }

    const slides = reactive([]);

    console.log(races);
    races.forEach(race => {
        slides.push({
            title: race.name,
            content: race.date
        });
    });
</script>

<style>

</style>