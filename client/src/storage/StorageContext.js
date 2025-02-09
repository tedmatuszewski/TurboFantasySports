import { AzureSASCredential } from "@azure/data-tables";
import Rider from "./Tables/Rider";
import Races from "./Tables/Races";
import Leagues from "./Tables/Leagues";
import Members from "./Tables/Members";

export async function StorageContext() {
    const key = "sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2026-02-08T02:17:40Z&st=2025-02-07T18:17:40Z&spr=https&sig=2tjjbUgVrAUD8UgrrKmZVftSJ0wrnxlgEiKfvIxh%2FFo%3D";
    const credential = new AzureSASCredential(key);

    return {
        Riders: Rider(credential),
        Races: Races(credential),
        Leagues: Leagues(credential),
        Members: Members(credential)
    }
}