using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tile.Core.Util;

namespace Tile.Core.Grasshopper
{
    public class GH_TileInstance : GH_Param<BlockInstance>
    {
        public GH_TileInstance() : base("TileReference", "EinRef", "Represents a reference to a Einstein block instance",
          "Einstein", "Einstein", GH_ParamAccess.item) { }
        public override Guid ComponentGuid => new Guid("16A048BE-9077-4314-8855-7B424F54C261");
    }
}
