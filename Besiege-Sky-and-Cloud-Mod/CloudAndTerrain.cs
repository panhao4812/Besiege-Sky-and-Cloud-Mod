//using System.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class SkyAndCloudMod2 : MonoBehaviour
    {
        //cloud
        private GameObject[] clouds = new GameObject[100];
        private GameObject cloudTemp;
        private bool isBoundairesAway = false;
        private Color CloudsColor = new Color(0.96f, 0.96f, 0.96f, 1);
        private float CloudBoxSize = 1000f;
        //terrain
        public Vector3 floorScale = new Vector3(1000, 500, 1000);
        float MapHeight = 100f;

        // Object.Destroy(GameObject.Find("Terrain"));
        // Object.Destroy(GameObject.Find("FloorBig"));
        void ResetFloor()
        {
            try
            {
                Texture2D HeightMap = (Texture2D)LoadTexture("HeightMap");
                TerrainData terrainData = new TerrainData();
                terrainData.size = new Vector3(HeightMap.width, MapHeight, HeightMap.height);
                terrainData.heightmapResolution = 513;
                terrainData.baseMapResolution = 513;
                terrainData.alphamapResolution = 512;
                terrainData.SetDetailResolution(32, 8);
                float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
                for (int i = 0; i < terrainData.heightmapWidth; i++)
                {
                    for (int j = 0; j < terrainData.heightmapHeight; j++)
                    {
                        heights[i, j] = HeightMap.GetPixel(i, j).grayscale * MapHeight;
                    }
                }
                terrainData.SetHeights(0, 0, heights);
                GameObject terrainObject = GameObject.Find("Terrain");
                terrainObject = Terrain.CreateTerrainGameObject(terrainData);
                Destroy(HeightMap);
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
                yield return new WaitForSeconds(0.02f);

                if (Input.GetKey(KeyCode.F5))
                {
                    ResetCloud();
                    // ResetFloor();
                }

            }
        }
        public void ResetCloud()
        {
            if (clouds[1] == null)
            {
                clouds = new GameObject[100];

                for (int i = 0; i < clouds.Length; i++)
                {
                    GameObject.DontDestroyOnLoad(clouds[i]);
                    clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(floorScale.y, floorScale.y * 2), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)), new Quaternion(0, 0, 0, 0));
                    clouds[i].GetComponent<ParticleSystem>().startColor = CloudsColor;
                    clouds[i].layer = 12;
                    Debug.Log(i.ToString() + ":" + clouds[i].name);
                    clouds[i].SetActive(true);
                    clouds[i].transform.SetParent(GameObject.Find("Sky and Ground Mod").transform);
                    clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                    clouds[i].GetComponent<ParticleSystem>().startLifetime = 5;
                    clouds[i].transform.localScale = new Vector3(15, 15, 15);
                    clouds[i].GetComponent<ParticleSystem>().maxParticles = (int)clouds[i].transform.position.y;
                    clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)));
                }
            }
            else
            {
                if (clouds[1] == null) { Debug.Log("UpdateCloud Failed!"); return; }
                foreach (GameObject cloud in clouds)
                {
                    float randomMove = UnityEngine.Random.Range(0.01f, 0.02f);
                    cloud.transform.position += new Vector3(randomMove, randomMove - 0.015f, randomMove);
                    cloud.transform.localScale *= 1 + randomMove - 0.015f;

                    if (cloud.transform.position.x > CloudBoxSize)
                    {
                        cloud.transform.position =
    new Vector3(-CloudBoxSize, cloud.transform.position.y, cloud.transform.position.z);
                    }
                    if (cloud.transform.position.z > CloudBoxSize)
                    {
                        cloud.transform.position =
    new Vector3(cloud.transform.position.x, cloud.transform.position.y, -CloudBoxSize);
                    }
                    if (cloud.transform.position.x < -CloudBoxSize)
                    {
                        cloud.transform.position =
    new Vector3(CloudBoxSize, cloud.transform.position.y, cloud.transform.position.z);
                    }
                    if (cloud.transform.position.z < -CloudBoxSize)
                    {
                        cloud.transform.position =
    new Vector3(cloud.transform.position.x, cloud.transform.position.y, CloudBoxSize);
                    }
                }

            }
        }
        void OnLoad()
        {
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
        void FixedUpdate()
        {

            if (cloudTemp == null) { cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud")); cloudTemp.SetActive(false); }

            DontDestroyOnLoad(cloudTemp);
            //这个要获取很多次才能成功
             try
            {
                //参数不要设置太离谱 否则找不到（摄像机视野有限）
                this.ResetCloud();
             
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        void Start()
        {
            // StartCoroutine(Function());
        }
    }

    public class SkyAndCloudMod3 : MonoBehaviour
    {
        private GameObject[] clouds = new GameObject[60];
        private GameObject cloudTemp;
        private int cloudAmount = 60;
        private float lowerCloudsMinHeight = 130f;
        private float lowerCloudsMaxHeight = 200f;
        private float higherCloudsMinHeight = 300;
        private float higherCloudsMaxHeight = 377.25f;
        private Color higherCloudsColor = new Color(1f, 1f, 1f, 1f);
        private Color lowerCloudsColor = new Color(0.92f, 0.9f, 0.8f, 1);
        public Vector3 floorScale = new Vector3(911, 10, 900);

        void OnLoad()
        {
        }
        void FixedUpdate()
        {

            if (cloudTemp == null) { cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud")); cloudTemp.SetActive(false); }

            DontDestroyOnLoad(cloudTemp);

            //   if (cloudAmountTemp != cloudAmount) { resetCloudsNow = true; clouds[1] = null; cloudAmountTemp = cloudAmount; try { for (int k = cloudAmount; k < clouds.Length; k++) { Destroy(clouds[k].gameObject); Destroy(shadow[k].gameObject); } } catch { } }
            try
            {
                floorScale = GameObject.Find("FloorBig").transform.localScale;
                if (clouds[1] == null && cloudAmount > 1)
                {
                    clouds = new GameObject[cloudAmount];

                    for (int i = 0; i < clouds.Length; i++)
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
                        Debug.Log(i.ToString() + ":" + clouds[i].name);
                        clouds[i].SetActive(true);
                        clouds[i].transform.SetParent(GameObject.Find("Sky and Ground Mod").transform);
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 5;
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = (int)clouds[i].transform.position.y;

                        clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)));

                    }
                }
                else
                {
                    /*
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
                    */
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

    }
}

