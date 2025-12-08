<template>
    <div class="d-flex align-items-center" style="height:100%;">
        <button  v-on:click="props.params.click(props.params.data)" class="btn btn-danger btn-sm">Remove</button>
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';
    
    const props = defineProps({
        params: { type: Object }
    });
</script>