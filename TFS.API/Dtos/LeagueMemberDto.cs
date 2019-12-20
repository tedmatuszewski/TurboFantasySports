using System;

namespace TFS.API
{
    public class LeagueMemberDto
    {
        public Guid Guid { get; set; }

        public Guid LeagueGuid { get; set; }

        public Guid UserGuid { get; set; }

        public string Name { get; set; }

        public string LeagueType { get; set; }
        
        public Guid LeagueManagerGuid { get; set; }
        
        public string LeagueName { get; set; }

        public int LeagueYear { get; set; }
    }
}