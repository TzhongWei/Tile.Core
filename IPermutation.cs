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
        bool SetNewBlock(List<string> Names, bool overwrite);
        bool PlaceBlock(Einstein MonoTile);
        void NewSetPatterns(List<AddPatternOption> Options);
        List<Curve> PreviewShape();
    }
}
