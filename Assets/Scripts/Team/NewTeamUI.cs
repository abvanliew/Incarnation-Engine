using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class NewTeamUI : MonoBehaviour
    {
        public InputField TeamName;
        public Button CreateTeamButton;
        public Button CancelButton;

        public void ClickCreateTeam()
        {
            CommitTeam();
        }

        public void ClickCancel()
        {
            INE.UI.OpenTeamList();
        }

        public void Activate( bool state = true )
        {
            TeamName.interactable = state;
            CreateTeamButton.interactable = state;
            CancelButton.interactable = state;
        }

        private void Start()
        {
            TeamName.onValidateInput += delegate ( string input, int charIndex, char addedChar ) { return ValidateChar( addedChar ); };
            TeamName.Select();
        }

        private char ValidateChar( char validateChar )
        {
            if( !Regex.IsMatch( validateChar.ToString(), INE.Format.ValidCharPattern ) )
            {
                validateChar = '\0';
            }

            return validateChar;
        }

        private async void CommitTeam()
        {
            if( Regex.IsMatch( TeamName.text, INE.Format.ValidNamePattern ) )
            {
                Activate( false );
                int teamCreated = await INE.Ledger.CommitNewTeam( TeamName.text );

                if( teamCreated >= 0 )
                {
                    bool teamLoaded = await INE.Ledger.LoadTeam( teamCreated );

                    if( teamLoaded )
                    {
                        INE.UI.OpenCharacterBuilder( null );
                    }
                    else
                    {
                        Debug.Log( "Failed to load team" );
                        INE.UI.OpenTeamList();
                    }
                }
                else
                {
                    Debug.Log( "Team failed to be created" );
                    Activate( true );
                }
            }
            else
            {
                Debug.Log( "Invalid Team Name" );
                //some code to flash the screen red or something
            }
        }
    }
}
