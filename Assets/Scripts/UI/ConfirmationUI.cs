using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public enum ConfirmationOption { Yes, No, Ok, Cancel };
    public delegate void ConfirmationClick( ConfirmationOption clicked );

    public class ConfirmationUI : MonoBehaviour
    {
        public Text Question;
        public Button YesButton;
        public Button NoButton;
        public Button OkButton;
        public Button CancelButton;

        private ConfirmationClick Click;

        public void ClickYes()
        {
            INE.UI.CloseDialog();
            Click( ConfirmationOption.Yes );
        }

        public void ClickNo()
        {
            INE.UI.CloseDialog();
            Click( ConfirmationOption.No );
        }

        public void ClickOk()
        {
            INE.UI.CloseDialog();
            Click( ConfirmationOption.Ok );
        }

        public void ClickCancel()
        {
            INE.UI.CloseDialog();
            Click( ConfirmationOption.Cancel );
        }

        public bool SetQuestion( ConfirmationClick click, string question, bool yesButton = false, bool noButton = false, bool okButton = false, bool cancelButton = false )
        {
            bool validDialog = false;

            if( click != null && ( yesButton || noButton || okButton || cancelButton ) )
            {
                validDialog = true;
                Click = click;
                Question.text = question ?? "Question?";
                YesButton.gameObject.SetActive( yesButton );
                NoButton.gameObject.SetActive( noButton );
                OkButton.gameObject.SetActive( okButton );
                CancelButton.gameObject.SetActive( cancelButton );
            }

            return validDialog;
        }
    }
}
