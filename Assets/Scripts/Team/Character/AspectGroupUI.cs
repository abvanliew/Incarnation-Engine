using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectGroupUI : MonoBehaviour
    {
        public Text CurrentRankBonus;
        public Text ProjectedRankBonus;
        public Text ProjectedLabel;
        public RectTransform AspectPrefab;
        public RectTransform AspectListParent;

        private INEAspectGroup AspectGroup;
        private bool InitialCharacter;
        private Dictionary<int, AspectItemUI> AspectsUI;

        public void SetAspects( INEAspectGroup aspectGroup, bool initialCharacter = false )
        {
            if( aspectGroup != null )
            {
                AspectGroup = aspectGroup;
                InitialCharacter = initialCharacter;
                if( InitialCharacter )
                    EnableProjection( false );

                //AspectGroup.BaseRanks();
                //AspectGroup.ProjectRanks( ExpGained, setToIdeal: newCharacter );
                //BaseExpMultiplier.text = FormatExpMultipler( AspectGroup.CurrentExpMultiplier );
                //TargetExpMultiplier.text = FormatExpMultipler( AspectGroup.ProjectedExpMultiplier );
                //MaxExpMultiplier.text = FormatExpMultipler( AspectGroup.IdealExpMultiplier );

                AspectsUI = new Dictionary<int, AspectItemUI>();

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null )
                    {
                        RectTransform newPrefab = Instantiate( AspectPrefab );
                        AspectsUI.Add( aspect.Key, newPrefab.GetComponent<AspectItemUI>() );
                        newPrefab.transform.SetParent( AspectListParent.transform, false );
                        AspectsUI[aspect.Key].SetAspect( this, aspect.Key, AspectGroup.AspectName( aspect.Key ), aspect.Value.TargetDistribution, 
                            aspect.Value.Current.Rank, aspect.Value.Current.Distribution, INE.Char.MaxDistribution, InitialCharacter );
                    }
                }
            }
        }

        public void EnableProjection( bool state = true )
        {
            ProjectedLabel.gameObject.SetActive( state );
            ProjectedRankBonus.gameObject.SetActive( state );

            if( AspectGroup != null && AspectGroup.Aspects != null )
            {
                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI != null )
                    {
                        if( AspectsUI.ContainsKey( aspect.Key ) )
                            AspectsUI[aspect.Key].EnableProjection( state );
                    }
                }
            }
        }

        public void SetDistribution( int key, float value )
        {
            if( AspectGroup.Aspects != null && AspectGroup.Aspects.ContainsKey( key ) )
            {
                AspectGroup.SetDistribution( key, value );
                UpdateValues();
            }
        }

        public void UpdateValues()
        {
            //AspectGroup.ProjectRanks( ExpGained, setToIdeal: newCharacter );
            //TargetExpMultiplier.text = FormatExpMultipler( AspectGroup.ProjectedExpMultiplier );
            //MaxExpMultiplier.text = FormatExpMultipler( AspectGroup.IdealExpMultiplier );

            //foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
            //{
            //    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
            //    {
            //        AspectsUI[aspect.Key].SetProjected( aspect.Value.Projected.Rank, aspect.Value.Projected.Distribution,
            //            aspect.Value.Ideal.Rank );
            //    }
            //}
        }

        private string FormatExpMultipler( float value )
        {
            return value <= 0 ? "-" : string.Format( "{0}%", Mathf.Round( 1000 * ( value - 1 ) ) / 10 );
        }
    }
}
