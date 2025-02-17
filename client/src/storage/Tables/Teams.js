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
        getByLeagueAndOwner: async function(leagueGuid, ownerId) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${leagueGuid}' and Owner eq '${ownerId}'` } });
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Team(entity));
            }

            return result;
        },
        getByLeagueAndOwnerAndNumber: async function(leagueGuid, ownerId, number) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${leagueGuid}' and Owner eq '${ownerId}' and Rider eq ${number}` } });
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Team(entity));
            }

            return result;
        },
        getByLeague: async function(leagueGuid) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${leagueGuid}'` } });
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