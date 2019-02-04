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
            INE.Ledger.StartNewTeam();
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

        private void PopulateTeams()
        {
            if( INE.Ledger.TeamList != null && INE.Ledger.TeamList.Count > 0 )
            {
                Teams = new List<TeamSelectorUI>();

                for( int i = 0; i < INE.Ledger.TeamList.Count; i++ )
                {
                    RectTransform newPrefab = Instantiate( TeamSelectorPrefab );
                    Teams.Add( newPrefab.GetComponent<TeamSelectorUI>() );
                    newPrefab.transform.SetParent( ListParent.transform, false );
                    Teams[i].SetTeam( INE.Ledger.TeamList[i] );
                }
            }
        }
    }
}