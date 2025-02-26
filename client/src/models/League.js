export default class League {
    constructor(entity, auth) {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.RowKey = entity.rowKey;
      this.Members = [];
    }
  }