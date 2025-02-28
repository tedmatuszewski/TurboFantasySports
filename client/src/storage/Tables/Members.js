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
        data: []
    }),
    getters: { },
    actions: {
        async getAll() {
            const entitiesIter = client.listEntities();
            let results = [];

            for await (const entity of entitiesIter) {
                results.push(new Member(entity));
            }

            return results;
        },
        async getByLeague(league) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}'` } });
            let results = [];

            for await (const entity of entitiesIter) {
                results.push(new Member(entity));
            }

            return results;
        },
        async getByLeagueAndEmail(league, email) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Email eq '${email}'` } });
            let result = null;

            for await (const entity of entitiesIter) {
                result = new Member(entity);
                break;
            }

            return result;
        },
        async create(entity) {
            entity.RowKey = generateGuid();
            entity.PartitionKey = config.partitionKey;

            await client.createEntity(entity);

            return new Member(entity);
        }
    }
  });