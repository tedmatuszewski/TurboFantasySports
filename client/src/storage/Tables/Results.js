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
        data: [],
    }),
    getters: { },
    actions: {
        getAll: async function() {
            const entitiesIter = client.listEntities();
            let result = [];
            
            for await (const entity of entitiesIter) {
              result.push(new Result(entity));
            }

            return result;
        },
        getByLeagueAndRace: async function(league, race) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Race eq '${race}'` } });
            let result = [];
            
            for await (const entity of entitiesIter) {
                result.push(new Result(entity));
            }

            return result;
        },
        getByLeague: async function(league) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}'` } });
            let result = [];
            
            for await (const entity of entitiesIter) {
                result.push(new Result(entity));
            }

            return result;
        },
        hasResults: async function(league, race) {
            const entitiesIter = client.listEntities({
                queryOptions: {
                    filter: `League eq '${league}' and Race eq '${race}'`,
                    select: ["PartitionKey"]
                }
            });

            let result = [];

            for await (const entity of entitiesIter) {
                result.push(entity);
            }

            return result.length > 0;
        }
    }
  });