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

        private TeamListUI Parent;
        private INETeamEntryResponse Team;

        public void SelectTeam()
        {
            LoadTeam();
        }

        public void SetTeam( INETeamEntryResponse team, TeamListUI parent )
        {
            if( team != null && parent != null )
            {
                Team = team;
                Parent = parent;
                TeamNameDisplay.text = Team.TeamName;
                WealthDisplay.text = INE.Format.Currency( Team.Wealth );
                CharacterCountDisplay.text = Team.CharacterCount.ToString();
                LeadershipDisplay.value = Team.Leadership < 0 ? 0 : Team.Leadership > 1 ? 1 : Team.Leadership;
            }
        }

        public void Activate( bool state = true )
        {
            SelectTeamButton.interactable = state;
            LeadershipDisplay.interactable = state;
        }

        private async void LoadTeam()
        {
            if( Team != null && Parent != null )
            {
                Parent.Activate( false );
                bool loaded = await INE.Ledger.LoadTeam( Team.Team );
                
                if( !loaded )
                {
                    Parent.Activate();
                }
            }
        }
    }
}