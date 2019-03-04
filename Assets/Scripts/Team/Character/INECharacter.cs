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
        public INEArchetype Archetype { get; private set; }
        public INEAspectGroup Attributes { get; private set; }
        public INEAspectGroup Skills { get; private set; }
        public List<int> Perks { get; private set; }
        public INELayout DefaultLayout { get; private set; }
        public INECharacter Clone { get { return (INECharacter)this.MemberwiseClone(); } }
        public bool DistributionValid
        {
            get
            {
                bool valid = false;

                if( Attributes != null && Skills != null )
                    valid = Attributes.DistributionValid && Skills.DistributionValid;

                return valid;
            }
        }

        public INECharacter()
        {
            FullName = "New Character";
            RaceID = 0;
            Tier = 5;
            Exp = 500;
            Archetype = INE.Data.Archetypes[0] ?? new INEArchetype();
            PopulateAspects();
            ChangeRace( RaceID );
            Perks = new List<int>();
            DefaultLayout = new INELayout();
        }

        public INECharacter( string fullName, int race, INEArchetype archetype, int tier, float exp,
            Dictionary<int, float> attributeDistributions, Dictionary<int, float> skillDistributions )
        {
            FullName = fullName ?? "New Character";
            Tier = tier < 1 ? 1 : tier > 5 ? 5 : tier;
            Exp = exp < 0 ? 0 : exp > MaxExp ? MaxExp : exp;
            Archetype = archetype ?? new INEArchetype();
            PopulateAspects();
            ChangeRace( RaceID );
            Attributes.SetTargetDistribution( attributeDistributions );
            Attributes.SetCurrentDistribution( attributeDistributions );
            Skills.SetTargetDistribution( skillDistributions );
            Skills.SetCurrentDistribution( skillDistributions );
            Perks = new List<int>();
            DefaultLayout = new INELayout();
        }

        public void ChangeRace( int newRace )
        {
            if( INE.Data.Races.ContainsKey( newRace ) )
            {
                RaceID = newRace;
                Attributes.SetModifiers( INE.Data.Races[newRace].RacialModifersPairs );
            }
        }

        public void ResetAspects( bool resetAttributes = true, bool resetSkills = true )
        {
            if( resetAttributes && Attributes != null )
            {
                Attributes.ResetTargetDistributions();
            }

            if( resetSkills && Skills != null )
            {
                Skills.ResetTargetDistributions();
            }
        }

        public void CurrentRanks()
        {
            if( Attributes != null )
                Attributes.CurrentRanks( INE.Char.AttributeRanks( Exp, Tier ) );
            if( Skills != null )
                Skills.CurrentRanks( INE.Char.SkillRanks( Exp, Tier ) );
        }

        public void ProjectRanks( float expGained, float retrainingGained = 0, bool setToIdeal = false )
        {
            float projectedRetraining = retrainingGained + INE.Char.RankRetraining( Exp, expGained, Tier );
            float projectedExp = Exp + expGained > MaxExp ? MaxExp : Exp + expGained;

            Attributes.ProjectRanks( INE.Char.AttributeRanks( projectedExp, Tier ), projectedRetraining, setToIdeal );
            Skills.ProjectRanks( INE.Char.SkillRanks( projectedExp, Tier ), projectedRetraining, setToIdeal );
        }

        private void PopulateAspects()
        {
            bool attributesPopulated = false;
            bool skillsPopulated = false;

            if( Archetype != null )
            {
                if( Archetype.AttributeIDs != null )
                {
                    Dictionary<int, float> distributions = new Dictionary<int, float>();

                    for( int i = 0; i < Archetype.AttributeIDs.Count; i++ )
                    {
                        if( INE.Char.ValidAspect( Archetype.AttributeIDs[i], AspectType.Attributes ) )
                        {
                            distributions.Add( Archetype.AttributeIDs[i], 0 );
                        }
                    }

                    Attributes = new INEAspectGroup( distributions, AspectType.Attributes );
                    attributesPopulated = true;
                }

                if( Archetype.SkillIDs != null )
                {
                    Dictionary<int, float> distributions = new Dictionary<int, float>();

                    for( int i = 0; i < Archetype.SkillIDs.Count; i++ )
                    {
                        if( INE.Char.ValidAspect( Archetype.SkillIDs[i], AspectType.Skills ) )
                        {
                            distributions.Add( Archetype.SkillIDs[i], 0 );
                        }
                    }

                    Skills = new INEAspectGroup( distributions, AspectType.Skills );
                    skillsPopulated = true;
                }
            }

            if( !attributesPopulated )
                Attributes = new INEAspectGroup( AspectType.Attributes );

            if( !skillsPopulated )
                Skills = new INEAspectGroup( AspectType.Skills );
        }
    }
}
