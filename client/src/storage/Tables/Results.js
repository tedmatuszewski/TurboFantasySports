import { TableClient } from "@azure/data-tables";
import Result from '../../models/Result';

export default function (credential) {
    const table = "Results";
    const account = "tedpersonalwebsite";
    const client = new TableClient(`https://${account}.table.core.windows.net`, table, credential);

    return {
        getAll: async function() {
            const entitiesIter = client.listEntities();
            let result = [];
            
            for await (const entity of entitiesIter) {
              result.push(new Result(entity));
            }

            return result;
        },
        getByLeagueAndRace: async function(league, race) {
            const entitiesIter = client.listEntities({ queryOptions: { filter: `League eq '${league}' and Race eq '${race}'` } });
            let result = [];
            
            for await (const entity of entitiesIter) {
                result.push(new Result(entity));
            }

            return result;
        }
    };
}