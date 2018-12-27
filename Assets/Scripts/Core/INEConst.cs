using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEConst
    {
        public static string[] SkillNames;
        
        public static double baseExpDistribAll; //each attribute receives this "percent" bonus when gaining exp
        
        public static ComplexPolyFunct SpecPointsFunc;
        public static ushort maxSpecPoints;

        public static PolynomialFunct supplyFunc;

        public static float baseDamageEfficiency;
        public static double baseMitigation;

        static INEConst()
        {
            //SkillNames = new string[] { "Striking", "Shooting", "Defense", "Disruption", "Combat Mobility", "Stealth", "Spell Mastery",
            //    "Fire", "Frost", "Electricity", "Water", "Benevolent", "Malevolent", "Earth" };

            baseExpDistribAll = .02888888d;

            maxSpecPoints = 16;
            SpecPointsFunc = new ComplexPolyFunct( new double[] { .02d }, new double[] { .5d } );

            supplyFunc = new PolynomialFunct( new double[] { -10d, 7.657d } );

            baseDamageEfficiency = 100.0f;
            baseMitigation = 0.9930924954370359d;
        }

        public static float EffectivenssCalc(float dmgEfficiency)
        {
            if( dmgEfficiency < 0 )
                dmgEfficiency = 0;

            return dmgEfficiency / ( dmgEfficiency + baseDamageEfficiency );
        }

        public static float MitigationValue(float mitigationAmount)
        {
            return (float)Math.Pow( baseMitigation, (double)mitigationAmount );
        }

        public static float MitigationValue(float[] mitigationAmount)
        {
            float mitigationTotal = 0f;

            //remove foreach and correct
            foreach( float mitigationFactor in mitigationAmount )
            {
                mitigationTotal += mitigationFactor;
            }
            return (float)Math.Pow( baseMitigation, mitigationTotal );
        }
    }
}
