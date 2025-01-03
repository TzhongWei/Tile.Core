using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
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
        private TilePatterns[] PatternsManager = new TilePatterns[5];

        public Einstein SetTile { private get; set; } = new Einstein();
        public Einstein_Resize(double size, Point3d StartPt)
        {
            if(size < 0) this.Hatsize = 1;
            else this.Hatsize = size;
            this.Translation = Transform.Translation(new Vector3d(StartPt.X, StartPt.Y, StartPt.Z));
            Label[] LabelTags = { Label.H, Label.H1, Label.T, Label.P, Label.F };

            //Initialise Patterns
            for (int i = 0; i < this.PatternsManager.Length; i++)
            {
                var Patterns = new TilePatterns();
                Patterns.label = LabelTags[i];
                Patterns.Patterns = new List<GeometryBase>();
                Patterns.PatternAtts = new List<ObjectAttributes>();
                Patterns.Guids = new List<System.Guid>();
                Patterns.ColourFromObject = false;
                this.PatternsManager[i] = Patterns;
            }
        }
        public void NewSetPatterns(List<AddPatternOption> Options)
        {
            PatternFunction.NewSetPatterns(Options, ref this.PatternsManager);
        }
        public void NewSetFrame()
        {
            PatternFunction.NewSetFrame(ref PatternsManager);
        }

        public bool SetNewBlock(List<string> Name = null, bool Blockoverride = true)
        {
            _HatID = new HatGroup<int>(PatternFunction.SetNewBlock(ref this.PatternsManager, Name, Blockoverride));
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
    }
}
