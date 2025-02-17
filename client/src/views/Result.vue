<template>
  <div>
    <h3>Result</h3>
    <p>{{ route.params.race }}</p>
    <router-link :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary">League Home</router-link>
          
  </div>
</template>

<script setup>
  import { useRoute } from 'vue-router';
  import { ref,onMounted,computed, reactive  } from "vue";
  
  const route = useRoute();

  onMounted(async () => {

  });
</script>

<style lang="css" scoped>
.next-steps .fa-link {
    margin-right: 5px;
}
</style>
