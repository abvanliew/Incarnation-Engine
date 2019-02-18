using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class TeamSelectorUI : MonoBehaviour
    {
        public Button SelectTeamButton;
        public Text TeamNameDisplay;
        public Text WealthDisplay;
        public Text CharacterCountDisplay;
        public Slider LeadershipDisplay;

        private INETeamEntryResponse Team;

        public void SelectTeam()
        {
            if( Team != null )
            {
                Debug.Log( string.Format( "{0} Selected", Team.TeamName ) );
                INE.Ledger.LoadTeam( Team.TeamIndex );
            }
        }

        public void SetTeam( INETeamEntryResponse team )
        {
            Team = team;
            RefreshUI();
        }

        public void Activate( bool state = true )
        {
            SelectTeamButton.interactable = state;
            LeadershipDisplay.interactable = state;
        }

        private void RefreshUI()
        {
            if( Team != null )
            {
                TeamNameDisplay.text = Team.TeamName;
                WealthDisplay.text = INE.Format.Currency( Team.Wealth );
                CharacterCountDisplay.text = Team.CharacterCount.ToString();
                LeadershipDisplay.value = Team.Leadership < 0 ? 0 : Team.Leadership > 1 ? 1 : Team.Leadership;
            }
        }
    }
}