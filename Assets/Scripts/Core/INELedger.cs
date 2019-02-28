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
                {
                    CreateInitialCharacter();
                }
                //else load up the team ui stuff
            }

            return loaded;
        }

        private void CreateInitialCharacter()
        {
            InitialCharacter = new INECharacter();
            INE.UI.OpenCharacterBuilder( InitialCharacter, true );
            //INE.UI.OpenCharacterBuilder( Characters[1], false );
        }

        public void NewCharacterBuild()
        {
            //Characters.Add( 
            //    new INECharacter(
            //        "Commander - Default", 0, INE.Data.Archetypes[0], 5, 400, new Dictionary<int, float>()
            //        {
            //            { 0, 120f }, //Strength
            //            { 1, 120f }, //Finesse
            //            { 2, 120f }, //Perception
            //            { 3, 120f }, //Speed
            //            { 4, 120f }, //Endurance
            //            { 5, 120f }, //Resistance
            //            { 6, 120f }, //Potency
            //            { 7, 120f }, //Essence
            //            { 8, 120f }  //Affinity
            //        },
            //        new Dictionary<int, float>()
            //        {
            //            { 0, 120f }, //Striking
            //            { 1, 120f }, //Shooting
            //            { 2, 120f }, //Defending
            //            { 3, 120f }, //Disruption
            //            { 4, 120f }, //Combat Mobility
            //            { 5, 120f }, //Stealth
            //            { 6, 120f }, //Spell Mastery
            //            { 7, 120f }, //Fire
            //            { 8, 120f }, //Frost
            //            { 9, 120f }, //Electricity
            //            { 10, 120f }, //Water
            //            { 11, 120f }, //Benevolent
            //            { 12, 120f }, //Malevolent
            //            { 13, 120f }  //Earth
            //        }
            //    ) );

            Characters.Add(
                new INECharacter(
                    "Commander - Tank", 0, INE.Data.Archetypes[0], 5, 400, new Dictionary<int, float>()
                    {
                        { 0, 80f }, //Strength
                        { 2, 60f }, //Perception
                        { 3, 20f }, //Speed
                        { 4, 80f }, //Endurance
                        { 5, 60f } //Resistance
                    },
                    new Dictionary<int, float>()
                    {
                        { 0, 80f }, //Striking
                        { 2, 120f }, //Defending
                        { 3, 40f }, //Disruption
                        { 4, 40f } //Combat Mobility
                    }
                ) );

            Characters.Add(
                new INECharacter(
                    "Commander - Evocation", 0, INE.Data.Archetypes[0], 5, 400, new Dictionary<int, float>()
                    {
                        { 2, 40f }, //Perception
                        { 4, 20f }, //Endurance
                        { 5, 25f }, //Resistance
                        { 6, 120f }, //Potency
                        { 7, 100f }, //Essence
                        { 8, 75f } //Affinity
                    },
                    new Dictionary<int, float>()
                    {
                        { 2, 40f }, //Defending
                        { 4, 20f }, //Combat Mobility
                        { 6, 80f }, //Spell Mastery
                        { 7, 120f }, //Fire
                        { 8, 120f }, //Frost
                        { 9, 120f }, //Electricity

                    }
                ) );

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
