using UnityEngine;
using System.Collections.Generic;

namespace IncarnationEngine
{
    public class INESound : MonoBehaviour
    {
        public static INESound act;
        
        FMOD.Studio.Bus MasterBus;

        Dictionary<string, FMODEvent> events;
        Dictionary<string, FMODparm> parms;
        public List<FMODSound> playing { get; private set; }

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
            playing = new List<FMODSound>();
            events = new Dictionary<string, FMODEvent>();
            parms = new Dictionary<string, FMODparm>();
        }

        void Start()
        {
            //populate list of parms
            //populate list of events
        }

        public void SetParmValue(string parmKey, float parmValue)
        {
            if (parms.ContainsKey(parmKey) 
                && parms[parmKey].Min >= parmValue && parms[parmKey].Max <= parmValue)
            {
                FMODparm newParm = parms[parmKey];
                newParm.Value = parmValue;
                parms[parmKey] = newParm;
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
        
        public void PlayOnce(string eventKey)
        {
            //FMODSound e;
        }
    }

    public struct FMODparm
    {
        public string Name;
        public float Value;
        public float Min;
        public float Max;

        public FMODparm(string newName, float newValue, float newMin, float newMax )
        {
            Name = newName;
            Value = newValue;
            Min = newMin;
            Max = newMax;
        }
    }

    public class FMODEvent
    {
        string eventPath;
        List<string> parmKeys = new List<string>();

        FMODEvent(string newPath)
        {
            eventPath = newPath;
            parmKeys = null;
        }

        FMODEvent(string newPath, List<string> newParms)
        {
            eventPath = newPath;
            parmKeys = newParms;
        }
    }

    public class FMODSound
    {
        FMOD.Studio.EventInstance e;
        public string eventKey { get; private set; }

        public FMODSound(string eventPath, string newKey)
        {
            e = FMODUnity.RuntimeManager.CreateInstance(eventPath);
            if (e != null)
                eventKey = newKey;
        }

        public void AddParameters(FMODparm[] parms)
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
                e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(newValue));
        }

        public void SetVolume(float newValue)
        {
            if (e != null && newValue >= 0.0f && newValue <= 1.0f)
                e.setVolume(newValue);
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

        public void Pause(bool state = true)
        {
            if (e != null)
                e.setPaused(state);
        }
    }
}