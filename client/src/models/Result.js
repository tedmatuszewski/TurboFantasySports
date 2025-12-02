export default class Result {
    constructor(entity) {
      this.rowKey = entity.rowKey;
      this.Rider = entity.Rider;
      this.Race = entity.Race;
      this.Points = entity.Points;
      this.League = entity.League;
      this.Place = entity.Place;
      this.Member = entity.Member;
    }
  }