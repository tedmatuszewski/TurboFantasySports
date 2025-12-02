export default class Member {
    constructor(entity) {
      this.rowKey = entity.rowKey;
      this.League = entity.League;
      this.Email = entity.Email;
      this.TeamName = entity.TeamName;
      this.DraftPosition = entity.DraftPosition || null;
      this.IsAdmin = entity.IsAdmin || false;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }

    toEntity() {
        return {
            "rowKey": this.rowKey,
            "League": this.League,
            "Email": this.Email,
            "TeamName": this.TeamName,
            "DraftPosition": this.DraftPosition,
            "IsAdmin": this.IsAdmin,
            "partitionKey": this.partitionKey,
            "etag": this.etag
        };
    }
  }