using System;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECharacterConstants
    {
        public readonly float BaseAspect;
        public readonly float RetrainingRatio;
        public readonly float ExcessExpConversion;
        public readonly float AspectWeightPower;
        public readonly float EvenWeightedRatio;
        public readonly float MaxDistribution;
        public readonly float RetrainingExpThreshold;

        public INECharacterConstants()
        {
            BaseAspect = 20;
            RetrainingRatio = 0.0025f;
            ExcessExpConversion = 0.15f;
            AspectWeightPower = 2;
            EvenWeightedRatio = 3f * Mathf.Pow( 1f / 3f, AspectWeightPower );
            MaxDistribution = 120;
            RetrainingExpThreshold = 100;
        }
    }
}