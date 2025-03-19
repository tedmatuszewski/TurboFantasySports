<template>
    <div class="player-wrapper">
        <img :src="rider.ImageUrl" v-if="rider.ImageUrl" class="avator" />
        <img src="../assets/profile.png" v-else class="avator" />
        <div v-if="rider.Injury !== undefined" class="injury" :title="rider.Injury">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"><path fill="#EB2A2E" d="M27.5 12.2H21V5.6c0-.9-.8-1.6-1.7-1.6h-6.7c-.8 0-1.6.7-1.6 1.6v6.5H4.5c-.9 0-1.7.7-1.7 1.7v6.7c0 .9.7 1.7 1.7 1.7H11v6.5c0 .9.7 1.7 1.7 1.7h6.7c.9 0 1.7-.7 1.7-1.7v-6.5h6.5c.9 0 1.7-.7 1.7-1.7v-6.7c-.1-.9-.9-1.6-1.8-1.6"></path></svg>
        </div>
    </div>
    
</template>

<script setup>
    import { defineProps } from 'vue';
    
    const props = defineProps({
        rider: { type: Object }
    });
</script>

<style scoped>
.player-wrapper {
    padding: 5px;
    display: inline-flex;
    align-items: center;
    justify-items: center;
    position: relative;
}

.avator {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    -o-object-fit: cover;
    object-fit: cover;
    box-shadow: inset 0 2px 4px #0000001a;
}

.injury {
    width: 20px;
    height: 20px;
    background: white;
    position: absolute;
    bottom: 7px;
    right: -3px;
    border-radius: 50%;
    background-color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, .2);
    text-align: center;;
}

.injury svg {
    width:14px;
    height:14px;
}
</style>