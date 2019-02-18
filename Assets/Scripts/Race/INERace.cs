using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INERace
    {
        public string FullName { get; private set; }
        public bool StarterRace { get; private set; }
        public List<float> RacialModifiers { get; private set; }
        public Dictionary<int, float> RacialModifersPairs
        {
            get
            {
                Dictionary<int, float> value = new Dictionary<int, float>();

                for( int i = 0; i < RacialModifiers.Count; i++ )
                {
                    value.Add( i, RacialModifiers[i] );
                }

                return value;
            }
        }

        public INERace()
        {
            FullName = "";
            StarterRace = false;
            RacialModifiers = new List<float>() { 1f, 1f, 1f, 1f, 1f, 1f };
        }

        public INERace( string fullName, bool starterRace, List<float> racialModifiers )
        {
            FullName = fullName ?? "";
            StarterRace = starterRace;
            RacialModifiers = new List<float>();
            for( int i = 0; i < 6; i++ )
            {
                RacialModifiers.Add( racialModifiers != null && i < racialModifiers.Count && racialModifiers[i] > 0 ? racialModifiers[i] : 1f );
            }
        }
    }
}
