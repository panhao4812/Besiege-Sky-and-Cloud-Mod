//using System.Threading.Tasks;
using spaar;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class Scene : MonoBehaviour
    {
        private GameObject[] clouds;
        private Vector3[] axis;
        private GameObject[] meshes;
        private GameObject[] Mwater;
        private GameObject[] meshtriggers;

        private GameObject cloudTemp;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 cloudScale = new Vector3(1000, 200, 1000);
        private Vector3 waterScale = new Vector3(0, 1, 0);
        private Vector3 MwaterScale = new Vector3(30, 1, 30);
        private Vector3 waterLocation = new Vector3(0, 0, 0);
        private Quaternion waterRotation = new Quaternion(0, 0, 0, 1);

        private int MeshSize = 0;
        private int CloudSize = 0;
        private int WaterSize = 0;
        private int TriggerSize = 0;

        private AssetBundle iteratorVariable1;
        private bool isSimulating = false;

        //UI
        public string DefaultSceneName = "SteelHill";
        private int _FontSize = 15;
        private Rect windowRect = new Rect(15f, Screen.height - 95f, 150f, 50f);
        private int windowID = spaar.ModLoader.Util.GetWindowID();
        private bool ShowGUI = true;
        List<string> _ButtonName = new List<string>();
        List<string> _SceneName = new List<string>();
        string _FloorBig = "[Default]";
        KeyCode _ReLoadScene = KeyCode.F7;
        KeyCode _RetrunToFloorBig = KeyCode.F10;
        KeyCode _DisplayUI = KeyCode.F6;
        KeyCode _ReloadUI = KeyCode.F5;
        void DefaultUI()
        {
            _ButtonName.Clear(); _SceneName.Clear(); _FloorBig = "[Default]";
            _FontSize = 15;
            ShowGUI = true;
            windowRect = new Rect(15f, Screen.height - 95f, 150f, 50f);
            _ReLoadScene = KeyCode.F7;
            _RetrunToFloorBig = KeyCode.F10;
            _DisplayUI = KeyCode.F6;
            _ReloadUI = KeyCode.F5;
        }
        void ReadUI()
        {
            DefaultUI();
            try
            {
                StreamReader srd;
                string Ci = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                if (Ci == "zh-CN")
                {
                    srd = File.OpenText(Application.dataPath + "/Mods/Blocks/UI/CHN.txt");
                }
                else
                {
                    srd = File.OpenText(Application.dataPath + "/Mods/Blocks/UI/EN.txt");
                }
                Debug.Log(Ci + "  " + Screen.width.ToString() + "*" + Screen.height.ToString());
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == Screen.width.ToString() + "*" + Screen.height.ToString() + "_Scene")
                        {
                            if (chara[1] == "fontsize")
                            {
                                _FontSize = Convert.ToInt32(chara[2]);
                            }
                            else if (chara[1] == "window_poistion")
                            {
                                windowRect.x = Convert.ToSingle(chara[2]);
                                windowRect.y = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[1] == "window_scale")
                            {
                                windowRect.width = Convert.ToSingle(chara[2]);
                                windowRect.height = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0") ShowGUI = false;
                                else ShowGUI = true;
                            }
                            else if (chara[1] == "buttonname")
                            {
                                _ButtonName.Add(chara[2]);
                            }
                            else if (chara[1] == "scenename")
                            {
                                _SceneName.Add(chara[2]);
                            }
                            else if (chara[1] == "reLoad_scene")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _ReLoadScene = outputkey;
                                // Debug.Log("_ReLoadScene:" + _ReLoadScene.ToString());
                            }
                            else if (chara[1] == "retrun_to_FloorBig")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _RetrunToFloorBig = outputkey;
                            }
                            else if (chara[1] == "display_UI")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _DisplayUI = outputkey;
                            }
                            else if (chara[1] == "reload_UI")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _ReloadUI = outputkey;
                            }
                        }
                    }
                }
                srd.Close();
                if (_ButtonName.Count != _SceneName.Count || _ButtonName.Count < 0)
                {
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadUISetting Failed!Button Error!");
                    _ButtonName.Clear(); _SceneName.Clear();
                }
                else
                {
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadUISetting Completed!");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadUISetting Failed!");
                Debug.Log(ex.ToString());
                DefaultUI();
                return;
            }
        }
        void LoadTrigger()
        {
            try
            {
                ClearTrigger();
                if (TriggerSize > 100) TriggerSize = 100;
                if (TriggerSize < 0) TriggerSize = 0;
                if (TriggerSize > 0)
                {
                    TimeUI.Triggers = new bool[TriggerSize];
                    meshtriggers = new GameObject[TriggerSize];
                    for (int i = 0; i < meshtriggers.Length; i++)
                    {
                        meshtriggers[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        meshtriggers[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshtriggers[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshtriggers[i].AddComponent<MTrigger>();
                        meshtriggers[i].GetComponent<MTrigger>().Index = i;
                        meshtriggers[i].name = "_meshtrigger" + i.ToString();
                        TimeUI.Triggers[i] = false;
                    }
                    TimeUI.TriggersIndex = -1;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("LoadTrigger Failed!");
                Debug.Log(ex.ToString());
            }
        }
        void ClearTrigger()
        {
            if (meshtriggers == null) return;
            if (meshtriggers.Length <= 0) return;
            Debug.Log("ClearMeshTriggers");
            TimeUI.Triggers = null;
            TimeUI.TriggersIndex = -2;
            for (int i = 0; i < meshtriggers.Length; i++)
            {
                Destroy(meshtriggers[i]);
            }
        }
        void LoadFloater()
        {
            try
            {
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();
                foreach (MyBlockInfo info in infoArray)
                {
                    if (info.gameObject.GetComponent<Floater>() == null)
                    {
                        info.gameObject.AddComponent<Floater>();
                    }
                }
            }
            catch
            {
                Debug.Log("LoadFloater Failed");
            }
        }
        void LoadScene(string SceneName)
        {
            try
            {
                Debug.Log(Application.dataPath);
                StreamReader srd;
                try
                {
                    srd = File.OpenText(Application.dataPath + "/Mods/Blocks/Scene/" + SceneName + ".txt");
                    while (srd.Peek() != -1)
                    {
                        string str = srd.ReadLine();
                        string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (chara.Length > 2)
                        {
                            if (chara[0] == "Meshes")
                            {
                                if (chara[1] == "size")
                                {
                                    this.MeshSize = Convert.ToInt32(chara[2]);
                                    LoadMesh();
                                }
                            }
                            else if (chara[0] == "Mesh")
                            {
                                int i = Convert.ToInt32(chara[1]);
                                if (chara[2] == "mesh")
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                                }
                                if (chara[2] == "wmesh")
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                                }
                                if (chara[2] == "emesh")
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.EMeshFromObj(chara[3]);
                                }
                                if (chara[2] == "heightmapmesh")
                                {
                                    Mesh mesh = GeoTools.LoadHeightMap(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToInt32(chara[5]),
                                    Convert.ToInt32(chara[6]),
                                    Convert.ToInt32(chara[7]),
                                    Convert.ToSingle(chara[8]),
                                    chara[9]);
                                    meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                                }
                                if (chara[2] == "plannarmesh")
                                {
                                    Mesh mesh = GeoTools.MeshFromPoints(
                                    Convert.ToInt32(chara[3]),
                                    Convert.ToInt32(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]));
                                    meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                                }
                                else if (chara[2] == "stexture")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                                }
                                else if (chara[2] == "materialcopy")
                                {
                                    try
                                    {
                                        meshes[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().material);
                                        Debug.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Log("MaterialCopy Failed");
                                        Debug.Log(ex.ToString());
                                    }
                                }

                                else if (chara[2] == "shader")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[3]);
                                }
                                else if (chara[2] == "texture")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                                }
                                else if (chara[2] == "setcolor")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetColor(chara[3], new Color(
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]),
                                    Convert.ToSingle(chara[7])));
                                }
                                else if (chara[2] == "setvector")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetVector(chara[3], new Vector4(
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]),
                                    Convert.ToSingle(chara[7])));
                                }
                                else if (chara[2] == "setfloat")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetFloat(chara[3], Convert.ToSingle(chara[4]));
                                }
                                else if (chara[2] == "meshcollider")
                                {
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);

                                }
                                else if (chara[2] == "wmeshcollider")
                                {
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);

                                }
                                else if (chara[2] == "dynamicFriction")
                                {
                                    float t = Convert.ToSingle(chara[3]);
                                    if (t < 0) t = 0;
                                    if (t > 1) t = 1;
                                    meshes[i].GetComponent<MeshCollider>().material.dynamicFriction = t;
                                }
                                else if (chara[2] == "staticFriction")
                                {
                                    float t = Convert.ToSingle(chara[3]);
                                    if (t < 0) t = 0;
                                    if (t > 1) t = 1;
                                    meshes[i].GetComponent<MeshCollider>().material.staticFriction = t;
                                }
                                else if (chara[2] == "bounciness")
                                {
                                    float t = Convert.ToSingle(chara[3]);
                                    if (t < 0) t = 0;
                                    if (t > 1) t = 1;
                                    meshes[i].GetComponent<MeshCollider>().material.bounciness = t;
                                }
                                else if (chara[2] == "frictionCombine ")
                                {
                                    if (chara[3] == "Average" || chara[3] == "average")
                                    { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Average; }
                                    else if (chara[3] == "Multiply" || chara[3] == "multiply")
                                    { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Multiply; }
                                    else if (chara[3] == "Minimum" || chara[3] == "minimum")
                                    { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum; }
                                    else if (chara[3] == "Maximum" || chara[3] == "maximum")
                                    { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Maximum; }
                                }
                                else if (chara[2] == "bounceCombine  ")
                                {
                                    if (chara[3] == "Average" || chara[3] == "average")
                                    { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Average; }
                                    else if (chara[3] == "Multiply" || chara[3] == "multiply")
                                    { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Multiply; }
                                    else if (chara[3] == "Minimum" || chara[3] == "minimum")
                                    { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Minimum; }
                                    else if (chara[3] == "Maximum" || chara[3] == "maximum")
                                    { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Maximum; }
                                }
                                else if (chara[2] == "location")
                                {
                                    meshes[i].transform.localPosition = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                                    //Debug.Log("meshes" + i.ToString() + ".loaction:" + meshes[i].transform.localPosition.ToString());
                                }
                                else if (chara[2] == "scale")
                                {
                                    meshes[i].transform.localScale = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                                    //Debug.Log("meshes" + i.ToString() + ".scale:" + meshes[i].transform.localScale.ToString());
                                }
                                else if (chara[2] == "rotation")
                                {
                                    meshes[i].transform.rotation = new Quaternion(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]));
                                }
                                else if (chara[2] == "eulerangles")
                                {
                                    meshes[i].transform.Rotate(new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5])), Space.Self);
                                }
                                else if (chara[2] == "euleranglesworld")
                                {
                                    meshes[i].transform.Rotate(new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5])), Space.World);
                                }
                                else if (chara[2] == "fromtorotation")
                                {
                                    meshes[i].transform.rotation = Quaternion.FromToRotation(
                                  new Vector3(Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5])),
                                    new Vector3(Convert.ToSingle(chara[6]),
                                    Convert.ToSingle(chara[7]),
                                    Convert.ToSingle(chara[8]))
                                    );
                                }
                            }
                            else if (chara[0] == "Triggers")
                            {
                                if (chara[1] == "size")
                                {
                                    this.TriggerSize = Convert.ToInt32(chara[2]);
                                    LoadTrigger();
                                }
                            }
                            else if (chara[0] == "Trigger")
                            {
                                int i = Convert.ToInt32(chara[1]);
                                if (chara[2] == "mesh")
                                {
                                    meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                                }
                                if (chara[2] == "wmesh")
                                {
                                    meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                                }
                                if (chara[2] == "scale")
                                {
                                    meshtriggers[i].transform.localScale = new Vector3(
                                   Convert.ToSingle(chara[3]),
                                   Convert.ToSingle(chara[4]),
                                   Convert.ToSingle(chara[5]));
                                }
                                if (chara[2] == "location")
                                {
                                    meshtriggers[i].transform.localPosition = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                                }
                                else if (chara[2] == "stexture")
                                {
                                    meshtriggers[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                                }
                                else if (chara[2] == "shader")
                                {
                                    meshtriggers[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
                                }
                                else if (chara[2] == "texture")
                                {
                                    meshtriggers[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                                }
                                else if (chara[2] == "color")
                                {
                                    meshtriggers[i].GetComponent<MeshRenderer>().material.color = new Color(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]));
                                }
                                else if (chara[2] == "meshcollider")
                                {
                                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
                                    meshtriggers[i].GetComponent<MeshCollider>().convex = true;
                                    meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
                                }
                                else if (chara[2] == "wmeshcollider")
                                {
                                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
                                    meshtriggers[i].GetComponent<MeshCollider>().convex = true;
                                    meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
                                }
                                else if (chara[2] == "rotation")
                                {
                                    meshes[i].transform.rotation = new Quaternion(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]));
                                }
                                else if (chara[2] == "fromtorotation")
                                {
                                    meshes[i].transform.rotation = Quaternion.FromToRotation(
                                  new Vector3(Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5])),
                                    new Vector3(Convert.ToSingle(chara[6]),
                                    Convert.ToSingle(chara[7]),
                                    Convert.ToSingle(chara[8]))
                                    );
                                }
                                else if (chara[2] == "eulerangles")
                                {
                                    meshes[i].transform.Rotate(new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5])), Space.Self);
                                }
                                else if (chara[2] == "euleranglesworld")
                                {
                                    meshes[i].transform.Rotate(new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5])), Space.World);
                                }
                                ///////////////////////////////////////////////
                            }
                            else if (chara[0] == "Cloud")
                            {

                                if (chara[1] == "size")
                                {
                                    this.CloudSize = Convert.ToInt32(chara[2]);
                                    LoadCloud();
                                }
                                else if (chara[1] == "floorScale" || chara[1] == "cloudScale")
                                {
                                    cloudScale = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "location")
                                {
                                    this.transform.localPosition = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "color")
                                {
                                    this.CloudsColor = new Color(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                                }
                            }
                            else if (chara[0] == "Camera")
                            {
                                if (chara[1] == "farClipPlane")
                                {
                                    GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = Convert.ToInt32(chara[2]);
                                }
                            }
                            else if (chara[0] == "Water")
                            {
                                if (chara[1] == "size")
                                {
                                    this.WaterSize = Convert.ToInt32(chara[2]);
                                    LoadWater();
                                }
                                else if (chara[1] == "scale")
                                {
                                    waterScale = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "mscale")
                                {
                                    MwaterScale = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "location")
                                {
                                    waterLocation = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "roation")
                                {
                                    waterRotation = new Quaternion(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Scene File error!");
                    Debug.Log(ex.ToString());
                    return;
                }
                srd.Close();
                GeoTools.HideFloorBig();
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadScene Completed!");
            }
            catch (System.Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadScene Failed!");
                Debug.Log(ex.ToString());
                return;
            }

        }
        void LoadCloud()
        {
            try
            {
                ClearCloud();
                if (cloudTemp == null) return;
                if (CloudSize < 0) CloudSize = 0;
                if (CloudSize > 1000) CloudSize = 1000;
                if (CloudSize == 0) { return; }
                else
                {
                    clouds = new GameObject[CloudSize];
                    axis = new Vector3[CloudSize];
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(
                            UnityEngine.Random.Range(-cloudScale.x + transform.localPosition.x, cloudScale.x + transform.localPosition.x),
                            UnityEngine.Random.Range(transform.localPosition.y, cloudScale.y + transform.localPosition.y),
                            UnityEngine.Random.Range(-cloudScale.z + transform.localPosition.z, cloudScale.z + transform.localPosition.z)),
                            new Quaternion(0, 0, 0, 0));
                        clouds[i].transform.SetParent(this.transform);
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].SetActive(true);
                        clouds[i].GetComponent<ParticleSystem>().startColor = CloudsColor;
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 6;
                        clouds[i].GetComponent<ParticleSystem>().startSpeed = 1.6f;
                        clouds[i].GetComponent<ParticleSystem>().emissionRate = 3;
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = 18;
                        axis[i] = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 1f, UnityEngine.Random.Range(-0.1f, 0.1f));
                    }
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==> Load Cloud Successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==> Load Cloud Failed");
                Debug.Log(ex.ToString());
                ClearCloud();
            }
        }
        void LoadMesh()
        {
            try
            {
                ClearMeshes();
                if (MeshSize > 100) MeshSize = 100;
                if (MeshSize < 0) MeshSize = 0;
                if (MeshSize > 0)
                {
                    meshes = new GameObject[MeshSize];
                    for (int i = 0; i < meshes.Length; i++)
                    {
                        meshes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        meshes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshes[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshes[i].name = "_mesh" + i.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("ResetMesh Failed!");
                Debug.Log(ex.ToString());
            }
        }
        void LoadWater()
        {
            try
            {
                ClearFloater();
                ClearWater();
                if (WaterSize <= 0) return;
                if (waterScale.x < 0) waterScale.x = 0;
                if (waterScale.x > 9) waterScale.x = 9;
                if (waterScale.z < 0) waterScale.z = 0;
                if (waterScale.z > 9) waterScale.z = 9;
                Mwater = new GameObject[((int)waterScale.x * 2 + 1) * ((int)waterScale.z * 2 + 1)];
                Mwater[0] = (GameObject)Instantiate(iteratorVariable1.LoadAsset("water4example (advanced)"), waterLocation, new Quaternion());
                Mwater[0].name = "water0";
                GeoTools.ResetWaterMaterial(ref Mwater[0].GetComponent<WaterBase>().sharedMaterial);
                Mwater[0].transform.localScale = MwaterScale;
                int index = 1;
                for (float k = -waterScale.x; k <= waterScale.x; k++)
                {
                    for (float i = -waterScale.z; i <= waterScale.z; i++)
                    {
                        if ((k != 0) || (i != 0))
                        {
                            Mwater[index] = Instantiate(Mwater[0]);
                            Mwater[index].name = "water" + index.ToString();
                            Mwater[index].transform.position = new Vector3((float)(k * 50 * Mwater[0].transform.localScale.x) + waterLocation.x,
                                waterLocation.y, (float)(i * 50 * Mwater[0].transform.localScale.z) + waterLocation.z);
                            index++;
                        }
                    }
                }
                if (Mwater.Length == 1) Mwater[0].transform.localRotation = this.waterRotation;
            }
            catch (Exception ex)
            {
                Debug.Log("Assets Failed");
                Debug.Log(ex.ToString());
            }
        }
        void ClearWater()
        {
            if (Mwater == null) return;
            if (Mwater.Length <= 0) return;
            Debug.Log("ClearWater");
            for (int i = 0; i < Mwater.Length; i++)
            {
                Destroy(GameObject.Find("water" + i.ToString() + "ReflectionMain Camera"));
                Destroy(Mwater[i]);
            }
        }
        void ClearCloud()
        {
            CloudsColor = new Color(1f, 1f, 1f, 1);
            if (clouds == null) return;
            if (clouds.Length <= 0) return;
            Debug.Log("ClearCloud");
            for (int i = 0; i < clouds.Length; i++)
            {
                Destroy(clouds[i]);
            }
        }
        void ClearMeshes()
        {
            if (meshes == null) return;
            if (meshes.Length <= 0) return;
            Debug.Log("ClearMeshes");
            for (int i = 0; i < meshes.Length; i++)
            {
                Destroy(meshes[i]);
            }

        }
        void ClearResource()
        {
            UnityEngine.Object.Destroy(this.cloudTemp);
            ClearWater();
            ClearCloud();
            ClearMeshes();
        }
        void ClearFloater()
        {
            try
            {
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();
                foreach (MyBlockInfo info in infoArray)
                {
                    if (info.gameObject.GetComponent<Floater>() != null)
                    {
                        Destroy(info.gameObject.GetComponent<Floater>());
                    }
                }
            }
            catch
            {
                Debug.Log("LoadFloater Failed");
            }
        }
        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        void FixedUpdate()
        {
            if (cloudTemp == null)
            {
                cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud"));
                cloudTemp.SetActive(false);
                DontDestroyOnLoad(cloudTemp);
                Debug.Log("Besiege_Sky_and_Cloud_Mod==> Get Cloud Temp Successfully");
            }
            if (clouds != null)
            {
                if (clouds[0] != null)
                {
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i].transform.RotateAround(this.transform.localPosition, axis[i], Time.deltaTime);
                        clouds[i].GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(30, 200);
                    }
                }
            }
            if (AddPiece.isSimulating && isSimulating == false)
            {
                if (Mwater != null) { if (Mwater[0] != null) LoadFloater(); }
                isSimulating = true;
            }
            else if (!AddPiece.isSimulating && isSimulating == true)
            {
                isSimulating = false;
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(_ReloadUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ReadUI();
            }
            if (Input.GetKeyDown(_ReLoadScene) && Input.GetKey(KeyCode.LeftControl))
            {
                ReadUI();
                LoadScene(DefaultSceneName);
            }
            if (Input.GetKeyDown(_RetrunToFloorBig) && Input.GetKey(KeyCode.LeftControl))
            {
                this.transform.localPosition = new Vector3(0, 500, 0);
                ClearWater();
                ClearCloud();
                ClearMeshes();
                ClearFloater();
                GeoTools.UnhideFloorBig();
            }
            if (Input.GetKeyDown(_DisplayUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }
        }
        void Start()
        {
            ReadUI();
            this.transform.localPosition = new Vector3(0, 500, 0);
            WWW iteratorVariable0 = new WWW("file:///" + Application.dataPath + "/Mods/Blocks/Shader/Water.unity3d.dll");
            iteratorVariable1 = iteratorVariable0.assetBundle;
            /* string[] names = iteratorVariable1.GetAllAssetNames();
             for (int i = 0; i < names.Length; i++)
             {
                 Debug.Log(names[i]);
             }*/
            Commands.RegisterCommand("DefaultSceneName", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    this.DefaultSceneName = args[0];
                    LoadScene(DefaultSceneName);
                }
                catch
                {
                    return "Besiege_Sky_and_Cloud_Mod==> DefaultSceneName Failed";
                }
                return "Besiege_Sky_and_Cloud_Mod==> DefaultSceneName Succeeded";
            }, "LoadScene DefaultSceneName");
        }
        void OnDisable()
        {
            ClearResource();
        }
        void OnDestroy()
        {
            ClearResource();
        }
        ////////////////////////////////////////////////////
        public void DoWindow(int windowID)
        {
            GUIStyle style = new GUIStyle
            {
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                active = { background = Texture2D.whiteTexture, textColor = Color.black },
                margin = { top = 5 },
                fontSize = _FontSize
            };
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("MouseDrag", style, new GUILayoutOption[0]);
            for (int i = 0; i < _SceneName.Count; i++)
            {
                if (GUILayout.Button(_ButtonName[i], style, new GUILayoutOption[0]) && !AddPiece.isSimulating)
                { DefaultSceneName = _SceneName[i]; LoadScene(DefaultSceneName); }
            }
            if (GUILayout.Button(_FloorBig, style, new GUILayoutOption[0]) && !AddPiece.isSimulating)
            {
                this.transform.localPosition = new Vector3(0, 500, 0);
                ClearWater();
                ClearCloud();
                ClearMeshes();
                ClearFloater();
                GeoTools.UnhideFloorBig();
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }
        private void OnGUI()
        {
            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);
            }
        }
    }

}