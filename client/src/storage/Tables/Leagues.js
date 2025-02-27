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
        leagues: [],
    }),
    getters: { },
    actions: {
        async get(rowKey) {
            const entity = await client.getEntity("1", rowKey);
            
            return new League(entity);
        },
        async getAll() {
            let result = [];
            const entitiesIter = client.listEntities();

            for await (const entity of entitiesIter) {
                result.push(new League(entity));
            }

            return result;
        }
    }
  });