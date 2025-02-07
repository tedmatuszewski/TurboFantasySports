export default class Rider {
    constructor(entity) {
      this.Name = entity.Name;
      this.Number = entity.Number;
      this.Id = entity.rowKey;
    }
  }