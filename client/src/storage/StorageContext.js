import Leagues from "./Tables/Leagues";
import Members from "./Tables/Members";
import Results from "./Tables/Results";
import Teams from './Tables/Teams';
import Feeds from "./Tables/Feeds";
import Races from "./Tables/Races";
import Riders from "./Tables/Riders";

export function useStorage() {
    return {
        Results: Results(),
        Leagues: Leagues(),
        Members: Members(),
        Teams: Teams(),
        Feeds: Feeds(),
        Races: Races(),
        Riders: Riders()
    };
}