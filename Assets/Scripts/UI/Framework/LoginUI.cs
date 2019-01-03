using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IncarnationEngine
{
    public class LoginUI : MonoBehaviour
    {
        public InputField UsernameInput;
        public InputField PasswordInput;
        public Button LoginButton;

        private EventSystem CurEventSystem;

        private void Start()
        {
            UsernameInput.Select();
        }

        private void Update()
        {
            bool reverseCycle = false;

            if( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) )
                reverseCycle = true;

            if( Input.GetKeyDown( KeyCode.Tab ) )
            {
                bool worked = false;

                if( EventSystem.current != null )
                {
                    GameObject selected = EventSystem.current.currentSelectedGameObject;
                    if( selected != null )
                    {
                        Selectable current = (Selectable)selected.GetComponent( "Selectable" );
                        if( current != null )
                        {
                            Selectable next = reverseCycle ? current.FindSelectableOnUp() : current.FindSelectableOnDown();
                            if( next != null )
                            {
                                next.Select();
                                worked = true;
                            }
                        }
                    }

                    if( !worked && !reverseCycle )
                        UsernameInput.Select();
                    else if( !worked & reverseCycle )
                        LoginButton.Select();
                }
            }

            if( Input.GetKeyDown( KeyCode.KeypadEnter ) || Input.GetKeyDown( KeyCode.Return ) )
            {
                ClickLogin();
            }
        }

        public void ClickLogin()
        {
            if( UsernameInput.text == "" )
            {
                Debug.Log( "Username required" );
            }
            else if( PasswordInput.text == "" )
            {
                Debug.Log( "Password required" );
            }
            else
            {
                INE.Login( UsernameInput.text, PasswordInput.text );
            }
        }
    }
}