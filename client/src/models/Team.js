export default class Team {
    constructor(entity) {
      this.RowKey = entity.rowKey;
      this.Rider = entity.Rider;
      this.League = entity.League;
      this.Member = entity.Member;
    }
  }