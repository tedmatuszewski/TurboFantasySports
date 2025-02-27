<template>
  <div class="container my-5">
    <div class="row justify-content-center">
      <div class="col-4-md">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ user?.name }}</h5>
            <p class="card-text">{{ user?.sub }}</p>
          </div>
          
          <ul class="list-group list-group-flush">
            <li class="list-group-item">Name: {{ user?.name }}</li>
            <li class="list-group-item">Email: {{ user?.email }}</li> 
            <li class="list-group-item">Nickname: {{ user?.nickname }}</li>
            <li class="list-group-item">Email Verified: {{ user?.email_verified }}</li>
          </ul>

          <div class="card-footer">
            <small class="text-body-secondary">Last updated {{ user?.updated_at }}</small>
          </div>
        </div>
      </div>
    </div>

    <div style="display:none;">
      {{ JSON.stringify(user, null, 2) }}
    </div>
  </div>
</template>

<script lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';

export default {
  name: "profile-view",
  setup() {
    const auth0 = useAuth0();
    
    return {
      user: auth0.user,
    }
  }
};
</script>

