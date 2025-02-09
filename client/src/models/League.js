export default class League {
    constructor(entity, auth) {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.Id = entity.rowKey;
      this.Members = [];
    }

    setMembers(members) {
      members = members.filter(member => member.LeagueGuid === this.Id);

      this.Members = members;
    }

    isMember(sub) {
      return  this.Members.some(member => member.UserGuid === sub);
    }
  }