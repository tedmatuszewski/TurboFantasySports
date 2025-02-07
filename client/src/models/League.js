export default class League {
    constructor(entity) {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.Id = entity.rowKey;
    }
  }