using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class NewController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class ActionState
    {
        public readonly ActionStateType actionStateType;
        readonly int maxSteps;
        readonly float maxConcentration;
        float curSteps;
        float curConcentration;
        int curIndex;
        List<ConcentraionStep> concentrations;

        public float Rate { get { return maxConcentration / maxSteps; } }

        public ActionState( 
            ActionStateType newType, 
            int newSteps, float newConcentation, 
            float initialSteps = 0f, float initialConcentration = 0f )
        {
            actionStateType = newType;
            maxSteps = newSteps < 1 ? 1 : newSteps;
            maxConcentration = newConcentation;
            curSteps = initialSteps;
            curConcentration = initialConcentration;
            curIndex = 0;

            if( actionStateType == ActionStateType.Continuous )
            {
                concentrations = new List<ConcentraionStep>( newSteps );
            }
            else
            {
                concentrations = null;
            }
        }

        public void Step( float value, float stepSize = 1f )
        {

        }
    }

    public enum ActionStateType
    { 
        Iterative,
        Continuous
    }

    struct ConcentraionStep
    {
        float step;
        float concentration;

        public ConcentraionStep( float newStep, float newConcentraion )
        {
            step = newStep;
            concentration = newConcentraion;
        }
    }
}
