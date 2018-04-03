using System;
using Mongo2Go;

namespace FRITeam.Swapify.BackendTest
{
    public static class MongoRunner
    {
        private static MongoDbRunner _runner;
        private static MongoRunnerType _type = MongoRunnerType.NotSet;

        public static void InitMongoRunner(MongoRunnerType type)
        {
            _type = type;
            switch (type)
            {
                case MongoRunnerType.Debug:
                    _runner = MongoDbRunner.StartForDebugging();
                    break;
                case MongoRunnerType.Test:
                    _runner = MongoDbRunner.Start();
                    break;
                default:
                    throw new Exception("MongoDB runner type is not set!");
            }            
        }


        public static MongoDbRunner GetRunner()
        {
            if (_type == MongoRunnerType.NotSet || _runner == null)
            {
                throw new Exception("MongoDB runner type is not set, call InitMongoRunner first!");
            }
            return _runner;
        }
        
        public static void Dispose()
        {
            _runner.Dispose();
        }
    }

    public enum MongoRunnerType
    {
        NotSet = 0,
        Debug = 1,
        Test = 2
    }
}
