using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class CombatController : MonoBehaviour
    {
        public string Name = "Default";
        public float CharHeight = 1.0f;
        public float CharWidth = 1.0f;
        public float QuartoReach = .4f;
        public float MedioReach = .8f;
        public bool PreferMedio = true;
        public float Speed = 2.0f;
        public float DashSpeed = 3.0f;
        public float TurnRate = 2.4f;
        public List<CombatController> targetChars;
        public GameObject BodyMesh;
        public GameObject CurMomentumMarker;
        public GameObject TargetMomentumMarker;
        public GameObject CurFacingMarker;
        public GameObject TargetFacingMarker;
        public GameObject ActiveEquipmentFacing;
        public GameObject MinReachMarker;
        public GameObject MaxReachMarker;
        public GameObject ChamberMarker;
        public bool Active = false;
        public ActionPointUI ActionPointUI;

        private readonly float Hover = .1f;
        private readonly float ReachBuffer = .01f;
        private bool display = true;
        private float MinReach;
        private float MaxReach;
        private ActionController mainControler;
        private int FixedCount;

        // Start is called before the first frame update
        void Start()
        {
            InitTransform();
            BehavoirInit(); 
            BehavoirTransform();

            // int mask = ( 1 << 1 ) + ( 1 << 3 );
            // for( int val = 1; val <= 32; val++ ) if( MaskComp( val, mask ) ) Debug.Log( $"{val} included" );

            Dictionary<string, List<SimpleAction>> ActionSequences = new Dictionary<string, List<SimpleAction>>();
            ActionSequences.Add( "SlowSwing", new List<SimpleAction>() {
                new SimpleAction( "Swing", 1600, 240 ),
                new SimpleAction( "Recover", 0, 3 ),
                new SimpleAction( "Chamber", 120, 24 ),
            } );
            ActionSequences.Add( "NormalSwing", new List<SimpleAction>() {
                new SimpleAction( "Swing", 800, 100 ),
                new SimpleAction( "Recover", 0, 2 ),
                new SimpleAction( "Chamber", 40, 8 ),
            } );
            ActionSequences.Add( "QuickSwing", new List<SimpleAction>() {
                new SimpleAction( "Swing", 750, 65),
                new SimpleAction( "Recover", 0, 1 ),
                new SimpleAction( "Chamber", 30, 6 ),
            } );
            ActionSequences.Add( "Block", new List<SimpleAction>() {
                new SimpleAction( "Tracking", 320, 32 ),
                new SimpleAction( "Block", 200, 12 ),
                new SimpleAction( "Recover", 0, 2 ),
                new SimpleAction( "Chamber", 20, 8 ),
            } );

            float max = 72;
            mainControler = new ActionController( max, 26f );
            if( ActionPointUI != null ) ActionPointUI.SetActionMax( max );

            mainControler.Weapons.Add( new SimpleWeapon( "Sword", ActionSequences["SlowSwing"], ActionSequences["NormalSwing"] ) );
            mainControler.Weapons.Add( new SimpleWeapon( "Axe", ActionSequences["SlowSwing"], ActionSequences["QuickSwing"] ) );
            mainControler.Weapons.Add( new SimpleWeapon( "Shield", ActionSequences["Block"], ActionSequences["Block"] ) );
        }

        bool MaskComp( int value, int mask ) { return ( mask & value ) == mask; }

        void FixedUpdate()
        {
            if( Active )
            {
                FixedCount++;
                // mainControler.Display();
                mainControler.Step( FixedCount );
            }
        }

        // Update is called once per frame
        void Update()
        {
            if( Active && ActionPointUI != null )
            {
                ActionPointUI.SetActionCur( mainControler.ActionPointCur );
                ActionPointUI.SetEffectiveness( mainControler.EffectivenessCur );
                ActionPointUI.SetActionList( mainControler.GetActionList() );
            }
        }

        private void InitTransform()
        {
            //this.transform.position = new Vector3( this.transform.position.x, CharHeight + Hover, this.transform.position.z );

            if( BodyMesh != null )
            {
                BodyMesh.transform.localScale = new Vector3( CharWidth, CharHeight, CharWidth );
            }
        }

        private void BehavoirInit()
        {
            float netReach = PreferMedio ? MedioReach : QuartoReach;
            MinReach = CharWidth / 2f + (PreferMedio ? QuartoReach : 0f);
            MaxReach = MinReach + ( netReach < ReachBuffer ? ReachBuffer : netReach );
        }

        private void BehavoirTransform()
        {
            if( ChamberMarker != null ) ChamberMarker.transform.localPosition = new Vector3( 0f, 0f, CharWidth / 2f );
            if( MinReachMarker != null ) MinReachMarker.transform.localPosition = new Vector3( 0f, 0f, MinReach );
            if( MaxReachMarker != null ) MaxReachMarker.transform.localPosition = new Vector3( 0f, 0f, MaxReach );
        }
    }

    public class ActionController
    {
        public float ActionPointCur;
        public float EffectivenessCur;
        public float ActionPointMax;
        public float ActionPointRecharge;
        public List<SimpleWeapon> Weapons;

        public ActionController( float max, float recharge )
        {
            ActionPointCur = .5f * max;
            ActionPointMax = max;
            EffectivenessCur = 1f;
            ActionPointRecharge = recharge;
            Weapons = new List<SimpleWeapon>();
        }

        public ActionController( float max, float recharge, List<SimpleWeapon> weapons )
        {
            ActionPointCur = .5f * max;
            ActionPointMax = max;
            EffectivenessCur = 1f;
            ActionPointRecharge = recharge;
            Weapons = weapons;
        }

        public List<string> GetActionList()
        {
            List<string> actList = new List<string>();

            for( int i = 0; i < Weapons.Count; i++ )
            {
                actList.Add( string.Format(
                    "{0}: {1}",
                    Weapons[i].Name, Weapons[i].ActionQueue[0].Name, Weapons[i].ActionQueue[0].CurAction
                ) );
            }

            return actList;
        }

        public void Step( int fixedCount )
        {
            float consumption = 0;
            float effectiveness = 1;

            // check for interuptions to action list
            // then check if resolving actions would trigger contiguous actions
            // calculate action effifency per creature
            // update all actions in progress
            // move to next action in list after actions complete
            // resolve all actions

            for( int i = 0; i < Weapons.Count; i++ )
            {
                consumption += Weapons[i].Rate;
            }

            if( consumption > ActionPointCur + ActionPointRecharge )
            {
                effectiveness = ( ActionPointCur + ActionPointRecharge ) / consumption;
                consumption *= effectiveness;
            }

            //if( fixedCount % 20 == 0 )
            //{
            //    Debug.Log( string.Format(
            //        "{0}: Weapon count {1}, Consumption {2}, Effectiveness {3}",
            //        fixedCount, Weapons.Count, consumption, effectiveness
            //    ) );
            //}

            EffectivenessCur = effectiveness;
            ActionPointCur += ActionPointRecharge - consumption;
            if( ActionPointCur > ActionPointMax ) ActionPointCur = ActionPointMax;

            for( int i = 0; i < Weapons.Count; i++ )
            {
                Weapons[i].Step( effectiveness, fixedCount );
            }
        }
    }

    public class SimpleWeapon
    {
        public string Name { get; private set; }
        public List<SimpleAction> ActionQueue;
        public List<SimpleAction> DefaultLoop;
        public float Rate => ActionQueue != null && ActionQueue.Count > 0 ? ActionQueue[0].Rate : 0;

        public SimpleWeapon( string name )
        {
            Name = name;
            ActionQueue = new List<SimpleAction>();
            DefaultLoop = new List<SimpleAction>();
        }

        public SimpleWeapon( string name, List<SimpleAction> actQueue )
        {
            Name = name;
            ActionQueue = new List<SimpleAction>();
            DefaultLoop = new List<SimpleAction>();

            for( int i = 0; i < actQueue.Count; i++ )
            {
                ActionQueue.Add( new SimpleAction( actQueue[i].Name, actQueue[i].TargetAction, actQueue[i].TargetSteps ) );
            }
        }

        public SimpleWeapon( string name, List<SimpleAction> actQueue, List<SimpleAction> defaultLoop )
        {
            Name = name;
            ActionQueue = new List<SimpleAction>();
            DefaultLoop = new List<SimpleAction>();

            for( int i = 0; i < actQueue.Count; i++ )
            {
                ActionQueue.Add( new SimpleAction( actQueue[i].Name, actQueue[i].TargetAction, actQueue[i].TargetSteps ) );
            }

            for( int i = 0; i < defaultLoop.Count; i++ )
            {
                DefaultLoop.Add( new SimpleAction( defaultLoop[i].Name, defaultLoop[i].TargetAction, defaultLoop[i].TargetSteps ) );
            }
        }

        public void AddSequence( List<SimpleAction> newSeq )
        {
            if( newSeq != null )
            {
                if( ActionQueue == null )
                {
                    ActionQueue = new List<SimpleAction>();
                }

                for( int i = 0; i < newSeq.Count; i++ )
                {
                    ActionQueue.Add( new SimpleAction( 
                        newSeq[i].Name, 
                        newSeq[i].TargetAction, 
                        newSeq[i].TargetSteps 
                    ) );
                }
            }
        }

        public void AddAction( SimpleAction newAct )
        {
            AddSequence( new List<SimpleAction> { newAct } );
        }

        public (bool, float) Step( float effectiveness, int fixedCount )
        {
            bool complete = false;
            float actionCost = 0;

            if( ActionQueue == null )
            {
                ActionQueue = new List<SimpleAction>();
            }

            if( ActionQueue.Count == 0 && DefaultLoop != null && DefaultLoop.Count > 0 )
            {
                AddSequence( DefaultLoop );
            }

            if( ActionQueue.Count > 0 )
            {
                (complete, actionCost) = ActionQueue[0].Step( effectiveness );

                if( complete )
                {
                    //Debug.Log( string.Format( 
                    //    "{0}: {1} with {2} at the value of {3}",
                    //    fixedCount, ActionQueue[0].Name, Name, actionCost 
                    //) );
                    ActionQueue.RemoveAt( 0 );
                }
            }

            if( ActionQueue.Count == 0 && DefaultLoop != null && DefaultLoop.Count > 0 )
            {
                //Debug.Log( "Default Action Sequence Set" );
                //for( int i = 0; i < DefaultLoop.Count; i++ )
                //{
                //    DefaultLoop[i].Display();
                //}
                AddSequence( DefaultLoop );
                //Debug.Log( string.Format(
                //    "Action Queue len {0}, Default loop len {1}",
                //    ActionQueue.Count, DefaultLoop.Count
                //) );
            }

            return ( complete, actionCost );
        }
    }

    public class SimpleAction
    {
        public readonly string Name;
        public readonly float TargetSteps;
        public readonly float TargetAction;
        public float CurSteps { get; private set; }
        public float CurAction { get; private set; }

        public float Rate { get { return TargetAction / TargetSteps; } }

        public SimpleAction( string name, float action, float steps )
        {
            Name = name;
            CurSteps = 0;
            CurAction = 0;
            TargetSteps = steps;
            TargetAction = action;
        }

        public void Display()
        {
            Debug.Log( string.Format( 
                "{0}: Steps {1} / {2}, Action {3} / {4}", 
                Name, CurSteps, TargetSteps, CurAction, TargetAction 
            ) );
        }

        public ( bool, float ) Step( float effectiveness = 1 )
        {
            CurSteps++;
            CurAction += effectiveness * Rate;

            return (CurSteps >= TargetSteps, CurAction );
        }
    }

    public struct ItemInUse
    {
        public List<ActionStep> ActionSteps;
        public List<ActionStep> DefaultLoop;
        public float steps;
        public float effectiveness;

    }

    public struct ActionStep
    {
        public enum ActionStates { Swing, Shoot, Cast, Counterspell, Block, Disarm, Clear, Trip, Track, Chamber, Swap, Recover }
        public ActionStates ActionState;
        public float TargetSteps;
        public float ActionRate;
        public bool Contiguous;
    }
}



