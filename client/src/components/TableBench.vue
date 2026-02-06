<template>
    <div class="d-flex align-items-center" style="height:100%;">
        🪑
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';
    
    const props = defineProps({
        params: { type: Object }
    });

    console.log("Bench Button Params:", props.params);
</script>