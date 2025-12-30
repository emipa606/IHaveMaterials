using System.IO;
using System.Reflection;
using System.Xml.Linq;
using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace IHaveMaterials;

public class IHaveMaterialsMod : Mod
{
    public static IHaveMaterialsMod Instance;
    public static string CurrentVersion;
    public readonly IHaveMaterialsSettings Settings;

    public IHaveMaterialsMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<IHaveMaterialsSettings>();
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);

        new Harmony("me.lubar.IHaveMaterials").PatchAll(Assembly.GetExecutingAssembly());
        importOldHugsLibSettings();
        // Initialize after defs loaded
        LongEventHandler.ExecuteWhenFinished(defsLoaded);
    }

    public override string SettingsCategory()
    {
        return "I Have Materials";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoWindowContents(inRect);
    }

    private static void importOldHugsLibSettings()
    {
        var hugsLibConfig = Path.Combine(GenFilePaths.SaveDataFolderPath, "HugsLib", "ModSettings.xml");
        if (!new FileInfo(hugsLibConfig).Exists)
        {
            return;
        }

        var xml = XDocument.Load(hugsLibConfig);
        var modNodeName = "me.lubar.IHaveMaterials";

        var modSettings = xml.Root?.Element(modNodeName);
        if (modSettings == null)
        {
            return;
        }

        foreach (var modSetting in modSettings.Elements())
        {
            if (modSetting.Name == "stonyFromMap")
            {
                Instance.Settings.StonyFromMap = bool.Parse(modSetting.Value);
                continue;
            }

            if (!modSetting.Name.ToString().StartsWith("stuff_"))
            {
                continue;
            }

            var defName = modSetting.Name.ToString()[6..];
            Instance.Settings.StuffByDefName[defName] = bool.Parse(modSetting.Value);
        }

        Instance.Settings.Write();
        xml.Root.Element(modNodeName)?.Remove();
        xml.Save(hugsLibConfig);

        Log.Message($"[{modNodeName}]: Imported old HugLib-settings");
    }

    private void defsLoaded()
    {
        Settings.RebuildStuffIfNeeded();
    }
}