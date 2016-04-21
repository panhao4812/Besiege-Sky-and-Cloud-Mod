//using System.Threading.Tasks;
using spaar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class Scene : MonoBehaviour
    {
        //cloud
        private GameObject[] clouds = new GameObject[30];
        private GameObject[] meshes = new GameObject[10];
        private Vector3[] axis = new Vector3[30];
        private GameObject cloudTemp;
        private bool IsCloudActice = true;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 floorScale = new Vector3(1000, 200, 1000);
        private int MeshSize = 10;
        private int CloudSize = 30;
        public string DefaultSceneName = "SteelHill";
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
                                    ResetMesh();
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
                            else if (chara[0] == "cloud")
                            {
                                if (chara[1] == "size")
                                {
                                    this.CloudSize = Convert.ToInt32(chara[2]);
                                }
                                else if (chara[1] == "location")
                                {
                                    this.transform.localPosition = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                else if (chara[1] == "scale")
                                {
                                    this.transform.localScale = new Vector3(
                                    Convert.ToSingle(chara[2]),
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]));
                                }
                                ClearCloud();
                            }
                            else if (chara[0] == "Camera")
                            {
                                if (chara[1] == "farClipPlane")
                                {
                                    GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = Convert.ToInt32(chara[2]);
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
        }
        void LoadCloud()
        {
            try
            {
                if (cloudTemp == null) return;
                if (CloudSize < 3) CloudSize = 3;
                if (CloudSize > 300) CloudSize = 300;
                if (clouds[1] == null)
                {
                    //ClearCloud();
                    clouds = new GameObject[CloudSize];
                    axis = new Vector3[CloudSize];
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        GameObject.DontDestroyOnLoad(clouds[i]);
                        clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(
                            UnityEngine.Random.Range(-floorScale.x, floorScale.x),
                            UnityEngine.Random.Range(0, floorScale.y),
                            UnityEngine.Random.Range(-floorScale.z, floorScale.z)),
                            new Quaternion(0, 0, 0, 0));
                        clouds[i].layer = 12;
                        clouds[i].SetActive(true);
                        clouds[i].transform.SetParent(GameObject.Find("Sky and Ground Mod").transform);
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].GetComponent<ParticleSystem>().startColor = CloudsColor;
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 6;
                        clouds[i].GetComponent<ParticleSystem>().startSpeed = 1.6f;
                        clouds[i].GetComponent<ParticleSystem>().emissionRate = 3;
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = 18;
                        axis[i] = new Vector3(UnityEngine.Random.Range(-0.02f, 0.02f), 1, UnityEngine.Random.Range(-0.02f, 0.02f));
                    }
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==> Reset Cloud Successfully");
                }
                else
                {
                    if (clouds[1] == null || IsCloudActice == false) return;
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i].transform.RotateAround(new Vector3(0, 0, 0), axis[i], Time.deltaTime);
                        clouds[i].GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(30, 200);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==> Load Cloud Failed");
                Debug.Log(ex.ToString());
            }
        }
        void ResetMesh()
        {
            try
            {
                if (MeshSize > 100) MeshSize = 100;
                if (MeshSize < 5) MeshSize = 5;

                ClearMeshes();
                meshes = new GameObject[MeshSize];

                for (int i = 0; i < meshes.Length; i++)
                {
                    if (meshes[i] == null) meshes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    try
                    {
                        meshes[i].transform.localScale = new Vector3(0, 0, 0);
                        meshes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshes[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshes[i].name = "_mesh" + i.ToString();
                    }
                    catch
                    {
                        Debug.Log("meshes[" + i.ToString() + "] error");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("ResetMesh Failed!");
                Debug.Log(ex.ToString());
            }
        }
        void FixedUpdate()
        {
            if (cloudTemp == null)
            {
                cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud"));
                cloudTemp.SetActive(false);
                DontDestroyOnLoad(cloudTemp);
                Debug.Log(": Besiege_Sky_and_Cloud_Mod==> Get Cloud Temp Successfully");
            }
            this.LoadCloud();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                try
                {
                    if (clouds[1] != null)
                    {
                        IsCloudActice = !IsCloudActice;
                        Debug.Log("IsCloudActice=" + IsCloudActice.ToString());
                        foreach (GameObject cloud in clouds)
                        {
                            if (!IsCloudActice) cloud.GetComponent<ParticleSystem>().Pause();
                            else cloud.GetComponent<ParticleSystem>().Play();
                        }
                    }
                }
                catch { }
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                ClearMeshes();
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                GeoTools.HideFloorBig();
                LoadScene(DefaultSceneName);
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                ClearMeshes();
            }
        }
        void Start()
        {
            Commands.RegisterCommand("DefaultSceneName", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    this.DefaultSceneName = args[0];
                }
                catch
                {
                    return "Besiege_Sky_and_Cloud_Mod==> DefaultSceneName Failed";
                }
                return "Besiege_Sky_and_Cloud_Mod==> DefaultSceneName Succeeded";
            }, "LoadScene DefaultSceneName");
        }
        void ClearCloud()
        {
            for (int i = 0; i < clouds.Length; i++)
            {
                Destroy(clouds[i]);
            }
        }
        void ClearMeshes()
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                Destroy(meshes[i]);
            }
        }
        void ClearResource()
        {
            UnityEngine.Object.Destroy(this.cloudTemp);
            for (int i = 0; i < clouds.Length; i++)
            {
                Destroy(clouds[i]);
            }
            for (int i = 0; i < meshes.Length; i++)
            {
                Destroy(meshes[i]);
            }
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

