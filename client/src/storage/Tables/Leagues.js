import { defineStore } from 'pinia';
import { TableClient } from "@azure/data-tables";
import League from '../../models/League';
import credential from '../credential';
import config from '../../config';
import { generateGuid } from '../generateGuid';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
const table = "Leagues";
const client = new TableClient(config.storageAccount, table, credential);

export default defineStore(table, {
    state: () => ({
        data: [],
    }),
    getters: { 
        getSingle: (state) => {
            return (rowKey) => state.data.find(league => league.rowKey === rowKey);
        },
        getAll: (state) => {
            return state.data;
        }
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
                console.log("Loading league entity", entity);
                this.data.push(new League(entity));
            }
console.log("Leagues loaded", this.data);
            return this.data;
        },
        async create (entity) {
            entity.rowKey = generateGuid();
            entity.partitionKey = config.partitionKey;

            await client.createEntity(entity);
            
            let league = new League(entity);
            this.data.push(league);

            return league;
        },
        async update(league) {
            var entity = league.toEntity();
            
            await client.updateEntity(entity, "Replace");
            
            const index = this.data.findIndex(x => x.rowKey === entity.rowKey);

            if (index !== -1) {
                this.data[index] = new League(entity);
            }

            return this.data[index];
        },
        async remove(key) {
            let index = this.data.findIndex(x => x.rowKey === key);
            if (index >= 0) {
                this.data.splice(index, 1);
            }

            await client.deleteEntity(config.partitionKey, key);
        }
    }
  });