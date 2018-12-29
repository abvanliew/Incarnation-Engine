using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class CharacterAspectUI : MonoBehaviour
    {
        public Text Name;
        public Text CurrentValue;
        public Slider CurrentDistribution;
        public Slider MinDistribution;
        public Slider MaxDistribution;
        public Text FinalValue;

        [HideInInspector] public AspectGroup Parent;

        public void DistributionChange()
        {
            Parent.UpdateCalculations();
        }

        public void SetAspectName( string name )
        {
            if( name != null )
                Name.text = name;
        }
    }
}
