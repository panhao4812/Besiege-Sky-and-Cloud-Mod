using spaar;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{

    public class BesiegeModLoader : spaar.ModLoader.Mod
    {
        public override string Name { get { return "Sky_and_Ground_Mod"; } }
        public override string DisplayName { get { return "Sky and Ground Mod"; } }
        public override string BesiegeVersion { get { return "v0.27"; } }
        public override string Author { get { return "zian1"; } }
        public override Version Version { get { return new Version("0.87"); } }
        public override bool CanBeUnloaded { get { return true; } }
        public GameObject temp;
        public override void OnLoad()
        {
            temp = new GameObject(); temp.name = "Sky and Ground Mod";
            temp.AddComponent<Scene>();
            temp.AddComponent<Ground>();
            UnityEngine.Object.DontDestroyOnLoad(temp);
        }
        public override void OnUnload()
        {
            UnityEngine.Object.Destroy(temp);
        }        
    }  
}
