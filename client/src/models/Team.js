export default class Team {
    constructor(entity) {
      this.League = entity.League;
      this.Rider = entity.Rider;
      this.Id = entity.rowKey;
      this.Owner = entity.Owner;
    }
  }