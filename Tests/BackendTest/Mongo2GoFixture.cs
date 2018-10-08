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
        private const bool RUN_WITH_STANDARD_PORT = false;

        private readonly MongoDbRunner _runner;
        public IMongoClient MongoClient { get; }

        public Mongo2GoFixture()
        {
            DbRegistration.Init();
            _runner = RUN_WITH_STANDARD_PORT ? MongoDbRunner.StartForDebugging() : MongoDbRunner.Start();
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
