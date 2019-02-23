using System;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEInterface
    {
        public HeaderUI Header;
        public LoginUI Login;
        public APITestUI ApiTest;
        public TeamListUI TeamList;
        public NewTeamUI NewTeam;
        public TeamUI Team;
        public CharacterBuilderUI CharacterBuilder;
        public CharacterListUI CharacterList;

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
    }
}
