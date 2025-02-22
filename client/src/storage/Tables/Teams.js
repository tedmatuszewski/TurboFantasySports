import { TableClient } from "@azure/data-tables";
import Team from '../../models/Team';
import { generateGuid } from "../generateGuid";

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
export default function (credential) {
    const table = "Teams";
    const account = "tedpersonalwebsite";
    const client = new TableClient(`https://${account}.table.core.windows.net`, table, credential);

    return {
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
    };
}