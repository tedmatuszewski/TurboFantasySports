<template>
    <a target="_blank" :href="Config.racerxRiderLink + id">
        <img style="height:25px;" src="../assets/racer-x-shield.png">
    </a>
</template>

<script setup>
    import { defineProps } from 'vue';
    import Config from "../data/config.json";
    const props = defineProps({
        id: { type: String }
    });
</script>