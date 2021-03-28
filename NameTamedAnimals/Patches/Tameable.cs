using NameTamedAnimals.Util;
using HarmonyLib;
using UnityEngine;
using NameTamedAnimals.Utils;
using System;
using UnityEngine.UI;

namespace NameTamedAnimals.Patches
{ 
    internal class Tameable_Patch
    {
        public static string TameableNameZDOKey = "TameableName";
        private static string TameableTextInputTopic = "Custom Name";
        private static int TameableMaxNameLength = 16;

        [HarmonyPatch(typeof(Tameable), "Awake")]
        [HarmonyPostfix]
        static void Tameable_Awake(ref Tameable __instance)
        {
            string animalName = __instance.m_nview.GetZDO().GetString(TameableNameZDOKey, "");
            if (!string.IsNullOrEmpty(animalName))
            {
                SetName(ref __instance,animalName, false);
            }
        }

        internal static void SetName(ref Tameable instance, string name, bool save)
        {
            if (!instance)
            {
                return;
            }

            if (instance.m_character)
            {
                instance.m_character.m_name = name;
            }
            
            if (instance.transform && instance.transform.GetComponent<Text>())
            {
                instance.transform.GetComponent<Text>().text = name;
            }
            if (save)
            {

                if (!PrivateArea.CheckAccess(instance.transform.position, 0f, false))
                {
                    Debug.LogWarning("Cannot set name on ZDO, you do not have access.");
                    return;
                }
                instance.m_nview.ClaimOwnership();
                instance.m_nview.GetZDO().Set(Tameable_Patch.TameableNameZDOKey, name);
            }
        }

        [HarmonyPatch(typeof(Tameable), "Interact")]
        [HarmonyPrefix]
        static bool Tameable_Interact(ref Tameable __instance, Humanoid user, bool hold)
        {

            bool validKey = Enum.TryParse(Settings.RenameModifierKey.Value, out KeyCode key);
            if (!validKey || !Input.GetKey(key))
            {
                return true;
            }

            if (!PrivateArea.CheckAccess(__instance.transform.position, 0f, false, true))
            {
                Debug.LogWarning("You don't have permission to edit this animal's name.");
                return true;
            }
            if (!__instance.m_character.IsTamed())
            {
                Debug.LogWarning("Cannot name an animal before they are tame.");
                return true;
            }

            TameableTextReciever textReciever = new TameableTextReciever(ref __instance, __instance.m_character.m_name);
            TextInput.instance.RequestText(textReciever, TameableTextInputTopic, TameableMaxNameLength);

            return false;
        }
    }
}
