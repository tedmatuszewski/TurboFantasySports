import { createApp, watch } from "vue";
import router from "./router";
import { createAuth0 } from "@auth0/auth0-vue";
import config from "./config.json";
import 'vue3-easy-data-table/dist/style.css';
import { createPinia } from 'pinia';
import { AppInsightsPlugin } from "vue3-application-insights";
import App from './App.vue';

const pinia = createPinia();
const app = createApp(App).use(pinia);
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
  trackInitialPageView: true,
  onLoaded: async function (sdk) {
    watch(() => auth0.user.value, (newValue) => {
        if (newValue) {
          sdk.context.user.authenticatedId = newValue.email;
        }
      },
      { immediate: true });
  }
};

app
  .use(router)
  .use(AppInsightsPlugin, aiOptions)
  .use(auth0)
  .mount("#app");