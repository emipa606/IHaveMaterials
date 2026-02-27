using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace IHaveMaterials;

[HarmonyPatch(typeof(Designator_Build), nameof(Designator_Build.ProcessInput), typeof(Event))]
internal static class Designator_Build_ProcessInput
{
    private static readonly MethodInfo checkCanInteract =
        AccessTools.Method(typeof(Designator_Build), "CheckCanInteract");

    private static readonly FieldInfo stuffDef = AccessTools.Field(typeof(Designator_Build), "stuffDef");
    private static readonly FieldInfo writeStuff = AccessTools.Field(typeof(Designator_Build), "writeStuff");

    private static bool Prefix(Designator_Build __instance, BuildableDef ___entDef)
    {
        if (__instance.Map == null)
        {
            return true;
        }

        if (!(bool)checkCanInteract.Invoke(__instance, []))
        {
            return true;
        }

        if (___entDef is not ThingDef { MadeFromStuff: true } thingDef)
        {
            return true;
        }

        var vanilla = __instance.Map.resourceCounter.AllCountedAmounts.Keys.Where(d =>
            d != null && d.IsStuff && d.stuffProps != null && d.stuffProps.CanMake(thingDef) &&
            (DebugSettings.godMode || __instance.Map.listerThings.ThingsOfDef(d).Count > 0));

        // Non-vanilla stuff begins
        var settings = IHaveMaterialsMod.Instance?.Settings;

        var forced = settings?.Stuff
            .Where(d => d.Key != null && d.Value && d.Key.stuffProps != null && d.Key.stuffProps.CanMake(thingDef))
            .Select(d => d.Key) ?? Enumerable.Empty<ThingDef>();
        if (settings?.StonyFromMap == true)
        {
            var rockWalls = Find.World.NaturalRockTypesIn(__instance.Map.Tile);
            var rockChunks = rockWalls.Where(t => t?.building?.mineableThing != null).Select(t => t.building.mineableThing);
            var rockBlocks = rockChunks.Where(d => d != null).SelectMany(d => (d.butcherProducts ?? Enumerable.Empty<ThingDefCountClass>()).Where(p => p?.thingDef != null).Select(p => p.thingDef));
            forced = forced.Union(rockChunks.Concat(rockBlocks)
                .Where(d => d != null && d.IsStuff && d.stuffProps != null && d.stuffProps.CanMake(thingDef)));
        }

        var merged = vanilla.Union(forced).Where(d => d.stuffProps != null)
            .OrderByDescending(d => d.stuffProps.commonality)
            .ThenBy(d => d.BaseMarketValue);
        // Non-vanilla stuff ends

        var options = merged.Select(item =>
        {
            string label;

            if (__instance.sourcePrecept == null)
            {
                label = GenLabel.ThingLabel(___entDef, item);
            }
            else
            {
                label = "ThingMadeOfStuffLabel".Translate(item.LabelAsStuff ?? item.label, __instance.sourcePrecept.Label ?? "");
            }

            return new FloatMenuOption((label ?? "").CapitalizeFirst(), delegate
            {
                //BaseProcessInput();
                Find.DesignatorManager.Select(__instance);
                stuffDef.SetValue(__instance, item);
                writeStuff.SetValue(__instance, true);
            }, item)
            {
                tutorTag = $"SelectStuff-{thingDef.defName ?? ""}-{item.defName ?? ""}"
            };
        }).ToList();

        if (options.Count == 0)
        {
            return true;
        }

        var floatMenu = new FloatMenu(options)
        {
            vanishIfMouseDistant = true,
            onCloseCallback = delegate { writeStuff.SetValue(__instance, true); }
        };
        Find.WindowStack.Add(floatMenu);
        Find.DesignatorManager.Select(__instance);

        return false;
    }
}