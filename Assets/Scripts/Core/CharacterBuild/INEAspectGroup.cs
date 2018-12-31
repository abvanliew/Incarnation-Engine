using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEAspectGroup
    {
        Dictionary<int, INEAspect> Aspects;
        float Exp;
        float MaxExp;

        public void GainExp( float exp, float fixedExp = 0, float fixedTraining = 0 ) { }
        public Dictionary<int, int> GetAspectRanks( float exp = 0, float fixedExp = 0, float fixedTraining = 0 )
        { return new Dictionary<int, int>() { { 0, 0 } }; }
        public void SetDistribution( Dictionary<int, float> dist )
        {
            
        }
        public INEAspectSet GUIUpdate( float exp, Dictionary<int, float> dist, float fixedExp = 0, float fixedTraining = 0 )
        { return new INEAspectSet(); }

    }

    public struct INEAspect
    {
        public int Rank;
        public float Distribution;
        public float Modifier;
    }

    public struct INEAspectSet
    {
        public Dictionary<int, INEAspect> NewAspects;
        public Dictionary<int, INEAspect> IdealAspects;
    }
}
