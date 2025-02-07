export default class Race {
    constructor(entity) {
      this.Location = entity.Location;
      this.Date = entity.Date;
      this.Region = entity.Region;
      this.Venue = entity.Venue;
      this.Id = entity.rowKey;
    }
  }