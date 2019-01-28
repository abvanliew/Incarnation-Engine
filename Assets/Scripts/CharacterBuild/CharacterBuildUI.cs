using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class CharacterBuildUI : MonoBehaviour
    {
        public Button DescriptionTab;
        public Button AttributesTab;
        public Button SkillsTab;
        public Button PerksTab;
        public Button SummaryTab;

        public RectTransform DescriptionFrame;
        public RectTransform BuildOptionsFrame;
        public RectTransform SummaryFrame;

        //Description code
        public InputField FullNameInput;
        public Text FullNameDisplay;
        public Dropdown RaceSelector;
        public Text RaceDisplay; 

        public AspectGroupUI AttributesGroup;
        public AspectGroupUI SkillsGroup;
        public PerksUI PerksGroup;

        public Button DoneButton;

        private bool NewCharacter;
        private INECharacterBuild CharacterRef;
        private INECharacterBuild CurrentCharacter;

        public void ClickDescription()
        {
            SwitchTab( description: true );
        }

        public void ClickAttributes()
        {
            SwitchTab( attributes: true );
        }

        public void ClickSkills()
        {
            SwitchTab( skills: true );
        }

        public void ClickPerks()
        {
            SwitchTab( perks: true );
        }

        public void ClickSummary()
        {
            SwitchTab( summary: true );
        }

        public void ClickDone()
        {

        }


        private void Start()
        {
            FullNameInput.onValidateInput += delegate( string input, int charIndex, char addedChar ) { return ValidateChar( addedChar ); };
        }

        private char ValidateChar( char validateChar )
        {
            if( !Regex.IsMatch( validateChar.ToString(), INE.ValidCharPattern ) )
            {
                validateChar = '\0';
            }
            
            return validateChar;
        }

        private void SwitchTab( bool description = false, bool attributes = false, bool skills = false, bool perks = false, bool summary = false )
        {
            DescriptionFrame.gameObject.SetActive( description );
            BuildOptionsFrame.gameObject.SetActive( attributes || skills || perks );
            AttributesGroup.gameObject.SetActive( attributes );
            SkillsGroup.gameObject.SetActive( skills );
            PerksGroup.gameObject.SetActive( perks );
            SummaryFrame.gameObject.SetActive( summary );
        }
    }
}
