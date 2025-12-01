<template>
    <div class="d-flex align-items-center" style="height:100%;">
        <button  v-on:click="props.params.click(props.params.data)" class="btn btn-success btn-sm mr-2">Choose</button>
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';
    
    const props = defineProps({
        params: { type: Object }
    });
</script>