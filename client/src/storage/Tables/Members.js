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
    getters: { 
        getByLeague2: (state) => {
            return (league) => state.data.filter(x => x.League === league);
        },
        getByLeagueAndEmail2: (state) => {
            return (league, email) => state.data.find(x => x.League === league && x.Email === email);
        },
        getAll: (state) => {
            return state.data;
        }
    },
    actions: {
        async fillData() {
            this.data.length = 0;

            const entitiesIter = client.listEntities();

            for await (const entity of entitiesIter) {
                this.data.push(new Member(entity));
            }

            return this.data;
        },
        async create(entity) {
            entity.RowKey = generateGuid();
            entity.PartitionKey = config.partitionKey;

            await client.createEntity(entity);

            let member = new Member(entity);
            this.data.push(member);

            return member;
        },
        async update(member) {
            var entity = member.toEntity();
            
            await client.updateEntity(entity, "Replace");
            
            const index = this.data.findIndex(x => x.RowKey === entity.rowKey);

            if (index !== -1) {
                this.data[index] = new Member(entity);
            }

            return this.data[index];
        }
    }
  });