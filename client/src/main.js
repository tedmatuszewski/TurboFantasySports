import { createApp } from "vue";
import { createRouter } from "./router";
import { createAuth0 } from "@auth0/auth0-vue";
import config from "./config.json";
import 'vue3-easy-data-table/dist/style.css';
import { createPinia } from 'pinia';
import { AppInsightsPlugin } from "vue3-application-insights";
import App from './App.vue';

const pinia = createPinia();
const app = createApp(App).use(pinia);
const router = createRouter(app);
const auth0 = createAuth0({
  domain: config.auth0.domain,
  clientId: config.auth0.clientId,
  authorizationParams: {
    redirect_uri: window.location.origin,
  }
});

const aiOptions = {
  connectionString: config.appInsights.instrumentationKey,
  router: router,
  trackAppErrors: true,
  onLoaded: function (sdk) {
    const auth01 = createAuth0();
    sdk.context.user.authenticatedId = auth01.user.value.email;
  }
};

app
  .use(router)
  .use(AppInsightsPlugin, aiOptions)
  .use(auth0)
  .mount("#app");