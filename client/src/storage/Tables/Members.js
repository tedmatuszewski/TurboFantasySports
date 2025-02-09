import { TableClient } from "@azure/data-tables";
import Member from "../../models/Member";
import { generateGuid } from "../generateGuid";

// https://learn.microsoft.com/en-us/javascript/api/overview/azure/data-tables-readme?view=azure-node-latest
export default function (credential) {
    const table = "Members";
    const account = "tedpersonalwebsite";
    const client = new TableClient(`https://${account}.table.core.windows.net`, table, credential);

    return {
        getAll: async function() {
            const entitiesIter = client.listEntities();
            let result = [];

            for await (const entity of entitiesIter) {
                result.push(new Member(entity));
            }

            return result;
        },
        create: async function(entity) {
            entity.RowKey = generateGuid();
            entity.PartitionKey = "1";

            await client.createEntity(entity);

            return new Member(entity);
        }
    };
}