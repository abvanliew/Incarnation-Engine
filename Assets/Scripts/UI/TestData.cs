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
            TestText.text = INECore.act.CurrentData;
        }

        public void ClickTest()
        {
            INECore.act.GetData();
        }
    }
}
