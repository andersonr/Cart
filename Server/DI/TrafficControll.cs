using System.Collections.Generic;

namespace Server.DI
{
    public class TrafficControll : ITrafficLock
    {
        public Dictionary<string, string> TrafficLock { get; set; }

        public TrafficControll()
        {
            TrafficLock = new Dictionary<string, string>();
        }
    }
}
