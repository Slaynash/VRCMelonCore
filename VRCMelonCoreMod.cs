using Il2CppSystem.Reflection;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.Core;

namespace VRCMelonCore
{
    public class VRCMelonCoreMod : MelonMod
    {
        private static Queue<IEnumerator> preFlowManagerQueue = new Queue<IEnumerator>();
        private static bool preFlowManagerProcessed = false;

        public static void RunBeforeFlowManager(IEnumerator func)
        {
            if(preFlowManagerProcessed)
            {
                MelonModLogger.LogError("Trying run a method before FlowManager, but FlowManager has already been ran");
                return;
            }

            preFlowManagerQueue.Enqueue(func);
        }





        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(ProcessRunBeforeFlowManager());
        }

        private IEnumerator ProcessRunBeforeFlowManager()
        {
            // Wait for the 'Application2' scene (the one with VRCFlowManagerVRC)
            while (SceneManager.GetActiveScene().buildIndex != 0)
                yield return null;

            // Discard if there is nothing queued
            if(preFlowManagerQueue.Count == 0)
            {
                MelonModLogger.LogError("Discarting PreFlowManager process, since there is no queued methods.");
                preFlowManagerProcessed = true;
                yield break;
            }

            // Get VRCFlowManagerVRC and disable it, plus stop all running coroutine
            VRCFlowManagerVRC flowManager = VRCFlowManagerVRC.field_Private_VRCFlowManager_0.Cast<VRCFlowManagerVRC>();
            flowManager.StopAllCoroutines();

            // Load the 'Ui' scene
            if (GameObject.Find("UserInterface") == null)
            {
                MelonModLogger.Log("Loading additive scene \"ui\"");
                SceneManager.LoadScene("ui", LoadSceneMode.Single);
            }

            MelonModLogger.Log("Waiting for VRCUiManager...");
            // Wait for VRCUiManager to load
            while (VRCUiManager.field_Protected_VRCUiManager_0 == null)
                yield return null;

            MelonModLogger.Log("Processing pre-FlowManager routines");
            // Process all sheduled actions before flow Manager init
            while (preFlowManagerQueue.Count > 0)
            {
                MelonModLogger.Log("Remaining coroutines in queue: " + preFlowManagerQueue.Count);
                yield return preFlowManagerQueue.Dequeue();
            }

            MelonModLogger.Log("Done! Starting game");
            preFlowManagerProcessed = true;

            // Start the VRCFlowManagerVRC Coroutine
            Type[] types = typeof(VRCFlowManagerVRC).GetNestedTypes().Where(t => t.GetMethod("MoveNext") != null).ToArray();
            Type vrcFlowManagerEnumeratorType = types[0].GetProperties().Length > types[1].GetProperties().Length ? types[1] : types[0];
            System.Reflection.ConstructorInfo constructor = vrcFlowManagerEnumeratorType.GetConstructor(new Type[] { typeof(int) });
            object o = constructor.Invoke(new object[] { 0 });
            o.GetType().GetProperty("field_Public_ArrayOf_Nested0_0").SetValue(o, (Il2CppStructArray<VRCFlowManager.Nested0>) new VRCFlowManager.Nested0[] { VRCFlowManager.Nested0.ShowUI });
            o.GetType().GetProperty("field_Public_VRCFlowManagerVRC_0").SetValue(o, flowManager);
            flowManager.StartCoroutine(((Il2CppSystem.Object)o).Cast<Il2CppSystem.Collections.IEnumerator>());

            MelonModLogger.Log("FlowManager coroutine started");
        }
    }
}
