using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IncarnationEngine
{
    public class INELedger
    {
        public List<INETeamEntryResponse> TeamList = new List<INETeamEntryResponse>();
        public INETeam CurrentTeam = null;
        public INECharacter InitialCharacter = null;
        public List<INECharacter> Characters = new List<INECharacter>();
        public bool TeamLoaded { get{ return CurrentTeam != null; } }

        public async Task<bool> LoadTeamList()
        {
            bool listPopulated = false;
            TeamList = await INE.GetData<List<INETeamEntryResponse>>( "team/list/" );

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
            bool loaded = false;
            INETeamSelector selectTeam = new INETeamSelector();
            selectTeam.Team = teamID;

            INETeamResponse team = await INE.PostData<INETeamResponse>( "team", selectTeam );
            if( team.Team == teamID )
            {
                loaded = true;
                List<INECharacterResponse> characters = await INE.PostData<List<INECharacterResponse>>( "team/character/list", selectTeam );
                if( characters.Count == 0 )
                    INE.UI.OpenCharacterBuilder( null );
            }

            return loaded;
        }

        private void CreateInitialCharacter()
        {
            Dictionary<int, float> newMods = new Dictionary<int, float>()
            {
                { 0, 1 },
                { 1, 1 },
                { 2, 1 },
                { 3, 1 },
                { 4, 1 },
                { 5, 1.5f }
            };
            InitialCharacter = new INECharacter( 500, true, newMods );
            INE.UI.OpenCharacterBuilder( InitialCharacter, true );
        }

        public void NewCharacterBuild()
        {
            CreateInitialCharacter();
        }
    }

    public class INETeamEntryResponse
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

    public class INETeamSelector
    {
        public int Team;
    }

    public class INENewTeamResponse
    {
        public int Team;
    }

    public class INETeamResponse
    {
        public int Team;
        public string TeamName;
        public float Wealth;
        public float Leadership;
    }

    public class INECharacterResponse
    {
        public int ID;
        public string FullName;
        public int Tier;
        public float Exp;
        public int Race;
    }
}
