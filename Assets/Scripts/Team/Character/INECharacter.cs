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
        public float ModelRadius { get; private set; }
        public float ModelHeight { get; private set; }
        List<int> Classes;
        public INEAspectGroup Attributes { get; private set; }
        public INEAspectGroup Skills { get; private set; }
        public List<int> Perks { get; private set; }
        public INELayout DefaultLayout { get; private set; }
        public INECharacter Clone { get { return (INECharacter)this.MemberwiseClone(); } }

        public INECharacter( float maxExp, bool isCaster, Dictionary<int, float> racialModifers )
        {
            int attributeCount = isCaster ? 9 : 6;
            Attributes = new INEAspectGroup( maxExp, attributeCount, racialModifers );
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
            Exp = 400;
            Classes = new List<int>();
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
    }
}
