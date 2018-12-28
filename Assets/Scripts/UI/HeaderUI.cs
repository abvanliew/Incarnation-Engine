using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class HeaderUI : MonoBehaviour
    {
        public Text Displayname;
        public Button SignOut;

        private void Update()
        {
            Displayname.text = INE.DisplayName;
        }

        public void ClickSignOut()
        {
            INE.SignOut();
        }
    }
}