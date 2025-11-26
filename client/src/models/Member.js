export default class Member {
    constructor(entity) {
      this.RowKey = entity.rowKey;
      this.League = entity.League;
      this.Email = entity.Email;
      this.TeamName = entity.TeamName;
      this.DraftPosition = entity.DraftPosition || null;
      this.IsAdmin = entity.IsAdmin || false;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }

    toEntity() {
        return {
            "rowKey": this.RowKey,
            "League": this.League,
            "Email": this.Email,
            "TeamName": this.TeamName,
            "DraftPosition": this.DraftPosition,
            "partitionKey": this.PartitionKey,
            "etag": this.etag
        };
    }
  }