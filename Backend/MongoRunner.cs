using Mongo2Go;

namespace BackendTest
{
    public class MongoRunner
    {
        private static MongoDbRunner aRunner;
        private static bool aIsDebugging;


        public static MongoDbRunner RunnerFoTests()
        {
            if (aRunner == null)
            {
                aRunner = MongoDbRunner.Start();
                aIsDebugging = false;
            }

            return aRunner;
        }

        public static MongoDbRunner RunnerForDebug()
        {
            if (aRunner == null)
            {
                aRunner = MongoDbRunner.StartForDebugging();
                aIsDebugging = true;
            }
            return aRunner;
        }

        public static void Dispose()
        {
            aRunner.Dispose();
        }
    }
}
