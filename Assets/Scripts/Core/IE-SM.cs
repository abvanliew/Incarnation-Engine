using UnityEngine;

public class PMSM : MonoBehaviour
{
    public static PMSM play;
    [SerializeField] [Range(0, 1)] public float MasterVolume;

    bool Phased;
    bool Muted;
    bool Paused;
    FMOD.Studio.Bus MasterBus;
    FMOD.Studio.EventInstance BackgroundMusic;
    parm _phase;
    parm _running;
    parm _musicPiano;
    parm _musicTime;

    //validate there there is only 1 instance of this game object
    void Awake()
    {
        if( play == null )
        {
            //makes this object persist through multiple scenes
            DontDestroyOnLoad(gameObject);

            //ensures that the static reference is correct
            play = this;
        }
        else if( play != this )
            //removes duplicates
            Destroy(gameObject);

        BackgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/music/msx");
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
    }
    
    void Start()
    {
        //the name in quotes must be the same as the FMOD Parameter
        _phase = new parm( "phase", 0.0f );
        _running = new parm( "running", 0.0f );
        _musicPiano = new parm( "piano", 0.0f );
        _musicTime = new parm( "time", 0.0f );
    }

    void Update()
    {
        MasterBus.setVolume(MasterVolume);
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

    public void SetVolume( float newValue )
    {
        if( newValue >= 0.0f && newValue <= 1.0f )
            MasterVolume = newValue;
    }

    public void Pause( bool newValue = true )
    {
        Paused = newValue;
        FMODUnity.RuntimeManager.PauseAllEvents(Paused);
    }

    public void TogglePause()
    {
        Paused = !Paused;
        FMODUnity.RuntimeManager.PauseAllEvents(Paused);
    }

    public void Mute( bool newValue = true)
    {
        Muted = newValue;
        FMODUnity.RuntimeManager.MuteAllEvents(Muted);
    }

    public void ToggleMute()
    {
        Muted = !Muted;
        FMODUnity.RuntimeManager.MuteAllEvents(Muted);
    }

    public void SetPhase(bool newValue)
    {
        Phased = newValue;
        _phase.Value = newValue ? 100.0f : 0.0f;
    }

    public void TogglePhase()
    {
        Phased = !Phased;
        _phase.Value = Phased ? 100.0f : 0.0f;
    }

    public void SetRunning(bool newValue)
    {
        _running.Value = newValue ? 1.0f : 0.0f;
    }

    public void SetPiano(float newValue)
    {
        if (newValue >= 0.0f && newValue <= 100.0f)
        {
            _musicPiano.Value = newValue;
            SetMusicParms();
        }
    }

    public void SetMusicTime(int newValue)
    {
        if (newValue >= 0 && newValue <= 3)
        {
            _musicTime.Value = newValue;
            SetMusicParms();
        }
    }

    private void SetMusicParms()
    {
        parm[] parms = new parm[] { _musicPiano, _musicTime };

        AddParameters(BackgroundMusic, parms);
    }

    public void Music()
    {
        SetMusicParms();
        BackgroundMusic.start();
    }

    public void Move()
    {
        parm[] parms = new parm[] { _phase, _running };

        PlayFMOD("event:/character/movement/footsteps", parms );
    }

    public void Jump()
    {
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/character/movement/jump", parms);
    }

    public void Land()
    {
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/character/movement/land", parms);
    }

    public void Damage()
    {
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/character/vocalizations/damage", parms);
    }

    public void Death()
    {
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/character/vocalizations/death", parms);
        SetMusicTime(3);
    }

    public void StartPhaze()
    {
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/character/phase/phase", parms);
    }

    public void SuccessfulPhaze()
    {
        /*
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/?", parms);
        */
    }

    public void FailedPhaze()
    {
        /*
        parm[] parms = new parm[] { _phase };

        PlayFMOD("event:/?", parms);
        */
    }

    public void PickupEnergy()
    {
        PlayFMOD("event:/pickups/energy");
    }

    public void PickupHealth()
    {
        PlayFMOD("event:/pickups/health");
    }
}

public struct parm
{
    public string Name;
    public float Value;

    public parm( string newName, float newValue )
    {
        Name = newName;
        Value = newValue;
    }
}