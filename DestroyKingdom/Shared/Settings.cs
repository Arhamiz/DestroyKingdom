using System;
using System.Diagnostics.CodeAnalysis;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Localization;

namespace DestroyKingdom.Shared;

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
internal class Settings : AttributeGlobalSettings<Settings>
{

    private string GroupNameAnnexationConditions = new TextObject("{=DK_001}Annexation conditions").ToString(); // private const
    private int DefaultAnnexedKingdomMaxFiefsAmounts = 0;
    private float DefaultAnnexedKingdomMaxStrengthRatio = 0.25f;
    private float DefaultAnnexingKingdomMinCultureFiefsPercentage = 0.4f;
    private string GroupNameReducedExecutionRelationshipPenalty = new TextObject("{=DK_002}Reduced rebel execution relationship penalty").ToString();
    private int DefaultRebelExecutionRelationPenaltyDenominatorAnnexing = 5;
    private int DefaultRebelExecutionRelationPenaltyDenominatorOthers = 3;
    private string GroupNameCollaborationChances = new TextObject("{=DK_003}Chances of collaboration").ToString();
    private float DefaultMinimumCollaborationChance = 0.1f;
    private float DefaultMaximumCollaborationChance = 0.5f;

    public override string Id => "Annexation_Settings";

    public override string DisplayName => new TextObject("{=DK_004}Annexation").ToString();

    public override string FolderName => "Annexation";

    public override string FormatType => "json2";

    [SettingPropertyInteger("{=DK_005}Maximum amount of fiefs of annexed kingdom", 0, 5, "0", HintText = "{=DK_006}Kingdom can be annexed only if it has less fiefs than configured by this value", Order = 0, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_007}Annexation conditions")]
    private int AnnexedKingdomMaxFiefsAmountInternal { get; set; } = 0;

    public static int AnnexedKingdomMaxFiefsAmount
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return instance == null ? 0 : instance.AnnexedKingdomMaxFiefsAmountInternal;
        }
    }

    [SettingPropertyFloatingInteger("{=DK_008}Maximum relative strength percentage of annexed kingdom", 0.1f, 0.5f, "0%", HintText = "{=DK_009}Kingdom can be annexed only if it it's total strength divided by your annexing kingdom strength is smaller than this value", Order = 1, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_010}Annexation conditions")]
    private float AnnexedKingdomMaxStrengthRatioInternal { get; set; } = 0.25f;

    public static int AnnexedKingdomMaxStrengthRatio
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return (int)Math.Round((instance != null ? (double)instance.AnnexedKingdomMaxStrengthRatioInternal : 0.25) * 100.0, 0);
        }
    }

    [SettingPropertyFloatingInteger("{=DK_011}Minimum culture fiefs", 0.0f, 1f, "0%", HintText = "{=DK_012}Kingdom can annex other kingdom only if percentage of fiefs that have annexed kingdom's culture controlled by this annexing kingdom is at least as big as this value", Order = 2, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_013}Annexation conditions")]
    private float AnnexingKingdomMinCultureFiefsPercentageInternal { get; set; } = 0.4f;

    public static int AnnexingKingdomMinCultureFiefsPercentage
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return (int)Math.Round((instance != null ? (double)instance.AnnexingKingdomMinCultureFiefsPercentageInternal : 0.40000000596046448) * 100.0, 0);
        }
    }

    [SettingPropertyInteger("{=DK_014}Execution relation penalty denominator (annexing kingdom)", 1, 10, "0", HintText = "{=DK_015}Relation penalty with nobles of annexing kingdom after executing rebel nobles of annexed kingdom will be divided by this number", Order = 3, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_016}Reduced rebel execution relationship penalty")]
    private int RebelExecutionRelationPenaltyDenominatorAnnexingInternal { get; set; } = 5;

    public static int RebelExecutionRelationPenaltyDenominatorAnnexing
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return instance == null ? 5 : instance.RebelExecutionRelationPenaltyDenominatorAnnexingInternal;
        }
    }

    [SettingPropertyInteger("{=DK_017}Execution relation penalty denominator (other kingdoms)", 1, 10, "0", HintText = "{=DK_018}Relation penalty with nobles of other kingdoms after executing rebel nobles of annexed kingdom will be divided by this number", Order = 4, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_019}Reduced rebel execution relationship penalty")]
    private int RebelExecutionRelationPenaltyDenominatorOthersInternal { get; set; } = 3;

    public static int RebelExecutionRelationPenaltyDenominatorOthers
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return instance == null ? 3 : instance.RebelExecutionRelationPenaltyDenominatorOthersInternal;
        }
    }

    [SettingPropertyFloatingInteger("{=DK_020}Minimum chance of collaboration", 0.1f, 1f, "0%", HintText = "{=DK_021}Minimum chance of vassal clans of annexed kingdom to join your kingdom after annexation (doesn't apply to ruler which will always join and mercenaries that will just terminate their contract).", Order = 5, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_022}Chances of collaboration")]
    private float MinimumCollaborationChanceInternal { get; set; } = 0.1f;

    public static int MinimumCollaborationChance
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return (int)Math.Round((instance != null ? (double)instance.MinimumCollaborationChanceInternal : 0.10000000149011612) * 100.0, 0);
        }
    }

    [SettingPropertyFloatingInteger("{=DK_023}Maximum chance of collaboration", 0.1f, 1f, "0%", HintText = "{=DK_024}Maximum chance of vassal clans of annexed kingdom to join your kingdom after annexation - the better your relations with lords of the kingdom are, the higher the chance they will join you. This value will be ignored if it's lower than minimum one.", Order = 6, RequireRestart = false)]
    [SettingPropertyGroup("{=DK_025}Chances of collaboration")]
    private float MaximumCollaborationChanceInternal { get; set; } = 0.5f;

    public static int MaximumCollaborationChance
    {
        get
        {
            Settings instance = GlobalSettings<Settings>.Instance;
            return (int)Math.Round((instance != null ? (double)instance.MaximumCollaborationChanceInternal : 0.5) * 100.0, 0);
        }
    }

}