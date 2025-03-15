export default class Race {
    constructor(entity) {
      this.Name = entity.Name;
      this.Date = entity.Date;
      this.Lites = entity.Lites;
      this.Racerx = entity.Racerx;
      this.RowKey = entity.rowKey;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }