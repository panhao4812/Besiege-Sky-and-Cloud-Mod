//using System.Threading.Tasks;
using spaar;
using spaar.ModLoader.UI;
using System;
using System.Collections;
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

        private GameObject cloudTemp;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 cloudScale = new Vector3(1000, 200, 1000);
        private Vector3 waterScale = new Vector3(2, 1, 2);
        private Vector3 waterLocation = new Vector3(0, 0, 0);

        private int MeshSize = 0;
        private int CloudSize = 0;
        private int WaterSize = 0;
        public string DefaultSceneName = "Ocean";
        AssetBundle iteratorVariable1;
        bool isSimulating = false; public bool ShowGUI = false;
        private Rect windowRect = new Rect(5f, 55f, 900f, 50f);
        private int windowID = spaar.ModLoader.Util.GetWindowID();

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
                                    Convert.ToInt32(chara[4]));
                                    meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                                }
                                else if (chara[2] == "stexture")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                                }
                                else if (chara[2] == "texture")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                                }
                                else if (chara[2] == "color")
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.color = new Color(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]));
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
                                    // Debug.Log("meshes" + i.ToString() + ".scale:" + meshes[i].transform.localScale.ToString());
                                }
                                else if (chara[2] == "rotaion")
                                {
                                    meshes[i].transform.localRotation = new Quaternion(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]),
                                    Convert.ToSingle(chara[6]));
                                }
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
                                else if (chara[1] == "location")
                                {
                                    waterLocation = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
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
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadScene Completed!");
                srd.Close();

            }
            catch (System.Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>LoadScene Failed!");
                Debug.Log(ex.ToString());
                return;
            }
            GeoTools.HideFloorBig();
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
                if (WaterSize == 0) return;
                Mwater = new GameObject[((int)waterScale.x*2+1) * ((int)waterScale.z*2+1)];
                Mwater[0] = (GameObject)Instantiate(iteratorVariable1.LoadAsset("water4example (advanced)"), waterLocation, new Quaternion());
                Mwater[0].name = "water0";
                Mwater[0].transform.localScale = new Vector3(1f, 1f, 1f);
                int index = 1;
                for (float k = -waterScale.x; k <= waterScale.x; k++)
                {
                    for (float i = -waterScale.z; i <= waterScale.z; i++)
                    {
                        if ((k != 0) || (i != 0))
                        {
                            Mwater[index] = Instantiate(Mwater[0]);
                            Mwater[index].name = "water" + index.ToString();
                            Mwater[index].transform.position = new Vector3((float)(k * 50) + waterLocation.x, waterLocation.y, (float)(i * 50) + waterLocation.z);     
                            index++;
                        }
                    }
                }

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
                Destroy(GameObject.Find("water"+i.ToString()+"ReflectionMain Camera"));
                Destroy(Mwater[i]);
            }
        }
        void ClearCloud()
        {
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
                Debug.Log(": Besiege_Sky_and_Cloud_Mod==> Get Cloud Temp Successfully");
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
                if (Mwater != null) { LoadFloater(); }
                isSimulating = true;
            }
            else if (!AddPiece.isSimulating && isSimulating == true)
            {
                isSimulating = false;
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7) && Input.GetKey(KeyCode.LeftControl))
            {
                LoadScene(DefaultSceneName);
            }
            if (Input.GetKeyDown(KeyCode.F10) && Input.GetKey(KeyCode.LeftControl))
            {
                this.transform.localPosition = new Vector3(0, 500, 0);
                ClearWater();
                ClearCloud();
                ClearMeshes();
                ClearFloater();
                GeoTools.UnhideFloorBig();
            }
            if (Input.GetKeyDown(KeyCode.F6) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }
        }
        void Start()
        {
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
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("GreenHill", new GUILayoutOption[0])) { DefaultSceneName = "GreenHill"; LoadScene(DefaultSceneName); }
            if (GUILayout.Button("SteelHillProtoType", new GUILayoutOption[0])) { DefaultSceneName = "SteelHillProtoType"; LoadScene(DefaultSceneName); }
            if (GUILayout.Button("SteelHill", new GUILayoutOption[0])) { DefaultSceneName = "SteelHill"; LoadScene(DefaultSceneName); }
            if (GUILayout.Button("Independence", new GUILayoutOption[0])) { DefaultSceneName = "Independence"; LoadScene(DefaultSceneName); }
            if (GUILayout.Button("Ocean", new GUILayoutOption[0])) { DefaultSceneName = "Ocean"; LoadScene(DefaultSceneName); }
            if (GUILayout.Button("HeightMap", new GUILayoutOption[0])) { DefaultSceneName = "HeightMap"; LoadScene(DefaultSceneName); }
            if (GUILayout.Button("Plannar", new GUILayoutOption[0])) { DefaultSceneName = "Plannar"; LoadScene(DefaultSceneName); }
            GUILayout.EndHorizontal();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }
        private void OnGUI()
        {         
            GUI.skin = ModGUI.Skin;
            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);
            }
        }
    }

}