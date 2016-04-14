


using UnityEngine;
namespace Besiege_Sky_and_Cloud_Mod
{
    public class TerrainLoaderDemo : MonoBehaviour
    {
        public static TerrainData terrainData = new TerrainData();
        public static GameObject terrainFinal = new GameObject();
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
        void ResetFloor()
        {
            try
            {
                Texture2D te2 = (Texture2D)LoadTexture("HeightMap");
                terrainData.size = new Vector3(500f, 200f, 500f);
                Vector3 position = new Vector3(-300f, GameObject.Find("FloorPos").transform.position.y - 0.1f, -300f);
                Quaternion rotation = new Quaternion();
                GameObject terrainObject = Terrain.CreateTerrainGameObject(TerrainLoaderDemo.terrainData);
                terrainFinal = (GameObject)Instantiate(terrainObject, position, rotation);
                terrainFinal.name = "NewFloorBig";
                terrainFinal.GetComponent<Terrain>().materialType = Terrain.MaterialType.Custom;
                terrainFinal.GetComponent<Terrain>().materialTemplate =
                    GameObject.Find("FloorBig").GetComponent<Renderer>().material;
                terrainFinal.transform.Translate(new Vector3(0f, -100f, 0f));
                terrainFinal.GetComponent<Terrain>().castShadows = true;
                terrainFinal.AddComponent<OnCollisionMine>();
                terrainData.heightmapResolution = 65;

                float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
                for (int i = 0; i < terrainData.heightmapWidth; i++)
                {
                    for (int j = 0; j < terrainData.heightmapHeight; j++)
                    {
                        heights[i, j] = te2.GetPixel(i * 2, j * 2).grayscale / 2;
                    }
                }
                terrainData.SetHeights(0, 0, heights);
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
                    int xBase = Mathf.RoundToInt(((
                        c.transform.position.x - TerrainLoaderDemo.terrainFinal.transform.position.x) / TerrainLoaderDemo.terrainData.size.x) * TerrainLoaderDemo.terrainData.heightmapWidth);
                    int yBase = Mathf.RoundToInt(((
                        c.transform.position.z - TerrainLoaderDemo.terrainFinal.transform.position.z) / TerrainLoaderDemo.terrainData.size.z) * TerrainLoaderDemo.terrainData.heightmapWidth);
                    float[,] heights = TerrainLoaderDemo.terrainData.GetHeights(xBase, yBase, 3, 3);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            heights[i, j] -= 0.005f;
                        }
                    }
                    TerrainLoaderDemo.terrainData.SetHeights(xBase, yBase, heights);
                }
            }
            catch
            {
            }
        }
    }
}

