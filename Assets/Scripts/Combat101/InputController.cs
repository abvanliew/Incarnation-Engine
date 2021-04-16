using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputController : MonoBehaviour
{
    public Camera MainCamera;
    public NavMeshAgent PlayerAgent;

    void Start()
    {
        PlayerAgent.updateRotation = false;
    }

    void Update()
    {
        if( Input.GetMouseButtonDown(1))
        {
            Ray clickPoint = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit clickHit;

            if( Physics.Raycast( clickPoint, out clickHit ) )
            {
                PlayerAgent.SetDestination( clickHit.point );
            }
        }
    }
}
