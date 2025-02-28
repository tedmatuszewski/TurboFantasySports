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
        data: {}
    }),
    getters: { },
    actions: {
        async getByLeagueAndRace(league, race) {
            let result = [];
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Race eq '${race}'` } });
            
            for await (const entity of entitiesIter) {
                result.push(new Result(entity));
            }

            return result;
        },
        async getByLeague(league) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}'` } });
            var result = [];
            
            for await (const entity of entitiesIter) {
                result.push(new Result(entity));
            }

            return result;
        },
        async hasResults(league, race) {
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