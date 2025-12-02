<template>
  <h1>Manage</h1>
<div class="container my-5">
    <div class="row">
      <div class="col-md-4">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ league?.Name }}</h5>
            <p>{{league?.Description}}</p>
          </div>
        </div>
        
        <div class="card mt-3">
          <div class="card-body">
            <h5 class="card-title">Actions</h5>
            <router-link  :to="{ name: 'league', params: { id: route.params.id } }" class="btn btn-primary btn-block mb-2">League Home</router-link>
            <button v-on:click="showAddMemberModal(null)" class="btn btn-primary btn-block mb-2">Add Member</button>
            <button v-on:click="showLeagueModal" class="btn btn-primary btn-block mb-2">Edit League Name/Description</button>
          </div>
        </div>
        
        <div class="card mt-3">
          <div class="card-body">
            <h5 class="card-title">Draft</h5>

            <ol>
              <li>You are responsible for setting up the league members initial teams.</li>
              <li>You can do this a couple ways:
                <ul>
                  <li>Get your entire league together and display the setup teams page on a screen for everyone to see. Then take turns picking riders.</li>
                  <li>Pick teams outside of the system, then use the setup team page to enter them.</li>
                </ul>
              </li>
              <li>In either case, before starting the draft, be sure all members have been added to the league.</li>
              <li>Once the members of each team have been selected, league members will see their team in their dashboard.</li>
            </ol>

            <router-link :to="{ name: 'draft', params: { id: route.params.id } }" class="btn btn-success btn-block mb-2">Setup Teams</router-link>
          </div>
        </div>
        
        <div class="card mt-3">
          <div class="card-body">
            <h5 class="card-title">Delete League</h5>
            <p>Be careful! {{ deleteMessage }} You will be prompted to confirm one more time.</p>
            <button v-on:click="deleteLeague" class="btn btn-danger btn-block mb-2">Delete League</button>
          </div>
        </div>
      </div> 
      
      <div class="col-md-8">
        <div class="alert alert-primary" role="alert">
          The table contains all members of this league. You can add new members, edit existing members, 
          or remove members from the league. Be careful when removing members, as this action cannot be undone.
        </div>

        <div class="alert alert-primary" role="alert">
          After creating your league, fill in all the members using the 'Add Member' button and the table. 
          Once a person has been added, they can login with the email you added to the table. They will see the league home page 
          and can manage their team.
        </div>

        <h3>League Members</h3>

        <ag-grid-vue :rowData="members" :columnDefs="colDefs" style="height: 320px;" :autoSizeStrategy="{ type: 'fitCellContents' }"></ag-grid-vue>

        <div class="mt-3">
          <Feed></Feed>
        </div>
      </div>
    </div>
  </div>

<div id="memberModal" class="modal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Member Profile</h5>
      </div>

      <div class="modal-body">
        <form @submit.prevent="createNewLeague">
          <div class="alert alert-primary" role="alert">
            Be sure to enter their exact email with no leading or trailing spaces. This will be used for their login.
          </div>

          <div class="form-group">
            <label class="form-label">Member Email*</label>
            <input v-model="member.Email" type="text" class="form-control" @change="handleMemberEmailChange" />
          </div>

          <div class="alert alert-primary" role="alert">
            This is the name of the team that will be displayed on the members league home page. They can change this later.
          </div>

          <div class="form-group">
            <label class="form-label">Team Name</label>
            <input v-model="member.TeamName" class="form-control" />
          </div>
        </form>
      </div>

      <div class="modal-footer">
        <button type="button" class="btn btn-primary" v-on:click="saveMember">Save</button>
        <button type="button" class="btn btn-secondary" v-on:click="closeMemberModal">Close</button>
      </div>
    </div>
  </div>
</div>

<div id="leagueModal" class="modal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">League</h5>
      </div>

      <div class="modal-body">
        <form>
          <div class="form-group">
            <label class="form-label">League Name*</label>
            <input v-model="league.Name" type="text" class="form-control" />
          </div>

          <div class="form-group">
            <label class="form-label">League Description</label>
            <input v-model="league.Description" class="form-control" />
          </div>
        </form> 
      </div>

      <div class="modal-footer">
        <button type="button" class="btn btn-primary" v-on:click="saveLeague">Save</button>
        <button type="button" class="btn btn-secondary" v-on:click="closeLeagueModal">Close</button>
      </div>
    </div>
  </div>
