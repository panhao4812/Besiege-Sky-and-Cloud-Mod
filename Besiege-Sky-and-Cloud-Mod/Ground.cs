


using System.Collections.Generic;
using UnityEngine;
namespace Besiege_Sky_and_Cloud_Mod
{
    public class Ground : MonoBehaviour
    {
        public string GroundTexture = "GroundTexture";
        public string HeightMap = "HeightMap";
        public int u = 200, v = 100;
        public float uscale = 5, vscale = 5, heightscale = 50;
        public float texturescale = 1f;
        private GameObject Mesh;
        void LoadHeightMap()
        {
            if (Mesh == null) { Mesh = GameObject.CreatePrimitive(PrimitiveType.Plane); Mesh.name = "Terrian1"; }
            try
            {               
                Mesh.transform.position = new Vector3(-u * uscale / 2, -heightscale, -v * vscale / 2);
                Mesh.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture(GroundTexture);
                Texture2D te2 = (Texture2D)GeoTools.LoadTexture(HeightMap);
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
                Mesh.GetComponent<MeshCollider>().sharedMesh.Clear();
                Mesh.GetComponent<MeshFilter>().mesh.Clear();
                Mesh.GetComponent<MeshCollider>().sharedMesh = mesh;
                Mesh.GetComponent<MeshFilter>().mesh = mesh;
            }
            catch (System.Exception ex)
            {
                Debug.Log("Besiege_Sky_and_Cloud_Mod==>ResetBigFloor Failed ! Please Open dev to search your FloorBig！");
                Debug.Log(ex.ToString());
                Destroy(Mesh);
                GeoTools.UnhideFloorBig();
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6)&& Input.GetKey(KeyCode.LeftControl))
            {
                GeoTools.HideFloorBig();
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
                LoadHeightMap();
            }
            if (Input.GetKeyDown(KeyCode.F10) && Input.GetKey(KeyCode.LeftControl))
            {
                GroundTexture = "GroundTexture";
                HeightMap = "White";   
                u = 200; v = 200;
                uscale = 5; vscale = 5; heightscale = 50;
                texturescale = 1f;
                ClearMesh();
                GeoTools.UnhideFloorBig();
            }
            if (Input.GetKeyDown(KeyCode.F7) && Input.GetKey(KeyCode.LeftControl))
            {
                ClearMesh();
            }
            }
         void ClearMesh()
        {
            Destroy(Mesh);
        }
        void OnDisable()
        {
            ClearMesh();
        }
        void OnDestroy()
        {
            ClearMesh();
        }
    }
}

