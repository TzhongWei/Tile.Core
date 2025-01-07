using Grasshopper.Kernel;
using System;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Tile.Core.Util;
using System.Numerics;

namespace Tile.Core.Grasshopper
{
    public class DisplayTile : GH_Component
    {
        public DisplayTile():base("DisplayEinsteinTile", "DisEin",
            "Display the einstein block from the block definiton", 
            "Einstein", "Einstein")
        { }
        public override Guid ComponentGuid => new Guid("FB10CCF6-BEB5-4673-9609-7C90EBAD9D24");
        protected override Bitmap Icon => base.Icon;
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("TileName", "N", "The name of the hat tile, the RhinoInstanceObject Name", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "The location to place the block", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TileInfo", "In", "The information about the file", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var Name = string.Empty;
            var PL = Rhino.Geometry.Plane.WorldXY;
            DA.GetData("TileName", ref Name);
            DA.GetData("Plane", ref PL);
            var TS = Transform.PlaneToPlane(Rhino.Geometry.Plane.WorldXY, PL);
            var Tile = HatTileDoc.BlockInstances.Find(Name);

            var TileCopy = (BlockInstance)Tile.DuplicateGeometry();
            TileCopy.Transform(TS);

            DA.SetData("TileInfo", TileCopy);
        }
    }
}
