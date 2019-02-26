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
        public Text DistributionWarningLabel;
        public RectTransform AspectPrefab;
        public RectTransform AspectListParent;

        private CharacterBuilderUI Parent;
        private INEAspectGroup AspectGroup;
        private Dictionary<int, AspectItemUI> AspectsUI;

        //Set Aspects is just to connect UI, it does no other initial value setup other than determining the starting posistion of the target slider
        public void SetAspects( CharacterBuilderUI parent, INEAspectGroup aspectGroup )
        {
            if( parent != null && aspectGroup != null )
            {
                Parent = parent;
                AspectGroup = aspectGroup;

                AspectsUI = new Dictionary<int, AspectItemUI>();

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null )
                    {
                        RectTransform newPrefab = Instantiate( AspectPrefab );
                        AspectsUI.Add( aspect.Key, newPrefab.GetComponent<AspectItemUI>() );
                        newPrefab.transform.SetParent( AspectListParent.transform, false );
                        AspectsUI[aspect.Key].SetAspect( this, aspect.Key, AspectGroup.AspectName( aspect.Key ), aspect.Value.TargetDistribution );
                    }
                }
            }
        }

        public void UpdateTargets()
        {
            if( Parent != null && AspectGroup != null )
            {
                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
                    {
                        AspectsUI[aspect.Key].UpdateTarget( aspect.Value.TargetDistribution );
                    }
                }

                UpdateWarning();
            }
        }

        public void SetInitial()
        {
            if( Parent != null && AspectGroup != null )
            {
                CurrentRankBonus.text = INE.Format.RankBonus( AspectGroup.ProjectedRankBonus );

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
                    {
                        AspectsUI[aspect.Key].SetInitial( aspect.Value.Projected.Rank );
                    }
                }

                UpdateWarning();
            }
        }

        public void UpdateInitial()
        {
            if( Parent != null && AspectGroup != null )
            {
                CurrentRankBonus.text = INE.Format.RankBonus( AspectGroup.ProjectedRankBonus );

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
                    {
                        AspectsUI[aspect.Key].UpdateInitial( aspect.Value.Projected.Rank );
                    }
                }
            }
        }

        public void SetCurrent()
        {
            if( Parent != null && AspectGroup != null )
            {
                CurrentRankBonus.text = INE.Format.RankBonus( AspectGroup.CurrentRankBonus );

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
                    {
                        AspectsUI[aspect.Key].SetCurrent( aspect.Value.Current.Rank, aspect.Value.Current.Distribution );
                    }
                }

                UpdateWarning();
            }
        }

        public void UpdateProjected()
        {
            if( Parent != null && AspectGroup != null )
            {
                ProjectedRankBonus.text = INE.Format.RankBonus( AspectGroup.ProjectedRankBonus );

                foreach( KeyValuePair<int, INEAspect> aspect in AspectGroup.Aspects )
                {
                    if( aspect.Value != null && AspectsUI.ContainsKey( aspect.Key ) )
                    {
                        AspectsUI[aspect.Key].UpdateProjected( aspect.Value.Projected.Rank, aspect.Value.Projected.Distribution );
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
            if( Parent != null && AspectGroup.Aspects != null && AspectGroup.Aspects.ContainsKey( key ) )
            {
                AspectGroup.SetTargetDistribution( key, value );
                Parent.Recalculate();
                UpdateWarning();
            }
        }

        public void Activate( bool state = true )
        {
            if( AspectGroup != null )
            {
                foreach( KeyValuePair<int, AspectItemUI> item in AspectsUI )
                {
                    item.Value.Activate( state );
                }
            }
        }

        private void UpdateWarning()
        {
            if( AspectGroup != null )
            {
                DistributionWarningLabel.gameObject.SetActive( !AspectGroup.DistributionValid );
            }
        }
    }
}
