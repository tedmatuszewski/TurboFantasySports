import { createRouter as createVueRouter, createWebHashHistory } from "vue-router";
import Home from "../views/Home.vue";
import Profile from "../views/Profile.vue";
import League from "../views/League.vue";
import Result from "../views/Result.vue";
import Standings from "../views/Standings.vue";
import Contact from "../views/Contact.vue";
import Matchup from "../views/Matchup.vue";
import { createAuthGuard } from "@auth0/auth0-vue";

export function createRouter(app) {
  return createVueRouter({
    routes: [
      {
        path: "/",
        name: "home",
        component: Home
      },
      {
        path: "/profile",
        name: "profile",
        component: Profile,
        beforeEnter: createAuthGuard(app)
      },
      {
        path: "/league/:id",
        name: "league",
        component: League,
        beforeEnter: createAuthGuard(app)
      },
      {
        path: "/matchup/:id",
        name: "matchup",
        component: Matchup,
        beforeEnter: createAuthGuard(app)
      },
      {
        path: "/standings/:id",
        name: "standings",
        component: Standings,
        beforeEnter: createAuthGuard(app)
      },
      {
        path: "/contact",
        name: "contact",
        component: Contact,
        beforeEnter: createAuthGuard(app)
      },
      {
        path: "/league/:id/:race",
        name: "result",
        component: Result,
        beforeEnter: createAuthGuard(app)
      }
    ],
    history: createWebHashHistory()
  })
}
