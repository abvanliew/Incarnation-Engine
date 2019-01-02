using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public enum INEDistributionType { Current, Target, Projected };

    public class INEAspectGroup
    {
        float Exp;
        float MaxExp;
        Dictionary<int, INEAspect> Aspects;

        public void GainExp( float expGained, float retrainingGained = 0 )
        {
            float netExp = 0;
            float excessExp = 0;
            float netRetraining = 0;

            if( Exp + expGained >= MaxExp )
            {
                netExp = MaxExp - Exp;
                excessExp = Exp + expGained - MaxExp;
            }
            else
            {
                netExp = expGained;
            }

            netRetraining = INE.RetrainingRatio * ( expGained + excessExp * INE.ExcessExpConversion ) + retrainingGained;

            //code to call permanent rank increase formula
        }

        public Dictionary<int, int> GetAspectRanks( float exp = 0, float fixedExp = 0, float fixedTraining = 0 )
        {
            Dictionary<int, int> ranks = null;

            if( Aspects != null )
            {

            }

            return ranks;
        }

        public Dictionary<int, float> GetDistribution( INEDistributionType type = INEDistributionType.Current )
        {
            Dictionary<int, float> dist = new Dictionary<int, float>();

            return dist;
        }

        public void SetDistribution( Dictionary<int, float> newDist )
        {
            if( Aspects != null && newDist != null )
            {
                foreach( KeyValuePair<int, float> entry in newDist )
                {
                    if( Aspects.ContainsKey( entry.Key ) )
                    {
                        Aspects[entry.Key].Ideal.Distribution = entry.Value;
                    }
                }
            }
        }

        public void UpdateThenProject( float exp, Dictionary<int, float> dist, 
            float fixedExp = 0, float fixedTraining = 0 )
        {
            //set new distribution target
            SetDistribution( dist );

            //calculate net exp 

            //calculate new effective distribution

            //calculate new ui elements
            //static ui elements are current effective distribution, current exp multiplier and current ranks
            //each update we will need the following
            // ideal distribution, ideal exp multiplier and ideal ranks
            // projected effective distribution, projected exp multiplier and projected ranks
            
        }

        private bool CalculateProjectedDistribution( float retraining )
        {
            bool changed = false;

            if( Aspects != null )
            {
                Dictionary<int, float> differences = new Dictionary<int, float>();
                float differenceMagnitude = 0;
                
                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    float difference = aspect.Value.Ideal.Distribution - aspect.Value.Current.Distribution;
                    differences.Add( aspect.Key, difference );
                    differenceMagnitude += Mathf.Abs( difference );
                }

                foreach( KeyValuePair<int, INEAspect> aspect in Aspects )
                {
                    float newValue;

                    if( differenceMagnitude == 0 )
                    {
                        newValue = aspect.Value.Current.Distribution;
                    }
                    else if( differenceMagnitude <= retraining )
                    {
                        newValue = aspect.Value.Ideal.Distribution;
                        changed = true;
                    }
                    else
                    {
                        newValue = aspect.Value.Current.Distribution
                            + differences[ aspect.Key ] * retraining / differenceMagnitude;
                        if( Mathf.Abs( newValue - aspect.Value.Ideal.Distribution ) < .001f )
                            newValue = aspect.Value.Ideal.Distribution;
                        changed = true;
                    }

                    aspect.Value.Projected.Distribution = newValue;
                }
            }

            return changed;
        }
        
        private void CalculateRanks( float exp, bool current = false, bool ideal = true, bool projected = true )
        {
            if( Aspects != null )
            {
                int count = 0;
                float expMultiplierPower = 1.1f;

                INEAspectCalculator currentAspect;
                currentAspect.SumDistribution = 0;
                currentAspect.SumWeightedRatios = 0;

                INEAspectCalculator idealAspect;
                idealAspect.SumDistribution = 0;
                idealAspect.SumWeightedRatios = 0;

                INEAspectCalculator projectedAspect;
                projectedAspect.SumDistribution = 0;
                projectedAspect.SumWeightedRatios = 0;


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

                if( currentSumDistribution > 0 )
                {
                    for( int i = 0; i < count; i++ )
                    {
                        ratios.Add( dist[i] / currentSumDistribution );
                        sumWeightedRatios += Mathf.Pow( ratios[i], INE.AspectWeightPower );
                    }

                    float evenWeightFactor = Mathf.Log( INE.EvenWeightedRatio * 
                        Mathf.Pow( count, INE.AspectWeightPower ) );
                    float weightFactor = Mathf.Log( sumWeightedRatios * Mathf.Pow( count, INE.AspectWeightPower ) );
                    float expMultiplier = Mathf.Pow( evenWeightFactor / weightFactor, expMultiplierPower );

                    for( int i = 0; i < count; i++ )
                    {
                        int newValue = Mathf.FloorToInt( modifiers[i] * 
                                ( expMultiplier * ratios[i] * exp + INE.BaseAspect ) );
                    }
                }
            }
        }

        private struct INEAspectCalculator
        {
            public float SumDistribution;
            public float SumWeightedRatios;
            public float WeightFactor;
            public float ExpMultiplier;
        }
    }

    public class INEAspect
    {
        public float Modifier;
        public INEAspectElement Current;
        public INEAspectElement Ideal;
        public INEAspectElement Projected;
    }
    
    public struct INEAspectElement
    {
        public int Rank;
        public float Distribution;
        public float Ratio;
    }
}
