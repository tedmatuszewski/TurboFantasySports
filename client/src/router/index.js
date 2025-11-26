import { createRouter, createWebHashHistory } from "vue-router";
import Home from "../views/Home.vue";
import Profile from "../views/Profile.vue";
import League from "../views/League.vue";
import Result from "../views/Result.vue";
import Standings from "../views/Standings.vue";
import Contact from "../views/Contact.vue";
import Matchup from "../views/Matchup.vue";
import Privacy from "../views/Privacy.vue";
import Riders from "../views/Riders.vue";
import CreateLeague from "../views/CreateLeague.vue";
import ManageLeague from "../views/ManageLeague.vue";
import Draft from "../views/Draft.vue";
import { createAuthGuard } from "@auth0/auth0-vue";

const routes = [
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
    path: "/privacy",
    name: "privacy",
    component: Privacy
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
  },
  {
    path: "/league/:id/riders",
    name: "riders",
    component: Riders,
    beforeEnter: createAuthGuard(app)
  },
  {
    path: "/league",
    name: "CreateLeague",
    component: CreateLeague,
    beforeEnter: createAuthGuard(app)
  },
  {
    path: "/league/:id/manage",
    name: "ManageLeague",
    component: ManageLeague,
    beforeEnter: createAuthGuard(app)
  },
  {
    path: "/league/:id/draft",
    name: "draft",
    component: Draft,
    beforeEnter: createAuthGuard(app)
  }
];

const router = createRouter({
    history: createWebHashHistory(),
    routes
});

export default router;
