using Reitti.Web.Models;
using System.Collections.Generic;

namespace Reitti.Web.Service
{
    public interface IRouteService
    {
        RouteDto GetRoute(string from, string to);
        IEnumerable<string> GetStops();
    }
}