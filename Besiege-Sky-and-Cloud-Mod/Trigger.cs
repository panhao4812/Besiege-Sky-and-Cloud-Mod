using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class MTrigger : MonoBehaviour
    {
        public int Index = -1;
        void start()
        {

        }
        void OnTriggerEnter(Collider other)
        {
            if (StatMaster.isSimulating)
            {
                if (TimeUI.Triggers != null && Index != -1)
                {
                    if (TimeUI.Triggers.Length > 0)
                    {
                       
                        if (this.Index >= 0 && this.Index < TimeUI.Triggers.Length)
                        {
                            if (TimeUI.TriggersIndex==Index-1)
                            {
                                //TimeUI.Triggers[this.Index] = true;
                                TimeUI.TriggersIndex = Index;
                            }
                        }
                    }
                }
            }
        }
    }
}