using System;
using System.Collections.Generic;
using System.Text;
using FRITeam.Swapify.BackendTest;
using MongoDB.Driver;

namespace BackendTest
{
    static class DBSettings
    {
        public static string ConnectionString() => MongoRunner.GetRunner().ConnectionString;

        public static IMongoDatabase Database =>
            new MongoClient(ConnectionString()).GetDatabase("TestingDB");    // run mongodb service and create testing db in it

        public static void InitDBSettings(MongoRunnerType mongodbRunnerType)
        {
            MongoRunner.InitMongoRunner(mongodbRunnerType);
        }
    }
}
