import { TableClient } from "@azure/data-tables";
import Result from '../../models/Result';
import credential from '../credential';
import { defineStore } from 'pinia';
import config from '../../config';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Results";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: []
    }),
    getters: { 
        getByLeague2: (state) => {
            return (league) => state.data.filter(x => x.League === league);
        },
        getByLeagueAndRace2: (state) => {
            return (league, race) => state.data.filter(x => x.League === league && x.Race === race);
        },
        hasResults2: (state) => {
            return (league, race) => state.data.filter(x => x.League === league && x.Race === race).length > 0;
        }
    },
    actions: {
        async fillData(){
            this.data.length = 0;

            const entitiesIter = client.listEntities();
            
            for await (const entity of entitiesIter) {
                this.data.push(new Result(entity));
            }

            return this.data;
        }
    }
  });