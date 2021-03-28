using BepInEx;
using HarmonyLib;
using NameTamedAnimals.Patches;

namespace NameTamedAnimals
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGuid = AuthorName + "." + ModName;
        private const string AuthorName = "Barril";
        private const string ModName = "NameTamedAnimals";
        private const string ModVer = "1.0.0";

        public static Plugin Instance { get; private set; }
        public static BepInEx.Configuration.ConfigFile config => Instance.Config;

        public Harmony HarmonyInstance { get; private set; }

        /// <summary>
        /// Awake is called when the script instance is being loaded. 
        /// </summary>
        private void Awake()
        {
            Instance = this;
            Settings.Init();

            Log.Init(Logger);

            HarmonyInstance = Harmony.CreateAndPatchAll(typeof(Tameable_Patch), ModGuid);
        }

        /// <summary>
        /// Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.
        /// OnDestroy occurs when a Scene or game ends.
        /// It is also called when your mod is unloaded, this is where you do clean up of hooks, harmony patches,
        /// loose GameObjects and loose monobehaviours.
        /// Loose here refers to gameobjects not attached
        /// to the parent BepIn GameObject where your BaseUnityPlugin is attached
        /// </summary>
        private void OnDestroy()
        {
            HarmonyInstance.UnpatchSelf();
        }
    }
}