using System;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEFormating
    {
        public readonly Dictionary<int, string> AttributeNames;
        public readonly Dictionary<int, string> SkillNames;
        public readonly string[] CurrencyFormats;
        public readonly string ValidNamePattern;
        public readonly string ValidCharPattern;

        public INEFormating()
        {
            AttributeNames = new Dictionary<int, string>()
            {
                { 0, "Strength" }, { 1, "Finesse" }, { 2, "Perception" }, { 3, "Speed" }, { 4, "Endurance" }, { 5, "Resistance" },
                { 6, "Potency" }, { 7, "Essence" }, { 8, "Affinity" }
            };
            SkillNames = new Dictionary<int, string>()
            {
                { 0, "Striking" }, { 1, "Shooting" }, { 2, "Defending" }, { 3, "Disruption" }, { 4, "Combat Mobility" }, { 5, "Stealth" },
                { 6, "Spell Mastery" }, { 7, "Fire" }, { 8, "Frost" }, { 9, "Electricity" }, { 10, "Water" }, { 11, "Benevolent" }, { 12, "Malevolent" }, { 13, "Earth" }
            };
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

        public string RankBonus( float value )
        {
            return value <= 0 ? "-" : string.Format( "{0}%", Mathf.Round( 1000 * ( value - 1 ) ) / 10 );
        }
    }
}