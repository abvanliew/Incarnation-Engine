using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointUI : MonoBehaviour
{
    public Slider ActionSlider;
    public Slider EffectivenessSlider;
    public Text ActionTextCur;
    public Text ActionTextMax;
    public Text CurrentActions;

    public void SetActionCur( float value )
    {
        ActionSlider.value = value;
        ActionTextCur.text = Mathf.Round( value ).ToString();
    }

    public void SetActionMax( float value )
    {
        ActionSlider.maxValue = value;
        ActionTextMax.text = Mathf.Round( value ).ToString();
    }

    public void SetEffectiveness( float value )
    {
        EffectivenessSlider.value = value;
    }

    public void SetActionList( List<string> actions )
    {
        CurrentActions.text = string.Join( System.Environment.NewLine, actions );
    }
}
