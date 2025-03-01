import { defineStore } from 'pinia';
import { TableClient } from "@azure/data-tables";
import League from '../../models/League';
import credential from '../credential';
import config from '../../config';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Leagues";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: [],
    }),
    getters: { 
        getSingle: (state) => {
            return (rowKey) => state.data.find(league => league.RowKey === rowKey);
        },
        getAll: (state) => {
            return state.data;
        }
    },
    actions: {
        async fillData() {
            this.data.length = 0;

            const entitiesIter = client.listEntities();

            for await (const entity of entitiesIter) {
                this.data.push(new League(entity));
            }

            return this.data;
        }
    }
  });