using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECharacterBuild
    {
        public INEAspectGroup Attributes;

        public INECharacterBuild( float maxExp, bool isCaster, Dictionary<int, float> racialModifers )
        {
            int attributeCount = isCaster ? 9 : 6;
            Attributes = new INEAspectGroup( maxExp, attributeCount, racialModifers  );
        }
    }
}