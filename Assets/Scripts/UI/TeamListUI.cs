﻿using System.Collections.Generic;
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

        public void PopulateTeams()
        {
            if( INE.Ledger.Teams != null && INE.Ledger.Teams.Count > 0 )
            {
                Teams = new List<TeamSelectorUI>();

                for( int i = 0; i < INE.Ledger.Teams.Count; i++ )
                {
                    RectTransform newPrefab = Instantiate( TeamSelectorPrefab );
                    Teams.Add( newPrefab.GetComponent<TeamSelectorUI>() );
                    newPrefab.transform.SetParent( ListParent.transform, false );
                    Teams[i].SetTeam( INE.Ledger.Teams[i] );
                }
            }
        }
    }
}