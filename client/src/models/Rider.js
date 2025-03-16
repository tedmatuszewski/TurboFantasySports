export default class League {
    constructor(entity) {
      this.Name = entity.Name;
      this.Number = entity.Number;
      this.Class = entity.Class;
      this.IsInjured = entity.IsInjured;
      this.ImageUrl = entity.ImageUrl;
      this.RowKey = entity.rowKey;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }