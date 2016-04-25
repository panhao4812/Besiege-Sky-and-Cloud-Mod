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
        public float WaterHeight = 0;
        public float Force = 0;
        float Drag = 0;
        float AngularDrag = 0;
        // Methods
         void FixedUpdate()
        {
            if (base.GetComponent<Rigidbody>() == null)
            {
                Destroy(this);
                return;
            }
            if (base.transform.position.y < WaterHeight)
            {
                base.GetComponent<Rigidbody>().drag = Drag + 1f + Force;
                base.GetComponent<Rigidbody>().angularDrag = AngularDrag + 1f + Force;         
               if (Force>0) base.GetComponent<Rigidbody>().useGravity = false;
            }
            else
            {
                base.GetComponent<Rigidbody>().drag = Drag;
                base.GetComponent<Rigidbody>().angularDrag = AngularDrag;
                if (this.Force > 0) base.GetComponent<Rigidbody>().useGravity = true;
            }
        }
         void Start()
        {
                       
            try
            {
                this.WaterHeight = GameObject.Find("water0").transform.localPosition.y;
                if (base.GetComponent<Rigidbody>() == null)
                {
                    Destroy(this);
                    return;
                }
                if (base.GetComponent<MyBlockInfo>().blockName == "SMALL WOOD BLOCK")
                {
                    this.Force = 1f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN BLOCK")
                {
                    this.Force = 2f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN POLE")
                {
                    this.Force = 1f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN PANEL")
                {
                    this.Force = 1f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "PROPELLER")
                {
                    this.Force = 2f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "SMALL PROPELLER")
                {
                    this.Force = 1f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "WING")
                {
                    this.Force = 4f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "WING PANEL")
                {
                    this.Force = 2f * base.gameObject.transform.localScale.magnitude;
                }
                else if (base.GetComponent<MyBlockInfo>().blockName == "PROPELLOR SMALL")
                {
                    this.Force = 1f * base.gameObject.transform.localScale.magnitude;
                }
                else
                {
                    this.Force = 0;
                }
                this.Drag = base.GetComponent<Rigidbody>().drag;
                this.AngularDrag = base.GetComponent<Rigidbody>().angularDrag;

            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                Destroy(this);
            }
        }

    }
}




