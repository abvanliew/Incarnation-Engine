using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEAspectGroup
    {
        public float Exp { get; private set; }
        public float MaxExp { get; private set; }
        public Dictionary<int, INEAspect> Aspects { get; private set; }
        public float CurrentExpMultiplier { get; private set; }
        public float IdealExpMultiplier { get; private set; }
        public float ProjectedExpMultiplier { get; private set; }

        public INEAspectGroup( float maxExp, int aspectCount, Dictionary<int, float> aspectModifers )
        {
            Exp = 0;
            MaxExp = maxExp <= 1 ? 1 : maxExp;
            if( aspectCount > 0 )
            {
                Aspects = new Dictionary<int, INEAspect>();
                for( int i = 0; i < aspectCount; i++ )
                {
                    float modifer = 1f;
                    if( aspectModifers != null && aspectModifers.ContainsKey( i ) )
                        modifer = aspectModifers[i];

                    Aspects.Add( i, new INEAspect( modifer, 0, 0 ) );
                }
            }
        }

        public void SetDistribution( int key, float value )
        {
            if( Aspects != null )
            {
                if( Aspects.ContainsKey( key ) )
                    Aspects[key].Ideal.Distribution = value < 0 ? 0 : value > INE.MaxDistribution ? INE.MaxDistribution : value;
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
                        Aspects[aspect.Key].Ideal.Distribution = 
                            aspect.Value < 0 ? 0 : aspect.Value > INE.MaxDistribution ? INE.MaxDistribution : aspect.Value;
                    }
                }
            }
        }

        public void GainExp( float expGained, float retrainingGained = 0 )
        {
            ExpRetraining net = NetGrowth( expGained, retrainingGained );
            Exp += net.Exp;
            if( Exp > MaxExp )
                Exp = MaxExp;
            CalculateDistribution( net.Retraining, true );
            CalculateRanks( current: true );
        }

        private ExpRetraining NetGrowth( float expGained, float retrainingGained = 0 )
        {
            ExpRetraining netGrowth;
            float excessExp = 0;

            if( Exp + expGained >= MaxExp )
            {
                netGrowth.Exp = MaxExp - Exp;
                excessExp = Exp + expGained - MaxExp;
            }
            else
            {
                netGrowth.Exp = expGained;
            }

            netGrowth.Retraining = INE.RetrainingRatio * ( expGained + excessExp * INE.ExcessExpConversion ) + retrainingGained;

            return netGrowth;
        }

        public void BaseRanks()
        {
            CalculateRanks( current: true );
        }

        public void ProjectRanks( float expGained, float retrainingGained = 0, bool setToIdeal = false )
        {
            ExpRetraining net = NetGrowth( expGained, retrainingGained );
            CalculateDistribution( net.Retraining, setToIdeal: setToIdeal );
            CalculateRanks( ideal: true, projected: true, projectedExp: net.Exp );
        }

        private void CalculateDistribution( float retraining, bool updateCurrent = false, bool setToIdeal = false )
        {
            if( setToIdeal )
            {
                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        if( updateCurrent )
                            aspect.Value.Current.Distribution = aspect.Value.Ideal.Distribution;
                        else
                            aspect.Value.Projected.Distribution = aspect.Value.Ideal.Distribution;
                    }
                }
            }
            else if( Aspects != null && retraining != 0 )
            {
                if( retraining < 0 )
                    retraining = 0;

                if( Exp > INE.RetrainingExpThreshold )
                    retraining = INE.RetrainingExpThreshold / Exp * retraining;

                Dictionary<int, float> differences = new Dictionary<int, float>();
                float differenceMagnitude = 0;
                
                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        float difference = aspect.Value.Ideal.Distribution - aspect.Value.Current.Distribution;
                        differences.Add( aspect.Key, difference );
                        differenceMagnitude += Mathf.Abs( difference );
                    }
                }

                bool changed = differenceMagnitude != 0;

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        float newValue;

                        if( !changed )
                        {
                            newValue = aspect.Value.Current.Distribution;
                        }
                        else if( differenceMagnitude <= retraining )
                        {
                            newValue = aspect.Value.Ideal.Distribution;
                        }
                        else
                        {
                            newValue = aspect.Value.Current.Distribution
                                + differences[aspect.Key] * retraining / differenceMagnitude;
                            if( Mathf.Abs( newValue - aspect.Value.Ideal.Distribution ) < .001f )
                                newValue = aspect.Value.Ideal.Distribution;
                        }

                        if( updateCurrent && changed )
                            aspect.Value.Current.Distribution = newValue;
                        else if( !updateCurrent )
                            aspect.Value.Projected.Distribution = newValue;
                    }
                }
            }
        }

        private void CalculateRanks( bool current = false, bool ideal = false, bool projected = false, float projectedExp = 0 )
        {
            if( Aspects != null )
            {
                int count = 0;

                CalculationSet currentAspect;
                currentAspect.SumDistribution = 0;
                currentAspect.SumWeightedRatios = 0;
                currentAspect.ExpMultiplier = 0;

                CalculationSet idealAspect;
                idealAspect.SumDistribution = 0;
                idealAspect.SumWeightedRatios = 0;
                idealAspect.ExpMultiplier = 0;

                CalculationSet projectedAspect;
                projectedAspect.SumDistribution = 0;
                projectedAspect.SumWeightedRatios = 0;
                projectedAspect.ExpMultiplier = 0;

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( aspect.Value != null )
                    {
                        if( current )
                            currentAspect.SumDistribution += aspect.Value.Current.Distribution;

                        if( ideal )
                            idealAspect.SumDistribution += aspect.Value.Ideal.Distribution;

                        if( projected )
                            projectedAspect.SumDistribution += aspect.Value.Projected.Distribution;

                        count++;
                    }
                }

                bool currentDistributed = current && currentAspect.SumDistribution > 0;
                bool idealDistributed = ideal && idealAspect.SumDistribution > 0;
                bool projectedDistributed = projected && projectedAspect.SumDistribution > 0;
                
                if( currentDistributed || idealDistributed || projectedDistributed )
                {
                    foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                    {
                        if( aspect.Value != null )
                        {
                            if( currentDistributed )
                            {
                                aspect.Value.Current.Ratio =
                                    aspect.Value.Current.Distribution / currentAspect.SumDistribution;
                                currentAspect.SumWeightedRatios +=
                                    Mathf.Pow( aspect.Value.Current.Ratio, INE.AspectWeightPower );
                            }

                            if( idealDistributed )
                            {
                                aspect.Value.Ideal.Ratio =
                                    aspect.Value.Ideal.Distribution / idealAspect.SumDistribution;
                                idealAspect.SumWeightedRatios +=
                                    Mathf.Pow( aspect.Value.Ideal.Ratio, INE.AspectWeightPower );
                            }

                            if( projectedDistributed )
                            {
                                aspect.Value.Projected.Ratio =
                                    aspect.Value.Projected.Distribution / projectedAspect.SumDistribution;
                                projectedAspect.SumWeightedRatios +=
                                    Mathf.Pow( aspect.Value.Projected.Ratio, INE.AspectWeightPower );
                            }
                        }
                    }

                    float expMultiplierPower = INE.ExpMultiplierPower( count );
                    float evenWeightFactor = Mathf.Log( INE.EvenWeightedRatio * Mathf.Pow( count, INE.AspectWeightPower ) );

                    if( currentDistributed )
                    {
                        currentAspect.ExpMultiplier = Mathf.Pow( evenWeightFactor / 
                            Mathf.Log( currentAspect.SumWeightedRatios * Mathf.Pow( count, INE.AspectWeightPower ) ), expMultiplierPower );
                    }

                    if( idealDistributed )
                    {
                        idealAspect.ExpMultiplier = Mathf.Pow( evenWeightFactor / 
                            Mathf.Log( idealAspect.SumWeightedRatios * Mathf.Pow( count, INE.AspectWeightPower ) ), expMultiplierPower );
                    }

                    if( projectedDistributed )
                    {
                        projectedAspect.ExpMultiplier = Mathf.Pow( evenWeightFactor / 
                            Mathf.Log( projectedAspect.SumWeightedRatios * Mathf.Pow( count, INE.AspectWeightPower ) ), expMultiplierPower );
                    }
                }

                if( current )
                    CurrentExpMultiplier = currentAspect.ExpMultiplier;

                if( ideal )
                    IdealExpMultiplier = idealAspect.ExpMultiplier;

                if( projected )
                    ProjectedExpMultiplier = projectedAspect.ExpMultiplier;

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    if( current )
                    {
                        aspect.Value.Current.Rank = Mathf.FloorToInt( aspect.Value.Modifier * 
                            ( currentAspect.ExpMultiplier * aspect.Value.Current.Ratio * Exp + INE.BaseAspect ) );
                    }

                    if( ideal )
                    {
                        aspect.Value.Ideal.Rank = Mathf.FloorToInt( aspect.Value.Modifier *
                            ( idealAspect.ExpMultiplier * aspect.Value.Ideal.Ratio * MaxExp + INE.BaseAspect ) );
                    }

                    if( projected )
                    {
                        aspect.Value.Projected.Rank = Mathf.FloorToInt( aspect.Value.Modifier *
                            ( projectedAspect.ExpMultiplier * aspect.Value.Projected.Ratio * projectedExp + INE.BaseAspect ) );
                    }
                }
            }
        }

        private struct CalculationSet
        {
            public float SumDistribution;
            public float SumWeightedRatios;
            public float ExpMultiplier;
        }

        private struct ExpRetraining
        {
            public float Exp;
            public float Retraining;
        }
    }

    public class INEAspect
    {
        public float Modifier;
        public INEAspectElement Current;
        public INEAspectElement Ideal;
        public INEAspectElement Projected;

        public INEAspect() { }

        public INEAspect( float modifer, float currentDistribution, float idealDistribution )
        {
            Modifier = modifer <= 0 ? 1 : modifer;
            Current.Distribution = currentDistribution;
            Ideal.Distribution = idealDistribution;
        }
    }
    
    public struct INEAspectElement
    {
        public int Rank;
        public float Distribution;
        public float Ratio;
    }
}
