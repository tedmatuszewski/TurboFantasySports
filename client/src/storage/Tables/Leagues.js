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
    getters: { },
    actions: {
        async get(rowKey) {
            const entity = await client.getEntity("1", rowKey);
            
            return new League(entity);
        },
        async getAll() {
            const entitiesIter = client.listEntities();
            let results = [];
            
            for await (const entity of entitiesIter) {
                results.push(new League(entity));
            }

            return results;
        }
    }
  });