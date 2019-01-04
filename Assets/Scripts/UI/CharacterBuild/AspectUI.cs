using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectUI : MonoBehaviour
    {
        public Text Name;
        public Text CurrentValue;
        public Text ProjectedValue;
        public Text IdealValue;
        public Slider CurrentDistribution;
        public Slider ProjectedDistribution;
        public Slider IdealDistribution;

        [HideInInspector] public AspectGroupUI Parent;

        private int Key;

        public void SetAspect( int key, string name, int currentRank, float currentDistribution,
            int projectedRank, float projectedDistribution, int idealRank, float idealDistribution )
        {
            Key = key;
            if( name != null )
                Name.text = name;
            CurrentValue.text = currentRank.ToString();
            CurrentDistribution.value = currentDistribution;
            ProjectedValue.text = projectedRank.ToString();
            ProjectedDistribution.value = projectedDistribution;
            IdealValue.text = idealRank.ToString();
            IdealDistribution.value = idealDistribution;
        }

        public void SetProjected( int projectedRank, float projectedDistribution, int idealRank )
        {
            ProjectedValue.text = projectedRank.ToString();
            ProjectedDistribution.value = projectedDistribution;
            IdealValue.text = idealRank.ToString();
        }

        public void DistributionChange()
        {
            Parent.SetDistribution( Key, IdealDistribution.value );
        }
    }
}
