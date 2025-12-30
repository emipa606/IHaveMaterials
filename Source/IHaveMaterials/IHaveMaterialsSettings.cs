using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace IHaveMaterials;

public class IHaveMaterialsSettings : ModSettings
{
    public bool StonyFromMap = true;

    private Vector2 scrollPosition = Vector2.zero;

    // Serialized
    public Dictionary<string, bool> StuffByDefName = new();

    // Runtime cache
    public Dictionary<ThingDef, bool> Stuff = new();

    public void RebuildStuffIfNeeded()
    {
        Stuff.Clear();

        foreach (var thing in DefDatabase<ThingDef>.AllDefs)
        {
            if (!thing.IsStuff)
                continue;

            bool defaultValue = thing.stuffProps.commonality >= 1f;

            if (!StuffByDefName.TryGetValue(thing.defName, out bool enabled))
            {
                enabled = defaultValue;
                StuffByDefName[thing.defName] = enabled;
            }

            Stuff[thing] = enabled;
        }
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref StonyFromMap, "StonyFromMap", true);
        Scribe_Collections.Look(ref StuffByDefName, "StuffByDefName", LookMode.Value, LookMode.Value);

        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            StuffByDefName ??= new Dictionary<string, bool>();
            RebuildStuffIfNeeded();
        }
    }

    public void DoWindowContents(Rect inRect)
    {
        // Reserve space for scrollbar
        Rect outRect = inRect;
        Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, GetViewHeight());

        Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

        var listing = new Listing_Standard();
        listing.Begin(viewRect);

        listing.CheckboxLabeled(
            "IHaveMaterials_stonyFromMap_title".Translate(),
            ref StonyFromMap,
            "IHaveMaterials_stonyFromMap_desc".Translate()
        );

        listing.GapLine();

        foreach (var pair in Stuff.OrderByDescending(p => p.Key.stuffProps.commonality))
        {
            bool value = pair.Value;

            listing.CheckboxLabeled(
                "IHaveMaterials_stuff_title".Translate(pair.Key.Named("THING")),
                ref value,
                "IHaveMaterials_stuff_desc".Translate(
                    pair.Key.description.Named("DESC"),
                    (pair.Key.modContentPack?.Name ?? "Unknown").Named("MOD")
                )
            );

            Stuff[pair.Key] = value;
            StuffByDefName[pair.Key.defName] = value;
        }

        listing.End();
        Widgets.EndScrollView();
    }


    private float GetViewHeight()
    {
        // Header + gap
        float height = 80f;

        // Each checkbox is ~30px, descriptions increase it a bit
        height += Stuff.Count * 32f;

        return height;
    }

}
