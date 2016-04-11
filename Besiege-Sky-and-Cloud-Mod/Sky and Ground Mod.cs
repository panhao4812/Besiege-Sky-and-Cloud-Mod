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
        public override string BesiegeVersion { get { return "v0.27"; } }
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
    public class SkyAndCloudMod : MonoBehaviour
    {

        private GameObject[] clouds = new GameObject[60];
        /*private GameObject floatingRock = new GameObject();
        private int floatingrocksCloneCount = 0;*/
        private GameObject cloudTemp;
      //  private GameObject godLightTemp;
     //   private GameObject rainTemp;
     //   private GameObject thunderCloudTemp;
        private int cloudAmount = 60;
        private int cloudAmountTemp = 0;
        private float cloudSizeScale = 1;
        private float lowerCloudsMinHeight = 130f;
        private float lowerCloudsMaxHeight = 200f;
        private float higherCloudsMinHeight = 300;
        private float higherCloudsMaxHeight = 377.25f;
        private Color higherCloudsColor = new Color(1f, 1f, 1f, 1f);
        private Color lowerCloudsColor = new Color(0.92f, 0.9f, 0.8f, 1);
        private Color SkyColor;
        private float[] cloudSpeed = new float[2];
        public bool CustomCloudSpeed = false;
        private bool isFogAway = false;
        public Vector3 floorScale = new Vector3(911,10,900);
        public float cameraDrawingRange = 1500;
        private bool isShadowOff = false;
        public bool isBoundairesAway = false;
        public bool IsNightMode = false;
        private bool resetCloudsNow = false;
        private int tempLevel;
        private GameObject sunS = new GameObject();
        private GameObject sun;
        public GameObject[] shadow;
        public string settingTemp;
        public string[] Settings;
        public bool settingTempHasBeenChanged = false;
        private bool DetectedStartedSimulating = false;
        void OnLoad()
        {
            sun = new GameObject();
            sunS = new GameObject();
            DontDestroyOnLoad(sun.gameObject);
        }
        void Start()
        {

            /*sun = new GameObject();
            sun.name = "SunShine";
            sun.SetActive(true);
            DontDestroyOnLoad(sun);

            sun.AddComponent<Light>();
            sun.AddComponent<LensFlare>();
sun.AddComponent<LensFlare>().flare = new Flare();
sun.GetComponent<LensFlare>().color = Color.yellow;
            sun.GetComponent<LensFlare>().brightness = 10;
            sun.GetComponent<LensFlare>().fadeSpeed = 0;
            sun.GetComponent<Light>().type = LightType.Point;
            sun.GetComponent<Light>().range = 3000;
            // sun.GetComponent<Light>().color = Color.red;
            sun.GetComponent<Light>().intensity = 0f;
            sun.GetComponent<Light>().shadows = LightShadows.Soft;*/
            //Application.LoadLevel (5);
            StartCoroutine(groundTexture());

            Commands.RegisterHelpMessage("SimpleIOBlocks commands:\n	IOSpheres [bool]\n	IOPulse [bool]\n	IOTickGUI [bool]");
            Commands.RegisterCommand("ResetCloudsAmount", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    int cccloudcloudAmount = int.Parse(args[0]);
                    if (cccloudcloudAmount < 0 ^ cccloudcloudAmount > 3000) { return "Your cloud amount is not available. "; }
                    else { cloudAmount = cccloudcloudAmount; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to cloud amount";
                }
                return "There will be " + cloudAmount.ToString() + " clouds";


            }, "Reset the amount of clouds. No bigger than 3000 and no less than 2.");//Amount

            Commands.RegisterCommand("ResetCloudsSizeScale", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float cccloudcloudSizeScale = float.Parse(args[0]);
                    if (cccloudcloudSizeScale <= 0) { return "Your cloud size scale is not available. "; }
                    else { cloudSizeScale = cccloudcloudSizeScale; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to cloud size scale";
                }

                return "The clouds' size scale will be " + cloudSizeScale.ToString();

            }, "Reset Clouds' Size Scale");//SizeScale

            Commands.RegisterCommand("ResetLowerCloudsMinHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float llllowerCloudsMinHeight = float.Parse(args[0]);
                    if (llllowerCloudsMinHeight >= lowerCloudsMaxHeight) { return "Your lower cloud minimum height is not available. "; }
                    else { lowerCloudsMinHeight = llllowerCloudsMinHeight; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to lower cloud minimum height";
                }

                return "The lower clouds' minimum height will be " + lowerCloudsMinHeight.ToString();

            }, "Reset Lower Clouds' Min Height");//ResetLowerCloudsMinHeight

            Commands.RegisterCommand("ResetLowerCloudsMaxHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float llllowerCloudsMaxHeight = float.Parse(args[0]);
                    if (llllowerCloudsMaxHeight <= lowerCloudsMinHeight) { return "Your lower cloud maximum height is not available. "; }
                    else { lowerCloudsMaxHeight = llllowerCloudsMaxHeight; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to lower cloud maximum height";
                }

                return "The lower clouds' maximum height will be " + lowerCloudsMaxHeight.ToString();

            }, "Reset Lower Clouds' Max Height");//ResetLowerCloudsMaxHeight

            Commands.RegisterCommand("ResetHigherCloudsMinHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float hhhhigherCloudsMinHeight = float.Parse(args[0]);
                    if (hhhhigherCloudsMinHeight >= higherCloudsMaxHeight) { return "Your higher cloud minimum height is not available. "; }
                    else { higherCloudsMinHeight = hhhhigherCloudsMinHeight; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to higher cloud minimum height";
                }

                return "The higher clouds' minimum height will be " + higherCloudsMinHeight.ToString();

            }, "Reset Higher Clouds' Min Height");//ResetHigherCloudsMinHeight

            Commands.RegisterCommand("ResetHigherCloudsMaxHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float hhhigherCloudsMaxHeight = float.Parse(args[0]);
                    if (hhhigherCloudsMaxHeight <= higherCloudsMinHeight) { return "Your higher cloud maximum height is not available. "; }
                    else { higherCloudsMaxHeight = hhhigherCloudsMaxHeight; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to higher cloud maximum height";
                }

                return "The higher clouds' maximum height will be " + higherCloudsMaxHeight.ToString();

            }, "Reset Higher Clouds' Max Height.");//ResetHigherCloudsMaxHeight

            Commands.RegisterCommand("ResetHigherCloudsColorRGBA", (args, notUses) =>
            {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n  ResetHigherCloudsColorRGBA 155 0 255 99";
                }
                try
                {
                    higherCloudsColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                    settingTempHasBeenChanged = true;
                }
                catch
                {
                    return "ERROR! Please do it like this\n  ResetHigherCloudsColorRGBA 155 0 255 99";
                }

                return "The higher cloud color will be " + higherCloudsColor.ToString();

            }, "Reset the color of higher clouds by R G B A.");//ResetHigherCloudsColor

            Commands.RegisterCommand("ResetLowerCloudsColorRGBA", (args, notUses) =>
            {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n  ResetLowerCloudsColorRGBA 155 0 255 99";
                }
                try
                {
                    lowerCloudsColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                    settingTempHasBeenChanged = true;
                }
                catch
                {
                    return "ERROR! Please do it like this\n  ResetLowCloudsColorRGBA 155 0 255 99";
                }

                return "The lower cloud color will be " + lowerCloudsColor.ToString();

            }, "Reset the color of lower clouds by R G B A.");//ResetLowerCloudsColor

            Commands.RegisterCommand("ResetSkyColorRGBA", (args, notUses) =>
            {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n ResetSkyColorRGBA 155 0 255 99";
                }
                try
                {
                    GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                    SkyColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                    settingTempHasBeenChanged = true;
                }
                catch
                {
                    return "ERROR! Please do it like this\n ResetSkyColorRGBA 155 0 255 99";
                }

                return "The sky color will be " + lowerCloudsColor.ToString();

            }, "Reset the color of sky by R G B A. Default is 144 166 180 100");//ResetSkyColor

            Commands.RegisterCommand("Re-ProduceAllClouds", (args, notUses) =>
            {

                resetCloudsNow = true;
                settingTempHasBeenChanged = true;
                return "The clouds will be re-produce";

            }, "Produce your clouds again");//Reproduce All Clouds

            Commands.RegisterCommand("CleanFog", (args, notUses) =>
            {

                try { GameObject.Find("Fog Volume").transform.position = new Vector3(0, Mathf.Infinity, 0);
                    isFogAway = true;
                    settingTempHasBeenChanged = true;
                    return "The Fog will be moved away";
                } catch { return "The Fog does not exist!"; }

            }, "Put the fog away to make your view cleaner");//Clean Fog

            Commands.RegisterCommand("ResetFog", (args, notUses) =>
            {

                try { GameObject.Find("Fog Volume").transform.position = new Vector3(0, GameObject.Find("Main Camera").transform.position.y - 50, 0);
                    isFogAway = false;
                    settingTempHasBeenChanged = true;
                    return "The Fog will be reset under your camera";
                } catch { return "The Fog does not exist!"; }

            }, "Put the fog back");//Reset Fog

            Commands.RegisterCommand("ResetCloudSpeed", (args, notUses) =>
           {

               if (args.Length < 2)
               {
                   return "ERROR!You need to have two speed value!";
               }
               try
               {
                   cloudSpeed[0] = float.Parse(args[0]);
                   cloudSpeed[1] = float.Parse(args[1]);
                   settingTempHasBeenChanged = true;
               }
               catch
               {
                   return "ERROR!";
               }
               CustomCloudSpeed = true;
               return "The speed will be: X: " + cloudSpeed[0] + "  Z: " + cloudSpeed[1];

           }, "Change the moving speed of yur clouds by x and z");//Cloud speed

            Commands.RegisterCommand("ResetFloorSizeScale", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    GameObject.Find("FloorBig").transform.localScale = new Vector3(float.Parse(args[0]), GameObject.Find("FloorBig").transform.localScale.y, float.Parse(args[1]));
                    floorScale = new Vector3(float.Parse(args[0]), GameObject.Find("FloorBig").transform.localScale.y, float.Parse(args[1]));
                    settingTempHasBeenChanged = true;
                }
                catch
                {
                    return "Could not parse " + args[0] + "and" + args[1] + "to floor size scale";
                }

                return "The floor's size scale will be " + GameObject.Find("FloorBig").transform.localScale.ToString();

            }, "Reset the size of the floor as big as you want.(default is 900 900)");//FloorSizeScale

            Commands.RegisterCommand("ResetCameraDrawingRange", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    cameraDrawingRange = float.Parse(args[0]);
                    if (cameraDrawingRange <= 1) { return "Your Range is not available. "; }
                    else { GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = cameraDrawingRange; settingTempHasBeenChanged = true; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to camara drawing range";
                }

                return "The camara drawing range will be " + GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane.ToString();

            }, "Reset the camera drawing range to the value you want (no less than 1; default is 1500)");//ResetCameraDrawingRange

            Commands.RegisterCommand("TurnOn/OffCloudShadows", (args, notUses) =>
            {

                try {
                    if (!isShadowOff)
                    {
                        foreach (GameObject shadowMaker in shadow) { shadowMaker.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; }
                        isShadowOff = true;
                        settingTempHasBeenChanged = true;
                        return "The shadow has been turned off";

                    }

                    else if (isShadowOff)
                    {
                        foreach (GameObject shadowMaker in shadow) { shadowMaker.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly; }
                        isShadowOff = false;
                        settingTempHasBeenChanged = true;
                        return "The shadow has been turned on";
                    }
                    else { return "Nothing Can be turn off/on"; }
                } catch { return "The shadows does not exist!"; }

            }, "Turn on/off your clouds' shadows");//Shadow

            /* Commands.RegisterCommand("AddOneFloatingRock", (args, notUses) =>
            {
            if (args.Length < 3)
            {
                return "ERROR!You don't have all four position and rotation values! (X, Y, Z, Rotation) \n Please do it like this\n  AddOneFloatingRock -120 110 56 92";
            }
                if (Application.loadedLevel == 23 ^ floatingRock != null)
                {
                    floatingRock = GameObject.Find("FloatingRocks");
                    GameObject.DontDestroyOnLoad(floatingRock);
                    DontDestroyOnLoad(floatingRock);
                    floatingRock.transform.parent = null;
                    floatingRock.SetActive(false);

                    try
                    {
                        Quaternion qtnon = new Quaternion();
                        qtnon.eulerAngles = new Vector3(0, float.Parse(args[3]), 0);
                        GameObject floatingrocksClone = new GameObject();
                        floatingrocksCloneCount += 1;
                        floatingrocksClone.name = ("floatingrocksClone" + floatingrocksCloneCount);
                        floatingrocksClone = (GameObject)GameObject.Instantiate(floatingRock, new Vector3(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])), qtnon);
                        floatingrocksClone.SetActive(true);
                    }
                    catch 
                    {
                        if (GameObject.Find("FloatingRocks"))
                        {
                            return "ERROR! Please do it like this\n  AddOneFloatingRock -120 110 56 93";
                        }
                        else { return "You need to go Level 18 to get FloatingRocks!"; }
                    }
                }
                else
                {
                    return "The" + floatingrocksCloneCount + "'s stone will be " + GameObject.Find("floatingrocksClone" + floatingrocksCloneCount).transform.position.ToString();
                }
                return "a";

            }, "Reset the color of lower clouds by X Y Z Rotation.");//AddOneFloatingRock

            Commands.RegisterCommand("DeleteAllFloatingStones", (args, notUses) =>
            {

                try
                {
                    if (floatingRocks.Length < 1) { return "No Rocks!"; }
                    else
                    {
                        for (int i = floatingRocks.Length; i >= 0; i--)
                        {
                            Destroy(floatingRocks[i]);
                        }

                        return "Done!";
                    }
                }
                catch { return "The stones does not existed!"; }

            }, "Delete All Floating Stones");//DeleteAllFloatingStones

            Commands.RegisterCommand("DeleteLatestFloatingStone", (args, notUses) =>
            {

                try
                {
                    if (floatingRocks.Length < 1) { return "No Rocks!"; }
                    else
                    {
                        Destroy(floatingRocks[floatingRocks.Length]);
                        

                        return "Done!";
                    }
                }
                catch { return "The stones does not existed!"; }

            }, "Delete Latest Floating Stone");//DeleteLatestFloatingStones

            Commands.RegisterCommand("ResetLatestStoneSizeScale", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float stoneSizeScale = float.Parse(args[0]);
                    if (stoneSizeScale <= 0) { return "Your stone size scale is not available. "; }
                    else { floatingRocks[floatingRocks.Length].transform.localScale = new Vector3(stoneSizeScale, stoneSizeScale, stoneSizeScale); }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to stone size scale";
                }

                return "The clouds' size scale will be " + floatingRocks[floatingRocks.Length].transform.localScale.x.ToString();

            }, "Reset the latest Stone's Size Scale");//StoneSizeScale*/

            Commands.RegisterCommand("NoWorldBoundaries", (args, notUses) =>
            {

                try { GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                    isBoundairesAway = true;
                    settingTempHasBeenChanged = true;
                    return "The World Boundaries will be moved away";
                } catch { return "The World Boundaries does not exist!"; }

            }, "Move the World Boundaries away");//Clean World Boundaries

            Commands.RegisterCommand("ResetWorldBoundaries", (args, notUses) =>
            {

                try { GameObject.Find("WORLD BOUNDARIES").transform.position = new Vector3(1, 1, 1);
                    isBoundairesAway = false;
                    settingTempHasBeenChanged = true;
                    return "The World Boundaries will be reset.";
                } catch { return "The World Boundaries does not exist!"; }

            }, "Put the World Boundaries back");//Reset World Boundaries

            Commands.RegisterCommand("On/OffNightMode", (args, notUses) =>
            {

                try
                {
                    if (!IsNightMode)
                    {
                        IsNightMode = true;
                        GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = new Color(0.1f, 0.1f, 0.1f);
                        GameObject.Find("Directional light").GetComponent<Light>().intensity = 0f;
                        GameObject.Find("Directional light").AddComponent<LensFlare>();
                        GameObject.Find("Directional light").GetComponent<LensFlare>().brightness = 10;
                        GameObject.Find("Directional light").GetComponent<LensFlare>().color = Color.white;
                        GameObject.Find("Directional light").GetComponent<LensFlare>().enabled = true;
                        GameObject.Find("Directional light").GetComponent<LensFlare>().fadeSpeed = 1;
                        ItIsNightSoEveryCloudShouldBeDeepDarkFantasy();
                        settingTempHasBeenChanged = true;
                        return "Night is coming......";
                    }
                    else
                    {
                        IsNightMode = false;
                        GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = new Color(0.5803922f, 0.6509804f, 0.7058824f, 1); ;
                        GameObject.Find("Directional light").GetComponent<Light>().intensity = 0.92f;
                        Destroy(GameObject.Find("Directional light").GetComponent<LensFlare>());
                        ItIsDaySoEveryCloudShouldBeBright();
                        settingTempHasBeenChanged = true;
                        return "Day is coming _(:3」∠)_";
                    }
                }
                catch { return "The Main Camera does not exists!"; }

            }, "Turn on/off the night mode");//Night
        }        
        //public int CurrentLevel = 2;
        IEnumerator groundTexture()
        {
            yield return new WaitForSeconds(0.01f);
            try
            {

                if (Input.GetKey(KeyCode.F5))
                {
                    WWW png = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png");
                    WWW jpg = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg");
                    GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture = null;
                    if (png.size > 5) 
                    {
                        try
                        {
                            GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png").texture;
                        }
                        catch { }
                        try
                        {
                            GameObject.Find("terrainObject").AddComponent<Renderer>();
                            GameObject.Find("terrainObject").GetComponent<Terrain>().materialTemplate =new Material(Shader.Find("Nature / Terrain / Diffuse"));
                            GameObject.Find("terrainObject").GetComponent<Terrain>().materialTemplate.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png").texture;
                        }
                        catch { }
                    }
                    else if (jpg.size > 5)
                    {
                        try
                        {
                            GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg").texture;
                        }
                        catch { }
                        try
                        {
                            GameObject.Find("terrainObject").AddComponent<Renderer>();
                            GameObject.Find("terrainObject").GetComponent<Terrain>().materialTemplate = new Material(Shader.Find("Nature / Terrain / Diffuse"));
                            GameObject.Find("terrainObject").GetComponent<Terrain>().materialTemplate.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg").texture;
                        }
                        catch { }
                    }
                    else { Debug.Log("There is no such a texture file named \"GroundTexture.png\" or \"GroundTexture.jpg\" \n under \\Besiege_Data\\Mods\\Blocks\\Textures\\! "); }
                }
            }
            catch { }
            StartCoroutine(groundTexture());
        }
        void Update()
        {
            try
            {
                if (isShadowOff)
                {
                    try
                    {
                        foreach (GameObject shadowMaker in shadow)
                        {
                            shadowMaker.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                        }
                    }
                    catch { }
                }
                if (isBoundairesAway)
                {
                    try { GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0); } catch { }
                }
                if (isFogAway)
                {
                    try { GameObject.Find("Fog Volume").transform.position = new Vector3(0, Mathf.Infinity, 0); } catch { }
                }
                try { GameObject.Find("FloorBig").transform.localScale = new Vector3(floorScale[0], floorScale[1], floorScale[2]); } catch { }
            }
            catch { }
            if (settingTempHasBeenChanged)
                {
                settingTempHasBeenChanged = false;
                settingTemp = "";

                settingTemp += cloudAmount;//0
                settingTemp += "|";

                settingTemp += cloudSizeScale;//1
                settingTemp += "|";

                settingTemp += lowerCloudsMinHeight;//2345
                settingTemp += "|";
                settingTemp += lowerCloudsMaxHeight;
                settingTemp += "|";
                settingTemp += higherCloudsMinHeight;
                settingTemp += "|";
                settingTemp += higherCloudsMinHeight;
                settingTemp += "|";

                settingTemp += higherCloudsColor.r + "," + higherCloudsColor.g + "," + higherCloudsColor.b + "," + higherCloudsColor.a;//67
                settingTemp += "|";
                settingTemp += lowerCloudsColor.r + "," + lowerCloudsColor.g + "," + lowerCloudsColor.b + "," + lowerCloudsColor.a;
                settingTemp += "|";

                settingTemp += SkyColor.r + "," + SkyColor.g + "," + SkyColor.b + "," + SkyColor.a;//8
                settingTemp += "|";

                settingTemp += CustomCloudSpeed.ToString();//9
                settingTemp += "|";


                settingTemp += cloudSpeed[0] + "," + cloudSpeed[1];//10
                settingTemp += "|";

                settingTemp += isFogAway;//11
                settingTemp += "|";

                settingTemp += floorScale.x + "," + floorScale.y + "," + floorScale.z;//12
                settingTemp += "|";

                settingTemp += cameraDrawingRange;//13
                settingTemp += "|";

                settingTemp += isShadowOff;//14
                settingTemp += "|";

                settingTemp += isBoundairesAway;//15
                settingTemp += "|";

                settingTemp += IsNightMode;//16
                File.WriteAllText(Application.dataPath + "/Mods/Sky and GroundTexture Mod Setting Tempelate.txt", settingTemp);
                settingTempHasBeenChanged = false;
            }
        }
        void FixedUpdate()
        {
            //Debug.Log(Application.loadedLevel);
            if (AddPiece.isSimulating && !DetectedStartedSimulating && IsNightMode)
            {
                DetectedStartedSimulating = true;
                ItIsNightSoEveryCloudShouldBeDeepDarkFantasy();
            }
            if (!AddPiece.isSimulating) { DetectedStartedSimulating = false; }
            try
            {
                sun.transform.position = GameObject.Find("Directional light").transform.forward * -600 + GameObject.Find("Main Camera").transform.position;
                //GameObject.Find("Directional light").transform.eulerAngles = new Vector3(GameObject.Find("Directional light").transform.eulerAngles.x, 120, GameObject.Find("Directional light").transform.eulerAngles.z);
                GameObject.Find("Directional light").GetComponent<Light>().shadows = LightShadows.Soft;
                GameObject.Find("Directional light").AddComponent<LensFlare>();
                /*if (!GameObject.Find("Sun Sphere"))
                {
                    GameObject sunS = new GameObject();
                    sunS = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sunS.GetComponent<Rigidbody>().isKinematic = false;
                    Destroy(sunS.GetComponent<Collider>());
                    sunS.GetComponent<Renderer>().receiveShadows = false;
                    sunS.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    sunS.name = "Sun Sphere";
                    sunS.transform.localScale = new Vector3(200, 200, 200);
                }

                try
                {
                    sunS.transform.position = GameObject.Find("Directional light").transform.forward * -700 + GameObject.Find("3D Hud Cam").transform.position;
                    Debug.Log("TRIED!");
                    sun.transform.position = GameObject.Find("Directional light").transform.forward * -600 + GameObject.Find("3D Hud Cam").transform.position;
                }
                catch { }*/

                /* WWW flare = new WWW("File:///" + Application.dataPath + "/Mods/50mm Zoom.flare");
                 UnityEngine.Flare flareO = flare.assetBundle.LoadAsset("50mm Zoom");
                 Flare anoFlare = UnityEngine.Object.Instantiate(flareO, GameObject.Find("Directional light").transform.position, GameObject.Find("Directional light").transform.rotation);
                 GameObject.Find("Directional light").AddComponent<Light>().flare = anoFlare;*/



            }
            catch {}
            
                if (tempLevel != Application.loadedLevel) {
                try
                {
                   // GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = Color.cyan;
                    tempLevel = Application.loadedLevel;
                    ReadSetting();
                }
                catch { }
                }

            if (cloudTemp == null) { cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud")); cloudTemp.SetActive(false); }
             /*if (godLightTemp != null) { Debug.Log("RunningG"); godLightTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("GodRays")); godLightTemp.SetActive(false); Debug.Log("GL"); }
             if (rainTemp != null) { Debug.Log("RunningR"); rainTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("Rain Particles")); rainTemp.SetActive(false); Debug.Log("RP"); } 
             if (thunderCloudTemp != null) { Debug.Log("RunningT"); thunderCloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("THUNDER CLOUD")); thunderCloudTemp.SetActive(false); Debug.Log("TC"); } 
            */DontDestroyOnLoad(cloudTemp);
            /*DontDestroyOnLoad(godLightTemp);
            DontDestroyOnLoad(rainTemp);
            DontDestroyOnLoad(thunderCloudTemp);*/

            if (cloudAmountTemp != cloudAmount) { resetCloudsNow = true; clouds[1] = null; cloudAmountTemp = cloudAmount; try { for (int k = cloudAmount; k < clouds.Length; k++) { Destroy(clouds[k].gameObject); Destroy(shadow[k].gameObject); } } catch { } }
            try
            {
                floorScale = GameObject.Find("FloorBig").transform.localScale;
                if (clouds[1] == null && cloudAmount > 1)
                {
                    clouds = new GameObject[cloudAmount];
                    shadow = new GameObject[cloudAmount];
                    for (int i = 0; i <= clouds.Length; i++)
                    {
                        
                        GameObject.DontDestroyOnLoad(clouds[i]);
                        if (i < (int)clouds.Length / 3)
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(higherCloudsMinHeight, higherCloudsMaxHeight), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)), new Quaternion(0, 0, 0, 0));
                            clouds[i].GetComponent<ParticleSystem>().startColor = higherCloudsColor;
                            clouds[i].layer = 12;
                        }
                        else
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(lowerCloudsMinHeight, lowerCloudsMaxHeight), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)), new Quaternion(0, 0, 0, 0));
                            clouds[i].GetComponent<ParticleSystem>().startColor = lowerCloudsColor;
                            clouds[i].layer = 12;
                        }
                        clouds[i].SetActive(true);
                        clouds[i].transform.SetParent(GameObject.Find("Sky and Ground Mod").transform);
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 5;
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = (int)clouds[i].transform.position.y;
                        /*shadow[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        Destroy(shadow[i].GetComponent<Collider>());//.transform.parent = shadow[i].transform;
                        shadow[i].GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly; 
                        shadow[i].layer = clouds[i].layer;
                        shadow[i].transform.position = clouds[i].transform.position;
                        shadow[i].transform.parent = clouds[i].transform;
                        shadow[i].transform.localPosition = new Vector3(0.5f, 0, 0);
                        shadow[i].transform.localEulerAngles = new Vector3(18,10,353);
                        shadow[i].transform.localScale = new Vector3(4,2.5f,2.5f);
                        shadow[i].GetComponent<Renderer>().receiveShadows = true;
                        foreach (Material mtrl in shadow[i].GetComponent<Renderer>().materials) {
                            mtrl.color= new Color(1, 1, 1, 0.3f);
                        }
                        Destroy(shadow[i].GetComponent<Renderer>().material.mainTexture);*/
                        clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)));
                        try
                        {
                          //  clouds[i].GetComponent<ParticleRenderer>().receiveShadows = true;
                            clouds[i].GetComponent<ParticleSystemRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                        catch { Debug.Log("Shadow failed!"); }




                    }
                }
                else
                {
                    foreach (GameObject cloud in clouds)
                    {
                        float randomMove = UnityEngine.Random.Range(0.01f, 0.02f);

                        if (Application.loadedLevel == 2) { cloud.transform.position = new Vector3(-9999, -9999, -9999); }
                        if (CustomCloudSpeed) { cloud.transform.position += new Vector3(cloudSpeed[0], randomMove - 0.015f, cloudSpeed[1]); }
                        else
                        {
                            cloud.transform.position += new Vector3(randomMove, randomMove - 0.015f, randomMove);
                        }
                        cloud.transform.localScale *= 1 + randomMove - 0.015f;
                        cloud.GetComponent<ParticleSystem>().startLifetime = 0.01f;
                        cloud.GetComponent<ParticleSystem>().startSize = cloudSizeScale * 30;
                        cloud.transform.localScale = new Vector3(15 * cloudSizeScale, 15 * cloudSizeScale, 15 * cloudSizeScale);
                        cloud.GetComponent<ParticleSystem>().startLifetime = 5;

                        if (cloud.transform.position.x > floorScale.x / 2 + 200) { cloud.transform.position = new Vector3(-floorScale.x / 2 - 195, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z > floorScale.z / 2 + 200) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, -floorScale.z / 2 - 195); }
                        if (cloud.transform.position.x < -floorScale.x / 2 - 200) { cloud.transform.position = new Vector3(floorScale.x / 2 + 195, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z < -floorScale.z / 2 - 200) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, floorScale.z / 2 + 195); }

                    }
                }
                if (resetCloudsNow)
                {
                    resetCloudsNow = false;
                    foreach (GameObject cloud in Resources.FindObjectsOfTypeAll(typeof(GameObject))) { if (cloud != cloudTemp && cloud.name.Equals("CLoud(Clone)(Clone)")) { Destroy(cloud); } }
                    clouds = new GameObject[cloudAmount];
                }
                //foreach (GameObject oneShadow in shadow) { if (isShadowOff) { oneShadow.transform.localScale = new Vector3(0, 0, 0); } else { oneShadow.transform.localScale = new Vector3(4, 2.5f, 2.5f); } }


            }
            catch { }
        }
     /*   void OnParticleCollision(GameObject other)
        {
            Rigidbody body = other.GetComponent<Rigidbody>();

            Vector3 direction = transform.position - other.transform.position;
            direction = direction.normalized;
           
            this.GetComponent<ParticleAddForce>().particleForce(body.GetRelativePointVelocity(body.transform.position));
            Debug.Log("push!");


        }*/
        void ItIsNightSoEveryCloudShouldBeDeepDarkFantasy()
        {
            for (int i = 0; i <= clouds.Length; i++)
            {
                if (clouds[1] == null && cloudAmount > 1)
                {
                    if (i < (int)clouds.Length / 3)
                    {
                        clouds[i].GetComponent<ParticleSystem>().startColor = higherCloudsColor / 20;
                    }
                    else
                    {
                        clouds[i].GetComponent<ParticleSystem>().startColor = lowerCloudsColor / 20;
                    }
                }
            }
            /*foreach (ParticleSystemRenderer f in FindObjectsOfType<ParticleSystemRenderer>())
            {
                try {
                    if (f.GetComponent<ParticleSystemRenderer>().material.name.Contains("Flame"))
                    {
                        f.gameObject.AddComponent<ShakeLightingWholalalalalalallalala>();
                    }
                } catch { }
            }*/
            foreach (FireController f in FindObjectsOfType<FireController>())
            {
                if (!f.GetComponent<ShakeLightingWholalalalalalallalala>())
                {
                    f.gameObject.AddComponent<ShakeLightingWholalalalalalallalala>();
                }
            }
            
          }
        void ItIsDaySoEveryCloudShouldBeBright()
        {
            for (int i = 0; i <= clouds.Length; i++)
            {
                if (clouds[1] == null && cloudAmount > 1)
                {
                    if (i < (int)clouds.Length / 3)
                    {
                        clouds[i].GetComponent<ParticleSystem>().startColor = higherCloudsColor;
                    }
                    else
                    {
                        clouds[i].GetComponent<ParticleSystem>().startColor = lowerCloudsColor;
                    }
                }
            }
            }
        void ReadSetting()
        {
            if (File.Exists(Application.dataPath + "/Mods/Sky and GroundTexture Mod Setting Tempelate.txt") && new FileInfo((Application.dataPath + "/Mods/Sky and GroundTexture Mod Setting Tempelate.txt")).Length >= 5)
            {
                settingTemp = File.ReadAllText(Application.dataPath + "/Mods/Sky and GroundTexture Mod Setting Tempelate.txt");
                //Debug.Log(settingTemp);
                Settings = settingTemp.Trim().Split('|');
                try
                {
                    cloudAmount = int.Parse(Settings[0]);

                    cloudSizeScale = int.Parse(Settings[1]);

                    lowerCloudsMinHeight = float.Parse(Settings[2]);
                    lowerCloudsMaxHeight = float.Parse(Settings[3]);
                    higherCloudsMinHeight = float.Parse(Settings[4]);
                    higherCloudsMinHeight = float.Parse(Settings[5]);

                    higherCloudsColor = new Color(float.Parse(Settings[6].Split(',')[0]), float.Parse(Settings[6].Split(',')[1]), float.Parse(Settings[6].Split(',')[2]), float.Parse(Settings[6].Split(',')[3]));
                    lowerCloudsColor = new Color(float.Parse(Settings[7].Split(',')[0]), float.Parse(Settings[7].Split(',')[1]), float.Parse(Settings[7].Split(',')[2]), float.Parse(Settings[7].Split(',')[3]));

                    SkyColor = new Color(int.Parse(Settings[8].Split(',')[0]), int.Parse(Settings[8].Split(',')[1]), int.Parse(Settings[8].Split(',')[2]), int.Parse(Settings[8].Split(',')[3]));

                    CustomCloudSpeed = bool.Parse(Settings[9]);
                    if (CustomCloudSpeed)
                    {
                        cloudSpeed[0] = float.Parse(Settings[10].Split(',')[0]);
                        cloudSpeed[1] = float.Parse(Settings[10].Split(',')[1]);
                    }

                    isFogAway = bool.Parse(Settings[11]);
                    if (isFogAway)
                    {
                        try { GameObject.Find("Fog Volume").transform.position = new Vector3(0, Mathf.Infinity, 0); } catch { }
                    }

                    floorScale = new Vector3(float.Parse(Settings[12].Split(',')[0]), float.Parse(Settings[12].Split(',')[1]), float.Parse(Settings[12].Split(',')[2]));
                    try { GameObject.Find("FloorBig").transform.localScale = new Vector3(floorScale[0], floorScale[1], floorScale[2]); } catch { }

                    cameraDrawingRange = float.Parse(Settings[13]);

                    isShadowOff = bool.Parse(Settings[14]);
                    if (isShadowOff)
                    {
                        try
                        {
                            foreach (GameObject shadowMaker in shadow)
                            {
                                shadowMaker.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                            }
                        }
                        catch { }
                    }
                    isBoundairesAway = bool.Parse(Settings[15]);
                    if (isBoundairesAway)
                    {
                        try { GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0); } catch { }
                    }
                    IsNightMode = bool.Parse(Settings[16].Trim());
                    if (IsNightMode)
                    {
                        try
                        {
                            GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = new Color(0.1f, 0.1f, 0.1f);
                            GameObject.Find("Directional light").GetComponent<Light>().intensity = 0f;
                            GameObject.Find("Directional light").AddComponent<LensFlare>();
                            GameObject.Find("Directional light").GetComponent<LensFlare>().brightness = 10;
                            GameObject.Find("Directional light").GetComponent<LensFlare>().color = Color.white;
                            GameObject.Find("Directional light").GetComponent<LensFlare>().enabled = true;
                            GameObject.Find("Directional light").GetComponent<LensFlare>().fadeSpeed = 1;
                            ItIsNightSoEveryCloudShouldBeDeepDarkFantasy();
                        }
                        catch { }
                    }
                    Debug.Log("Finished Loading All Settings!");
                }
                catch { Debug.Log("Your Setting Tempelate cannot be read!"); }
            }
            else
            {
                File.Create(Application.dataPath + "/Mods/Sky and GroundTexture Mod Setting Tempelate.txt");
            }//读取设定

            /*    Commands.RegisterCommand("TryFloatingStones", (args, notUses) =>
                {

                    try { AsyncOperation poop = Application.LoadLevelAsync(22); return poop + "Level will be load."; } catch { return "The Level cannot be load!"; }

                }, "Try load floating stones");//Try load Floating Stones*/


        }
    }
    public class ShakeLightingWholalalalalalallalala : MonoBehaviour
    {
        public float FireLightIntensity;
        public float FireLightRange;
        public float Shakefrequency;
        public float ShakeMax;
        public float ShakeMin;
        public Color FireColor;
        public Light ThisLight;
        public bool ShakeTheLight = true;
    //    private float lastFUTime;
        /*private int FUcount;*/

        void Start ()
        {
            //Pre-Setting
            FireColor = new Color(0.9f,0.6f,0);
            ShakeMax = 1.3f;
            ShakeMin = 0.9f;
            FireLightIntensity = 2f;
            FireLightRange = 7f;

            float ScaleMag = Vector3.Magnitude(this.transform.localScale);

            if (!this.gameObject.GetComponent<Light>())
            {
                ThisLight = this.gameObject.AddComponent<Light>();
                ThisLight.type = LightType.Point;
                ThisLight.color = FireColor;
                ThisLight.intensity = FireLightIntensity * ScaleMag;
                ThisLight.range = FireLightRange * ScaleMag;
                ThisLight.enabled = false;
                /*lastFUTime = this.GetComponent<ParticleSystem>().time;*/
            }
            else 
            {
                DestroyImmediate(this.GetComponent<ShakeLightingWholalalalalalallalala>());
            }
        }

        /*void FixedUpdate()//弃用方案
        {
            int Burning = 1;
            float ScaleMag = Vector3.Magnitude(this.transform.localScale);
             if (this.GetComponent<ParticleSystem>() && FUcount % 2 ==0 )
            {
               if (this.GetComponent<ParticleSystem>().time == lastFUTime) { Burning--; }
                lastFUTime = this.GetComponent<ParticleSystem>().time;
            }
            else {
                Destroy(this.GetComponent<ShakeLightingWholalalalalalallalala>());
                    }
            if (ShakeTheLight)
            {
                ThisLight.intensity = FireLightIntensity * ScaleMag * (UnityEngine.Random.value * (ShakeMax-ShakeMin) + ShakeMin) * Burning;
                ThisLight.range = FireLightRange * ScaleMag * (UnityEngine.Random.value * (ShakeMax - ShakeMin) + ShakeMin);
                ThisLight.color = FireColor;
            }
            }
        }*/
        void FixedUpdate()
        {
            float ScaleMag = Vector3.Magnitude(this.transform.localScale);
             if (this.GetComponent<FireController>())
            {
                ThisLight.enabled = this.GetComponent<FireController>().onFire;
            }
            else {
                Destroy(this.GetComponent<ShakeLightingWholalalalalalallalala>());
                    }
            if (ShakeTheLight)
            {
                ThisLight.intensity = FireLightIntensity /* ScaleMag */;
                ThisLight.range = FireLightRange /* ScaleMag */ * (UnityEngine.Random.value * (ShakeMax - ShakeMin) + ShakeMin);
                ThisLight.color = FireColor;
            }
            }
        }
    }
