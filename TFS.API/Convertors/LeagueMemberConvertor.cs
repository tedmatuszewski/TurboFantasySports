using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFS.API.Data;

namespace TFS.API.Convertors
{
    public static class LeagueMemberConvertor
    {
        public static LeagueMemberDto Convert(this LeagueMember entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new LeagueMemberDto()
            {
                Guid = entity.Guid,
                LeagueGuid = entity.LeagueGuid,
                UserGuid = entity.UserGuid,
                Name = entity.Name,
                LeagueManagerGuid = entity.LeagueGu.ManagerGuid,
                LeagueName = entity.LeagueGu.Name,
                LeagueType = entity.LeagueGu.Type,
                LeagueYear = entity.LeagueGu.Year
            };
        }
    }
}
