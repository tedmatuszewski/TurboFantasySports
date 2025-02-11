export default class Team {
    constructor(entity) {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.Id = entity.rowKey;
    }
  }