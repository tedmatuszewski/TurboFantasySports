import { TableClient } from "@azure/data-tables";
import League from '../../models/League';

export default function (credential) {
    const table = "Leagues";
    const account = "tedpersonalwebsite";
    const client = new TableClient(`https://${account}.table.core.windows.net`, table, credential);

    return {
        getAll: async function() {
            const entitiesIter = client.listEntities();
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new League(entity));
            }

            return result;
        }
    };
}