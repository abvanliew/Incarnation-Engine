using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class TeamListUI : MonoBehaviour
    {
        public Button NewTeam;
        public RectTransform ListParent;
        public RectTransform TeamSelectorPrefab;
        private List<TeamSelectorUI> Teams;

        public void ClickNewTeam()
        {
            INE.UI.OpenNewTeam();
        }

        public void Activate( bool state = true )
        {
            NewTeam.interactable = state;
            if( Teams != null && Teams.Count > 0 )
            {
                for( int i = 0; i < Teams.Count; i++ )
                {
                    Teams[i].Activate( state );
                }
            }
        }

        private void OnEnable()
        {
            PopulateTeams();
        }

        private async void PopulateTeams()
        {
            if( Teams == null )
            {
                Teams = new List<TeamSelectorUI>();
            }
            else
            {
                for( int i = 0; i < Teams.Count; i++ )
                {
                    Teams[i].gameObject.SetActive( false );
                }
            }

            await INE.Ledger.LoadTeamList();

            if( INE.Ledger.TeamList != null && INE.Ledger.TeamList.Count > 0 )
            {
                int apiCount = INE.Ledger.TeamList.Count;
                int uiCount = Teams.Count;
                int maxCount = apiCount >= uiCount ? apiCount : uiCount;

                for( int i = 0; i < maxCount; i++ )
                {
                    if( i >= uiCount )
                    {
                        RectTransform newPrefab = Instantiate( TeamSelectorPrefab );
                        Teams.Add( newPrefab.GetComponent<TeamSelectorUI>() );
                        newPrefab.transform.SetParent( ListParent.transform, false );
                    }

                    if( i < apiCount )
                    {
                        Teams[i].gameObject.SetActive( true );
                        Teams[i].SetTeam( INE.Ledger.TeamList[i] );
                    }
                }
            }
        }
    }
}