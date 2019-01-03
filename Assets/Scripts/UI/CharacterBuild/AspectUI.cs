using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectUI : MonoBehaviour
    {
        public Text Name;
        public Text CurrentValue;
        public Slider CurrentDistribution;
        public Text FinalValue;

        [HideInInspector] public AspectGroupUI Parent;

        private int Key;

        public void DistributionChange()
        {
            Parent.SetDistribution( Key, 1f );
            
        }

        public void SetAspect( int key, string name )
        {
            if( name != null )
                Name.text = name;

            Key = key;
        }
    }
}
