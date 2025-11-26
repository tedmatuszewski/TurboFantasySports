export default class League {
    constructor(entity) {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.RowKey = entity.rowKey;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }

    toEntity() {
        return {
            "rowKey": this.RowKey,
            "Name": this.Name,
            "Description": this.Description,
            "partitionKey": this.PartitionKey,
            "etag": this.etag
        };
    }
  }