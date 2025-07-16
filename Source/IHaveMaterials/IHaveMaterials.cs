using System.Collections.Generic;
using HugsLib;
using HugsLib.Settings;
using Verse;

namespace IHaveMaterials;

public class IHaveMaterials : ModBase
{
    internal static IHaveMaterials Instance;

    public readonly Dictionary<ThingDef, SettingHandle<bool>> Stuff = new();

    public SettingHandle<bool> StonyFromMap;

    public IHaveMaterials()
    {
        Instance = this;
    }

    public override string ModIdentifier => "me.lubar.IHaveMaterials";

    public override void DefsLoaded()
    {
        StonyFromMap = Settings.GetHandle(
            "stonyFromMap",
            "IHaveMaterials_stonyFromMap_title".Translate(),
            "IHaveMaterials_stonyFromMap_desc".Translate(),
            true
        );

        Stuff.Clear();

        foreach (var thing in DefDatabase<ThingDef>.AllDefs)
        {
            if (!thing.IsStuff)
            {
                continue;
            }

            Stuff.Add(thing, Settings.GetHandle(
                $"stuff_{thing.defName}",
                "IHaveMaterials_stuff_title".Translate(thing.Named("THING")),
                "IHaveMaterials_stuff_desc".Translate(thing.description.Named("DESC"),
                    (thing.modContentPack?.Name ?? "Unknown").Named("MOD")),
                thing.stuffProps.commonality >= 1
            ));
        }
    }
}