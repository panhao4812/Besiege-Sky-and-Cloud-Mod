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
        GameObject Pre = null;
        void Start()
        {

        }

        void LoadWater()
        {
            try {
                Pre = (GameObject)Instantiate(
                   new Vector3(0, 3, 0), new Quaternion());  
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
        }

        void ClearWater()
        {
            Destroy(Pre);
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
