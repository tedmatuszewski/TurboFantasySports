<template>
    <div class="d-flex align-items-center" style="height:100%;">
        <button  v-on:click="props.params.edit(props.params.data)" class="btn btn-success btn-sm mr-2">Edit</button>
        <button  v-on:click="props.params.delete(props.params.data)" class="btn btn-danger btn-sm">Delete</button>
        <!-- <div class="flex-grow-1">
            <a href="#" v-on:click.prevent="props.params.click(props.params.data.RowKey)">{{ props.params.data.Name }}</a>
        </div>

        <div>
            <a target="_blank" :href="Config.racerxRiderLink + props.params.data.RowKey">
                <img style="height:25px;" src="../assets/racer-x-shield.png">
            </a>
        </div> -->
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';
    // import Config from "../config.json";
    // import Headshot from './Headshot.vue';
    
    const props = defineProps({
        params: { type: Object }
    });
</script>