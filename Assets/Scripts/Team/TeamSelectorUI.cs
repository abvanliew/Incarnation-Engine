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

        private INETeamEntry Team;

        public void SelectTeam()
        {
            if( Team != null )
            {
                Debug.Log( string.Format( "{0} Selected", Team.TeamName ) );
            }
        }

        public void SetTeam( INETeamEntry team )
        {
            Team = team;
            RefreshUI();
        }

        private void RefreshUI()
        {
            if( Team != null )
            {
                TeamNameDisplay.text = Team.TeamName;
                WealthDisplay.text = INE.FormatCurrency( Team.Wealth );
                CharacterCountDisplay.text = Team.CharacterCount.ToString();
                LeadershipDisplay.value = Team.Leadership < 0 ? 0 : Team.Leadership > 1 ? 1 : Team.Leadership;
            }
        }
    }
}