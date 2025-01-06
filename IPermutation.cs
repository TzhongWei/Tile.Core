using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Tile.Core
{
    public interface IPermutation
    {
        bool PlaceBlock(Einstein MonoTile);
        List<Curve> PreviewShape();
    }
}
