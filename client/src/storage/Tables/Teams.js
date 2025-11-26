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
    getters: { 
        getByLeague2: (state) => {
            return (league) => state.data.filter(x => x.League === league);
        },
        getByLeagueAndRider: (state) => {
            return (league, rider) => state.data.find(x => x.League === league && x.Rider === rider);
        },
        getOne: (state) => {
            return (rowKey) => state.data.find(x => x.RowKey === rowKey);
        },
        getByLeagueAndOwnerAndNumber2: (state) => {
            return (league, member, rider) => state.data.filter(x => x.League === league && x.Member === member && x.Rider === rider);
        },
        getByLeagueAndMember2: (state) => {
            return (league, member) => state.data.filter(x => x.League === league && x.Member === member);
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
                this.data.push(new Team(entity));
            }

            return this.data;
        },
        async create(entity) {
            entity.RowKey = generateGuid();
            entity.PartitionKey = config.partitionKey;

            await client.createEntity(entity);
            
            let team = new Team(entity);
            this.data.push(team);

            return team;
        },
        async remove(key) {
            let index = this.data.findIndex(x => x.RowKey === key);
            if (index >= 0) {
                this.data.splice(index, 1);
            }

            await client.deleteEntity(config.partitionKey, key);
        }
    }
  });