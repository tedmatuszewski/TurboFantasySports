using System;
using Azure.Data.Tables;

namespace jobs.Models;

public class Base
{

    public Base(TableEntity entity) 
    { 
        this.PartitionKey = entity.PartitionKey;
        this.RowKey = entity.RowKey;
    }

    public Base() { }

    public string PartitionKey { get; set; } = "2";

    public string RowKey { get; set; } = Guid.NewGuid().ToString();
}
