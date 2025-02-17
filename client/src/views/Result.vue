<template>
  <div>
    <h3>Result</h3>
    <p>{{ route.params.race }}</p>
    <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
          
    <table class="table table-sm table-hover">
      <thead>
        <tr>
          <th>Position</th>
          <th>Rider</th>
          <th>Class</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="race in raceResults" :value="race.race">
          <td>{{race.position}}</td>
          <td>{{race.number}}</td>
          <td>{{ race.class }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
  import { useRoute } from 'vue-router';
  import { ref,onMounted,computed, reactive  } from "vue";
  import results from '../data/results.json';
  
  const route = useRoute();

  let raceResults = results.filter(r => r.race === route.params.race);

  onMounted(async () => {

  });
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
