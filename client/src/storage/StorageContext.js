import { AzureSASCredential } from "@azure/data-tables";
import Leagues from "./Tables/Leagues";
import Members from "./Tables/Members";
import Results from "./Tables/Results";
import Teams from './Tables/Teams';

export async function StorageContext() {
    const key = "sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2026-02-08T02:17:40Z&st=2025-02-07T18:17:40Z&spr=https&sig=2tjjbUgVrAUD8UgrrKmZVftSJ0wrnxlgEiKfvIxh%2FFo%3D";
    const credential = new AzureSASCredential(key);

    return {
        Results: Results(credential),
        Leagues: Leagues(credential),
        Members: Members(credential),
        Teams: Teams(credential)
    }
}