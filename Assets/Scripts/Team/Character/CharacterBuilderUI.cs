using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class CharacterBuilderUI : MonoBehaviour
    {
        //Tab selections
        public Button AppearanceTabButton;
        public Button AspectsTabButton;
        public Button InventoryTabButton;
        public Button ActionsTabButton;
        public Button SpellsTabButton;
        public Button SummaryTabButton;

        //Header
        public Image ProfileIcon;
        public InputField FullNameInput;
        public Text FullNameDisplay;
        public Dropdown RaceSelector;
        public Text RaceDisplay;
        public Text TierDisplay;
        public RectTransform ExpPanel;
        public Slider ExpBar;
        public Slider ExpProjection;
        public Text ProjectionLabel;
        public Dropdown ProjectionSelector;

        //public PerksUI PerksGroup;
        public AspectGroupUI AttributeGroup;
        public AspectGroupUI SkillGroup;

        public Button DoneButton;

        private bool InitialCharacter;
        private INECharacter ReferenceCharacter;
        private INECharacter CurrentCharacter;

        private List<int> RaceIDs;
        private readonly Color WarningBackground = new Color( 1f, .6f, .6f );

        public void ClickAppearanceTab()
        {

        }

        public void ClickAspectTab()
        {

        }

        public void ClickInventoryTab()
        {

        }

        public void ClickActionsTab()
        {

        }

        public void ClickSpellsTab()
        {

        }

        public void ClickSummaryTab()
        {

        }

        public void ClickDone()
        {
            //confirm initial character?
            //close out and open whatever else was under it
        }

        public void ChangeName()
        {
            CheckNameInvalid();
        }

        public void ChangeRace()
        {
            if( RaceIDs != null && RaceSelector.value >= 0 && RaceSelector.value < RaceIDs.Count )
            {
                CurrentCharacter.ChangeRace( RaceIDs[RaceSelector.value] );
                Recalculate();
            }
        }

        public void ChangeProjection()
        {
            SetProjectionState( ProjectionSelector.value );
            Recalculate();
        }

        public void ChangeExpProjection()
        {
            if( ExpProjection.value < CurrentCharacter.Exp )
                ExpProjection.value = CurrentCharacter.Exp;
        }

        public void SetCharacter( INECharacter targetCharacter, bool initialCharacter = false )
        {
            ReferenceCharacter = targetCharacter;
            CurrentCharacter = targetCharacter.Clone;
            InitialCharacter = initialCharacter;

            PopulateCharacter();
        }

        public void Activate( bool state = true )
        {

        }

        private void Start()
        {
            FullNameInput.onValidateInput += delegate ( string input, int charIndex, char addedChar ) { return ValidateChar( addedChar ); };
        }

        private char ValidateChar( char validateChar )
        {
            if( !Regex.IsMatch( validateChar.ToString(), INE.Format.ValidCharPattern ) )
            {
                validateChar = '\0';
            }

            return validateChar;
        }

        private void SwitchTab( bool appearance = false, bool aspect = false, bool summary = false )
        {
            //DescriptionFrame.gameObject.SetActive( description );
            //BuildOptionsFrame.gameObject.SetActive( attributes || skills || perks );
            //AttributesGroup.gameObject.SetActive( attributes );
            //SkillsGroup.gameObject.SetActive( skills );
            //PerksGroup.gameObject.SetActive( perks );
            //SummaryFrame.gameObject.SetActive( summary );
        }

        private void PopulateCharacter()
        {
            if( CurrentCharacter != null )
            {
                //Hook up attribute and skull UI code
                AttributeGroup.SetAspects( this, CurrentCharacter.Attributes );
                SkillGroup.SetAspects( this, CurrentCharacter.Skills );

                SpellsTabButton.gameObject.SetActive( CurrentCharacter.IsCaster );

                //some code to populate character icon

                //prep tier
                int tier = CurrentCharacter.Tier < 1 ? 1 : CurrentCharacter.Tier > 5 ? 5 : CurrentCharacter.Tier;
                ExpPanel.anchorMax = new Vector2( (float)tier / 5f, 1f );
                TierDisplay.text = tier.ToString();
                ExpBar.maxValue = CurrentCharacter.MaxExp;
                ExpBar.value = CurrentCharacter.Exp;
                ExpProjection.maxValue = CurrentCharacter.MaxExp;

                //Defaults to no projection
                ProjectionSelector.value = 0;
                SetProjectionState( 0 );

                if( InitialCharacter )
                {
                    FullNameInput.text = CurrentCharacter.FullName;

                    //Populate Race list and update race reference
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
                    //Ensure that current character has the values for Race
                    ChangeRace();

                    //Hide static name and race displays replacing them with edit fields, also disable projection selections
                    FullNameInput.gameObject.SetActive( true );
                    FullNameDisplay.gameObject.SetActive( false );
                    RaceSelector.gameObject.SetActive( true );
                    RaceDisplay.gameObject.SetActive( false );
                    ProjectionLabel.gameObject.SetActive( false );
                    ProjectionSelector.gameObject.SetActive( false );

                    //Calculate initial numbers
                    CurrentCharacter.ProjectRanks( CurrentCharacter.Exp, setToIdeal: true );
                    AttributeGroup.SetInitial();
                    SkillGroup.SetInitial();
                }
                else
                {
                    //set name and race
                    FullNameDisplay.text = CurrentCharacter.FullName;
                    RaceDisplay.text = INE.Data.Races.ContainsKey( CurrentCharacter.RaceID ) ? INE.Data.Races[CurrentCharacter.RaceID].FullName : "Unknown Race";

                    //Use only static name and race displays, enable projection options
                    FullNameInput.gameObject.SetActive( false );
                    FullNameDisplay.gameObject.SetActive( true );
                    RaceSelector.gameObject.SetActive( false );
                    RaceDisplay.gameObject.SetActive( true );
                    ProjectionLabel.gameObject.SetActive( true );
                    ProjectionSelector.gameObject.SetActive( true );

                    //Set current character values
                    CurrentCharacter.CurrentRanks();
                    AttributeGroup.SetCurrent();
                    SkillGroup.SetCurrent();
                }
            }
        }

        private void CheckNameInvalid()
        {
            if( Regex.IsMatch( FullNameInput.text, INE.Format.ValidNamePattern ) )
            {
                FullNameInput.targetGraphic.color = Color.white;
            }
            else
            {
                FullNameInput.targetGraphic.color = WarningBackground;
            }
        }

        private void SetProjectionState( int state )
        {
            if( state == 0 )
            {
                ExpProjection.value = CurrentCharacter.Exp;
                ExpProjection.interactable = false;
                AttributeGroup.EnableProjection( false );
                SkillGroup.EnableProjection( false );
            }
            else if( state == 1 )
            {
                ExpProjection.interactable = true;
                AttributeGroup.EnableProjection( true );
                SkillGroup.EnableProjection( true );
            }
            else if( state == 2 )
            {
                ExpProjection.value = CurrentCharacter.MaxExp;
                ExpProjection.interactable = false;
                AttributeGroup.EnableProjection( true );
                SkillGroup.EnableProjection( true );
            }
        }

        public void Recalculate()
        {
            if( InitialCharacter )
            {
                CurrentCharacter.ProjectRanks( CurrentCharacter.Exp, setToIdeal: true );
                AttributeGroup.UpdateInitial();
                SkillGroup.UpdateInitial();
            }
            else if( ProjectionSelector.value == 1 )
            {
                CurrentCharacter.ProjectRanks( ExpProjection.value - CurrentCharacter.Exp );
                AttributeGroup.UpdateProjected();
                SkillGroup.UpdateProjected();
            }
            else if( ProjectionSelector.value == 2 )
            {
                CurrentCharacter.ProjectRanks( ExpProjection.value - CurrentCharacter.Exp, setToIdeal: true );
                AttributeGroup.UpdateProjected();
                SkillGroup.UpdateProjected();
            }
        }
    }
}