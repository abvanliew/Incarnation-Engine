using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECharacter
    {
        string FullName;
        int RaceID;
        int Tier;
        float Exp;
        float ModelRadius;
        float ModelHeight;
        List<int> Classes;
        public INEAspectGroup Attributes;
        public INEAspectGroup Skills;
        List<int> Perks;
        INELayout DefaultLayout;

        public INECharacter( float maxExp, bool isCaster, Dictionary<int, float> racialModifers )
        {
            int attributeCount = isCaster ? 9 : 6;
            Attributes = new INEAspectGroup( maxExp, attributeCount, racialModifers );
        }

        public void ChangeRace( int newRace )
        {

        }
    }
}
