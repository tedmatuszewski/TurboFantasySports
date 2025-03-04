export default class League {
    constructor(entity) {
      this.Action = entity.Action;
      this.League = entity.League;
      this.Member = entity.Member;
      this.RowKey = entity.rowKey;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }