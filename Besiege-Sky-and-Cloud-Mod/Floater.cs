using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class Floater : MonoBehaviour
    {
        // Fields
        private float AngularDrag = 0f;
        private float Drag = 0f;
        private float Force = 0f;
        private float ForceScale = 8f;
        private float minVolue = 0f;
        private float volume = 0f;
        private float WaterHeight = 0f;
        private bool fireTag = false;
        // Methods
        private void FixedUpdate()
        {
            if (base.GetComponent<Rigidbody>() == null)
            {
                UnityEngine.Object.Destroy(this);
            }
            else if (base.transform.position.y < (this.WaterHeight - this.minVolue))
            {
                if (fireTag) base.GetComponent<FireTag>().WaterHit();
                base.GetComponent<Rigidbody>().drag = (this.Drag + 3f) + (this.Force * this.ForceScale);
                base.GetComponent<Rigidbody>().angularDrag = (this.AngularDrag + 3f) + (this.Force * this.ForceScale);
                base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, this.Force - 0.125f, 0f), ForceMode.Impulse);
                base.GetComponent<Rigidbody>().useGravity = false;
            }
            else if (base.transform.position.y > this.WaterHeight)
            {
                base.GetComponent<Rigidbody>().drag = this.Drag;
                base.GetComponent<Rigidbody>().angularDrag = this.AngularDrag;
                base.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        private void Start()
        {
            try
            {
                this.WaterHeight = GameObject.Find("water0").transform.localPosition.y;
                if (base.GetComponent<Rigidbody>() == null)
                {
                    UnityEngine.Object.Destroy(this);
                    return;
                }
                else
                {
                    float x = base.gameObject.transform.localScale.x;
                    float y = base.gameObject.transform.localScale.y;
                    float z = base.gameObject.transform.localScale.z;
                    if ((x >= y) && (x >= z))
                    {
                        this.minVolue = x;
                    }
                    else if ((y >= x) && (y >= z))
                    {
                        this.minVolue = y;
                    }
                    else
                    {
                        this.minVolue = z;
                    }
                    this.volume = (x * y) * z;
                    this.Drag = base.GetComponent<Rigidbody>().drag;
                    this.AngularDrag = base.GetComponent<Rigidbody>().angularDrag;
                    if (base.GetComponent<FireTag>() != null) fireTag = true;
                    if (base.GetComponent<MyBlockInfo>().blockName == "SMALL WOOD BLOCK")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN BLOCK")
                    {
                        this.Force = (4f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN POLE")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN PANEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "PROPELLER")
                    {
                        this.Force = (8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "SMALL PROPELLER")
                    {
                        this.Force = (4f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WING")
                    {
                        this.Force = (8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WING PANEL")
                    {
                        this.Force = (4f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "PROPELLOR SMALL")
                    {
                        this.Force = (1f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "Rocket")
                    {
                        this.Force = (1f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WHEEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "LARGE WHEEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WHEEL FREE")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "UNPOWERED LARGE WHEEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "FLYING SPIRAL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "BALLOON")
                    {
                        this.Force = (8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "FLAMETHROWER")
                    {
                        this.Force = (1f * this.volume) / this.ForceScale;
                    }
                    else
                    {
                        this.Force = 0f;
                    }

                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception.ToString());
                UnityEngine.Object.Destroy(this);
            }
        }
    }



    ///////////////////////////////////////////
}




