import { defineStore } from 'pinia';
import { TableClient } from "@azure/data-tables";
import Race from '../../models/Race';
import credential from '../credential';
import config from '../../config';
import { generateGuid } from '../generateGuid';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Races";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: [],
    }),
    getters: { 
        count: (state) => {
            const uniqueKeys = new Set(state.data.map(race => race.RowKey));
            return uniqueKeys.size;
        },
    },
    actions: {
        async fillData() {
            this.data.length = 0;

            const entitiesIter = client.listEntities();

            for await (const entity of entitiesIter) {
                this.data.push(new Race(entity));
            }

            this.data.sort((a, b) => new Date(a.Date) - new Date(b.Date));

            return this.data;
        },
        getRaceByKey(key) {
            return this.data.find(race => race.RowKey === key);
        },
        isNextUpcomingRace(race) { 
            const now = new Date();
            const raceDate = new Date(race.Date);
            
            if (raceDate <= now) return false;
            
            const upcomingRace = this.data.filter(r => new Date(r.Date) > now)[0];
            
            return race.RowKey === upcomingRace.RowKey;
        },
        getNextUpcomingRaceIndex() {
            for(let i = 0; i<this.data.length; i++) {
                if (this.isNextUpcomingRace(this.data[i])) {
                    return (i+1);
                }
            }
        },
        getNextUpcomingRace() {
            let races = this.data;

            for(let i = 0; i<races.length; i++) {
                if (this.isNextUpcomingRace(races[i])) {
                    return races[i];
                }
            }
        },
        getPreviousRace() {
            let races = this.data;
            const now = new Date();
            let previousRace = null;
        
            for(let i = 0; i < races.length; i++) {
                const raceDate = new Date(races[i].Date);
        
                if (raceDate < now) {
                    previousRace = races[i];
                } else {
                    break; // Since races are sorted by date, we can stop once we find a race in the future
                }
            }
        
            return previousRace;
        }
    }
  });