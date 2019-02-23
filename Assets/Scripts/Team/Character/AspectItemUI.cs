using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectItemUI : MonoBehaviour
    {
        public Text DisplayName;
        public Text CurrentValue;
        public Text ProjectedValue;
        public Slider CurrentDistribution;
        public Slider TargetDistribution;
        public Slider ProjectedDistribution;

        private int Key;
        private AspectGroupUI Parent;

        public void SetAspect( AspectGroupUI parent, int key, string displayName, float targetDistribution, int currentRank, float currentDistribution, 
            float distributionMax, bool initialCharacter = false )
        {
            Parent = parent;
            Key = key;
            if( displayName != null )
                DisplayName.text = displayName;
            TargetDistribution.value = targetDistribution;
            TargetDistribution.maxValue = INE.Char.MaxDistribution;
            CurrentValue.text = currentRank.ToString();

            if( initialCharacter )
            {
                CurrentDistribution.gameObject.SetActive( false );
                EnableProjection( false );
            }
            else
            {
                CurrentDistribution.value = currentDistribution;
                CurrentDistribution.maxValue = INE.Char.MaxDistribution;
                CurrentDistribution.gameObject.SetActive( true );
                ProjectedDistribution.maxValue = INE.Char.MaxDistribution;
            }
        }

        public void SetCurrent( int currentRank )
        {
            CurrentValue.text = currentRank.ToString();
        }

        public void SetProjected( int projectedRank, float projectedDistribution )
        {
            ProjectedValue.text = projectedRank.ToString();
            ProjectedDistribution.value = projectedDistribution;
        }

        public void EnableProjection( bool state = true )
        {
            ProjectedValue.gameObject.SetActive( state );
            ProjectedDistribution.gameObject.SetActive( state );
        }

        public void DistributionChange()
        {
            if( Parent != null )
                Parent.SetDistribution( Key, TargetDistribution.value );
        }
    }
}
