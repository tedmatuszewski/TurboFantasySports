export default class Member {
    constructor(entity) {
      this.UserGuid = entity.UserGuid;
      this.LeagueGuid = entity.LeagueGuid;
      this.Id = entity.rowKey;
      this.TeamName = entity.TeamName;
    }
  }