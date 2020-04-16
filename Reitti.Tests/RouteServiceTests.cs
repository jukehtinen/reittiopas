using Microsoft.Extensions.Logging.Abstractions;
using Reitti.Web.Service;
using Xunit;

namespace Reitti.Tests
{
    public class RouteServiceTests
    {
        [Fact]
        public void GetRoute_Simple()
        {
            var routeService = new RouteService(new NullLogger<RouteService>());

            var routeAA = routeService.GetRoute("A", "A");
            Assert.Equal(0, routeAA.TotalTime);
            Assert.Empty(routeAA.Stops);

            var routeAB = routeService.GetRoute("A", "B");
            Assert.Equal(3, routeAB.TotalTime);
            Assert.Equal(1, routeAB.Stops.Count);
            Assert.Equal("vihreä", routeAB.Stops[0].Line);
            Assert.Equal("A", routeAB.Stops[0].Stop1);
            Assert.Equal("B", routeAB.Stops[0].Stop2);

            var routeBA = routeService.GetRoute("B", "A");
            Assert.Equal(3, routeBA.TotalTime);
            Assert.Equal(1, routeBA.Stops.Count);
            Assert.Equal("vihreä", routeBA.Stops[0].Line);
            Assert.Equal("B", routeBA.Stops[0].Stop1);
            Assert.Equal("A", routeBA.Stops[0].Stop2);
        }

        [Fact]
        public void GetRoute_Longer()
        {
            var routeService = new RouteService(new NullLogger<RouteService>());

            var routeKB = routeService.GetRoute("K", "B");
            Assert.Equal(15, routeKB.TotalTime);
            Assert.Equal(5, routeKB.Stops.Count);
            Assert.Equal("K", routeKB.Stops[0].Stop1);
            Assert.Equal("keltainen", routeKB.Stops[0].Line);
            Assert.Equal("keltainen", routeKB.Stops[1].Line);
            Assert.Equal("keltainen", routeKB.Stops[2].Line);
            Assert.Equal("sininen", routeKB.Stops[3].Line);
            Assert.Equal("vihreä", routeKB.Stops[4].Line);
            Assert.Equal("B", routeKB.Stops[4].Stop2);
        }

        [Fact]
        public void GetRoute_Errors()
        {
            var routeService = new RouteService(new NullLogger<RouteService>());

            Assert.Null(routeService.GetRoute("X", "B"));
            Assert.Null(routeService.GetRoute("X", ""));
            Assert.Null(routeService.GetRoute("", "B"));
        }
    }
}
