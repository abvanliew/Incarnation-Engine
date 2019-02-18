using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class CharacterBuilderUI : MonoBehaviour
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

        private bool InitialCharacter;
        private INECharacter CharacterRef;
        private INECharacter CurrentCharacter;

        private List<int> RaceIDs;

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
            //confirm initial character?
            //close out and open whatever else was under it
        }

        public void ChangeRace()
        {
            CurrentCharacter.ChangeRace( RaceSelector.value );
        }

        public void SetCharacter( INECharacter targetCharacter, bool initialCharacter = false )
        {
            CharacterRef = targetCharacter;
            CurrentCharacter = targetCharacter.Clone;
            InitialCharacter = initialCharacter;
            PopulateCharacter();
        }

        public void Activate( bool state = true )
        {

        }

        private void Start()
        {
            FullNameInput.onValidateInput += delegate( string input, int charIndex, char addedChar ) { return ValidateChar( addedChar ); };
        }

        private char ValidateChar( char validateChar )
        {
            if( !Regex.IsMatch( validateChar.ToString(), INE.Format.ValidCharPattern ) )
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

        private void PopulateCharacter()
        {
            if( InitialCharacter )
            {
                RaceIDs = new List<int>();
                List<string> raceNames = new List<string>();

                foreach( KeyValuePair<int, INERace> race in INE.Data.Races )
                {
                    if( race.Value.StarterRace )
                    {
                        RaceIDs.Add( race.Key );
                        raceNames.Add( race.Value.FullName );
                    }
                }

                RaceSelector.ClearOptions();
                RaceSelector.AddOptions( raceNames );
                RaceSelector.value = 0;
                ChangeRace();

                AttributesGroup.SetAspects( CurrentCharacter.Attributes );
                AttributesGroup.ExpGained = CurrentCharacter.Exp;

                FullNameInput.text = CurrentCharacter.FullName;

                FullNameInput.gameObject.SetActive( true );
                FullNameDisplay.gameObject.SetActive( false );
                RaceSelector.gameObject.SetActive( true );
                RaceDisplay.gameObject.SetActive( false );
            }
            else
            {
                FullNameInput.gameObject.SetActive( false );
                FullNameDisplay.gameObject.SetActive( true );
                RaceSelector.gameObject.SetActive( false );
                RaceDisplay.gameObject.SetActive( true );
            }
        }
    }
}
