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
        public CharacterBuildUI CharacterBuild;
        public CharacterListUI CharacterList;

        public INEInterface( INEInterfaceList newUI )
        {
            Header = newUI.Header;
            Login = newUI.Login;
            ApiTest = newUI.ApiTest;
            TeamList = newUI.TeamList;
            CharacterBuild = newUI.CharacterBuild;
            CharacterList = newUI.CharacterList;
        }

        public void OpenLogin()
        {
            DisableAll();
            Login.gameObject.SetActive( true );
        }

        public void OpenTeamList()
        {
            DisableAll();
            TeamList.gameObject.SetActive( true );
        }

        public void OpenTeam()
        {
            DisableAll();
            //Team.gameObject.SetActive( true );
        }

        public void OpenFormationBuilder( INEFormation editFormation )
        {
            DisableAll();
            //Team.gameObject.SetActive( true );
        }

        public void OpenCharacterList()
        {
            DisableAll();
            TeamList.gameObject.SetActive( true );
        }

        public void OpenCharacterBuilder( INECharacter editCharacter )
        {
            DisableAll();
            TeamList.gameObject.SetActive( true );
        }

        private void DisableAll()
        {
            Login.gameObject.SetActive( false );
            ApiTest.gameObject.SetActive( false );
            TeamList.gameObject.SetActive( false );
            CharacterBuild.gameObject.SetActive( false );
            CharacterList.gameObject.SetActive( false );
        }
    }

    [Serializable] public class INEInterfaceList
    {
        [SerializeField] public HeaderUI Header;
        [SerializeField] public LoginUI Login;
        [SerializeField] public APITestUI ApiTest;
        [SerializeField] public TeamListUI TeamList;
        [SerializeField] public CharacterBuildUI CharacterBuild;
        [SerializeField] public CharacterListUI CharacterList;
    }
}
