using UnityEngine;
using System.Collections.Generic;

namespace IncarnationEngine
{
    public class INELedger
    {
        public List<INETeam> Teams;

        public void StartNewTeam()
        {

        }

        public void LoadTeam()
        {

        }

        public void RefreshTeamList()
        {
            if( Teams != null )
            {
                INE.Core.UI.TeamList.PopulateTeams();
            }
        }
    }

    public class INETeam
    {
        public int TeamIndex;
        public string TeamName;
        public float Wealth;
        public int CharacterCount;
        public float Leadership;
    }
}
