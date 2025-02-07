import { TableClient } from "@azure/data-tables";
import Rider from '../../models/Rider';

export default function (credential) {
    const table = "Riders";
    const account = "tedpersonalwebsite";
    const client = new TableClient(`https://${account}.table.core.windows.net`, table, credential);

    return {
        getAll: async function() {
            const entitiesIter = client.listEntities();
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Rider(entity));
            }

            return result;
        }
    };
}