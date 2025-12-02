import { defineStore } from 'pinia';
import { TableClient } from "@azure/data-tables";
import Outcome from '../../models/Outcome';
import credential from '../credential';
import config from '../../config';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Outcomes";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: [],
    }),
    getters: { 
        count: (state) => {
            const uniqueKeys = new Set(state.data.map(o => o.Race));
            
            return uniqueKeys.size;
        },
    },
    actions: {
        async fillData() {
            this.data.length = 0;

            const entitiesIter = client.listEntities({
                queryOptions: {
                    filter: `PartitionKey eq '${config.partitionKey}'`
                }
            });

            for await (const entity of entitiesIter) {
                this.data.push(new Outcome(entity));
            }
    
            return this.data;
        },
        getByRider(rider) {
            return this.data
                .filter(o => o.Rider === rider);
        }
    }
  });