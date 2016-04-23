using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class SimpleWater : MonoBehaviour
    {
        GameObject Mwater = null;
        void Start()
        {

        }
     
        void LoadWater()
        {
            if (Mwater == null) { Mwater = GameObject.CreatePrimitive(PrimitiveType.Plane); Mwater.name = "Water1"; }
            try {
                Mwater.transform.localPosition = new Vector3(0, 3, 0);
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
                LoadWater();
            }
            if (Mwater != null)
            {
                Renderer r = Mwater.GetComponent<Renderer>();
                if (!r)
                {
                    return;
                }
                Material mat = r.sharedMaterial;
                if (!mat)
                {
                    return;
                }

                Vector4 waveSpeed = mat.GetVector("WaveSpeed");
                float waveScale = mat.GetFloat("_WaveScale");
                float t = Time.time / 20.0f;

                Vector4 offset4 = waveSpeed * (t * waveScale);
                Vector4 offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1.0f), Mathf.Repeat(offset4.y, 1.0f),
                    Mathf.Repeat(offset4.z, 1.0f), Mathf.Repeat(offset4.w, 1.0f));
                mat.SetVector("_WaveOffset", offsetClamped);
            }
        }
        void ClearWater()
        {
            Destroy(Mwater);
        }
        void OnDisable()
        {
            ClearWater();
        }
        void OnDestroy()
        {
            ClearWater();
        }

    }
}
