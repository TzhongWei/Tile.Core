using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rhino;
using Rhino.DocObjects;

namespace Tile.Core.Util
{
    public class BlockInstance
    {
        public Label BlockLabel { get; private set; }
        public string BlockName { get; private set; }
        public int BlockIndex { get; private set; }
        public bool IsBlock { get; private set; }
        public InstanceDefinition RhinoInstance 
            => Rhino.RhinoDoc.ActiveDoc.InstanceDefinitions.Find(BlockName);
        public BlockInstance(Label blockLabel, string blockName)
        {
            BlockLabel = blockLabel;
            BlockName = blockName;
            var RhinoBlock = Rhino.RhinoDoc.ActiveDoc.InstanceDefinitions.Find(blockName);
            BlockIndex =
                RhinoBlock == null ? - 1 : RhinoBlock.Index;
            if (BlockIndex == -1)
                IsBlock = false;
        }
        public override bool Equals(object obj)
        => (obj is BlockInstance) && ((BlockInstance) obj).BlockIndex == BlockIndex;
        public override int GetHashCode()
        {
            string Code = this.BlockIndex.ToString() + ((int)this.BlockLabel).ToString() + this.BlockName;
            return Code.GetHashCode();
        }
        public override string ToString()
        {
            return this.ToJson();
        }
        public string ToJson()
        {
            var Instance = this.RhinoInstance;
            var Infor = new Dictionary<string, string>
            {
                {"Hat", Instance.GetUserString("Hat") },
                {"Label", Instance.GetUserString("Label") },
                {"BlockName", BlockName },
                {"ID", this.BlockIndex.ToString()}
            };
            return JsonConvert.SerializeObject(Infor, Formatting.Indented);
        }
    }
}
