<template>
    <div class="d-flex align-items-center" style="height:100%;">
        <Headshot :rider="props.params.data" :width="29" />

        <div class="flex-grow-1">
            <a href="#" v-on:click.prevent="props.params.click(props.params.data.rowKey)">{{ props.params.data.Name }}</a>
        </div>

        <div>
            <a target="_blank" :href="Config.racerxRiderLink + props.params.data.rowKey">
                <img style="height:25px;" src="../assets/racer-x-shield.png">
            </a>
        </div>
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';
    import Config from "../config.json";
    import Headshot from './Headshot.vue';
    
    const props = defineProps({
        params: { type: Object }
    });
</script>