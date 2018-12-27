using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Amazon;
using Amazon.Extensions.CognitoAuthentication;

namespace IncarnationEngine
{
    [Serializable] public class INECore : MonoBehaviour
    {
        public static INECore act;
        private INEAuthorizor auth;

        public INEInterface ui;
        public string CurrentData;
        public string DisplayName
        { get { return auth != null ? auth.DisplayName : ""; } }

        private string SessionPath;
        RegionEndpoint DefaultEndpoint = RegionEndpoint.USEast1;
        string DefaultPoolID = "us-east-1_qDlRDAqit";
        string DefaultClientID = "69qjjs5euih1kok6cdjvru1r6o";
        string DefaultRequestURL = "https://6zq8xeebml.execute-api.us-east-1.amazonaws.com/dev/";

        //validate there there is only 1 instance of this game object
        private void Awake()
        {
            if( act == null )
            {
                //makes this object persist through multiple scenes
                DontDestroyOnLoad( gameObject );

                //ensures that the static reference is correct
                act = this;
            }
            //removes duplicates
            else if( act != this )
                Destroy( gameObject );
        }

        private void Start()
        {
            SessionPath = Application.persistentDataPath + "/session.dat";
            DefaultEndpoint = RegionEndpoint.USEast1;
            DefaultPoolID = "us-east-1_qDlRDAqit";
            DefaultClientID = "69qjjs5euih1kok6cdjvru1r6o";
            DefaultRequestURL = "https://6zq8xeebml.execute-api.us-east-1.amazonaws.com/dev";
            LoadAuthorizer();
        }

        public async void Login( string username, string password )
        {
            auth = new INEAuthorizor( DefaultEndpoint, DefaultPoolID, DefaultClientID, username );
            bool authorized = await auth.Login( password );

            if( authorized )
            {
                ui.LoginPanel.SetActive( false );
                ui.TestData.SetActive( true );
                SaveAuthorizer();
            }
        }

        public void SignOut()
        {
            if( auth != null )
            {
                auth.SignOut();
                ui.TestData.SetActive( false );
                ui.LoginPanel.SetActive( true );
            }
        }

        public void GetData()
        {
            if( auth != null )
                StartCoroutine( auth.GetData( DefaultRequestURL ) );
        }

        private async void LoadAuthorizer()
        {
            bool authorized = false;

            if( File.Exists( SessionPath ) )
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open( SessionPath, FileMode.Open );
                INESession session = (INESession)bf.Deserialize( file );
                file.Close();

                auth = new INEAuthorizor( session.Endpoint, session.PoolID, session.ClientID, session.Username );
                authorized = await auth.Login( session.Password );

                if( !authorized )
                    File.Delete( SessionPath );
            }

            if( !authorized )
            {
                auth = null;
                ui.LoginPanel.SetActive( true );
            }
            else
                ui.TestData.SetActive( true );
        }

        private async void SaveAuthorizer()
        {
            bool live = await auth.Connected();

            if( live )
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create( SessionPath );
                bf.Serialize( file, auth.SaveSession );
                file.Close();
            }
        }
    }

    [Serializable]
    public class INESession
    {
        public RegionEndpoint Endpoint { get { return RegionEndpoint.GetBySystemName( EndpointName ); } }
        [SerializeField] private readonly string EndpointName;
        [SerializeField] public readonly string PoolID;
        [SerializeField] public readonly string ClientID;
        [SerializeField] public readonly string Username;
        [SerializeField] public readonly string Password;

        public INESession( RegionEndpoint endpoint, string poolID, string clientID, string username, string password )
        {
            EndpointName = endpoint.SystemName;
            PoolID = poolID;
            ClientID = clientID;
            Username = username;
            Password = password;
        }
    }
}
