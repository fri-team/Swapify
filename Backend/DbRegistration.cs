using FRITeam.Swapify.Entities;
using MongoDB.Bson.Serialization;

namespace Backend
{
    public static class DbRegistration
    {
        private static bool _isInicialized = false;
        public static void Init() {
            if (!_isInicialized)
            {
                BsonClassMap.RegisterClassMap<Timetable>(map =>
                {
                    map.AutoMap();
                    map.MapField("_blocks").SetElementName("AllBlocks");
                });
                _isInicialized = true;
            }
        }

    }
}
