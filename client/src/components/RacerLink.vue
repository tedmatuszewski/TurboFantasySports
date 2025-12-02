<template>
        <a target="_blank" :href="Config.racerxRiderLink + props.params.data.rowKey">
            <img style="height:25px;" src="../assets/racer-x-shield.png">
        </a>
</template>

<script setup>
    import { defineProps } from 'vue';
    import Config from "../config.json";
    
    const props = defineProps({
        params: { type: Object }
    });
</script>