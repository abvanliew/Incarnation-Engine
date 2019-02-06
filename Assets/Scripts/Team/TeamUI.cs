using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class TeamUI : MonoBehaviour
    {
        public Button NewFormationButton;
        public Button CharactersButton;
        public Button InventoryButton;
        public Button SpellBookButton;
        public Button BackButton;
        
        public void Activate( bool state = true )
        {
            NewFormationButton.interactable = state;
            CharactersButton.interactable = state;
            InventoryButton.interactable = state;
            SpellBookButton.interactable = state;
            BackButton.interactable = state;
        }

        public void ClickNewFormation()
        {

        }

        public void ClickBackButton()
        {

        }

        public void ClickCharactersButton()
        {

        }

        public void ClickInventoryButton()
        {

        }

        public void ClickSpellBookButton()
        {

        }
    }
}
