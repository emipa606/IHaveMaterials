using HarmonyLib;
using Verse;

namespace IHaveMaterials;

public class IHaveMaterialsMod : Mod
{
    public static IHaveMaterialsMod Instance;
    public IHaveMaterialsSettings Settings;

    public IHaveMaterialsMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<IHaveMaterialsSettings>();

        var harmony = new Harmony("me.lubar.IHaveMaterials");
        harmony.PatchAll();

        // Initialize after defs loaded
        LongEventHandler.ExecuteWhenFinished(DefsLoaded);        
    }

    public override string SettingsCategory()
    {
        return "IHM_ModName".Translate();
    }

    public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
    {
        Settings.DoWindowContents(inRect);
    }

    private void DefsLoaded()
    {
        Settings.RebuildStuffIfNeeded();
    }
}
