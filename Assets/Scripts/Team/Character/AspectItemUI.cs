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

        public void SetAspect( AspectGroupUI parent, int key, string displayName, float targetDistribution )
        {
            if( parent != null )
            {
                Parent = parent;
                Key = key;
                if( displayName != null )
                    DisplayName.text = displayName;

                TargetDistribution.maxValue = INE.Char.MaxDistribution;
                CurrentDistribution.maxValue = INE.Char.MaxDistribution;
                ProjectedDistribution.maxValue = INE.Char.MaxDistribution;

                TargetDistribution.value = targetDistribution;
            }
        }

        public void UpdateTarget( float distribution )
        {
            if( Parent != null )
            {
                TargetDistribution.value = distribution;
            }
        }

        public void SetInitial( int rank )
        {
            if( Parent != null )
            {
                CurrentDistribution.gameObject.SetActive( false );
                EnableProjection( false );
                UpdateInitial( rank );
            }
        }

        public void UpdateInitial( int rank )
        {
            if( Parent != null )
            {
                CurrentValue.text = rank.ToString();
            }
        }

        public void SetCurrent( int currentRank, float currentDistribution )
        {
            if( Parent != null )
            {
                CurrentDistribution.value = currentDistribution;
                CurrentDistribution.gameObject.SetActive( true );
                CurrentValue.text = currentRank.ToString();
            }
        }

        public void UpdateProjected( int projectedRank, float projectedDistribution )
        {
            if( Parent != null )
            {
                ProjectedValue.text = projectedRank.ToString();
                ProjectedDistribution.value = projectedDistribution;
            }
        }

        public void EnableProjection( bool state = true )
        {
            if( Parent != null )
            {
                ProjectedValue.gameObject.SetActive( state );
                ProjectedDistribution.gameObject.SetActive( state );
            }
        }

        public void DistributionChange()
        {
            if( Parent != null )
                Parent.SetDistribution( Key, TargetDistribution.value );
        }

        public void Activate( bool state = true )
        {
            TargetDistribution.interactable = state;
        }
    }
}
