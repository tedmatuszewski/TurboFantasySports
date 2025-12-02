export default class Outcome {
    constructor(entity) {
      this.rowKey = entity.rowKey;
      this.Rider = entity.Rider;
      this.Race = entity.Race;
      this.Points = entity.Points;
      this.Place = entity.Place;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }
  }