import Leagues from "./Tables/Leagues";
import Members from "./Tables/Members";
import Results from "./Tables/Results";
import Teams from './Tables/Teams';
import Feeds from "./Tables/Feeds";
import Races from "./Tables/Races";
import Riders from "./Tables/Riders";
import Outcomes from "./Tables/Outcomes";

export function useStorage() {
    let context = {
        Results: Results(),
        Leagues: Leagues(),
        Members: Members(),
        Teams: Teams(),
        Feeds: Feeds(),
        Races: Races(),
        Riders: Riders(),
        Outcomes: Outcomes()
    };

    context.getTeam = (league, member) => {
        let team = context.Teams.getByLeagueAndMember2(league, member);
        let riders = [];

        context.Riders.data.forEach(rider => {
            let mine = team.map(a => a.Rider).indexOf(rider.RowKey);
            
            if (mine > -1) {
            riders.push(rider);
            }
        });

        return riders;
    }

    context.getAvailableRiders = (league) => {
        let riders = [];
        let teams = context.Teams.getByLeague2(league);
        let taken = teams.map(a => a.Rider);
        
        context.Riders.data.filter(r => taken.indexOf(r.RowKey) === -1).forEach(rider => {
            riders.push(rider);
        });

        return riders;
    }

    return context;
}