//BehavoirInit();
//Vector2 myTopDownPos = new Vector2( this.transform.position.x, this.transform.position.z );
//List<string> targetInfo = new List<string>();
//bool targetFound = false;
//Vector3 moveTowards = new Vector3();

//for( int i = 0; i < targetChars.Count; i++ )
//{
//    Vector2 targetTopDown = new Vector2(
//            targetChars[i].transform.position.x,
//            targetChars[i].transform.position.z
//        );
//    float targetDist = Vector2.Distance( myTopDownPos, targetTopDown )  - targetChars[i].CharWidth / 2f;

//    if( !targetFound && ( targetDist < MinReach || targetDist > MaxReach ) )
//    {
//        targetFound = true;
//        float dir = targetDist > MaxReach ? 1.0f : -1.0f;
//        moveTowards = new Vector3( dir * targetTopDown.x, this.transform.position.y, dir * targetTopDown.y );
//        this.transform.position = Vector3.MoveTowards(
//            this.transform.position,
//            moveTowards,
//            Speed * Time.deltaTime 
//        );
//    }

//    if( display )
//    {
//        string targetName = targetChars[i].Name;
//        targetInfo.Add( $"{targetName} ( {targetDist} )" );
//    }

//    if( targetFound && !display ) { break; }
//}

//if( display )
//{
//    string displayInfo = targetInfo.Count > 0 ? string.Join( ", ", targetInfo ) : "nothing";
//    Debug.Log( $"{Name} is targeting {displayInfo}" );
//    display = false;
//}