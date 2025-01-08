using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Tile.Core.Util;

namespace Tile.Core
{
    public interface IPermutation
    {
        bool PlaceBlock(Einstein MonoTile, out List<BlockInstance> Tiles);
        List<Curve> PreviewShape();
    }
}
