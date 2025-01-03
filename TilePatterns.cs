
using System.Collections.Generic;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace Tile.Core
{
    public struct TilePatterns
    {
        public bool HasFrame;
        public Label label;
        public List<GeometryBase> Patterns;
        public bool Frame;
        public List<ObjectAttributes> PatternAtts;
        public bool ColourFromObject;
        public List<System.Guid> Guids;
    }
}
