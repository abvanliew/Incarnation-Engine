using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Amazon;
using Amazon.Extensions.CognitoAuthentication;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IncarnationEngine
{
    [Serializable] public class INE : MonoBehaviour
    {
        public static INE Core { get; private set; }
        public static INELedger Ledger { get; private set; }
        [SerializeField] public INEInterface UI = new INEInterface();
        private static INEAuthorizor Auth;

        private static string SessionPath;

        //This block of code needs to be replaced, probably by a scriptable object with the data loaded into it
        static RegionEndpoint DefaultEndpoint = RegionEndpoint.USEast1;
        static string DefaultPoolID = "us-east-1_qDlRDAqit";
        static string DefaultClientID = "69qjjs5euih1kok6cdjvru1r6o";
        static string BaseURL = "https://6zq8xeebml.execute-api.us-east-1.amazonaws.com/dev/";

        public static string AccountDisplayName { get { return Auth != null ? Auth.DisplayName : ""; } }
        public static readonly string[] AttributeNames;
        public static readonly string[] SkillNames;
        public static readonly string[] CurrencyFormats;
        public static readonly float BaseDamageEfficiency;
        public static readonly double BaseMitigation;
        public static readonly float BaseAspect;
        public static readonly float RetrainingRatio;
        public static readonly float ExcessExpConversion;
        public static readonly float AspectWeightPower;
        public static readonly float EvenWeightedRatio;
        public static readonly float MaxDistribution;

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
            LoadSession();
        }

        static INE()
        {
            //SkillNames = new string[] { "Striking", "Shooting", "Defense", "Disruption", "Combat Mobility", "Stealth", "Spell Mastery",
            //    "Fire", "Frost", "Electricity", "Water", "Benevolent", "Malevolent", "Earth" };
            CurrencyFormats = new string[] { "{0} Platinum", "{0} Gold", "{0} Silver", "{0} Copper" };

            BaseDamageEfficiency = 100.0f;
            BaseMitigation = 0.9930924954370359d;

            BaseAspect = 20;
            RetrainingRatio = 0.0025f;
            ExcessExpConversion = 0.15f;
            AspectWeightPower = 2;
            EvenWeightedRatio = 3f * Mathf.Pow( 1f / 3f, AspectWeightPower );
            MaxDistribution = 120;
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

            if( authorized )
            {
                SaveSession();
            }

            INE.Ledger.Teams = await GetData<INETeam>( "team/list/" );

            Ledger.RefreshTeamList();

            return authorized;
        }

        public static void SignOut()
        {
            if( Auth != null )
                Auth.SignOut();
        }

        public static async Task<List<T>> GetData<T>( string callPath )
        {
            HttpContent res = await Auth.GetData( BaseURL + callPath );
            INEResponse<T> parse = await res.ReadAsAsync<INEResponse<T>>();
            
            return parse.body;
        }

        private static async void LoadSession()
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

        public static string FormatCurrency( float value )
        {
            //1 platinum is 100 gold
            //1 gold is 100 silver
            //1 silver is 100 copper

            List<string> currencies = new List<string>();
            float remainder = value / 100; //expect value to be in gold, convert it to platinum

            for( int i = 0; i < CurrencyFormats.Length; i++ )
            {
                int numberPieces = Mathf.FloorToInt( remainder );
                currencies.Add( String.Format( CurrencyFormats[i], numberPieces ) );
                remainder = 100 * ( remainder - numberPieces );
            }

            return currencies.Count > 0 ? String.Join( ", ", currencies ) : String.Format( CurrencyFormats[1], 0 );
        }

        public static float EffectivenssCalc( float dmgEfficiency )
        {
            if( dmgEfficiency < 0 )
                dmgEfficiency = 0;

            return dmgEfficiency / ( dmgEfficiency + BaseDamageEfficiency );
        }

        public static float MitigationValue( float mitigationAmount )
        {
            return (float)Math.Pow( BaseMitigation, (double)mitigationAmount );
        }

        public static float MitigationValue( float[] mitigationAmount )
        {
            float mitigationTotal = 0f;

            //remove foreach and correct
            foreach( float mitigationFactor in mitigationAmount )
            {
                mitigationTotal += mitigationFactor;
            }
            return (float)Math.Pow( BaseMitigation, mitigationTotal );
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
        public List<T> body;
    }
}
