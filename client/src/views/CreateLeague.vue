<template>
  <div class="container my-4">
    <div class="row">
      <div class="col-md-8 mb-3">
        <h1 class="title">Create Your League</h1>

        <div class="alert alert-primary" role="alert">
          <strong>Important:</strong> By creating this league, you will become the league president. 
            You'll be responsible for adding other members, managing the league settings, 
            and configuring teams for all participants.
        </div>

        <form @submit.prevent="createNewLeague" class="league-form">
          <div class="form-group">
            <label class="form-label">League Name*</label>
            <input v-model="leagueName" type="text" class="form-control" />
          </div>

          <div class="form-group">
            <label class="form-label">League Description</label>
            <textarea v-model="leagueDescription" class="form-control" rows="4"></textarea>
          </div>

          <button type="submit" class="btn btn-primary">Create League</button>
        </form>
      </div>
      
      <div class="col-md-4 d-flex align-items-center justify-content-center">
        <Donate />
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref } from "vue";
  import { useStorage } from '../storage/StorageContext';
  import { useAuth0 } from '@auth0/auth0-vue';
  import router from '../router/index.js';
  import Donate from "../components/Donate.vue";

  const leagueName = ref("");
  const leagueDescription = ref("");
  const storage = useStorage();
  const auth0 = useAuth0();

  async function createNewLeague() {
    if (!leagueName.value || !leagueName.value.trim()) {
      alert("League name is required");

      return;
    }

    if (!leagueDescription.value || !leagueDescription.value.trim()) {
      alert("League description is required");

      return;
    }

    let league = await storage.Leagues.create({
      Name: leagueName.value,
      Description: leagueDescription.value
    });

    let member = await storage.Members.create({
      League: league.rowKey,
      Email: auth0.user.value.email,
      IsAdmin: true,
      TeamName: "Team Snacks"
    });
    
    console.log(league, member);

    await storage.Feeds.create({ League: league.rowKey, Member: member.rowKey, Action: `Created league ${leagueName.value}` });
    
    router.push({ name: 'ManageLeague', params: { id: league.rowKey } });
  }
</script>

<style scoped>
</style>