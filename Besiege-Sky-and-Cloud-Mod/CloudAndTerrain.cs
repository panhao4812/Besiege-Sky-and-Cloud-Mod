//using System.Threading.Tasks;
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
        private GameObject[] clouds = new GameObject[50];
        private Vector3[] axis = new Vector3[50];
        private GameObject cloudTemp;
        private bool IsCloudActice = true;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 floorScale = new Vector3(1000, 400, 1000);
        public string BaseShip = "BaseShip";
        public string BaseShipDeck = "BaseShipDeck";
        public string BaseShipTexture = "BaseShipTexture";
        public string GroundTexture = "GroundTexture";
        public string HeightMap = "HeightMap";
        public int CloudSize = 50;
        public int u = 200, v = 100;
        public float uscale = 5, vscale = 5, heightscale = 50;
        public float texturescale = 1f;
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
        void FloorBigFromData()
        {
            try
            {
                GameObject FB = GameObject.Find("FloorBig");
                FB.GetComponent<Renderer>().material.mainTexture = LoadTexture(BaseShipTexture);
                Debug.Log(Application.dataPath);
                Mesh mesh = MeshFromObj(Application.dataPath + "/Mods/Blocks/Floor/" + BaseShip + ".obj");
                Mesh mesh2 = MeshFromObj(Application.dataPath + "/Mods/Blocks/Floor/" + BaseShipDeck + ".obj");
                if (mesh == null || mesh2 == null)
                {
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==> GetMeshFailed!");
                    return;
                }
                try { FB.GetComponent<MeshCollider>().sharedMesh.Clear(); }
                catch
                {
                    FB.AddComponent<MeshCollider>();
                    FB.GetComponent<MeshCollider>().sharedMesh.Clear();
                }
                FB.GetComponent<MeshFilter>().mesh.Clear();
                FB.GetComponent<MeshCollider>().sharedMesh = mesh2;
                FB.GetComponent<MeshFilter>().mesh = mesh;
                FB.transform.localScale = new Vector3(1, 1, 1);
                FB.transform.position = new Vector3(0, -10, 0);
                Destroy(FB.GetComponent<BoxCollider>());
                Destroy(GameObject.Find("FloorGrid"));
            }
            catch (System.Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>FloorBigFromData Failed! Please Open dev to search your FloorBig！");
                Debug.Log(ex.ToString());
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
                FB.GetComponent<Renderer>().material.mainTexture = LoadTexture(GroundTexture);
                Texture2D te2 = (Texture2D)LoadTexture(HeightMap);
                if (te2.width < u || te2.height < v)
                {
                    Debug.Log("Besiege_Sky_and_Cloud_Mod==>ResetBigFloor Failed ! Need a larger Height map！");
                    return;
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
                        newUV.Add(new Vector2((float)i / (float)u* texturescale, (float)j / (float)v* texturescale));
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
                FB.transform.localScale = new Vector3(1, 1, 1);
                FB.transform.position = new Vector3(-u * uscale / 2, -heightscale, -v * vscale / 2);
                Destroy(FB.GetComponent<BoxCollider>());
                Destroy(GameObject.Find("FloorGrid"));
            }
            catch (System.Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>ResetBigFloor Failed ! Please Open dev to search your FloorBig！");
                Debug.Log(ex.ToString());
            }
        }
        Texture LoadTexture(string TextureName)
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
        void ResetCloud()
        {
            if (CloudSize < 10) CloudSize = 10;
            if (CloudSize > 300) CloudSize = 300;
            if (clouds[1] == null)
            {
                clouds = new GameObject[CloudSize];

                for (int i = 0; i < clouds.Length; i++)
                {
                    GameObject.DontDestroyOnLoad(clouds[i]);
                    clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(
                        UnityEngine.Random.Range(-floorScale.x, floorScale.x),
                        UnityEngine.Random.Range(floorScale.y, floorScale.y * 2),
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
                //这个在选择关口界面
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
                ResetFloorBig();
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                FloorBigFromData();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                for (int i = 0; i < clouds.Length; i++)
                {
                    Destroy(clouds[i]);
                }
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                BaseShip = "BaseShip";
                BaseShipDeck = "BaseShipDeck";
                BaseShipTexture = "BaseShipTexture";
                GroundTexture = "GroundTexture";
                HeightMap = "HeightMap";
                CloudSize = 50;
                u = 200; v = 100;
                uscale = 5; vscale = 5; heightscale = 50;
                texturescale = 1f;
            }
        }
        void Start()
        {
            //StartCoroutine(Function());
        }
        void ClearResource()
        {
            UnityEngine.Object.Destroy(this.cloudTemp);
            for (int i = 0; i < clouds.Length; i++)
            {
                Destroy(clouds[i]);
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

