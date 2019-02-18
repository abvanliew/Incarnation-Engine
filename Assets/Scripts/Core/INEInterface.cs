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
        public CharacterBuilderUI CharacterBuild;
        public CharacterListUI CharacterList;

        public INEInterface( INEInterfaceList newUI )
        {
            Header = newUI.Header;
            Login = newUI.Login;
            ApiTest = newUI.ApiTest;
            TeamList = newUI.TeamList;
            NewTeam = newUI.NewTeam;
            Team = newUI.Team;
            CharacterBuild = newUI.CharacterBuild;
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

        public void OpenCharacterBuilder( INECharacter editCharacter )
        {
            SwitchMenu( characterBuilder: true );

            if( editCharacter == null )
            {
                CharacterBuild.SetCharacter( new INECharacter(), true );
            }
        }

        private void SwitchMenu( bool login = false, bool apiTest = false, bool teamList = false, bool newTeam = false, bool team = false,
            bool characterBuilder = false, bool characterList = false )
        {
            Login.gameObject.SetActive( login );
            ApiTest.gameObject.SetActive( apiTest );
            TeamList.gameObject.SetActive( teamList );
            NewTeam.gameObject.SetActive( newTeam );
            Team.gameObject.SetActive( team );
            CharacterBuild.gameObject.SetActive( characterBuilder );
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
        [SerializeField] public CharacterBuilderUI CharacterBuild;
        [SerializeField] public CharacterListUI CharacterList;
    }
}
