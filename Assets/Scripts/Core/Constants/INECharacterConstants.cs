using System;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public enum AspectType { Attributes, Skills };

    public class INECharacterConstants
    {
        public readonly float BaseAspect;
        public readonly float RetrainingRatio;
        public readonly float RetrainingIncrement;
        public readonly float ExcessExpConversion;
        public readonly float WeightPower;
        public readonly float EvenWeightedRatio;
        public readonly float BaseRankPower;
        public readonly float UnevenRankPower;
        public readonly float MaxDistribution;
        public readonly float MinModifier;
        public readonly float ExpPerTier;
        public readonly float BaseAttributeRanks;
        public readonly float AttributeRankIncrementPerTier;
        public readonly float SkillRankRatio;

        public INECharacterConstants()
        {
            BaseAspect = 25;
            RetrainingIncrement = 250;
            RetrainingRatio = 0.01f;
            ExcessExpConversion = 0.25f;
            WeightPower = 2;
            EvenWeightedRatio = 3f * Mathf.Pow( 1f / 3f, WeightPower );
            BaseRankPower = 0.5f;
            UnevenRankPower = 1.5f;
            MaxDistribution = 120;
            MinModifier = 0.05f;
            ExpPerTier = 1000;
            BaseAttributeRanks = 1000;
            AttributeRankIncrementPerTier = 500;
            SkillRankRatio = 0.8f;
        }

        public float EvenRankPower( int aspectCount )
        {
            return BaseRankPower;
        }

        public bool ValidAspect( int key, AspectType type )
        {
            return true;
        }

        public float MaxExp( int tier )
        {
            float exp = 0;

            if( tier > 0 && tier <= 5 )
            {
                exp = ExpPerTier * tier;
            }

            return exp;
        }

        public float AttributeRanks( float exp, int tier )
        {
            float ranks = 0;

            if( tier > 0 && tier <= 5 && exp > 0 )
            {
                float maxExp = MaxExp( tier );
                exp = exp <= maxExp ? exp : maxExp;
                float expProgress = exp / maxExp;

                ranks = ( BaseAttributeRanks + (float)( tier - 1 ) * AttributeRankIncrementPerTier ) * expProgress * ( 2 - expProgress );
            }

            return ranks;
        }

        public float SkillRanks( float exp, int tier )
        {
            float ranks = 0;

            if( exp > 0 )
            {
                float maxExp = MaxExp( tier );
                exp = exp <= maxExp ? exp : maxExp;
                ranks = SkillRankRatio * exp;
            }

            return ranks;
        }

        public float PerkCount( int tier )
        {
            return tier < 3 ? 1 : tier < 5 ? 2 : 3;
        }

        public float RankRetraining( float currentExp, float expGained, int tier )
        {
            float retraining = 0;

            if( currentExp > 0 && expGained > 0 && tier > 0 && tier < 5 )
            {
                //find max tier and cap current exp at it (it should never be above it)
                float maxExp = MaxExp( tier );
                currentExp = currentExp <= maxExp ? currentExp : maxExp;

                //find total exp with excess exp count with a bonus
                float totalExp = currentExp + expGained + ( currentExp + expGained > maxExp ? ExcessExpConversion * ( currentExp + expGained - maxExp ) : 0 );

                retraining = RetrainingRatio * ( Mathf.Log( totalExp ) - Mathf.Log( currentExp ) );
            }

            return retraining;
        }
    }
}