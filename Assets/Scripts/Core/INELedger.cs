using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IncarnationEngine
{
    public class INELedger
    {
        public List<INETeamSummary> TeamList = new List<INETeamSummary>();
        public INETeam CurrentTeam = null;
        public INECharacter InitialCharacter = null;
        public bool TeamLoaded { get{ return CurrentTeam != null; } }

        public async Task<bool> LoadTeamList()
        {
            bool listPopulated = false;
            TeamList = await INE.GetData<List<INETeamSummary>>( "team/list/" );

            if( TeamList != null && TeamList.Count > 0 )
            {
                listPopulated = true;
            }

            return listPopulated;
        }

        public async Task<int> CommitNewTeam( string newTeamName )
        {
            int teamCreated = -1;
            
            if( Regex.IsMatch( newTeamName, INE.Format.ValidNamePattern ) )
            {
                INENewTeamResponse result = await INE.PostData<INENewTeamResponse>( "team/new", new INENewTeamPost( newTeamName ) );
                if( result != null)
                {
                    teamCreated = result.Team;
                }
            }

            return teamCreated;
        }

        public void StartInitialCharacter()
        {
            //Validate that current team has no characters
            if( CurrentTeam != null && CurrentTeam.CharacterCount == 0 )
            {
                CreateInitialCharacter();
            }
        }

        public async Task<bool> LoadTeam( int teamID )
        {
            //Run a Get request to pull down the full team data
            //If it fails to get team, refresh team list
            //Otherwise launch into game menu
            Debug.Log( "UI Off" );
            await Task.Delay( 500 );
            Debug.Log( "UI On" );

            return true;
        }

        private void CreateInitialCharacter()
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
            InitialCharacter = new INECharacter( 1000, true, newMods );
            INE.UI.CharacterBuild.AttributesGroup.SetAspects( InitialCharacter.Attributes );
        }
    }

    public class INETeamSummary
    {
        public int TeamIndex;
        public string TeamName;
        public float Wealth;
        public int CharacterCount;
        public float Leadership;
    }

    public class INENewTeamPost
    {
        public string TeamName;

        public INENewTeamPost( string teamName )
        {
            TeamName = teamName;
        }
    }

    public class INENewTeamResponse
    {
        public int Team;
    }
}
