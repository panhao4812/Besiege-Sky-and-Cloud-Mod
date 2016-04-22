using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class GeoTools
    {
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
        public static Mesh MeshFromObj(string Objname)
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
                srd = File.OpenText(Application.dataPath + "/Mods/Blocks/Floor/" + Objname + ".obj");
            }
            catch
            {
                Debug.Log("Open " + Objname + " failed");
                return null;
            }
            try
            {
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
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
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.normals = newNormals.ToArray();
                Debug.Log("ReadFile " + Objname + " Completed!" + "Vertices:" + newVertices.Count.ToString());
                srd.Close();
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mesh.Optimize();
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model " + Objname + " error!");
                Debug.Log("newUV==>" + newUV.Count.ToString());
                Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
                Debug.Log("newNormals==>" + newNormals.Count.ToString());
                Debug.Log(ex.ToString());
            }
            return mesh;
        }

        public static Mesh WMeshFromObj(string Objname)
        {

            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            List<Vector3> newNormals = new List<Vector3>();
            Mesh mesh = new Mesh();
            StreamReader srd;
            try
            {
                srd = File.OpenText(Application.dataPath + "/Mods/Blocks/Floor/" + Objname + ".obj");
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
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "v")
                        {
                            Vector3 v1 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));
                            newVertices.Add(v1);
                        }
                        else if (chara[0] == "vt")
                        {
                            Vector2 uv1 = new Vector2(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]));

                            newUV.Add(uv1);
                        }
                        else if (chara[0] == "vn")
                        {
                            Vector3 v2 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));

                            newNormals.Add(v2);
                        }
                        else if (chara[0] == "f")
                        {
                            if (chara.Length == 4)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);

                            }
                            if (chara.Length == 5)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                int d = Convert.ToInt32(chara[4].Split('/')[0]);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(b - 1);
                                triangleslist.Add(c - 1);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(c - 1);
                                triangleslist.Add(d - 1);
                            }
                        }
                    }
                }
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.normals = newNormals.ToArray();
                Debug.Log("ReadFile " + Objname + " Completed!" + "Vertices:" + newVertices.Count.ToString());
                srd.Close();
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mesh.Optimize();
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model " + Objname + " error!");
                Debug.Log("newUV==>" + newUV.Count.ToString());
                Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
                Debug.Log("newNormals==>" + newNormals.Count.ToString());
                Debug.Log(ex.ToString());
            }
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
        public static void HideFloorBig()
        {
            try
            {
                GameObject.Find("FloorBig").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("FloorGrid").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = 2500;
            }
            catch { }
        }
        public static void UnhideFloorBig()
        {
            try
            {
                GameObject.Find("FloorBig").transform.localScale = new Vector3(1000, 1, 1000);
                GameObject.Find("FloorBig").transform.localPosition = new Vector3(0, 0, 0);
                GameObject.Find("FloorGrid").transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("FloorGrid").transform.localPosition = new Vector3(0, 0, 0);
                GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = 1500;
            }
            catch { }
        }
    }
}
