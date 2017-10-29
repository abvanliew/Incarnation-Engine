using UnityEngine;
using System;
using System.Collections.Generic;

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;


namespace IncarnationEngine
{
    public class INECore : MonoBehaviour
    {
        public static INECore act;
        private Dataset TestData;

        string IdentityPoolId = "us-east-1_qDlRDAqit";
        string Region = RegionEndpoint.USEast1.SystemName;
        public string MainData;
        public bool SaveData;
        public bool SyncData;

        private RegionEndpoint _Region
        {
            get { return RegionEndpoint.GetBySystemName( Region ); }
        }

        private CognitoAWSCredentials _credentials;
        private CognitoAWSCredentials Credentials
        {
            get
            {
                if( _credentials == null )
                    _credentials = new CognitoAWSCredentials( IdentityPoolId, _Region );
                return _credentials;
            }
        }

        private CognitoSyncManager _syncManager;
        private CognitoSyncManager SyncManager
        {
            get
            {
                if( _syncManager == null )
                {
                    _syncManager = new CognitoSyncManager( Credentials, new AmazonCognitoSyncConfig { RegionEndpoint = _Region } );
                }
                return _syncManager;
            }
        }

        //validate there there is only 1 instance of this game object
        void Awake()
        {
            if( act == null )
            {
                //makes this object persist through multiple scenes
                DontDestroyOnLoad( gameObject );

                //ensures that the static reference is correct
                act = this;
            }
            else if( act != this )
                //removes duplicates
                Destroy( gameObject );
        }

        void Start()
        {
            UnityInitializer.AttachToGameObject( this.gameObject );

            TestData = SyncManager.OpenOrCreateDataset( "TestData" );

            MainData = "test 1";

            TestData.OnSyncSuccess += this.HandleSyncSuccess;
            TestData.OnSyncFailure += this.HandleSyncFailure;
            TestData.OnSyncConflict = this.HandleSyncConflict;
            TestData.OnDatasetMerged = this.HandleDatasetMerged;
            TestData.OnDatasetDeleted = this.HandleDatasetDeleted;
        }

        void Update()
        {
            if( SaveData )
            {
                TestData.Put( "MainData", MainData );

                SaveData = false;
            }

            if( SyncData )
            {
                TestData.SynchronizeAsync();

                SyncData = false;
            }
        }

        private bool HandleDatasetDeleted( Dataset dataset )
        {
            Debug.Log( dataset.Metadata.DatasetName + " Dataset has been deleted" );

            // Clean up if necessary 

            // returning true informs the corresponding dataset can be purged in the local storage and return false retains the local dataset
            return true;
        }

        public bool HandleDatasetMerged( Dataset dataset, List<string> datasetNames )
        {
            Debug.Log( dataset + " Dataset needs merge" );
            // returning true allows the Synchronize to resume and false cancels it
            return true;
        }

        private bool HandleSyncConflict( Amazon.CognitoSync.SyncManager.Dataset dataset, List<SyncConflict> conflicts )
        {
            Debug.Log( "OnSyncConflict" );
            List<Amazon.CognitoSync.SyncManager.Record> resolvedRecords = new List<Amazon.CognitoSync.SyncManager.Record>();

            foreach( SyncConflict conflictRecord in conflicts )
            {
                // This example resolves all the conflicts using ResolveWithRemoteRecord 
                // SyncManager provides the following default conflict resolution methods:
                //      ResolveWithRemoteRecord - overwrites the local with remote records
                //      ResolveWithLocalRecord - overwrites the remote with local records
                //      ResolveWithValue - for developer logic  
                resolvedRecords.Add( conflictRecord.ResolveWithRemoteRecord() );
            }

            // resolves the conflicts in local storage
            dataset.Resolve( resolvedRecords );

            // on return true the synchronize operation continues where it left,
            //      returning false cancels the synchronize operation
            return true;
        }

        private void HandleSyncSuccess( object sender, SyncSuccessEventArgs e )
        {

            var dataset = sender as Dataset;

            if( dataset.Metadata != null )
            {
                Debug.Log( "Successfully synced for dataset: " + dataset.Metadata );
            }
            else
            {
                Debug.Log( "Successfully synced for dataset" );
            }

            if( dataset == TestData )
            {
                MainData = string.IsNullOrEmpty( TestData.Get( "MainData" ) ) ? "Test 1" : dataset.Get( "MainData" );
            }
        }

        private void HandleSyncFailure( object sender, SyncFailureEventArgs e )
        {
            var dataset = sender as Dataset;
            Debug.Log( "Sync failed for dataset : " + dataset.Metadata.DatasetName );
            Debug.LogException( e.Exception );
        }
    }
}
