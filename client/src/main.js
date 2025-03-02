import { createApp } from "vue";
import { createRouter } from "./router";
import { createAuth0 } from "@auth0/auth0-vue";
import config from "./config.json";
import 'vue3-easy-data-table/dist/style.css';
import App from './App.vue';
import { createPinia } from 'pinia';
import { AppInsightsPlugin } from "vue3-application-insights";

const app = createApp(App);
const pinia = createPinia();
const router = createRouter(app);

const aiOptions = {
  connectionString: config.appInsights.instrumentationKey,
  router: router,
  trackAppErrors: true,
};

app
  .use(pinia)
  .use(router)
  .use(AppInsightsPlugin, aiOptions)
  .use(
    createAuth0({
      domain: config.auth0.domain,
      clientId: config.auth0.clientId,
      authorizationParams: {
        redirect_uri: window.location.origin,
      }
    })
  )
  .mount("#app");