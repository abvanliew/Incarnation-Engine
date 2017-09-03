using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INECore : MonoBehaviour
    {
        public static INECore act;

        //validate there there is only 1 instance of this game object
        void Awake()
        {
            if (act == null)
            {
                //makes this object persist through multiple scenes
                DontDestroyOnLoad(gameObject);

                //ensures that the static reference is correct
                act = this;
            }
            else if (act != this)
                //removes duplicates
                Destroy(gameObject);
        }
    }
}
