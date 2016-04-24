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
        private GameObject[] meshes;
        private Vector3[] axis;
        private GameObject cloudTemp;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 floorScale = new Vector3(1000, 200, 1000);
        private int MeshSize = 0;
        private int CloudSize = 0;
        private GameObject Mwater = null;
        private int WaterSize = 0;
        public string DefaultSceneName = "Independence";
        AssetBundle iteratorVariable1;
        void LoadFloater()
        {
            try
            {
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();                 
                foreach (MyBlockInfo info in infoArray)
                {
                   if( info.gameObject.GetComponent<Floater>()==null){
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
                       Destroy( info.gameObject.GetComponent<Floater>());
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
                                    Mesh mesh =GeoTools.LoadHeightMap(
                                     Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToInt32(chara[5]),
                                    Convert.ToInt32(chara[6]),
                                     Convert.ToInt32(chara[7]),
                                    Convert.ToSingle(chara[8]),
                                    chara[9]);
                                    meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                     meshes[i].GetComponent<MeshCollider>().sharedMesh=mesh;
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
                                else if (chara[1] == "floorScale")
                                {
                                    floorScale = new Vector3(
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
                                    Mwater.transform.localScale = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "location")
                                {
                                    Mwater.transform.localPosition = new Vector3(
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
                            UnityEngine.Random.Range(-floorScale.x + transform.localPosition.x, floorScale.x + transform.localPosition.x),
                            UnityEngine.Random.Range(transform.localPosition.y, floorScale.y + transform.localPosition.y),
                            UnityEngine.Random.Range(-floorScale.z + transform.localPosition.z, floorScale.z + transform.localPosition.z)),
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
                        axis[i] = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 1f, UnityEngine.Random.Range(-0.5f, 0.5f));
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
                ClearWater();
                if (WaterSize == 0) return;
                Mwater = (GameObject)Instantiate(iteratorVariable1.LoadAsset("water4example (advanced)"), new Vector3(0f, 0f, 0f), new Quaternion());
                Mwater.name = "Water1";
                
                
                 
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
            UnityEngine.Object.Destroy(Mwater);
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
            UnityEngine.Object.Destroy(Mwater);
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
                if ( clouds[0] != null)
                {
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i].transform.RotateAround(this.transform.localPosition, axis[i], Time.deltaTime);
                        clouds[i].GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(30, 200);
                    }
                }
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8) && Input.GetKey(KeyCode.LeftControl))
            {
                LoadFloater();
            }
            if (Input.GetKeyDown(KeyCode.F9) && Input.GetKey(KeyCode.LeftControl))
            {
                ClearFloater();
            }
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
                GeoTools.UnhideFloorBig();
            }
        }
        void Start()
        {
            this.transform.localPosition = new Vector3(0, 500, 0);
            WWW iteratorVariable0 = new WWW("file:///" + Application.dataPath + "/Mods/Blocks/Shader/Water.unity3d.dll");
            iteratorVariable1 = iteratorVariable0.assetBundle;

            string[] names = iteratorVariable1.GetAllAssetNames();
            for (int i = 0; i < names.Length; i++)
            {
                Debug.Log(names[i]);
            }

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
    }
}

