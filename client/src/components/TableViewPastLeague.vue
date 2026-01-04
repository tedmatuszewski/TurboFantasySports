<template>
    <div class="d-flex align-items-center" style="height:100%;">
        <router-link :to="{ name: 'standings', params: { id: props.params.data.LeagueId }, query: { partition: props.params.data.Partition } }">View Results</router-link>
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';
    
    const props = defineProps({
        params: { type: Object }
    });
</script>