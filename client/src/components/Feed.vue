<template>
  <div id="tracking">
    <div class="text-center tracking-status-intransit">
        <p class="tracking-status text-tight">Action Log</p>
    </div>
    <div class="tracking-list">
        <div class="tracking-item" v-for="item in feed">
          <div class="tracking-icon status-intransit">
              <i class="fas fa-circle"></i>
          </div>
          <div class="tracking-date">{{ formatDate(item.timestamp) }}<span>{{ formatTime(item.timestamp) }}</span></div>
          <div class="tracking-content">{{ getMemberName(item.Member) }}<span>{{ item.Action }}</span></div>
        </div>
    </div>
  </div>
</template>

<script setup>
  import { ref,onMounted,computed,watch } from "vue";
  import { useStorage } from "../storage/StorageContext";
  import { useAuth0 } from '@auth0/auth0-vue';
  
  const storage = useStorage();
  
  let feed = ref([]);
  let members = ref([]);

  onMounted(async () => {
    feed.value =  storage.Feeds.data;
    members.value = storage.Members.data;
  });

  function getMemberName(id) {
    let member = members.value.find(m => m.RowKey == id);
    let name = member ? member.TeamName : '';

    if(member.IsAdmin) {
      name += " ⭐";
    }

    return name;
  }

  function formatDate(date) {
    const options = { day: '2-digit', month: 'short' };
    return new Date(date).toLocaleDateString('en-US', options);
  }

  function formatTime(date) {
    const options = { hour: '2-digit', minute: '2-digit', hour12: true };
    return new Date(date).toLocaleTimeString('en-US', options);
  }
  
</script>

<style scoped>

.tracking-detail {
 padding:3rem 0
}
#tracking {
 margin-bottom:1rem
}
[class*=tracking-status-] p {
 margin:0;
 font-size:1.1rem;
 color:#fff;
 text-transform:uppercase;
 text-align:center
}
[class*=tracking-status-] {
 padding:1.6rem 0
}
.tracking-status-intransit {
 background-color:#62aeff;
}
.tracking-status-outfordelivery {
 background-color:#f5a551
}
.tracking-status-deliveryoffice {
 background-color:#f7dc6f
}
.tracking-status-delivered {
 background-color:#4cbb87
}
.tracking-status-attemptfail {
 background-color:#b789c7
}
.tracking-status-error,.tracking-status-exception {
 background-color:#d26759
}
.tracking-status-expired {
 background-color:#616e7d
}
.tracking-status-pending {
 background-color:#ccc
}
.tracking-status-inforeceived {
 background-color:#214977
}
.tracking-list {
 border:1px solid #e5e5e5;
 overflow-y: auto;
 max-height: 400px;
}
.tracking-item {
 border-left:1px solid #e5e5e5;
 position:relative;
 padding:2rem 1.5rem .5rem 2.5rem;
 font-size:.9rem;
 margin-left:3rem;
 min-height:5rem
}
.tracking-item:last-child {
 padding-bottom:4rem
}
.tracking-item .tracking-date {
 margin-bottom:.5rem
}
.tracking-item .tracking-date span {
 color:#888;
 font-size:85%;
 padding-left:.4rem
}
.tracking-item .tracking-content {
 padding:.5rem .8rem;
 background-color:#f4f4f4;
 border-radius:.5rem
}
.tracking-item .tracking-content span {
 display:block;
 color:#888;
 font-size:85%
}
.tracking-item .tracking-icon {
 line-height:2.6rem;
 position:absolute;
 left:-1.3rem;
 width:2.6rem;
 height:2.6rem;
 text-align:center;
 border-radius:50%;
 font-size:1.1rem;
 background-color:#fff;
 color:#fff
}
.tracking-item .tracking-icon.status-sponsored {
 background-color:#f68
}
.tracking-item .tracking-icon.status-delivered {
 background-color:#4cbb87
}
.tracking-item .tracking-icon.status-outfordelivery {
 background-color:#f5a551
}
.tracking-item .tracking-icon.status-deliveryoffice {
 background-color:#f7dc6f
}
.tracking-item .tracking-icon.status-attemptfail {
 background-color:#b789c7
}
.tracking-item .tracking-icon.status-exception {
 background-color:#d26759
}
.tracking-item .tracking-icon.status-inforeceived {
 background-color:#214977
}
.tracking-item .tracking-icon.status-intransit {
 color:#e5e5e5;
 border:1px solid #e5e5e5;
 font-size:.6rem
}
@media(min-width:992px) {
 .tracking-item {
  margin-left:10rem
 }
 .tracking-item .tracking-date {
  position:absolute;
  left:-10rem;
  width:7.5rem;
  text-align:right
 }
 .tracking-item .tracking-date span {
  display:block
 }
 .tracking-item .tracking-content {
  padding:0;
  background-color:transparent
 }
}
</style>