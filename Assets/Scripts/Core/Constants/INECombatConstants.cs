using System;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECombatConstants
    {
        public readonly float BaseDamageEfficiency;
        public readonly double BaseMitigation;

        public INECombatConstants()
        {
            BaseDamageEfficiency = 100.0f;
            BaseMitigation = 0.9930924954370359d;
        }

        public float EffectivenssCalc( float dmgEfficiency )
        {
            if( dmgEfficiency < 0 )
                dmgEfficiency = 0;

            return dmgEfficiency / ( dmgEfficiency + BaseDamageEfficiency );
        }

        public float MitigationValue( float mitigationAmount )
        {
            return (float)Math.Pow( BaseMitigation, (double)mitigationAmount );
        }

        public float MitigationValue( float[] mitigationAmount )
        {
            float mitigationTotal = 0f;

            //remove foreach and correct
            foreach( float mitigationFactor in mitigationAmount )
            {
                mitigationTotal += mitigationFactor;
            }
            return (float)Math.Pow( BaseMitigation, mitigationTotal );
        }
    }
}