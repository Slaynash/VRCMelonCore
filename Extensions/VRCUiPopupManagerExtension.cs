using Il2CppSystem.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace VRCMelonCore.Extensions
{
    public static class VRCUiPopupManagerExtension
    {
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.field_Protected_VRCUiManager_0.HideScreen("POPUP"); // Old code from build 864
            
        }

        public static void ShowInputPopupWithCancel(this VRCUiPopupManager vrcUiPopupManager, string title, string content, InputField.InputType inputType, bool numeric,
            string buttonText, Action<string, List<KeyCode>, Text> buttonAction, Action cancelAction, string placeholder = "Enter text....", bool hideOnSubmit = true, Action<VRCUiPopup> onCreated = null)
        {
            Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text> action = new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>((s, code, text) =>
            {
                List<KeyCode> keyCodes = new List<KeyCode>();
                for (int i = 0; i < code.Count; ++i)
                    keyCodes.Add(code[i]);
                buttonAction?.Invoke(s, keyCodes, text);
            });
            vrcUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_1(
                title, content, inputType, numeric,
                buttonText, action,
                cancelAction ?? new Action(() => { }), placeholder, hideOnSubmit, onCreated ?? new Action<VRCUiPopup>(p => { }));
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_Action_1_VRCUiPopup_0(title, content, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_1(title, content, buttonText, buttonAction, onCreated);
        
        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_1(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);
    }
}
