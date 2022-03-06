using System.Linq;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Notifications;
using MongoDB.Bson.Serialization;

namespace Backend
{
    public static class DbRegistration
    {
        private static bool _isInicialized = false;
        public static void Init() {
            if (!_isInicialized)
            {
                _isInicialized = true;
                BsonClassMap.RegisterClassMap<Timetable>(map =>
                {
                    map.AutoMap();
                    map.MapField("_blocks").SetElementName("AllBlocks");
                    
                });

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
