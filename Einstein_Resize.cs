using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.UI;
using Tile.Core.Util;

namespace Tile.Core
{
    public class Einstein_Resize: Einstein, IPermutation
    {
        private double Hatsize = 1;
        public int[] BlocksId = new int[5];
        private Transform Translation = new Transform();
        private HatGroup<int> _HatID;
        public List<GeometryBase> HPatterns = new List<GeometryBase>();
        //private TilePatterns[] PatternsManager = new TilePatterns[5];

        public Einstein SetTile { private get; set; } = new Einstein();
        public Einstein_Resize(double size, Point3d StartPt)
        {
            if(size < 0) this.Hatsize = 1;
            else this.Hatsize = size;
            this.Translation = Transform.Translation(new Vector3d(StartPt.X, StartPt.Y, StartPt.Z));
            Label[] LabelTags = { Label.H, Label.H1, Label.T, Label.P, Label.F };
        }
        public bool SelectBlockID(List<string> Name)
        {
            if (Name.Count != 5)
            {
                if (Name.Count > 5)
                {
                    var NewName = new List<string>();
                    for (int i = 0; i < 5; i++)
                    {
                        NewName.Add(Name[i]);
                    }
                }
                else
                    while (Name.Count != 5)
                    {
                        Name.Add(Name.Last());
                    }
            }

            for (int i = 0; i < Name.Count; i++)
            {
                _HatID[i] = HatTileDoc.BlockInstances.Find(Name[i]).BlockIndex;
                if (_HatID[i] == -1)
                    throw new Exception($"The {Name[i]} isn't existed.");
            }
            return true;
        }
        public List<Curve> PreviewShape()
        {
            if (SetTile.Hat_Transform.Count <= 0) return new List<Curve>();
            object LockObj = new object();
            ConcurrentBag<Curve> HatCrvs = new ConcurrentBag<Curve>();
            ConcurrentBag<int> Seq = new ConcurrentBag<int>();
            var TS = this.SetTile.Hat_Transform;
            var Scale = Transform.Scale(Point3d.Origin, Hatsize);
            Parallel.For(0, TS.Count, i =>
            {
                var HatShape = new Einstein.HatTile("Outline").PreviewShape;
                var Final = Translation * Scale * TS[i];
                Seq.Add(i);
                HatShape.Transform(Final);
                HatCrvs.Add(HatShape);
            });
            List<Curve> sortedCurve = new List<Curve>();
            lock (LockObj)
            {
                var indexes = Seq.ToList();
                indexes.Sort();
                sortedCurve = indexes.Select(i => HatCrvs.ElementAt(i)).ToList();
            }
            return sortedCurve;
        }
        public bool PlaceBlock(Einstein MonoTile)
        {
            if (this.Hatsize < 0 || MonoTile.Hat_Labels.Count != MonoTile.Hat_Transform.Count ||
                    MonoTile.Hat_Labels.Count < 0)
                return false;
            var Doc = RhinoDoc.ActiveDoc;
            string[] LayerName = { "Hat_H", "Hat_H1", "Hat_T", "Hat_P", "Hat_F" };
            if (_HatID.Hat_F_ID < 0)
                throw new Exception("Objects hasn't been defined as blocks");

            var labels = MonoTile.Hat_Labels;
            var Transforms = MonoTile.Hat_Transform;
            var Scale = Transform.Scale(Point3d.Origin, Hatsize);
            for (int i = 0; i < Transforms.Count; i++)
            {
                var Final = Translation * Scale * Transforms[i];
                ObjectAttributes Att = new ObjectAttributes();
                switch (labels[i])
                {
                    case "H":
                        Att.LayerIndex = Doc.Layers.FindName(LayerName[0]).Index;
                        Doc.Objects.AddInstanceObject(_HatID[0], Final, Att);
                        break;
                    case "H1":
                        Att.LayerIndex = Doc.Layers.FindName(LayerName[1]).Index;
                        Doc.Objects.AddInstanceObject(_HatID[1], Final, Att);
                        break;
                    case "T":
                        Att.LayerIndex = Doc.Layers.FindName(LayerName[2]).Index;
                        Doc.Objects.AddInstanceObject(_HatID[2], Final, Att);
                        break;
                    case "P":
                        Att.LayerIndex = Doc.Layers.FindName(LayerName[3]).Index;
                        Doc.Objects.AddInstanceObject(_HatID[3], Final, Att);
                        break;
                    case "F":
                        Att.LayerIndex = Doc.Layers.FindName(LayerName[4]).Index;
                        Doc.Objects.AddInstanceObject(_HatID[4], Final, Att);
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }
    }
}
