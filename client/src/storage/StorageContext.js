import Leagues from "./Tables/Leagues";
import Members from "./Tables/Members";
import Results from "./Tables/Results";
import Teams from './Tables/Teams';

export async function StorageContext() {
    return {
        Results: Results(),
        Leagues: Leagues(),
        Members: Members(),
        Teams: Teams()
    }
}