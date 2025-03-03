export default class Member {
    constructor(entity) {
      this.RowKey = entity.rowKey;
      this.League = entity.League;
      this.Email = entity.Email;
      this.TeamName = entity.TeamName;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }

    toEntity() {
        return {
            "rowKey": this.RowKey,
            "League": this.League,
            "Email": this.Email,
            "TeamName": this.TeamName,
            "partitionKey": this.PartitionKey,
            "etag": this.etag
        };
    }
  }