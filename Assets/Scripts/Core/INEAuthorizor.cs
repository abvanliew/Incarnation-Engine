using System;
using System.Threading.Tasks;
using UnityEngine;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
        public string IDToken { get { return User.SessionTokens.IdToken; } }
        private readonly HttpClient Client;

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
            Client = new HttpClient();
        }

        public async Task<bool> Login( string password )
        {
            bool authorized = await Authorize( password: password );

            if( authorized )
                SaveSession = new INESession( Endpoint, PoolID, ClientID, Username, password );
            else
                Debug.Log( "Failed to login" );
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
                        Client.DefaultRequestHeaders.Accept.Clear();
                        Client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( User.SessionTokens.IdToken );
                        Debug.Log( User.SessionTokens.IdToken );
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

        public async Task<HttpContent> GetData( string path )
        {
            HttpContent returnContent = null;

            if( SessionActive )
            {
                try
                {
                    HttpResponseMessage response = await Client.GetAsync( path );

                    if( response.IsSuccessStatusCode )
                    {
                        returnContent = response.Content;
                    }
                }
                catch( Exception e )
                {
                    Debug.Log( e );
                }
            }
            
            return returnContent;
        }

        public async Task<HttpContent> PostData( string path, string bodyJson )
        {
            HttpContent returnContent = null;

            if( SessionActive )
            {
                try
                {
                    Debug.Log( "Pre post call" );
                    Debug.Log( bodyJson );

                    HttpResponseMessage response = await Client.PostAsync( path, new StringContent( bodyJson, Encoding.UTF8, "application/json" ) );

                    Debug.Log( "After post call" );

                    if( response != null && response.IsSuccessStatusCode )
                    {
                        returnContent = response.Content;
                    }

                    Debug.Log( returnContent );
                }
                catch( Exception e )
                {
                    Debug.Log( e );
                }
            }

            return returnContent;
        }
    }
}
