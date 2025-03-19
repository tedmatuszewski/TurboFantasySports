export default class Rider {
    constructor(entity) {
      this.Name = entity.Name;
      this.Number = entity.Number;
      this.Class = entity.Class;
      this.Injury = entity.Injury;
      this.ImageUrl = entity.ImageUrl;
      this.Entries = entity.Entries;
      this.RowKey = entity.rowKey;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }