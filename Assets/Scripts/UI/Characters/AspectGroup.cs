using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectGroup : MonoBehaviour
    {
        public Text AspectStatus;
        public RectTransform AspectPrefab;
        public RectTransform AspectParent;
        public float ExpGained;

        private List<CharacterAspectUI> Aspects;

        string[] Attributes = new string[] { "Strength", "Finesse", "Perception", "Speed", "Endurance", "Resistance",
                "Potency", "Essence", "Affinity" };
        float[] RacialMods = new float[] { 1, 1, 1, 1, 1, 1.5f, 1, 1, 1 };

        private void Start()
        {

            Aspects = new List<CharacterAspectUI>();

            for( int i = 0; i < Attributes.Length; i++ )
            {
                RectTransform newPrefab = Instantiate( AspectPrefab );
                Aspects.Add( newPrefab.GetComponent<CharacterAspectUI>() );
                newPrefab.transform.SetParent( AspectParent.transform, false );
                Aspects[i].SetAspectName( Attributes[i] );
                Aspects[i].CurrentValue.text = Mathf.Round( 100f * RacialMods[i] ).ToString();
                Aspects[i].Parent = this;
            }

            UpdateCalculations();
        }

        public void UpdateCalculations()
        {
            if( Aspects != null )
            {
                bool validDist = false;
                int count = 9;
                float bonusExpPow = 1.1f;
                float sumDist = 0;
                float sumSqrDist = 0;
                float sumRatios = 0;
                float bonusFactor = 1f;

                for( int i = 0; i < count; i++ )
                {
                    sumDist += Aspects[i].CurrentDistribution.value;
                    sumSqrDist += Mathf.Pow( Aspects[i].CurrentDistribution.value, 2 );
                }

                if( sumDist > 0 )
                {
                    float sumSqrRatios = 0;

                    for( int i = 0; i < count; i++ )
                    {
                        sumRatios += Aspects[i].CurrentDistribution.value / sumDist;
                        sumSqrRatios += Mathf.Pow( Aspects[i].CurrentDistribution.value / sumDist, 2 );
                    }

                    float maxWeightFactor = Mathf.Log( 3.0f / 8.0f * Mathf.Pow( count, 2.0f ) );
                    float curWeightFactor = Mathf.Log( Mathf.Pow( count, 2.0f ) * sumSqrRatios / sumRatios );

                    //if( curWeightFactor <= maxWeightFactor )
                    if( true )
                    {
                        validDist = true;
                        bonusFactor = Mathf.Pow( maxWeightFactor / curWeightFactor, bonusExpPow );

                        float displayBonus = Mathf.Round( 1000f * ( bonusFactor - 1f ) ) / 10f;
                        AspectStatus.text = string.Format( "Attribute Bonus: {0}%", displayBonus );
                    }
                }

                if( !validDist )
                {
                    AspectStatus.text = "Uneven Distribution";

                    for( int i = 0; i < count; i++ )
                    {
                        Aspects[i].FinalValue.text = "";
                    }
                }

                for( int i = 0; i < count; i++ )
                {
                    Aspects[i].FinalValue.text = validDist ? 
                        Mathf.Round( RacialMods[i] * ( bonusFactor * Aspects[i].CurrentDistribution.value / sumDist / sumRatios * ExpGained 
                        + 100f ) ).ToString() 
                        : "";

                    float sumOthers = sumDist - Aspects[i].CurrentDistribution.value;
                    float sumOthersSqr = sumSqrDist - Mathf.Pow( Aspects[i].CurrentDistribution.value, 2 );

                    int minDist = Mathf.CeilToInt( ( 3 * sumOthers - 2 * Mathf.Sqrt( 6 * Mathf.Pow( sumOthers, 2 ) - 10 * sumOthersSqr ) ) / 5 );
                    if( minDist <= 0 )
                    {
                        Aspects[i].MinDistribution.gameObject.SetActive( false );
                    }
                    else
                    {
                        Aspects[i].MinDistribution.value = minDist > 120 ? 120 : minDist;
                        Aspects[i].MinDistribution.gameObject.SetActive( true );
                    }
                    int maxDist = Mathf.RoundToInt( ( 3 * sumOthers + 2 * Mathf.Sqrt( 6 * Mathf.Pow( sumOthers, 2 ) - 10 * sumOthersSqr ) ) / 5 );
                    if( maxDist >= 120 )
                    {
                        Aspects[i].MaxDistribution.gameObject.SetActive( false );
                    }
                    else
                    {
                        Aspects[i].MaxDistribution.value = maxDist < 0 ? 120 : 120 - maxDist;
                        Aspects[i].MaxDistribution.gameObject.SetActive( true );
                    }
                }
            }
        }
    }
}
