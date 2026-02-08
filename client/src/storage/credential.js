import { AzureSASCredential } from "@azure/data-tables";

const key = "sv=2024-11-04&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2030-01-31T22:11:01Z&st=2026-02-08T13:56:01Z&spr=https&sig=rhXwwjQy5Est6EvsWxVwqCNAyiXnW4zn1VPJGyqJcCg%3D";
const credential = new AzureSASCredential(key);

export default credential;
