//using System.Threading.Tasks;
using spaar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class SkyAndCloudMod2 : MonoBehaviour
    {
        private bool isBoundairesAway = false;
        //cloud
        private GameObject[] clouds = new GameObject[30];
        private Vector3[] axis = new Vector3[30];
        private GameObject cloudTemp;
        private bool IsCloudActice = true;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 floorScale = new Vector3(1000, 200, 1000);
        private int MeshSize = 10;

        public string GroundTexture = "GroundTexture";
        public string HeightMap = "HeightMap";
        public int CloudSize = 30;
        public int u = 200, v = 100;
        public float uscale = 5, vscale = 5, heightscale = 50;
        public float texturescale = 1f;

        private GameObject[] meshes = new GameObject[10];

        public static Mesh MeshFromPoints(int u, int v)
        {
            Mesh mesh = new Mesh();
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    newVertices.Add(new Vector3(i, 0, j));
                    newUV.Add(new Vector2((float)i / (float)u, (float)j / (float)v));
                    if (i > 0 && j > 0)
                    {
                        triangleslist.Add((j - 1) * u + i - 1);
                        triangleslist.Add((j - 1) * u + i);
                        triangleslist.Add((j) * u + i);
                        triangleslist.Add((j - 1) * u + i - 1);
                        triangleslist.Add((j) * u + i);
                        triangleslist.Add((j) * u + i - 1);
                    }
                }
            }
            mesh.vertices = newVertices.ToArray();
            mesh.uv = newUV.ToArray();
            mesh.triangles = triangleslist.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
        public static Mesh MeshFromObj(string ObjPath)
        {
            List<Vector3> Normals = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            List<Vector3> newNormals = new List<Vector3>();
            Mesh mesh = new Mesh();
            StreamReader srd;
            try
            {
                srd = File.OpenText(ObjPath);
            }
            catch
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==> File open failed");
                return null;
            }
            try
            {
                while (srd.Peek() != -1)
                {
                    String str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "v")
                        {
                            Vector3 v1 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));
                            Vertices.Add(v1);
                        }
                        else if (chara[0] == "vt")
                        {
                            Vector2 uv1 = new Vector2(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]));

                            UV.Add(uv1);
                        }
                        else if (chara[0] == "vn")
                        {
                            Vector3 v2 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));

                            Normals.Add(v2);
                        }
                        else if (chara[0] == "f")
                        {
                            if (chara.Length == 4)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 1);
                                triangleslist.Add(newVertices.Count + 2);
                                newVertices.Add(Vertices[a - 1]);
                                newVertices.Add(Vertices[b - 1]);
                                newVertices.Add(Vertices[c - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[1].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[2].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[3].Split('/')[2]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[1].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[2].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[3].Split('/')[1]) - 1]);
                            }
                            if (chara.Length == 5)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                int d = Convert.ToInt32(chara[4].Split('/')[0]);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 1);
                                triangleslist.Add(newVertices.Count + 2);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 2);
                                triangleslist.Add(newVertices.Count + 3);
                                newVertices.Add(Vertices[a - 1]);
                                newVertices.Add(Vertices[b - 1]);
                                newVertices.Add(Vertices[c - 1]);
                                newVertices.Add(Vertices[d - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[1].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[2].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[3].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[4].Split('/')[2]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[1].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[2].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[3].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[4].Split('/')[1]) - 1]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model error!");
                Debug.Log(ex.ToString());
            }
            Debug.Log("newVertices==>" + newVertices.Count.ToString());
            Debug.Log("newUV==>" + newUV.Count.ToString());
            Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
            Debug.Log("newNormals==>" + newNormals.Count.ToString());
            mesh.vertices = newVertices.ToArray();
            mesh.uv = newUV.ToArray();
            mesh.triangles = triangleslist.ToArray();
            mesh.normals = newNormals.ToArray();
            Debug.Log("Besiege_Sky_and_Cloud_Mod==>ReadFile Completed!");
            srd.Close();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
        public static Texture LoadTexture(string TextureName)
        {
            try
            {
                WWW png = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/" + TextureName + ".png");
                WWW jpg = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/" + TextureName + ".jpg");
                if (png.size > 5)
                {
                    return png.texture;
                }
                else if (jpg.size > 5)
                {
                    return jpg.texture;
                }
                else {
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==>No image in folder or image could not be used!");
                    return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
                }
            }
            catch
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>No image in folder,use white image instead !");
                return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
            }
        }
        void LoadScene(string SceneName)
        {
            try
            {
                ResetMesh();
                Debug.Log(Application.dataPath);
                StreamReader srd;
                try
                {
                    srd = File.OpenText(SceneName);
                    while (srd.Peek() != -1)
                    {
                        String str = srd.ReadLine();
                        string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (chara[0] == "Mesh")
                        {

                        }
                        else if(chara[0] == "Mesh")
                        {

                        }
                        else if (chara[0] == "Mesh")
                        {

                        }
                        else if (chara[0] == "Mesh")
                        {

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
        void ResetFloorBig()
        {
            if (uscale < 2) uscale = 2;
            if (vscale < 2) vscale = 2;
            if (uscale > 100) uscale = 100;
            if (vscale > 100) vscale = 100;
            if (heightscale < 10) heightscale = 10;
            if (heightscale > 100) heightscale = 100;
            if (u < 32) u = 32;
            if (v < 32) v = 32;
            if (u > 2048) u = 2048;
            if (v > 2048) v = 2048;
            if (texturescale > 2048) texturescale = 2048;
            if (texturescale < 0.01f) texturescale = 0.01f;
            try
            {
                GameObject FB = GameObject.Find("FloorBig");
                FB.transform.localScale = new Vector3(1, 1, 1);
                FB.transform.position = new Vector3(-u * uscale / 2, -heightscale, -v * vscale / 2);
                FB.GetComponent<Renderer>().material.mainTexture = LoadTexture(GroundTexture);
                Texture2D te2 = (Texture2D)LoadTexture(HeightMap);
                if (te2.width < u || te2.height < v)
                {
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==>ResetBigFloor Failed ! Need a larger Height map！");
                    u = te2.width; v = te2.height;
                }
                List<Vector3> newVertices = new List<Vector3>();
                List<Vector2> newUV = new List<Vector2>();
                List<int> triangleslist = new List<int>();
                Mesh mesh = new Mesh();
                for (int j = 0; j < v; j++)
                {
                    for (int i = 0; i < u; i++)
                    {
                        newVertices.Add(new Vector3(i * uscale, te2.GetPixel(i, j).grayscale * heightscale, j * vscale));
                        newUV.Add(new Vector2((float)i / (float)u * texturescale, (float)j / (float)v * texturescale));
                        if (i > 0 && j > 0)
                        {
                            triangleslist.Add((j - 1) * u + i - 1);
                            triangleslist.Add((j) * u + i);
                            triangleslist.Add((j - 1) * u + i);

                            triangleslist.Add((j - 1) * u + i - 1);
                            triangleslist.Add((j) * u + i - 1);
                            triangleslist.Add((j) * u + i);
                        }
                    }
                }
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                try { FB.GetComponent<MeshCollider>().sharedMesh.Clear(); }
                catch
                {
                    FB.AddComponent<MeshCollider>();
                    FB.GetComponent<MeshCollider>().sharedMesh.Clear();
                }
                FB.GetComponent<MeshFilter>().mesh.Clear();
                FB.GetComponent<MeshCollider>().sharedMesh = mesh;
                FB.GetComponent<MeshFilter>().mesh = mesh;

                FB.GetComponent<BoxCollider>().transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("FloorGrid").transform.localScale = new Vector3(0, 0, 0);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>ResetBigFloor Failed ! Please Open dev to search your FloorBig！");
                Debug.Log(ex.ToString());
            }
        }
        void ResetCloud()
        {
            if (CloudSize < 10) CloudSize = 10;
            if (CloudSize > 300) CloudSize = 300;
            if (clouds[1] == null)
            {
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
                    //Debug.Log(i.ToString() + ":" + clouds[i].name);
                    clouds[i].SetActive(true);
                    clouds[i].transform.SetParent(GameObject.Find("Sky and Ground Mod").transform);
                    clouds[i].transform.localScale = new Vector3(15, 15, 15);
                    //Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
                    //clouds[i].transform.LookAt();
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
        void ResetMesh()
        {
            try
            {
                GameObject FB = GameObject.Find("FloorBig");
                FB.GetComponent<BoxCollider>().transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("FloorGrid").transform.localScale = new Vector3(0, 0, 0);
                FB.transform.localScale = new Vector3(0, 0, 0);
                if (meshes[1] == null) meshes = new GameObject[MeshSize];
                for (int i = 0; i < meshes.Length; i++)
                {
                    if (meshes[1] == null) meshes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    try
                    {
                        meshes[i].transform.localScale = new Vector3(0, 0, 0);
                        meshes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshes[i].GetComponent<MeshFilter>().mesh.Clear();
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
        void MoveBoundary()
        {
            if (!isBoundairesAway)
            {
                isBoundairesAway = true;
                try
                {
                    GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==> WORLD BOUNDARIES Succeeded!");
                }
                catch (System.Exception ex)
                {
                    Debug.Log("WORLD BOUNDARIES Failed!");
                    Debug.Log(ex.ToString());
                }
            }
        }
        void FixedUpdate()
        {
            if (AddPiece.isSimulating) { MoveBoundary(); } else { isBoundairesAway = false; }
            if (cloudTemp == null)
            {
                //这个在星球界面
                cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud"));
                cloudTemp.SetActive(false);
                DontDestroyOnLoad(cloudTemp);
                Debug.Log(": Besiege_Sky_and_Cloud_Mod==> Get Cloud Temp Successfully");
            }
            try
            {
                this.ResetCloud();//参数不要设置太离谱 否则找不到（摄像机视野有限）
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
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
            if (Input.GetKeyDown(KeyCode.F10))
            {
                GroundTexture = "GroundTexture";
                HeightMap = "HeightMap";
                CloudSize = 50;
                u = 200; v = 100;
                uscale = 5; vscale = 5; heightscale = 50;
                texturescale = 1f;
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                ClearCloud();
                ResetFloorBig();
            }
        }
        void Start()
        {
            Commands.RegisterCommand("LoadScene", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    string SceneName = args[0];
                }
                catch
                {
                    return "Besiege_Sky_and_Cloud_Mod==> LoadScene Failed";
                }
                return "Besiege_Sky_and_Cloud_Mod==> LoadScene Succeeded";
            }, "LoadScene SceneName");
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

