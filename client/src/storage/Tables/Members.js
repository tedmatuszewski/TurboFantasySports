import { TableClient } from "@azure/data-tables";
import Member from "../../models/Member";
import { generateGuid } from "../generateGuid";
import credential from '../credential';
import { defineStore } from 'pinia';
import config from '../../config';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Members";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: [],
    }),
    getters: { },
    actions: {
        async getAll() {
            const entitiesIter = client.listEntities();
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Member(entity));
            }

            return result;
        },
        getByLeague: async function(leagueGuid) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${leagueGuid}'` } });
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Member(entity));
            }

            return result;
        },
        getByLeagueAndEmail: async function(league, email) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Email eq '${email}'` } });
            let result = null;

            for await (const entity of entitiesIter) {
                result = new Member(entity);
                break;
            }

            return result;
        },
        create: async function(entity) {
            entity.RowKey = generateGuid();
            entity.PartitionKey = "1";

            await client.createEntity(entity);

            return new Member(entity);
        }
    }
  });