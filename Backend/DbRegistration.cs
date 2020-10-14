using System.Linq;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Notifications;
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

                BsonClassMap.RegisterClassMap<Notification>(classMap =>
                {
                    classMap.AutoMap();
                    classMap.SetIsRootClass(true);

                    // register all types derived from Notification
                    var notificationType = typeof(Notification);
                    notificationType.Assembly.GetTypes()
                        .Where(notificationType.IsAssignableFrom).ToList()
                        .ForEach(classMap.AddKnownType);
                });
            }
        }

    }
}
