using System.Collections.Generic;

namespace Reitti.Web.Models
{
    public class RouteDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public int TotalTime { get; set; }
        public IList<RouteSegment> Stops { get; set; } = new List<RouteSegment>();
    }
}