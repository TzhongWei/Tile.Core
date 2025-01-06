using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tile.Core.Util
{
    /// <summary>
    /// This is a static class managing the hat_tile instance file
    /// </summary>
    public static class HatTileDoc
    {
        public static BlockInstanceManager BlockInstances;
        static HatTileDoc() 
        {
            BlockInstances = InitialBlock();
        }
        /// <summary>
        /// Provide the block name list
        /// </summary>
        /// <returns></returns>
        public static List<string> HatBlock_NameList()
            => BlockInstances.Select(x => x.BlockName).ToList();
        public static List<string> HatBlock_NameList(Label label)
            => BlockInstances.Where(x => x.BlockLabel == label).Select(x=>x.BlockName).ToList();
        /// <summary>
        /// Initialise the block system from rhino
        /// </summary>
        /// <returns></returns>
        private static BlockInstanceManager InitialBlock()
        {
            var Manager = new BlockInstanceManager();
            var RHDoc = Rhino.RhinoDoc.ActiveDoc.InstanceDefinitions;

            foreach(var Instance in RHDoc)
            {
                if (Instance.GetUserString("Hat") != "HatDoc") continue;
                Manager.Add(Instance);
            }
            return Manager;
        }
        /// <summary>
        /// Add new hat instance in both rhino and this program. The reference point is set to origin point.
        /// </summary>
        /// <param name="Name">Name of this instance</param>
        /// <param name="label"></param>
        /// <param name="tilePatterns">the tile patterns</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        internal static bool AddNewHatInstance(string Name, TilePatterns tilePatterns, out int ID)
        {
            var Ins = RhinoDoc.ActiveDoc.InstanceDefinitions;
            if(Ins.Find(Name) is null)
            {
                ID = Ins.Add(Name, 
                    "This is Einstein Hat Tile Program blocks",
                    Point3d.Origin,
                    tilePatterns.Patterns,
                    tilePatterns.PatternAtts
                    );

                //SetUserString
                var InsObj = Ins[ID];
                InsObj.SetUserString("Hat", "Hat");
                InsObj.SetUserString("BlockName", Name);
                InsObj.SetUserString("Label", tilePatterns.label.ToString());
                InsObj.SetUserString("ID", ID.ToString());

                BlockInstances.Add( InsObj );

                return true;
            }
            else
            {
                ID = -1;
                return false;
            }
        }
        internal static bool RemoveHatInstance(string Name)
        {
            if(!BlockInstances.Contains(Name)) return false;
            else
            {
                BlockInstances.Remove(Name);
                return true;
            }
        }
    }
}
