export default class League {
    constructor(entity) {
      this.Action = entity.Action;
      this.League = entity.League;
      this.Member = entity.Member;
      this.rowKey = entity.rowKey;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }