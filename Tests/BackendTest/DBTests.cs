using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace BackendTest
{
    static class DBSettings
    {
        public static string ConnectionString(bool isTest) => isTest ?
             MongoRunner.RunnerFoTests().ConnectionString : MongoRunner.RunnerForDebug().ConnectionString;

        public static IMongoDatabase Database =>
            new MongoClient(DBSettings.ConnectionString(false)).GetDatabase("TestingDB");    // run mongodb service and create testing db in it
    }
}
