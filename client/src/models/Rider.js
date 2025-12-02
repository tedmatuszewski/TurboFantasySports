export default class Rider {
    constructor(entity) {
      this.Name = entity.Name;
      this.Number = entity.Number;
      this.Class = entity.Class;
      this.Injury = entity.Injury;
      this.ImageUrl = entity.ImageUrl;
      this.Entries = entity.Entries;
      this.TotalPoints = entity.TotalPoints;
      this.TotalPlaces = entity.TotalPlaces;
      this.Wins = entity.Wins;
      this.Podiums = entity.Podiums;
      this.TopFives = entity.TopFives;
      this.TopTens = entity.TopTens;
      this.AveragePoints = entity.AveragePoints;
      this.AveragePlace = entity.AveragePlace;
      this.TotalOutcomes = entity.TotalOutcomes;
      this.rowKey = entity.rowKey;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
      this.timestamp = entity.timestamp;
    }
  }