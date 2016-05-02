using spaar.ModLoader.UI;
using System;
using System.Timers;
using UnityEngine;

namespace Besiege_Sky_and_Cloud_Mod
{
    public class TimeUI : MonoBehaviour
    {
        public static bool[] Triggers;
        public static int TriggersIndex=-1;
        private int windowID = spaar.ModLoader.Util.GetWindowID();
        private Rect windowRect = new Rect(0f, 240f, 150f, 120f);
        public bool ShowGUI = true; public bool AutoMode = false;

        private string RealTime = ""; private string Mtimer = ""; 
        private DateTime m_StartTime;  private string IndexSlider = "";
        private bool TimerON = false;
       

        void Start()
        {
            m_StartTime = System.DateTime.Now;          
            RealTime = m_StartTime.ToShortDateString();
            Mtimer = "00:00:00:00";
            IndexSlider = "0 / 0"; 
            
        }
        void DoWindow(int windowID)
        {
            GUIStyle style0 = new GUIStyle
            {
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleLeft,
                margin = { top = 5 },
                fontSize = 15
            };
            GUIStyle style = new GUIStyle
            {
                //font = GUI.skin.font, 
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                active = { background = Texture2D.whiteTexture , textColor = Color.black},                   
                margin = { top = 5 },
                fontSize = 15            
            };

            GUILayout.BeginVertical(new GUILayoutOption[0]);

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("[模式]", style, new GUILayoutOption[0]);
            GUILayout.Label(RealTime, style, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("[开始/暂停]", style, new GUILayoutOption[0]))
            {
                if (!AutoMode)
                {
                    TimerON = !TimerON;
                }
            }
            if (GUILayout.Button("[归零]", style, new GUILayoutOption[0]))
            {
               
                TimerON = false;
                m_StartTime = System.DateTime.Now;
                Mtimer = "00:00:00:00";
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
         //   if (GUILayout.Button("[自动/手动]", style, new GUILayoutOption[0]))
          //  {
        //        AutoMode = !AutoMode;
        //    }
            GUILayout.Label("   "+Mtimer, style0, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal(new GUILayoutOption[0]);
          //  GUILayout.Label("计时点", style, new GUILayoutOption[0]);
          //  GUILayout.Label(IndexSlider, style, new GUILayoutOption[0]);
          //  GUILayout.EndHorizontal();

          
            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }

        void OnGUI()
        {
            //GUI.skin = ModGUI.Skin;
            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }

        }
        int AccStep = 0;
        void timerStep()
        {
            TimeSpan span = DateTime.Now - m_StartTime;
            DateTime n = new DateTime(span.Ticks);
            Mtimer = n.ToString("HH:mm:ss:ff");
        }
        void FixedUpdate()
        {
            if (ShowGUI)
            {
                RealTime = System.DateTime.Now.ToString("HH:mm:ss");
                if (TimerON)
                {
                    if (AccStep == 2) timerStep();
                    else if (AccStep == 4) timerStep();
                    else if (AccStep == 6) timerStep();
                    else if (AccStep == 8) timerStep();
                    else if (AccStep >= 10)
                    {
                        timerStep();
                        if (this.IndexSlider != null)
                        {
                            if (this.IndexSlider.Length > 0)
                            {
                                IndexSlider = TriggersIndex.ToString()+" / "+ Triggers.Length.ToString();
                            }
                        }
                        AccStep = 0;
                    }
                    AccStep++;
                }
            }
        }
    }
}
