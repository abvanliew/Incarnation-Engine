using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEDefs : MonoBehaviour
    {
        public static INEDefs get;

        //validate there there is only 1 instance of this game object
        void Awake()
        {
            if (get == null)
            {
                //makes this object persist through multiple scenes
                DontDestroyOnLoad(gameObject);

                //ensures that the static reference is correct
                get = this;
            }
            else if (get != this)
                //removes duplicates
                Destroy(gameObject);
        }
        
        public void init()
        {
            //start up stuff here
        }
        
        
    }
}
