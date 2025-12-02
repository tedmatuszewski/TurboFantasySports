export default class League {
    constructor(entity) {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.DraftComplete = entity.DraftComplete;
      this.rowKey = entity.rowKey;
      this.partitionKey = entity.partitionKey;
      this.etag = entity.etag;
    }

    toEntity() {
        return {
            "rowKey": this.rowKey,
            "Name": this.Name,
            "Description": this.Description,
            "DraftComplete": this.DraftComplete,
            "partitionKey": this.partitionKey,
            "etag": this.etag
        };
    }
  }