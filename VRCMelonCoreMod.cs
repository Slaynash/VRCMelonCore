﻿using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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





        private void OnApplicationStart()
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
                MelonModLogger.LogError("Discarting BeforeFlowManager process, since there is no queued methods.");
                preFlowManagerProcessed = true;
                yield break;
            }

            // Get VRCFlowManagerVRC and disable it, plus stop all running coroutine
            VRCFlowManager flowManager = VRCFlowManager.field_VRCFlowManager_0;
            flowManager.StopAllCoroutines();
            flowManager.enabled = false;

            // Load the 'Ui' scene
            if (GameObject.Find("UserInterface") == null)
            {
                MelonModLogger.Log("Loading additive scene \"ui\"");
                SceneManager.LoadScene("ui", LoadSceneMode.Single);
            }

            // Wait for VRCUiManager to load
            while (VRCUiManager.field_VRCUiManager_0 == null)
                yield return null;

            // Process all sheduled actions before flow Manager init
            while (preFlowManagerQueue.Count > 0)
                yield return preFlowManagerQueue.Dequeue();

            preFlowManagerProcessed = true;

            // Enable VRCFlowManager back, and start the routine
            flowManager.enabled = true;
            flowManager.Start();
        }
    }
}
