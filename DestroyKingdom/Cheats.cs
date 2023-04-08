using System;
using System.Collections.Generic;
using System.Linq;
using DestroyKingdom.Data;
using DestroyKingdom.Extensions;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
namespace DestroyKingdom;

public class Cheats
{
    private string VassalizeAllRebelsInfo = new TextObject("{=DK_026}You need to provide kingdom name as a parameter (spaces are ignored): 'annexation.vassalize_all_rebels [kingdom]'.").ToString();
    private string VassalizeClanInfo = new TextObject("{=DK_027}You need to provide kingdom name and clan name as a parameters (spaces are ignored): 'annexation.vassalize_clan [kingdom] [clan]'.").ToString();

    [CommandLineFunctionality.CommandLineArgumentFunction("vassalize_all_rebels", "{=DK_028}annexation")]
    [UsedImplicitly]
    public static string VassalizeAllRebels(List<string> strings)
    {
        if (strings.IsEmpty<string>())
            return new TextObject("{=DK_029}You need to provide kingdom name as a parameter (spaces are ignored): 'annexation.vassalize_all_rebels [kingdom]'.").ToString();
        Kingdom kingdom1 = KingdomExtensions.AllActiveKingdomsFactions().Find((Predicate<Kingdom>)(kingdom => kingdom.Name.ToString().ToLower().Replace(" ", "") == strings[0]));
        if (kingdom1 == null)
            return new TextObject("{=DK_030}Couldn't find kingdom with ").ToString() + strings[0] + new TextObject("{=DK_031} name. You need to provide kingdom name as a parameter (spaces are ignored): 'annexation.vassalize_all_rebels [kingdom]'.").ToString();
        List<Clan> list = Clan.All.Where<Clan>((Func<Clan, bool>)(clan =>
        {
            if (clan.IsEliminated)
                return false;
            AnnexationRebelClansStorage instance = AnnexationRebelClansStorage.Instance;
            return instance != null && instance.IsRebelClanAgainstAnnexingKingdom(clan, kingdom1);
        })).ToList<Clan>();
        foreach (Clan clan in list)
        {
            if (clan.GetStanceWith((IFaction)kingdom1).IsAtWar)
                MakePeaceAction.Apply((IFaction)clan, (IFaction)kingdom1);
            ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom1);
        }
        TextObject txt1 = new TextObject("{=DK_032}{NAME} clans without kingdom joined {KINGDOM}.");
        txt1.SetTextVariable("NAME", list.Count.ToString());
        txt1.SetTextVariable("KINGDOM", kingdom1.Name.ToString());
        return txt1.ToString(); //string.Format("{0} clans without kingdom joined {1}.", (object)list.Count, (object)kingdom1.Name);
    }

    [CommandLineFunctionality.CommandLineArgumentFunction("vassalize_clan", "{=DK_033}annexation")]
    [UsedImplicitly]
    public static string VassalizeClan(List<string> strings)
    {
        if (strings.Count < 2)
            return new TextObject("{=DK_034}You need to provide kingdom name and clan name as a parameters (spaces are ignored): 'annexation.vassalize_clan [kingdom] [clan]'.").ToString();
        Kingdom kingdom1 = KingdomExtensions.AllActiveKingdomsFactions().Find((Predicate<Kingdom>)(kingdom => kingdom.Name.ToString().ToLower().Replace(" ", "") == strings[0]));
        if (kingdom1 == null)
            return new TextObject("{=DK_035}Couldn't find kingdom with ").ToString() + strings[0] + new TextObject("{=DK_036} name. You need to provide kingdom name and clan name as a parameters (spaces are ignored): 'annexation.vassalize_clan [kingdom] [clan]'.").ToString();
        Clan clan1 = Clan.All.ToList<Clan>().Find((Predicate<Clan>)(clan => clan.Name.ToString().ToLower().Replace(" ", "") == strings[1]));
        if (clan1 == null)
            return new TextObject("{=DK_037}Couldn't find clan with ").ToString() + strings[1] + new TextObject("{=DK_038} name. You need to provide kingdom name and clan name as a parameters (spaces are ignored): 'annexation.vassalize_clan [kingdom] [clan]'.").ToString();
        if (clan1.GetStanceWith((IFaction)kingdom1).IsAtWar)
            MakePeaceAction.Apply((IFaction)clan1, (IFaction)kingdom1);
        ChangeKingdomAction.ApplyByJoinToKingdom(clan1, kingdom1);
        TextObject txt1 = new TextObject("{=DK_039}{NAME} clan joined {KINGDOM}.");
        txt1.SetTextVariable("NAME", clan1.Name.ToString());
        txt1.SetTextVariable("KINGDOM", kingdom1.Name.ToString());
        return txt1.ToString(); // string.Format("{0} clan joined {1}.", (object)clan1.Name, (object)kingdom1.Name);
    }

}