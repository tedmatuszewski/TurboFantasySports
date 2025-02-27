import { TableClient } from "@azure/data-tables";
import Team from '../../models/Team';
import { generateGuid } from "../generateGuid";
import credential from '../credential';
import { defineStore } from 'pinia';
import config from '../../config';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Teams";
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
                result.push(new Team(entity));
            }

            return result;
        },
        get: async function(rowKey) {
            const entity = await client.getEntity("1", rowKey);
            
            return new League(entity);
        },
        getByLeagueAndOwnerAndNumber: async function(league, member, rider) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Member eq '${member}' and Rider eq '${rider}'` } });
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Team(entity));
            }

            return result;
        },
        getByLeagueAndMember: async function(league, member) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Member eq '${member}'` } });
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Team(entity));
            }

            return result;
        },
        getByLeague: async function(league) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}'` } });
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Team(entity));
            }

            return result;
        },
        create: async function(entity) {
            entity.RowKey = generateGuid();
            entity.PartitionKey = "1";

            await client.createEntity(entity);
            
            return new Team(entity);
        },
        remove: async function(key) {
            await client.deleteEntity("1", key);
        },
    }
  });