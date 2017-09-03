using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IncarnationEngine
{
    public class INEConst
    {
        public static string[] SkillNames;

        public static double baseExpDistrib; //scalar multiplier on attrib calculations
        public static double baseExpDistribAll; //each attribute receives this "percent" bonus when gaining exp
        public static float minDistrib;
        public static float maxDistrib;
        public static double distribPower;

        public static ushort minAttribDist; //minimum number of attributes that have to be disributed
        //public static Attributes[] defaultAttribDist;
        public static ushort minSkillDist; //minimum number of skills that have to be disributed
        //public static Skills[] defaultSkillDist;

        public static ComplexPolyFunct SpecPointsFunc;
        public static ushort maxSpecPoints;

        public static PolynomialFunct supplyFunc;

        public static float baseDamageEfficiency;
        public static double baseMitigation;

        static INEConst()
        {
            //SkillNames = new string[] { "Striking", "Shooting", "Defense", "Disruption", "Combat Mobility", "Stealth", "Spell Mastery",
            //    "Fire", "Frost", "Electricity", "Water", "Benevolent", "Malevolent", "Earth" };

            baseExpDistrib = .942222224d;
            baseExpDistribAll = .02888888d;
            minDistrib = 0f;
            maxDistrib = 2f;
            distribPower = Math.Log( 7d / 16d ) / ( -2d * Math.Log( 2d ) );

            minAttribDist = 3;
            //defaultAttribDist = new Attributes[] { Attributes.Endurance, Attributes.Speed, Attributes.Resistance }; //should be an array of index values with the same length of min attribute dist with no repeats
            minSkillDist = 3;
            //defaultSkillDist = new Skills[] { Skills.Defense, Skills.Striking, Skills.CombatMobility };

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