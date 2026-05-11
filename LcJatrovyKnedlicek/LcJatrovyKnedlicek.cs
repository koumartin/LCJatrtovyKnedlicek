using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using LcJatrovyKnedlicek;
using LethalLib.Modules;
using UnityEngine;

namespace LCJatrovyKnedlicek;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalLib.Plugin.ModGUID, BepInDependency.DependencyFlags.HardDependency)]
public class LcJatrovyKnedlicek : BaseUnityPlugin
{
    public static LcJatrovyKnedlicek Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;

    private static AssetBundle? _assetBundle;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _assetBundle = AssetBundle.LoadFromFile(Path.Combine(assemblyLocation, "jatrovyknedlicek"));
        if (_assetBundle == null) {
            Logger.LogError("Failed to load custom assets."); // ManualLogSource for your plugin
            return;
        }

        var item = _assetBundle.LoadAsset<Item>("Assets/Scrap/");
        Utilities.FixMixerGroups(item.spawnPrefab);
        NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
        Items.RegisterScrap(item, 100, Levels.LevelTypes.All);

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
}
