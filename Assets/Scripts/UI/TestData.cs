using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class TestData : MonoBehaviour
    {
        public Button TestButton;
        public Text TestText;

        private void Update()
        {
            //TestText.text = INE.Act.CurrentData;
        }

        public void ClickTest()
        {
            //INE.GetData<INETeam>( "team/list/" );
        }
    }
}
