using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECharacter
    {
        public string FullName { get; private set; }
        public int RaceID { get; private set; }
        public int Tier { get; private set; }
        public float Exp { get; private set; }
        public float MaxExp { get { return INE.Char.MaxExp( Tier ); } }
        public float PerkRetraining { get; private set; }
        public float ModelRadius { get; private set; }
        public float ModelHeight { get; private set; }
        public bool IsCaster { get; private set; }
        public string Archetype { get; private set; }
        public INEAspectGroup Attributes { get; private set; }
        public INEAspectGroup Skills { get; private set; }
        public List<int> Perks { get; private set; }
        public INELayout DefaultLayout { get; private set; }
        public INECharacter Clone { get { return (INECharacter)this.MemberwiseClone(); } }

        public INECharacter( float maxExp, bool isCaster, Dictionary<int, float> racialModifers )
        {
            int attributeCount = isCaster ? 9 : 6;
            Attributes = new INEAspectGroup( attributeCount, racialModifers, INEAspectGroup.AspectType.Attributes );
            FullName = "New Character";
            Tier = 5;
            Exp = 500;
            PerkRetraining = .5f;
        }

        public INECharacter( string name, int race, int tier )
        {
            FullName = name;
            RaceID = race;
            Tier = tier;
        }

        public INECharacter()
        {
            FullName = "";
            RaceID = 0;
            Tier = 5;
            Exp = 500;
            Attributes = new INEAspectGroup( 9 );
            Skills = new INEAspectGroup( 3 );
            Perks = new List<int>();
            DefaultLayout = new INELayout();
        }

        public void ChangeRace( int newRace )
        {
            if( INE.Data.Races.ContainsKey( newRace ) )
            {
                Attributes.SetModifiers( INE.Data.Races[newRace].RacialModifersPairs );
            }
        }

        public void CurrentRanks()
        {
            if( Attributes != null )
                Attributes.CurrentRanks( INE.Char.AttributeRanks( Exp, Tier ) );
            if( Skills != null )
                Skills.CurrentRanks( INE.Char.SkillRanks( Exp, Tier ) );
        }

        public void ProjectRanks( float expGained, float retrainingGained = 0 )
        {
            float projectedRetraining = retrainingGained + INE.Char.RankRetraining( Exp, expGained, Tier );
            float projectedExp = Exp + expGained > MaxExp ? MaxExp : Exp + expGained;

            Attributes.ProjectRanks( INE.Char.AttributeRanks( projectedExp, Tier ), projectedRetraining );
            Skills.ProjectRanks( INE.Char.SkillRanks( projectedExp, Tier ), projectedRetraining );
        }
    }
}
