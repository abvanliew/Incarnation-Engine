using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectGroupUI : MonoBehaviour
    {
        public Text BaseExpMultiplier;
        public Text TargetExpMultiplier;
        public Text MaxExpMultiplier;
        public RectTransform AspectPrefab;
        public RectTransform AspectParent;
        public float ExpGained;
        public bool newCharacter = true;

        private INEAspectGroup AspectGroup;
        private Dictionary<int, AspectUI> AspectsUI;

        public void SetAspects( INEAspectGroup aspectGroup )
        {
            if( aspectGroup != null )
            {
                AspectGroup = aspectGroup;
                AspectGroup.BaseRanks();
                AspectGroup.ProjectRanks( ExpGained, setToIdeal: newCharacter );
                BaseExpMultiplier.text = FormatExpMultipler( AspectGroup.CurrentExpMultiplier );
                TargetExpMultiplier.text = FormatExpMultipler( AspectGroup.ProjectedExpMultiplier );
                MaxExpMultiplier.text = FormatExpMultipler( AspectGroup.IdealExpMultiplier );

                AspectsUI = new Dictionary<int, AspectUI>();

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null )
                    {
                        RectTransform newPrefab = Instantiate( AspectPrefab );
                        AspectsUI.Add( aspect.Key, newPrefab.GetComponent<AspectUI>() );
                        newPrefab.transform.SetParent( AspectParent.transform, false );
                        AspectsUI[aspect.Key].SetAspect( aspect.Key, INE.Format.AttributeNames[aspect.Key], 
                            aspect.Value.Current.Rank, aspect.Value.Current.Distribution,
                            aspect.Value.Projected.Rank, aspect.Value.Projected.Distribution,
                            aspect.Value.Ideal.Rank, aspect.Value.Ideal.Distribution );
                        if( newCharacter )
                        {
                            AspectsUI[aspect.Key].CurrentDistribution.gameObject.SetActive( false );
                            AspectsUI[aspect.Key].ProjectedDistribution.gameObject.SetActive( false ); 
                        }
                        AspectsUI[aspect.Key].Parent = this;
                    }
                }
            }
        }

        public void SetDistribution( int key, float value )
        {
            if( AspectGroup.Aspects.ContainsKey( key ) )
            {
                AspectGroup.SetDistribution( key, value );
                AspectGroup.ProjectRanks( ExpGained, setToIdeal: newCharacter );
                TargetExpMultiplier.text = FormatExpMultipler( AspectGroup.ProjectedExpMultiplier );
                MaxExpMultiplier.text = FormatExpMultipler( AspectGroup.IdealExpMultiplier );

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
                    {
                        AspectsUI[aspect.Key].SetProjected( aspect.Value.Projected.Rank, aspect.Value.Projected.Distribution,
                            aspect.Value.Ideal.Rank );
                    }
                }
            }
        }

        private string FormatExpMultipler( float value )
        {
            return value <= 0 ? "-" : string.Format( "{0}%", Mathf.Round( 1000 * ( value - 1 ) ) / 10 );
        }
    }
}
