//using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class CloudAndTerrain : MonoBehaviour
    {
        //cloud
        private GameObject[] clouds = new GameObject[100];
        private GameObject cloudTemp;
        private bool isBoundairesAway = false;
        private Color CloudsColor = new Color(0.92f, 0.9f, 0.8f, 1);
        private float CloudBoxSize = 3000f;
        //terrain
        float MapHeight = 200f;
        void Update()
        {

        }
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
            catch
            {
                Debug.Log("ResetFloor Failed!");
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
                        ResetFloor();
                    }
               
            }
        }
        void ResetCloud()
        {
            this.cloudTemp = Instantiate(GameObject.Find("CLoud"));
            this.cloudTemp.SetActive(false);
            for (int i = 0; i < clouds.Length; i++)
            {
                Vector3 location = new Vector3(
                    UnityEngine.Random.Range(-CloudBoxSize, CloudBoxSize),
                    UnityEngine.Random.Range(CloudBoxSize, CloudBoxSize * 2),
                    UnityEngine.Random.Range(-CloudBoxSize, CloudBoxSize));
                Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);
                clouds[i] = (GameObject)Instantiate(cloudTemp, location, rotation);
                this.clouds[i].transform.localScale = new Vector3(15f, 15f, 15f);
                this.clouds[i].GetComponent<ParticleSystem>().startColor = this.CloudsColor;
                this.clouds[i].GetComponent<ParticleSystem>().startSize = 40f;
                this.clouds[i].GetComponent<ParticleSystem>().startLifetime = 5f;
                this.clouds[i].GetComponent<ParticleSystem>().maxParticles =
                    (int)(this.clouds[i].transform.position.y / 3);
                this.clouds[i].layer = 12;
                //DontDestroyOnLoad(clouds[i]);
                clouds[i].SetActive(true);
                this.clouds[i].transform.LookAt(new Vector3(
                    UnityEngine.Random.Range(-100, 100),
                    UnityEngine.Random.Range(0, -100),
                    UnityEngine.Random.Range(-100, 100)));
            }
        }
        void UpdateCloud()
        {
            if (clouds[0] == null) { Debug.Log("UpdateCloud Failed!"); return; }
            foreach (GameObject cloud in clouds)
            {
                float randomMove = UnityEngine.Random.Range(0.1f, 0.2f);
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
        void MoveBoundary()
        {
            if (isBoundairesAway)
            {
                isBoundairesAway = true;
                try
                {
                    GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                    Debug.Log("WORLD BOUNDARIES Seccessed!");
                }
                catch
                {
                    isBoundairesAway = false;
                    Debug.Log("WORLD BOUNDARIES Failed!");
                }
            }
        }
        void FixUpdate()
        {
            if (Application.loadedLevel == 2) return;
            MoveBoundary();
            try
            {
                if (clouds[0] == null) ResetCloud();
                if (AddPiece.isSimulating) { UpdateCloud(); }
            }
            catch
            {
                Debug.Log("Cloud Failed!");
            }
        }
        void Start()
        {
            ResetCloud();
            StartCoroutine(Function());
        }
    }
}

