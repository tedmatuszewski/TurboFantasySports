export default class Team {
    constructor(entity) {
      this.rowKey = entity.rowKey;
      this.Rider = entity.Rider;
      this.League = entity.League;
      this.Member = entity.Member;
      this.IsBench = entity.IsBench || false;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }

    toEntity() {
        return {
            "rowKey": this.rowKey,
            "Rider": this.Rider,
            "League": this.League,
            "Member": this.Member,
            "IsBench": this.IsBench,
            "partitionKey": this.partitionKey,
            "etag": this.etag
        };
    }
  }