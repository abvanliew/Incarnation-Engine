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
            ExpRetraining net = NetGrowth( expGained, retrainingGained );

            //code to call permanent rank increase formula
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

        public void SetDistribution( Dictionary<int, float> newDistribution )
        {
            if( Aspects != null && newDistribution != null )
            {
                foreach( KeyValuePair<int, float> entry in newDistribution )
                {
                    if( Aspects.ContainsKey( entry.Key ) )
                    {
                        Aspects[entry.Key].Ideal.Distribution = entry.Value;
                    }
                }
            }
        }

        public void SetDistribution( int key, float value )
        {
            if( Aspects != null )
            {
                if( Aspects.ContainsKey( key ) )
                    Aspects[key].Ideal.Distribution = value;
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

        private bool UpdateDistribution( float retraining, bool updateCurrent = false )
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

                    if( updateCurrent )
                        aspect.Value.Current.Distribution = newValue;
                    else
                        aspect.Value.Projected.Distribution = newValue;
                }
            }

            return changed;
        }

        private void CalculateRanks( bool current = false, bool ideal = true, bool projected = true, float projectedExp )
        {
            if( Aspects != null )
            {
                int count = 0;
                float expMultiplierPower = 1.1f;

                CalculationSet currentAspect;
                //currentAspect.SumDistribution = 0;
                //currentAspect.SumWeightedRatios = 0;
                //currentAspect.ExpMultiplier = 0;
                
                CalculationSet idealAspect;
                //idealAspect.SumDistribution = 0;
                //idealAspect.SumWeightedRatios = 0;
                //idealAspect.ExpMultiplier = 0;
                
                CalculationSet projectedAspect;
                //projectedAspect.SumDistribution = 0;
                //projectedAspect.SumWeightedRatios = 0;
                //projectedAspect.ExpMultiplier = 0;
                
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
    }
    
    public struct INEAspectElement
    {
        public int Rank;
        public float Distribution;
        public float Ratio;
    }
}
