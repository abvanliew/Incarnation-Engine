using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Amazon;
using System.Net.Http;
using Newtonsoft.Json;

namespace IncarnationEngine
{
    [Serializable] public class INE : MonoBehaviour
    {
        public static INE Core { get; private set; }
        public static INELedger Ledger { get; private set; }
        public static INEReferenceData Data { get; private set; }
        public static INEInterface UI { get; private set; }
        [SerializeField] public INEInterfaceList UIList = new INEInterfaceList();

        public static INEFormating Format { get; private set; } = new INEFormating();
        public static INECharacterConstants Char { get; private set; } = new INECharacterConstants();
        public static INECombatConstants Combat { get; private set; } = new INECombatConstants();

        private static INEAuthorizor Auth;
        private static string SessionPath;

        //This block of code needs to be replaced, probably by a scriptable object with the data loaded into it
        static RegionEndpoint DefaultEndpoint = RegionEndpoint.USEast1;
        static string DefaultPoolID = "us-east-1_qDlRDAqit";
        static string DefaultClientID = "69qjjs5euih1kok6cdjvru1r6o";
        static string BaseURL = "https://6zq8xeebml.execute-api.us-east-1.amazonaws.com/dev/";

        public static string AccountDisplayName { get { return Auth != null ? Auth.DisplayName : ""; } }

        //validate there there is only 1 instance of this game object
        private void Awake()
        {
            if( Core == null )
            {
                //makes this object persist through multiple scenes
                DontDestroyOnLoad( gameObject );

                //ensures that the static reference is correct
                Core = this;
                Ledger = new INELedger();
                Data = new INEReferenceData();
                UI = new INEInterface( UIList );
            }
            //removes duplicates
            else if( Core != this )
                Destroy( gameObject );
        }
        
        private void Start()
        {
            SessionPath = Application.persistentDataPath + "/session.dat";
            DefaultEndpoint = RegionEndpoint.USEast1;
            DefaultPoolID = "us-east-1_qDlRDAqit";
            DefaultClientID = "69qjjs5euih1kok6cdjvru1r6o";
            BaseURL = "https://6zq8xeebml.execute-api.us-east-1.amazonaws.com/dev/";

            TestStart();
        }

        private async void TestStart()
        {
            Data.LoadBaseData();
            //Ledger.NewCharacterBuild();

            bool authorized = await LoadSession();

            if( authorized )
            {
                bool listLoaded = await Ledger.LoadTeamList();

                if( listLoaded )
                    UI.OpenTeamList();
            }
            else
            {
                UI.OpenLogin();
            }
        }

        public static async Task<bool> Login( string username, string password, 
            RegionEndpoint endpoint = null, string poolID = null, string clientID = null )
        {
            RegionEndpoint newEndpoint = DefaultEndpoint;
            if( endpoint != null )
                newEndpoint = endpoint;

            string newPoolID = DefaultPoolID;
            if( poolID != null )
                newPoolID = poolID;

            string newClientID = DefaultClientID;
            if( clientID != null )
                newClientID = clientID;

            Auth = new INEAuthorizor( newEndpoint, newPoolID, newClientID, username );
            bool authorized = await Auth.Login( password );

            return authorized;
        }

        public static void SignOut()
        {
            if( Auth != null )
                Auth.SignOut();
        }

        public static async Task<T> GetData<T>( string callPath )
        {
            INEResponse<T> parse = new INEResponse<T>();

            if( Auth != null )
            {
                HttpContent response = await Auth.GetData( BaseURL + callPath );
                parse = await response.ReadAsAsync<INEResponse<T>>();
            }

            return parse.body;
        }

        public static async Task<string> GetDataAsJson( string callPath )
        {
            HttpContent res = await Auth.GetData( BaseURL + callPath );
            string parse = await res.ReadAsStringAsync();

            return parse;
        }

        public static async Task<T> PostData<T>( string callPath, object data )
        {
            T responseData = default(T);
            string json = JsonConvert.SerializeObject( data, Formatting.Indented );

            Debug.Log( json );

            HttpContent res = await Auth.PostData( BaseURL + callPath, json );

            if( res != null )
            {
                INEResponse<T> parse = await res.ReadAsAsync<INEResponse<T>>();
                responseData = parse.body;
            }

            return responseData;
        }

        public static async Task<string> PostDataAsJson( string callPath, string data )
        {
            HttpContent res = await Auth.PostData( BaseURL + callPath, data );
            string parse = await res.ReadAsStringAsync();

            return parse;
        }

        private static async Task<bool> LoadSession()
        {
            bool authorized = false;

            if( File.Exists( SessionPath ) )
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open( SessionPath, FileMode.Open );
                INESession session = (INESession)bf.Deserialize( file );
                file.Close();

                authorized = await Login( session.Username, session.Password, 
                    endpoint: session.Endpoint, poolID: session.PoolID, clientID: session.ClientID );

                if( !authorized )
                    File.Delete( SessionPath );
            }

            if( !authorized )
            {
                Auth = null;
            }

            return authorized;
        }

        private static void SaveSession()
        {
            if( Auth.SessionActive )
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create( SessionPath );
                bf.Serialize( file, Auth.SaveSession );
                file.Close();
            }
        }
    }

    [Serializable] public class INESession
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

    public struct INEResponse<T>
    {
        public int statusCode;
        public T body;
    }
}