export default class Outcome {
    constructor(entity) {
      this.RowKey = entity.rowKey;
      this.Rider = entity.Rider;
      this.Race = entity.Race;
      this.Points = entity.Points;
      this.Place = entity.Place;
      this.PartitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }
  }