using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECharacterBuild
    {
        string FullName;
        int RaceID;
        int Tier;
        List<int> Classes;
        float Exp;
        public INEAspectGroup Attributes;
        public INEAspectGroup Skills;
        List<int> Perks;

        public INECharacterBuild( float maxExp, bool isCaster, Dictionary<int, float> racialModifers )
        {
            int attributeCount = isCaster ? 9 : 6;
            Attributes = new INEAspectGroup( maxExp, attributeCount, racialModifers  );
        }
    }
}