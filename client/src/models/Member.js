export default class Member {
    constructor(entity) {
      this.RowKey = entity.rowKey;
      this.League = entity.League;
      this.Email = entity.Email;
      this.TeamName = entity.TeamName;
    }
  }