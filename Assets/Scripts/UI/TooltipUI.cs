using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class TooltipUI : MonoBehaviour
    {
        public Text Tooltip;

        public void SetText( string newText )
        {
            Tooltip.text = newText;
        }
    }
}
