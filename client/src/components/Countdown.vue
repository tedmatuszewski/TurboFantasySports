<template>
    <div>
        <div class="timer">
            <div class="mr-2">
                <span class="days" id="day">{{ days }}</span> 
                <div class="smalltext">Days</div>
            </div>

            <div class="mr-2">
                <span class="hours" id="hour">{{ hours }}</span> 
                <div class="smalltext">Hours</div>
            </div>

            <div class="mr-2">
                <span class="minutes" id="minute">{{ minutes }}</span> 
                <div class="smalltext">Minutes</div>
            </div>

            <div>
                <span class="seconds" id="second">{{ seconds }}</span> 
                <div class="smalltext">Seconds</div>
            </div>

            <p id="time-up"></p>
        </div>
    </div>
</template>

<script setup>
    import { defineProps, ref } from 'vue';
    import { useStorage } from '../storage/StorageContext';

    const props = defineProps({ id: { type: String } });
    const storage = useStorage();

    let days= 0;
    let hours = 0;
    let minutes = 0;
    let seconds = ref(0);
    let nextRace = storage.Races.getNextUpcomingRace();
    let deadline = new Date(nextRace.Date).getTime();

    let x = setInterval(function() {
        let currentTime = new Date().getTime();                
        let t = deadline - currentTime; 

        days = Math.floor(t / (1000 * 60 * 60 * 24)); 
        hours = Math.floor((t%(1000 * 60 * 60 * 24))/(1000 * 60 * 60)); 
        minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60)); 
        seconds.value = Math.floor((t % (1000 * 60)) / 1000); 

        if (t < 0) {
            clearInterval(x); 
        } 
    }, 1000);  
</script>

<style scoped>
h2 {
  font-weight: 500;
  margin: 0 0 20px;
}
.timer {
  color: #fff;
  display: inline-block;
  font-weight: 100;
  text-align: center;
  font-size: 30px;
    margin-right: 15px;
}
.timer div {
  padding: 10px;
  border-radius: 3px;
  background: #000000;
  display: inline-block;
  font-size: 26px;
  font-weight: 400;
  width: 66px;
}
.timer .smalltext {
  color: #888888;
  font-size: 12px;
  font-weight: 500;
  display: block;
  padding: 0;
  width: auto;
}
.timer #time-up {
  margin: 8px 0 0;
  text-align: left;
  font-size: 14px;
  font-style: normal;
  color: #000000;
  font-weight: 500;
  letter-spacing: 1px;
}
</style>