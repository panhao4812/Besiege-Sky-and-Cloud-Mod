using System;
//using System.Threading.Tasks;
using System.IO;
using spaar.ModLoader;
using System.Collections;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class CloudAndTerrain : MonoBehaviour
    {
        //cloud
        private GameObject[] clouds = new GameObject[100];
        private GameObject cloudTemp;
        bool isBoundairesAway = false;
        private int cloudAmountTemp;
        //terrain
        public static TerrainData terrain = new TerrainData();
        public static GameObject terrainFinal = new GameObject();
        private GameObject terrainObject = new GameObject();

        void Update()
        {
            if (AddPiece.isSimulating)
            {



            }
        }
        void FixUpdate()
        {



            if ((this.clouds[1] == null) && (this.cloudAmount > 1))
            {

                for (int j = 0; j < this.clouds.Length; j = num2 + 1)
                {
                    DontDestroyOnLoad(this.clouds[j]);
                    if (j < (this.clouds.Length / 3))
                    {
                        this.clouds[j] = (GameObject)Object.Instantiate(this.cloudTemp, new Vector3(Random.Range((float)((-this.floorScale.x / 2f) - 200f), (float)((this.floorScale.x / 2f) + 200f)), Random.Range(this.higherCloudsMinHeight, this.higherCloudsMaxHeight), Random.Range((float)((-this.floorScale.z / 2f) - 200f), (float)((this.floorScale.z / 2f) + 200f))), new Quaternion(0f, 0f, 0f, 0f));
                        this.clouds[j].GetComponent<ParticleSystem>().startColor = this.higherCloudsColor;
                        this.clouds[j].layer = 12;
                    }
                    else
                    {
                        this.clouds[j] = (GameObject)Object.Instantiate(this.cloudTemp, new Vector3(Random.Range((float)((-this.floorScale.x / 2f) - 200f), (float)((this.floorScale.x / 2f) + 200f)), Random.Range(this.lowerCloudsMinHeight, this.lowerCloudsMaxHeight), Random.Range((float)((-this.floorScale.z / 2f) - 200f), (float)((this.floorScale.z / 2f) + 200f))), new Quaternion(0f, 0f, 0f, 0f));
                        this.clouds[j].GetComponent<ParticleSystem>().startColor = this.lowerCloudsColor;
                        this.clouds[j].layer = 12;
                    }
                    this.clouds[j].SetActive(true);
                    this.clouds[j].GetComponent<ParticleSystem>().startSize = 30f;
                    this.clouds[j].GetComponent<ParticleSystem>().startLifetime = 5f;
                    this.clouds[j].transform.localScale = new Vector3(15f, 15f, 15f);
                    this.clouds[j].GetComponent<ParticleSystem>().maxParticles = (int)this.clouds[j].transform.position.y;
                    this.shadow[j] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Object.Destroy(this.shadow[j].GetComponent<Collider>());
                    this.shadow[j].layer = this.clouds[j].layer;
                    this.shadow[j].transform.position = this.clouds[j].transform.position;
                    this.shadow[j].transform.parent = this.clouds[j].transform;
                    this.shadow[j].transform.localPosition = new Vector3(0.5f, 0f, 0f);
                    this.shadow[j].transform.localEulerAngles = new Vector3(18f, 10f, 353f);
                    this.shadow[j].transform.localScale = new Vector3(4f, 2.5f, 2.5f);
                    this.shadow[j].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    this.shadow[j].GetComponent<Renderer>().receiveShadows = true;
                    foreach (Material material in this.shadow[j].GetComponent<Renderer>().materials)
                    {
                        material.color = new Color(1f, 1f, 1f, 0.3f);
                    }
                    Object.Destroy(this.shadow[j].GetComponent<Renderer>().material.mainTexture);
                    this.clouds[j].transform.LookAt(new Vector3(Random.Range((float)((-this.floorScale.x / 2f) - 200f), (float)((this.floorScale.x / 2f) + 200f)), Random.Range((float)-700f, (float)700f), Random.Range((float)((-this.floorScale.z / 2f) - 200f), (float)((this.floorScale.z / 2f) + 200f))));
                    try
                    {
                        this.clouds[j].GetComponent<ParticleSystemRenderer>().shadowCastingMode = ShadowCastingMode.On;
                    }
                    catch
                    {
                        Debug.Log("Shadow failed!");
                    }
                    num2 = j;
                }
            }
        }
        void Start()
        {
            if (isBoundairesAway)
            {
                try
                {
                    GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                    isBoundairesAway = true;
                }
                catch { }
            }
            if (this.cloudTemp == null)
            {
                this.cloudTemp = Instantiate(GameObject.Find("CLoud"));
                this.cloudTemp.SetActive(false);
            }
            for (int i = 0; i < clouds.Length; i++)
            {
                Vector3 location = new Vector3(
                    UnityEngine.Random.Range(-1500, 1500),
                    UnityEngine.Random.Range(-1500, 1500),
                    UnityEngine.Random.Range(1500, 3000));
                Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);
                clouds[i] = (GameObject)Instantiate(cloudTemp, location, rotation);
                clouds[i].SetActive(true);
                //DontDestroyOnLoad(clouds[i]);
            }
        }
    }
}
}
