import { TableClient } from "@azure/data-tables";
import Team from '../../models/Team';

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
export default function (credential) {
    const table = "Leagues";
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
    };
}