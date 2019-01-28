using UnityEngine;
using System.Collections.Generic;

namespace IncarnationEngine
{
    public class INELedger
    {
        public List<INETeam> Teams;
        public INECharacterBuild CharacterBuild;
        
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

        public void NewCharacterBuild()
        {
            Dictionary<int, float> newMods = new Dictionary<int, float>()
            {
                { 1, 1 },
                { 2, 1 },
                { 3, 1 },
                { 4, 1 },
                { 5, 1 },
                { 6, 1.5f }
            };
            CharacterBuild = new INECharacterBuild( 1000, true, newMods );
            INE.Core.UI.CharacterBuildPanel.AttributesGroup.SetAspects( CharacterBuild.Attributes );
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
