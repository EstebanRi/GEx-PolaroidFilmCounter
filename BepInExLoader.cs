using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PolaroidFilmCounter
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        public const string
            MODNAME = "PolaroidFilmCounter",
            AUTHOR = "EstebanRi",
            GUID = "com." + AUTHOR + "." + MODNAME,
            VERSION = "1.0.0.0";

        public static BepInEx.Logging.ManualLogSource log;

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            log.LogMessage("Registering MainComponent in Il2Cpp");

            try
            {
                // Register our custom Types in Il2Cpp
                ClassInjector.RegisterTypeInIl2Cpp<MainComponent>();

                var go = new GameObject("MainObject");
                go.AddComponent<MainComponent>();
                Object.DontDestroyOnLoad(go);
            }
            catch
            {
                log.LogError("FAILED to Register Il2Cpp Type: MainComponent!");
            }

            try
            {
                var harmony = new Harmony(String.Format("{0}.{1}.il2cpp",AUTHOR,MODNAME));

                // Our Primary Unity Event Hooks 

                // Awake
                var originalAwake = AccessTools.Method(typeof(foncPolaroid), "Awake");
                //log.LogMessage("Harmony - Original Method: " + originalAwake.DeclaringType.Name + "." + originalAwake.Name);
                var postAwake = AccessTools.Method(typeof(MainComponent), "Awake");
                //log.LogMessage("Harmony - Postfix Method: " + postAwake.DeclaringType.Name + "." + postAwake.Name);
                harmony.Patch(originalAwake, postfix: new HarmonyMethod(postAwake));

                // Update
                var originalUpdate = AccessTools.Method(typeof(foncPolaroid), "Flash"); 
                //log.LogMessage("Harmony - Original Method: " + originalUpdate.DeclaringType.Name + "." + originalUpdate.Name);
                var postUpdate = AccessTools.Method(typeof(MainComponent), "Update");
                //log.LogMessage("Harmony - Postfix Method: " + postUpdate.DeclaringType.Name + "." + postUpdate.Name);
                harmony.Patch(originalUpdate, postfix: new HarmonyMethod(postUpdate));
                //log.LogMessage("Harmony - Runtime Patch's Applied");
            }
            catch
            {
                log.LogError("Harmony - FAILED to Apply Patch's!");
            }
        }
    }
}
