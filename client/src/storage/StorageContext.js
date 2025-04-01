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

    context.getTeamWithPoints = (league, race) => {
        let results = null;
        
        if(race) {
            results = context.Results.getByLeagueAndRace2(league, race);
        } else {
            results = context.Results.getByLeague2(league);
        }
        
        let members = context.Members.getByLeague2(league);
        let teams = context.Teams.getByLeague2(league);
        let table = [];
    
        members.forEach(member => {
          let rr1 = results.filter(result => result.Member === member.RowKey);
          let rr2 = teams.filter(t => t.Member === member.RowKey);
    
          table.push({
            TeamName: member.TeamName,
            Points: rr1.map(r => r.Points).reduce((a, b) => a + b, 0),
            Team: rr2,
            Results: rr1
          });
        });
    
        table.sort((a, b) => b.Points - a.Points);
        table.forEach((t, i) => t.Place = (i+1));

        return table;
    };

    return context;
}