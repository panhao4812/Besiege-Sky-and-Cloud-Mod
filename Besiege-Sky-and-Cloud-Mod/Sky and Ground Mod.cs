using System;
//using System.Threading.Tasks;
using System.IO;
using spaar.ModLoader;
using System.Collections;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{

    public class BesiegeModLoader : Mod
    {
        public override string Name { get { return "Sky_and_Ground_Mod"; } }
        public override string DisplayName { get { return "Sky and Ground Mod"; } }
        public override string BesiegeVersion { get { return "0.27"; } }
        public override string Author { get { return "覅是&zian1"; } }
        public override Version Version { get { return new Version("0.86"); } }
        public override bool CanBeUnloaded { get { return true; } }
        public GameObject temp;
        public override void OnLoad()
        {
            temp = new GameObject();
            temp.name = "Sky and Ground Mod";
            temp.AddComponent<CloudAndTerrain>();
            GameObject.DontDestroyOnLoad(temp);
        }
        public override void OnUnload()
        {
            GameObject.Destroy(temp);
        }
    }
}
