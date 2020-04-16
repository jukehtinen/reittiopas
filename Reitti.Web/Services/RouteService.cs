using Microsoft.Extensions.Logging;
using Reitti.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Reitti.Web.Service
{
    public class RouteService : IRouteService
    {
        private readonly ILogger<RouteService> _logger;
        private Reittiopas _reittiOpas;
        private int[,] _routeMatrix;
        private List<RouteSegment> _edges;

        public RouteService(ILogger<RouteService> logger)
        {
            _logger = logger;
            BuildNetwork();
        }

        public RouteDto GetRoute(string from, string to)
        {
            var fromIndex = _reittiOpas.Stops.IndexOf((from ?? string.Empty).ToUpperInvariant());
            var toIndex = _reittiOpas.Stops.IndexOf((to ?? string.Empty).ToUpperInvariant());
            if (fromIndex == -1 || toIndex == -1)
            {
                return null;
            }

            // Get shortest path to target.
            var (totalTime, path) = Dijkstra(fromIndex, toIndex);

            // Match stops with lines.
            var stops = new List<RouteSegment>();
            string currentLine = null;
            for (var i = 0; i < path.Count - 1; i++)
            {
                var stop1 = _reittiOpas.Stops[path[i]];
                var stop2 = _reittiOpas.Stops[path[i + 1]];

                // Get line going thru these stops, prefer current line if possible (no extra changes).
                var availableLines = _edges.Where(e => (e.Stop1 == stop1 && e.Stop2 == stop2) || (e.Stop1 == stop2 && e.Stop2 == stop1));
                var useLine = availableLines.FirstOrDefault(l => l.Line == currentLine) ?? availableLines.FirstOrDefault();
                currentLine = useLine.Line;

                _logger.LogDebug($"{stop1} -> {stop2} with {useLine.Line}");

                stops.Add(new RouteSegment { Stop1 = stop1, Stop2 = stop2, Line = useLine.Line, Time = useLine.Time });
            }

            return new RouteDto { From = from, To = to, TotalTime = totalTime, Stops = stops };
        }

        public IEnumerable<string> GetStops() => _reittiOpas.Stops;

        private void BuildNetwork()
        {
            _reittiOpas = JsonSerializer.Deserialize<Reittiopas>(File.ReadAllText("reittiopas.json"));
            _routeMatrix = new int[_reittiOpas.Stops.Count, _reittiOpas.Stops.Count];
            _edges = new List<RouteSegment>();

            foreach (var line in _reittiOpas.Lines)
            {
                var stops = line.Value;
                for (var i = 0; i < stops.Length - 1; i++)
                {
                    var stop1 = stops[i];
                    var stop2 = stops[i + 1];
                    var road = _reittiOpas.Roads.FirstOrDefault(r => (r.From == stop1 && r.To == stop2) || (r.To == stop1 && r.From == stop2));

                    // Going both directions
                    _routeMatrix[_reittiOpas.Stops.IndexOf(stop1), _reittiOpas.Stops.IndexOf(stop2)] = road.Time;
                    _routeMatrix[_reittiOpas.Stops.IndexOf(stop2), _reittiOpas.Stops.IndexOf(stop1)] = road.Time;

                    _edges.Add(new RouteSegment { Stop1 = stop1, Stop2 = stop2, Line = line.Key, Time = road.Time });
                }
            }
        }

        private (int totalTime, List<int> nodes) Dijkstra(int fromIndex, int toIndex)
        {
            // Find distances with dijkstra's algorithm (keep track of node parents while at it).
            var nodes = _routeMatrix.GetLength(0);

            var shortest = new int[nodes];
            var visited = new bool[nodes];
            var parents = new int[nodes];

            for (var i = 0; i < nodes; i++)
            {
                shortest[i] = int.MaxValue;
            }

            shortest[fromIndex] = 0;
            parents[fromIndex] = -1;

            for (var i = 1; i < nodes; i++)
            {
                var nearest = -1;
                var shortestDistance = int.MaxValue;
                for (var v = 0; v < nodes; v++)
                {
                    if (!visited[v] && shortest[v] < shortestDistance)
                    {
                        nearest = v;
                        shortestDistance = shortest[v];
                    }
                }

                visited[nearest] = true;

                for (var v = 0; v < nodes; v++)
                {
                    var edgeDistance = _routeMatrix[nearest, v];

                    if (edgeDistance > 0 && ((shortestDistance + edgeDistance) < shortest[v]))
                    {
                        parents[v] = nearest;
                        shortest[v] = shortestDistance + edgeDistance;
                    }
                }
            }

            // Find path from fromIndex to toIndex
            var path = new List<int>();
            void RecursePath(int index, int[] parents)
            {
                if (index == -1)
                    return;

                path.Add(index);
                RecursePath(parents[index], parents);
            }
            RecursePath(toIndex, parents);

            path.Reverse();
            return (shortest[toIndex], path);
        }
    }
}