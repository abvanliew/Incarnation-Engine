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

        public async void LoadTeamList()
        {
            TeamList = await INE.GetData<INETeamSummary>( "team/list/" );

            if( TeamList != null )
            {
                INE.UI.OpenTeamList();
            }
        }

        public void StartNewTeam()
        {
            //CurrentTeam = new INETeam();
        }

        public void CommitNewTeam( string newTeamName )
        {
            if( Regex.IsMatch( newTeamName, INE.Format.ValidNamePattern ) )
            {
                //Start post command
                //Get returned Team ID and update CurrentTeam with it
                //Open CharacterBuilder for new character
            }
        }

        public void StartInitialCharacter()
        {
            //Validate that current team has no characters
            if( CurrentTeam != null && CurrentTeam.CharacterCount == 0 )
            {
                CreateInitialCharacter();
            }
        }

        public async void LoadTeam( int teamID )
        {
            //Run a Get request to pull down the full team data
            //If it fails to get team, refresh team list
            //Otherwise launch into game menu
            Debug.Log( "UI Off" );
            await Task.Delay( 500 );
            Debug.Log( "UI On" );
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
}
