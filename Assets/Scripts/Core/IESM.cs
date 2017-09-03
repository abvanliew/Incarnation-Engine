using UnityEngine;
using System.Collections.Generic;

namespace IncarnationEngine
{
    public class IESM : MonoBehaviour
    {
        public static IESM act;
        
        FMOD.Studio.Bus MasterBus;

        Dictionary<string, FMODEvent> events = new Dictionary<string, FMODEvent>();
        Dictionary<string, parm> parms = new Dictionary<string, parm>();

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
            
            //set master bus
            MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        }

        void Start()
        {
            
        }

        private void PlayFMOD(string eventPath, parm[] parms = null)
        {
            FMOD.Studio.EventInstance e;

            e = FMODUnity.RuntimeManager.CreateInstance(eventPath);
            e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

            if (parms != null && parms.Length > 0)
                AddParameters(e, parms);

            e.start();
            e.release();
        }

        private void AddParameters(FMOD.Studio.EventInstance e, parm[] parms)
        {
            FMOD.Studio.ParameterInstance parmPass;

            if (parms != null && parms.Length > 0)
            {
                for (int i = 0; i < parms.Length; i++)
                {
                    e.getParameter(parms[i].Name, out parmPass);
                    if (parmPass != null)
                        parmPass.setValue(parms[i].Value);
                }
            }
        }

        public void SetMasterVolume(float newValue)
        {
            if (newValue >= 0.0f && newValue <= 1.0f)
                MasterBus.setVolume(newValue);
        }

        public void Pause(bool state = true)
        {
            FMODUnity.RuntimeManager.PauseAllEvents(state);
        }

        public void Mute(bool state = true)
        {
            FMODUnity.RuntimeManager.MuteAllEvents(state);
        }

        /*
        public void Move()
        {
            parm[] parms = new parm[] { _phase, _running };

            PlayFMOD("event:/character/movement/footsteps", parms);
        }
        */
    }

    public struct parm
    {
        public string Name;
        public float Value;
        public float Min;
        public float Max;

        public parm(string newName, float newValue, float newMin, float newMax )
        {
            Name = newName;
            Value = newValue;
            Min = newMin;
            Max = newMax;
        }
    }

    public class FMODEvent
    {
        FMOD.Studio.EventInstance e;
        public List<string> parmKeys = new List<string>();

        public FMODEvent() { }

        public FMODEvent(string eventPath)
        {
            e = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        }

        public FMODEvent(string eventPath, List<string> newKeys)
        {
            e = FMODUnity.RuntimeManager.CreateInstance(eventPath);
            parmKeys = newKeys;
        }

        public void SetEvent(string eventPath)
        {
            e = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        }

        public void AddParameters(parm[] parms)
        {
            if (e != null)
            {
                FMOD.Studio.ParameterInstance parmPass;

                if (parms != null && parms.Length > 0)
                {
                    for (int i = 0; i < parms.Length; i++)
                    {
                        e.getParameter(parms[i].Name, out parmPass);
                        if (parmPass != null)
                            parmPass.setValue(parms[i].Value);
                    }
                }
            }
        }

        public void SetPosistion(Vector3 newValue)
        {
            if (e != null)
            {
                e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(newValue));
            }
        }

        public void Play()
        {
            if (e != null)
                e.start();
        }

        public void Release()
        {
            if (e != null)
                e.release();
        }

        public void SetVolume(float newValue)
        {
            if (e != null && newValue >= 0.0f && newValue <= 1.0f)
                e.setVolume(newValue);
        }

        public void Pause(bool state = true)
        {
            if (e != null)
                e.setPaused(state);
        }
    }
}