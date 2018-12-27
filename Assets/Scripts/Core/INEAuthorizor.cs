using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace IncarnationEngine
{
    public class INEAuthorizor
    {
        private readonly RegionEndpoint Endpoint;
        private readonly string PoolID;
        private readonly string ClientID;
        private readonly string Username;
        private readonly AmazonCognitoIdentityProviderClient Provider;
        private readonly CognitoUserPool UserPool;
        private readonly CognitoUser User;
        public INESession SaveSession { get; private set; }
        public string DisplayName { get { return SessionActive ? User.Username : ""; } }
        public bool SessionActive { get { return User.SessionTokens != null; } }

        public INEAuthorizor( RegionEndpoint endpoint, string poolID, string clientID, string username )
        {
            Endpoint = endpoint;
            PoolID = poolID;
            ClientID = clientID;
            Username = username;
            Provider = new AmazonCognitoIdentityProviderClient( new AnonymousAWSCredentials(), Endpoint );
            UserPool = new CognitoUserPool( PoolID, ClientID, Provider );
            User = new CognitoUser( Username, ClientID, UserPool, Provider );
            SaveSession = null;
        }

        public async Task<bool> Login( string password )
        {
            bool authorized = await Authorize( password: password );
            if( authorized )
                SaveSession = new INESession( Endpoint, PoolID, ClientID, Username, password );
            return authorized;
        }

        private async Task<bool> Authorize( string password )
        {
            if( password != null )
            {
                try
                {
                    AuthFlowResponse AuthResponse = await User.StartWithSrpAuthAsync
                            ( new InitiateSrpAuthRequest() { Password = password } ).ConfigureAwait( false );   
                    
                    if( AuthResponse != null )
                    {
                        Debug.Log( "User successfully authenticated" );
                        return true;
                    }
                }
                catch( NotAuthorizedException e )
                {
                    Debug.Log( "Authentication Error" );
                    Debug.Log( e );
                    return false;
                }
            }
            return false;
        }

        public void SignOut()
        {
            User.SignOut();
        }

        public async Task<bool> Connected()
        {
            try
            {
                GetUserResponse res = await User.GetUserDetailsAsync();

                if( res != null )
                {
                    Debug.Log( "Connection is alive" );
                    return true;
                }
            }
            catch( NotAuthorizedException e )
            {
                Debug.Log( "Authentication Error" );
                Debug.Log( e );
            }
            return false;
        }

        public IEnumerator GetData( string RequestURL )
        {
            UnityWebRequest www = UnityWebRequest.Get( RequestURL );
            www.SetRequestHeader( "Authorization", User.SessionTokens.IdToken );
            yield return www.SendWebRequest();

            if( www.isNetworkError || www.isHttpError )
            {
                Debug.Log( www.error );
                Debug.Log( www.downloadHandler.text );
            }
            else
                INECore.act.CurrentData = www.downloadHandler.text;
        }
    }
}