</div>
</template>

<script setup>
  import { useStorage } from '../storage/StorageContext';
  import { ref,onMounted,computed, reactive  } from "vue";
  import TableEditDelete from '../components/TableEditDelete.vue';
  import { useRoute } from 'vue-router';
  import { AgGridVue } from "ag-grid-vue3";
  import Feed from "../components/Feed.vue";
  import { useAuth0 } from '@auth0/auth0-vue';

  const auth0 = useAuth0();
  const storage = useStorage();
  const route = useRoute();
  const league = ref({});
  const members = ref([]);
  const member = ref({});

  const deleteMessage = computed(() => {
    return `All results and data associated with this league will be permanently deleted. This action cannot be undone.`;
  });

  const me = computed(() => {
    return members.value.find(member => member.Email === auth0.user.value?.email);
  });

  const colDefs = ref([
    { 
      headerName: "Actions",
      field: "Name",
      pinned: 'left',
      cellRenderer: TableEditDelete,
      cellRendererParams: {
        edit: showAddMemberModal,
        delete: deleteMember
      }
    },
    { field: "Email", headerName: "Member Email" },
    { field: "TeamName", headerName: "Team Name" },
    { field: "IsAdmin", headerName: "Admin Status" }
  ]);
  
  let memberModal = null;
  let leagueModal = null;

  onMounted(() => {
    league.value = storage.Leagues.getSingle(route.params.id);
    members.value = storage.Members.getByLeague2(route.params.id);

    memberModal = new bootstrap.Modal(document.getElementById('memberModal'), {});
    leagueModal = new bootstrap.Modal(document.getElementById('leagueModal'), {});
  });

  async function saveMember() {
    if (member.value.rowKey) {
      await storage.Members.update(member.value);
      await storage.Feeds.create({ League: route.params.id, Member: me.value.rowKey, Action: `Edited member ${member.value.Email} team name or email` });
    } else {
      member.value.League = route.params.id;
      member.value.IsAdmin = false;
      await storage.Members.create(member.value);
      await storage.Feeds.create({ League: route.params.id, Member: me.value.rowKey, Action: `Added member ${member.value.Email} to the league` });
    }

    members.value = await storage.Members.getByLeague2(route.params.id);

    closeMemberModal();
  }

  function showAddMemberModal(user) {
    if (user) {
      member.value = user;
    } 

    memberModal.show();
  }

  function showLeagueModal() {
    leagueModal.show();
  }

  async function deleteMember(user) {
    if(confirm(`Are you sure you want to remove ${user.Email} from the league?`) == false) {
      return;
    }

    await storage.Members.remove(user.rowKey);
    await storage.Feeds.create({ League: route.params.id, Member: me.value.rowKey, Action: `Removed member ${user.Email} from league` });
    members.value = await storage.Members.getByLeague2(route.params.id);
  }

  async function deleteLeague() {
    if(confirm(`Are you sure you want to delete the league ${league.value.Name}? ` + deleteMessage.value) == false) {
      return;
    }

    await storage.Leagues.remove(league.value.rowKey);
    
    window.location.href = "/";
  }

  function closeMemberModal() {
    member.value = {};
    memberModal.hide();
  }

  function closeLeagueModal() {
    leagueModal.hide();
  }

  async function saveLeague() {
    await storage.Leagues.update(league.value);
    await storage.Feeds.create({ League: route.params.id, Member: me.value.rowKey, Action: `Updated league ${league.value.Name} name or description` });
    
    closeLeagueModal();
  }

  function handleMemberEmailChange() {
    if (member.value.TeamName == null || member.value.TeamName.trim() === "") {
      member.value.TeamName = "Team " + member.value.Email.trim().toLowerCase();
    }
  }
</script>

<style lang="css" scoped>

</style>
