using System;
using System.Collections.Generic;
using System.Text;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace BackendTest
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<Mongo2GoFixture>
    {
    }

    public class Mongo2GoFixture : IDisposable
    {
        private const bool RUN_FOR_DEBUG = false;

        private readonly MongoDbRunner _runner;
        public  IMongoClient MongoClient { get; }

        public Mongo2GoFixture()
        {
            _runner = RUN_FOR_DEBUG ? MongoDbRunner.StartForDebugging() : MongoDbRunner.Start();
            MongoClient = new MongoClient(_runner.ConnectionString);

        }
        #region IDisposable

        public void Dispose()
        {
            _runner.Dispose();
        }

        #endregion
    }
}
