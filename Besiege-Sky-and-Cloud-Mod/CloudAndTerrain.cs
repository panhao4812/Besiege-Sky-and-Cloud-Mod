//using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
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
        //terrain
        private Vector3 floorScale = new Vector3(1000, 400, 1000);
        public static TerrainData terrainData = new TerrainData();
        public static GameObject terrainFinal = new GameObject();


        public Mesh MeshFromPoints(Vector3[] pl, int u, int v)
        {
            if (u * v > pl.Length || u < 2 || v < 2) return null;
            Mesh mesh = new Mesh();
            mesh.vertices = pl;
            List<int> triangleslist = new List<int>();
            for (int i = 1; i < u; i++)
            {
                for (int j = 1; j < v; j++)
                {
                    triangleslist.Add((j - 1) * u + i - 1);
                    triangleslist.Add((j - 1) * u + i);
                    triangleslist.Add((j) * u + i);
                    triangleslist.Add((j - 1) * u + i - 1);
                    triangleslist.Add((j) * u + i);
                    triangleslist.Add((j) * u + i - 1);
                }
            }
            mesh.triangles = triangleslist.ToArray();
            return mesh;
        }

        void ResetBigFloor()
        {
            try
            {

                List<Vector3> newVertices = new List<Vector3>();
                List<Vector2> newUV = new List<Vector2>();
                List<int> triangleslist = new List<int>();
                Texture2D te2 = (Texture2D)LoadTexture("1122512418-1");
                GameObject FB = GameObject.Find("FloorBig");
                FB.GetComponent<Renderer>().material.mainTexture = te2;
                Destroy(FB.GetComponent<BoxCollider>());
                Mesh mesh = FB.GetComponent<MeshFilter>().mesh;
                mesh.Clear();
               
                int u = 65, v = 65;
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        newVertices.Add(new Vector3(i / 100, j / 100, te2.GetPixel(i * 2, j * 2).grayscale / 2));
                        newUV.Add(new Vector2(i, j));
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
            }
            catch (System.Exception ex)
            {
                Debug.Log("ResetBigFloor Failed!");
                Debug.Log(ex.ToString());
            }
        }
        void ResetFloor()
        {
            try
            {
                Texture2D te2 = (Texture2D)LoadTexture("1122512418-1");
                terrainData.size = new Vector3(500f, 200f, 500f);
                Vector3 position = new Vector3(-300f, GameObject.Find("FloorPos").transform.position.y - 0.1f, -300f);
                Quaternion rotation = new Quaternion();
                GameObject terrainObject = Terrain.CreateTerrainGameObject(SkyAndCloudMod2.terrainData);
                terrainFinal = (GameObject)Instantiate(terrainObject, position, rotation);
                terrainFinal.name = "NewFloorBig";
                terrainFinal.GetComponent<Terrain>().materialType = Terrain.MaterialType.Custom;
                terrainFinal.GetComponent<Terrain>().materialTemplate =
                    GameObject.Find("FloorBig").GetComponent<Renderer>().material;
                // terrainFinal.GetComponent<Terrain>().materialTemplate.mainTexture = te2;
                //new Material(Shader.Find("Nature / Terrain / Diffuse"));
                terrainFinal.transform.Translate(new Vector3(0f, -100f, 0f));
                terrainFinal.GetComponent<Terrain>().castShadows = true;
                terrainFinal.AddComponent<OnCollisionMine>();
                terrainData.heightmapResolution = 65;

                float[,] heights = SkyAndCloudMod2.terrainData.GetHeights(0, 0, SkyAndCloudMod2.terrainData.heightmapWidth, SkyAndCloudMod2.terrainData.heightmapHeight);
                for (int i = 0; i < SkyAndCloudMod2.terrainData.heightmapWidth; i++)
                {
                    for (int j = 0; j < SkyAndCloudMod2.terrainData.heightmapHeight; j++)
                    {
                        heights[i, j] = te2.GetPixel(i * 2, j * 2).grayscale / 2;
                    }
                }
                SkyAndCloudMod2.terrainData.SetHeights(0, 0, heights);

                Destroy(GameObject.Find("Terrain"));
                Destroy(GameObject.Find("FloorBig"));
                Destroy(te2);
            }
            catch (System.Exception ex)
            {
                Debug.Log("ResetFloor Failed!");
                Debug.Log(ex.ToString());
            }
        }

        Texture LoadTexture(string TextureName)
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
                Debug.Log("No image in folder");
                return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
            }
        }
        IEnumerator Function()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        void ResetCloud()
        {
            if (clouds[1] == null)
            {
                clouds = new GameObject[50];

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
                    Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
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
            if (isBoundairesAway)
            {
                isBoundairesAway = true;
                try
                {
                    GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                    Debug.Log("WORLD BOUNDARIES Succeeded!");
                }
                catch (System.Exception ex)
                {
                    isBoundairesAway = false;
                    Debug.Log("WORLD BOUNDARIES Failed!");
                    Debug.Log(ex.ToString());
                }
            }
        }
        int debugsign1 = 0;
        void FixedUpdate()
        {
            if (AddPiece.isSimulating) MoveBoundary();
            if (cloudTemp == null)
            {
                //这个有时候要获取很多次才能成功
                cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud"));
                cloudTemp.SetActive(false);
                DontDestroyOnLoad(cloudTemp);
                Debug.Log(debugsign1.ToString() + ": Besiege_Sky_and_Cloud_Mod==> Get Cloud Temp Successfully"); debugsign1++;
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

            if (Input.GetKeyDown(KeyCode.F7))
            {
                IsCloudActice = !IsCloudActice;
                Debug.Log("IsCloudActice=" + IsCloudActice.ToString());
                foreach (GameObject cloud in clouds)
                {
                    if (!IsCloudActice) cloud.GetComponent<ParticleSystem>().Pause();
                    else cloud.GetComponent<ParticleSystem>().Play();
                }
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                ResetFloor();
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                ResetBigFloor();
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
    public class OnCollisionMine : MonoBehaviour
    {
        // Methods
        private void OnCollisionEnter(Collision c)
        {
            try
            {
                if (c.transform.GetComponent<MyBlockInfo>().blockName == "DRILL")
                {
                    int xBase = Mathf.RoundToInt(((c.transform.position.x - SkyAndCloudMod2.terrainFinal.transform.position.x) / SkyAndCloudMod2.terrainData.size.x) * SkyAndCloudMod2.terrainData.heightmapWidth);
                    int yBase = Mathf.RoundToInt(((c.transform.position.z - SkyAndCloudMod2.terrainFinal.transform.position.z) / SkyAndCloudMod2.terrainData.size.z) * SkyAndCloudMod2.terrainData.heightmapWidth);
                    float[,] heights = SkyAndCloudMod2.terrainData.GetHeights(xBase, yBase, 3, 3);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            heights[i, j] -= 0.005f;
                        }
                    }
                    SkyAndCloudMod2.terrainData.SetHeights(xBase, yBase, heights);
                }
            }
            catch
            {
            }
        }
    }


}

