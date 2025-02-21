export default class Member {
    constructor(entity) {
      this.RowKey = entity.rowKey;
      this.LeagueGuid = entity.LeagueGuid;
      this.UserGuid = entity.UserGuid;
      this.TeamName = entity.TeamName;
    }
  }