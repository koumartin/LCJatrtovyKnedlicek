using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using LcJatrovyKnedlicek;
using LethalLib.Modules;
using UnityEngine;

namespace LCJatrovyKnedlicek;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalLib.Plugin.ModGUID)]
public class LcJatrovyKnedlicek : BaseUnityPlugin
{
    public static LcJatrovyKnedlicek Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;

    private static AssetBundle AssetBundle;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(assemblyLocation, "jatrovyknedlicek"));
        if (AssetBundle == null) {
            Logger.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
            return;
        }

        var item = AssetBundle.LoadAsset<Item>("Assets/Scrap/jatrovyknedlicek.asset");
        Utilities.FixMixerGroups(item.spawnPrefab);
        NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
        Items.RegisterScrap(item, 100, Levels.LevelTypes.All);

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
}
