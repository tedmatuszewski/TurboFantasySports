export default class Race {
    constructor(entity) {
      this.Name = entity.Name;
      this.Date = entity.Date;
      this.Lites = entity.Lites;
      this.Racerx = entity.Racerx;
      this.rowKey = entity.rowKey;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }