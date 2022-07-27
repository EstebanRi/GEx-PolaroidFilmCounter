using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Input = BepInEx.IL2CPP.UnityEngine.Input;
using CodeStage.AntiCheat.ObscuredTypes;

namespace PolaroidFilmCounter
{


        public class MainComponent : MonoBehaviour
        {
            public MainComponent(IntPtr ptr) : base(ptr)
            {
            }


            // Harmony Patch's must be static!
            [HarmonyPostfix]
            public static void Awake(ref foncPolaroid __instance)
            {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane.name = "FilmCounter";
            plane.transform.parent = __instance.transform;
            plane.GetComponent<Renderer>().material.shader = Shader.Find("HDRP/Lit");
            plane.GetComponent<Renderer>().material.color = Color.black;
            plane.transform.localScale = new Vector3(0.02f, 0.02f, 0.001f);
            plane.transform.localRotation = Quaternion.Euler(0, 0, 0);
            plane.transform.localPosition = new Vector3(0.095f, 0f, 0.045f);

            // Create Canvas GameObject.
            GameObject canvasGO = new GameObject();
            canvasGO.transform.parent = plane.transform;
            canvasGO.name = "FilmCounterCanvas";
            canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            canvasGO.transform.localPosition = new Vector3(0, 0, 0);

            // Get canvas from the GameObject.
            Canvas canvas;
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
            canvas.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            canvas.transform.localPosition = new Vector3(0, 0, 1);

            // Create the Text GameObject.
            GameObject textGO = new GameObject();
            textGO.transform.parent = canvasGO.transform;
            textGO.transform.localScale = new Vector3(1, 1, 1);
            textGO.AddComponent<Text>();
            

            // Set Text component properties.
            Text text = textGO.GetComponent<Text>();
            text.name = "CounterNumber";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = __instance.nb.fakeValue.ToString();
            text.fontSize = 48;
            text.color = Color.red;
            text.alignment = TextAnchor.MiddleCenter;            
            

            // Provide Text position and size using RectTransform.
            RectTransform rectTransform;
            rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(600, 200);

        }

        [HarmonyPostfix]
            public static void Start()
            {

            }

            [HarmonyPostfix]
            public static void Update(ref foncPolaroid __instance)
            {

                BepInExLoader.log.LogMessage("FLASH!");
                BepInExLoader.log.LogMessage(__instance.nb.fakeValue.ToString());
                __instance.transform.Find("FilmCounter").Find("FilmCounterCanvas").Find("CounterNumber").GetComponent<Text>().text = __instance.nb.fakeValue.ToString();

                Event.current.Use();
                
            }
        }
    
}
