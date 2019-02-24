using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEAspectGroup
    {
        public Dictionary<int, INEAspect> Aspects { get; private set; }
        public float CurrentRankBonus { get; private set; }
        public float ProjectedRankBonus { get; private set; }
        public AspectType Type { get; private set; }
        public bool DistributionValid { get; private set; }

        public INEAspectGroup( Dictionary<int, float> distributions, AspectType type )
        {
            if( distributions != null )
            {
                Type = type;
                Aspects = new Dictionary<int, INEAspect>();
                bool distributionValid = false;

                foreach( KeyValuePair<int, float> item in distributions )
                {
                    if( INE.Char.ValidAspect( item.Key, type ) )
                    {
                        float distribution = item.Value < 0 ? 0 : item.Value > INE.Char.MaxDistribution ? INE.Char.MaxDistribution : item.Value;

                        if( distribution > 0 )
                            distributionValid = true;

                        Aspects.Add( item.Key, new INEAspect( 1f, distribution, distribution ) );
                    }
                }

                DistributionValid = distributionValid;
            }
        }

        public INEAspectGroup()
        {
            Type = AspectType.Attributes;
            Aspects = new Dictionary<int, INEAspect>();
            for( int i = 0; i < 6; i++ )
            {
                Aspects.Add( i, new INEAspect() );
            }

            DistributionValid = false;
        }

        public INEAspectGroup( AspectType type )
        {
            Type = type;
            Aspects = new Dictionary<int, INEAspect>();
            for( int i = 0; i < 6; i++ )
            {
                Aspects.Add( i, new INEAspect() );
            }

            DistributionValid = false;
        }

        public void SetDistribution( int key, float value )
        {
            if( Aspects != null )
            {
                if( Aspects.ContainsKey( key ) )
                    Aspects[key].TargetDistribution = value < 0 ? 0 : value > INE.Char.MaxDistribution ? INE.Char.MaxDistribution : value;
            }
        }

        public void SetDistribution( Dictionary<int, float> newDistribution )
        {
            if( Aspects != null && newDistribution != null )
            {
                foreach( KeyValuePair<int, float> aspect in newDistribution )
                {
                    if( Aspects.ContainsKey( aspect.Key ) )
                    {
                        Aspects[aspect.Key].TargetDistribution = 
                            aspect.Value < 0 ? 0 : aspect.Value > INE.Char.MaxDistribution ? INE.Char.MaxDistribution : aspect.Value;
                    }
                }
            }
        }

        public void SetModifiers( Dictionary<int, float> newModifiers )
        {
            foreach( KeyValuePair<int, float> modifier in newModifiers )
            {
                if( Aspects.ContainsKey( modifier.Key ) )
                {
                    Aspects[modifier.Key].Modifier = modifier.Value < INE.Char.MinModifier ? INE.Char.MinModifier : modifier.Value;
                }
            }
        }

        public string AspectName( int key )
        {
            string name = "";

            if( Aspects.ContainsKey( key ) )
            {
                if( Type == AspectType.Attributes && INE.Format.AttributeNames.ContainsKey( key ) )
                {
                    name = INE.Format.AttributeNames[key];
                }
                else if( Type == AspectType.Skills && INE.Format.SkillNames.ContainsKey( key ) )
                {
                    name = INE.Format.SkillNames[key];
                }
            }

            return name;
        }

        public void CurrentRanks( float totalRanks )
        {
            CalculateRanks( totalRanks, current: true );
        }

        public void ProjectRanks( float totalRanks, float retraining = 0, bool setToIdeal = false )
        {
            ProjectionDistributions( retraining, setToIdeal );
            CalculateRanks( totalRanks, projected: true );
        }

        private void ProjectionDistributions( float retraining, bool setToIdeal = false )
        {
            if( Aspects != null && setToIdeal )
            {
                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        aspect.Value.Projected.Distribution = aspect.Value.TargetDistribution;
                    }
                }
            }
            else if( Aspects != null && retraining >= 0 )
            {
                float differenceMagnitude = 0;
                float currentDistributionSum = 0;
                Dictionary<int, float> differences = new Dictionary<int, float>();

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        float diff = aspect.Value.TargetDistribution - aspect.Value.Current.Distribution;

                        differences.Add( aspect.Key, diff );
                        differenceMagnitude += Mathf.Abs( diff );
                        currentDistributionSum += aspect.Value.Current.Distribution;
                    }
                }

                bool changed = differenceMagnitude > 0 && currentDistributionSum > 0;

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        float newValue;

                        if( !changed )
                        {
                            newValue = aspect.Value.Current.Distribution;
                        }
                        else if( retraining >= differenceMagnitude / currentDistributionSum  )
                        {
                            newValue = aspect.Value.TargetDistribution;
                        }
                        else
                        {
                            newValue = aspect.Value.Current.Distribution + differences[aspect.Key] * retraining * currentDistributionSum / differenceMagnitude;
                            if( Mathf.Abs( newValue - aspect.Value.TargetDistribution ) < .001f )
                                newValue = aspect.Value.TargetDistribution;
                        }

                        aspect.Value.Projected.Distribution = newValue;
                    }
                }
            }
        }

        private void CalculateRanks( float totalRanks, bool current = false, bool projected = false )
        {
            bool distributionValid = false;

            if( Aspects != null )
            {
                int count = 0;
                CalculationSet currentAspect = new CalculationSet( 0, 0, 0 );
                CalculationSet projectedAspect = new CalculationSet( 0, 0, 0 );

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        if( current )
                            currentAspect.SumDistribution += aspect.Value.Current.Distribution;

                        if( projected )
                            projectedAspect.SumDistribution += aspect.Value.Projected.Distribution;

                        count++;
                    }
                }

                bool currentDistributed = currentAspect.SumDistribution > 0;
                bool projectedDistributed = projectedAspect.SumDistribution > 0;

                if( currentDistributed || projectedDistributed )
                {
                    distributionValid = true;

                    foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                    {
                        if( aspect.Value != null )
                        {
                            if( currentDistributed )
                            {
                                aspect.Value.Current.Ratio = aspect.Value.Current.Distribution / currentAspect.SumDistribution;
                                currentAspect.SumWeightedRatios += Mathf.Pow( aspect.Value.Current.Ratio, INE.Char.WeightPower );
                            }

                            if( projectedDistributed )
                            {
                                aspect.Value.Projected.Ratio = aspect.Value.Projected.Distribution / projectedAspect.SumDistribution;
                                projectedAspect.SumWeightedRatios += Mathf.Pow( aspect.Value.Projected.Ratio, INE.Char.WeightPower );
                            }
                        }
                    }

                    float evenRankPower = INE.Char.EvenRankPower( count );
                    float evenWeightFactor = Mathf.Log( INE.Char.EvenWeightedRatio * Mathf.Pow( count, INE.Char.WeightPower ) );

                    if( currentDistributed )
                    {
                        float currentWeightFactor = Mathf.Log( currentAspect.SumWeightedRatios * Mathf.Pow( count, INE.Char.WeightPower ) );
                        float rankPower = evenWeightFactor > currentWeightFactor ? evenRankPower : INE.Char.UnevenRankPower;

                        currentAspect.RankMultiplier = Mathf.Pow( evenWeightFactor / currentWeightFactor, rankPower );
                    }

                    if( projectedDistributed )
                    {
                        float projectedWeightFactor = Mathf.Log( projectedAspect.SumWeightedRatios * Mathf.Pow( count, INE.Char.WeightPower ) );
                        float rankPower = evenWeightFactor > projectedWeightFactor ? evenRankPower : INE.Char.UnevenRankPower;

                        projectedAspect.RankMultiplier = Mathf.Pow( evenWeightFactor / projectedWeightFactor, rankPower );
                    }
                }

                if( current )
                    CurrentRankBonus = currentAspect.RankMultiplier;

                if( projected )
                    ProjectedRankBonus = projectedAspect.RankMultiplier;

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( current )
                    {
                        aspect.Value.Current.Rank = Mathf.FloorToInt( aspect.Value.Modifier * 
                            ( currentAspect.RankMultiplier * aspect.Value.Current.Ratio * totalRanks + INE.Char.BaseAspect ) );
                    }

                    if( projected )
                    {
                        aspect.Value.Projected.Rank = Mathf.FloorToInt( aspect.Value.Modifier *
                            ( projectedAspect.RankMultiplier * aspect.Value.Projected.Ratio * totalRanks + INE.Char.BaseAspect ) );
                    }
                }
            }

            DistributionValid = distributionValid;
        }

        private struct CalculationSet
        {
            public float SumDistribution;
            public float SumWeightedRatios;
            public float RankMultiplier;

            public CalculationSet( float sumDistribution, float sumWeightedRatios, float rankMultiplier )
            {
                SumDistribution = sumDistribution;
                SumWeightedRatios = sumWeightedRatios;
                RankMultiplier = rankMultiplier;
            }
        }
    }

    public class INEAspect
    {
        public float Modifier;
        public float TargetDistribution;
        public INEAspectElement Current;
        public INEAspectElement Projected;

        public INEAspect()
        {
            Modifier = 1;
            TargetDistribution = 0;
            Current.Distribution = 0;
        }

        public INEAspect( float modifer, float currentDistribution, float targetDistribution )
        {
            Modifier = modifer < INE.Char.MinModifier ? INE.Char.MinModifier : modifer;
            Current.Distribution = currentDistribution;
            TargetDistribution = targetDistribution;
        }
    }
    
    public struct INEAspectElement
    {
        public int Rank;
        public float Distribution;
        public float Ratio;
    }
}
