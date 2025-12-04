import { defineStore } from 'pinia';
import { TableClient } from "@azure/data-tables";
import Feed from '../../models/Feed';
import credential from '../credential';
import config from '../../config';
import { generateGuid } from '../generateGuid';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Feeds";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: [],
    }),
    getters: { 
        getByLeague: (state) => {
            return (league) => state.data.filter(x => x.League === league);
        },
        getSingle: (state) => {
            return (rowKey) => state.data.find(league => league.rowKey === rowKey);
        },
        getAll: (state) => {
            return () => state.data;
        }
    },
    actions: {
        async create (entity) {
            entity.rowKey = generateGuid();
            entity.partitionKey = config.partitionKey;
            entity.timestamp = new Date().toISOString();

            await client.createEntity(entity);
            
            let team = new Feed(entity);
            this.data.unshift(team);

            return team;
        },
        async fillData() {
            this.data.length = 0;

            const entitiesIter = client.listEntities({
                queryOptions: {
                    filter: `PartitionKey eq '${config.partitionKey}'`
                }
            });

            for await (const entity of entitiesIter) {
                this.data.push(new Feed(entity));
            }
            
            this.data.sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp));

            return this.data;
        }
    }
  });