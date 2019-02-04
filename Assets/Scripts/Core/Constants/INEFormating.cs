using System;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEFormating
    {
        public readonly string[] AttributeNames;
        public readonly string[] SkillNames;
        public readonly string[] CurrencyFormats;
        public readonly string ValidNamePattern;
        public readonly string ValidCharPattern;

        public INEFormating()
        {
            AttributeNames = new string[] { "Strength", "Finesse", "Perception", "Speed", "Endurance", "Resistance",
                "Potency", "Essence", "Affinity" };
            SkillNames = new string[] { "Striking", "Shooting", "Defense", "Disruption", "Combat Mobility", "Stealth", "Spell Mastery",
                "Fire", "Frost", "Electricity", "Water", "Benevolent", "Malevolent", "Earth" };
            CurrencyFormats = new string[] { "{0} Platinum", "{0} Gold", "{0} Silver", "{0} Copper" };

            ValidNamePattern = "^[A-Za-z]([-A-Za-z0-9' ]{1,30})[A-Za-z0-9]$";
            ValidCharPattern = "[-A-Za-z0-9' ]";
        }

        public string Currency( float value )
        {
            //1 platinum is 100 gold
            //1 gold is 100 silver
            //1 silver is 100 copper

            List<string> currencies = new List<string>();
            float remainder = value / 100; //expect value to be in gold, convert it to platinum

            for( int i = 0; i < CurrencyFormats.Length; i++ )
            {
                int numberPieces = Mathf.FloorToInt( remainder );
                currencies.Add( String.Format( CurrencyFormats[i], numberPieces ) );
                remainder = 100 * ( remainder - numberPieces );
            }

            return currencies.Count > 0 ? String.Join( ", ", currencies ) : String.Format( CurrencyFormats[1], 0 );
        }
    }
}