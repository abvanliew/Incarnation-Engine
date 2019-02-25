using System;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEInterface
    {
        private readonly HeaderUI Header;
        private readonly LoginUI Login;
        private readonly APITestUI ApiTest;
        private readonly TeamListUI TeamList;
        private readonly NewTeamUI NewTeam;
        private readonly TeamUI Team;
        private readonly CharacterBuilderUI CharacterBuilder;
        private readonly CharacterListUI CharacterList;

        private readonly ConfirmationUI ConfirmationDialog;
        private readonly RectTransform GreyScreen;

        public INEInterface( INEInterfaceList newUI )
        {
            Header = newUI.Header;
            Login = newUI.Login;
            ApiTest = newUI.ApiTest;
            TeamList = newUI.TeamList;
            NewTeam = newUI.NewTeam;
            Team = newUI.Team;
            CharacterBuilder = newUI.CharacterBuilder;
            CharacterList = newUI.CharacterList;
            ConfirmationDialog = newUI.ConfirmationDialog;
            GreyScreen = newUI.GreyScreen;
        }

        public void OpenLogin()
        {
            SwitchMenu( login: true );
        }

        public void OpenTeamList()
        {
            SwitchMenu( teamList: true );
        }

        public void OpenNewTeam()
        {
            SwitchMenu( newTeam: true );
        }

        public void OpenTeam()
        {
            SwitchMenu( team: true );
        }

        public void OpenFormationBuilder( INEFormation editFormation )
        {
            //SwitchMenu();
        }

        public void OpenCharacterList()
        {
            SwitchMenu( characterList: true );
        }

        public void OpenCharacterBuilder( INECharacter editCharacter, bool initialCharacter = false )
        {
            if( editCharacter != null )
            {
                SwitchMenu( characterBuilder: true );
                CharacterBuilder.SetCharacter( editCharacter, initialCharacter );
            }
        }

        public bool OpenConfirmationDialog( ConfirmationClick click, string question, 
            bool yesButton = false, bool noButton = false, bool okButton = false, bool cancelButton = false )
        {
            bool validDialog = false;

            if( click != null && ( yesButton || noButton || okButton || cancelButton ) )
            {
                validDialog = ConfirmationDialog.SetQuestion( click, question, yesButton, noButton, okButton, cancelButton );

                if( validDialog )
                {
                    GreyScreen.gameObject.SetActive( true );
                    ConfirmationDialog.gameObject.SetActive( true );
                }
            }

            return validDialog;
        }

        public void CloseDialog()
        {
            GreyScreen.gameObject.SetActive( false );
            ConfirmationDialog.gameObject.SetActive( false );
        }

        private void SwitchMenu( bool login = false, bool apiTest = false, bool teamList = false, bool newTeam = false, bool team = false,
            bool characterBuilder = false, bool characterList = false )
        {
            if( Login != null )
                Login.gameObject.SetActive( login );
            if( ApiTest != null )
                ApiTest.gameObject.SetActive( apiTest );
            if( TeamList != null )
                TeamList.gameObject.SetActive( teamList );
            if( NewTeam != null )
                NewTeam.gameObject.SetActive( newTeam );
            if( Team != null )
                Team.gameObject.SetActive( team );
            if( CharacterBuilder != null )
                CharacterBuilder.gameObject.SetActive( characterBuilder );
            if( CharacterList != null )
                CharacterList.gameObject.SetActive( characterList );
        }
    }

    [Serializable] public class INEInterfaceList
    {
        [SerializeField] public HeaderUI Header;
        [SerializeField] public LoginUI Login;
        [SerializeField] public APITestUI ApiTest;
        [SerializeField] public TeamListUI TeamList;
        [SerializeField] public NewTeamUI NewTeam;
        [SerializeField] public TeamUI Team;
        [SerializeField] public CharacterBuilderUI CharacterBuilder;
        [SerializeField] public CharacterListUI CharacterList;
        [SerializeField] public ConfirmationUI ConfirmationDialog;
        [SerializeField] public RectTransform GreyScreen;
    }
}
