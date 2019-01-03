using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class AspectGroupUI : MonoBehaviour
    {
        public Text AspectStatus;
        public RectTransform AspectPrefab;
        public RectTransform AspectParent;
        public float ExpGained;

        private List<AspectUI> Aspects;

        string[] Attributes = new string[] { "Strength", "Finesse", "Perception", "Speed", "Endurance", "Resistance",
                "Potency", "Essence", "Affinity" };
        float[] RacialMods = new float[] { 1, 1, 1, 1, 1, 1.5f, 1, 1, 1 };

        private void Start()
        {

            Aspects = new List<AspectUI>();

            for( int i = 0; i < Attributes.Length; i++ )
            {
                RectTransform newPrefab = Instantiate( AspectPrefab );
                Aspects.Add( newPrefab.GetComponent<AspectUI>() );
                newPrefab.transform.SetParent( AspectParent.transform, false );
                Aspects[i].SetAspect( i, Attributes[i] );
                Aspects[i].CurrentValue.text = Mathf.Round( 100f * RacialMods[i] ).ToString();
                Aspects[i].Parent = this;
            }
        }

        public void SetDistribution( int key, float value )
        {

        }
    }
}
