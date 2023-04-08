using DestroyKingdom.Actions.KingdomAnnexation;
using DestroyKingdom.Conditions;
using DestroyKingdom.Extensions;
using DestroyKingdom.Shared;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace DestroyKingdom.CampaignBehaviors
{
    internal class PlayerRulerAnnexationConversationCampaignBehavior : CampaignBehaviorBase
    {
        private string AnnexationRequestedToken = "annexation_requested";
        private string FirstReasonToken = "annexation_give_first_reason";
        private string AfterFirstReasonToken = "annexation_need_second_reason";
        private string SecondReasonToken = "annexation_give_second_reason";
        private string AfterSecondReasonToken = "annexation_need_third_reason";
        private string ThirdReasonToken = "annexation_give_third_reason";
        private string AfterThirdReasonToken = "annexation_need_fourth_reason";
        private string FourthReasonToken = "annexation_give_fourth_reason";
        private string OathStartToken = "annexation_oath";

        public override void RegisterEvents() => CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener((object)this, new Action<CampaignGameStarter>(PlayerRulerAnnexationConversationCampaignBehavior.AddDialogues));

        public override void SyncData(IDataStore dataStore)
        {
        }

        private static void AddDialogues(CampaignGameStarter starter)
        {
            starter.AddPlayerLine("annexation_demand", "hero_main_options", "annexation_requested", "{=DK_040}I demand you and {HERO_KINGDOM} to recognize my authority as {HERO_KINGDOM_TITLE} of {HERO_KINGDOM_CULTURE}.", new ConversationSentence.OnConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.PlayerIsRulerAndHeroIsRulerCondition));
            starter.AddDialogLine("annexation_ask_first_reason", "annexation_requested", "annexation_give_first_reason", "{=DK_041}And why should I do that?");
            starter.AddPlayerLine("annexation_strength", "annexation_give_first_reason", "annexation_need_second_reason", "{=DK_042}{PLAYER_KINGDOM} is far stronger than {HERO_KINGDOM}.", clickableCondition: new ConversationSentence.OnClickableConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.HeroKingdomStrengthClickableCondition));
            starter.AddPlayerLine("annexation_no_reasons", "annexation_give_first_reason", "lord_pretalk", "{=DK_043}Nevermind.");
            starter.AddDialogLine("annexation_ask_second_reason", "annexation_need_second_reason", "annexation_give_second_reason", "{=DK_044}That's true, but I would need more reasons for that kind of decision.");
            starter.AddPlayerLine("annexation_low_fiefs", "annexation_give_second_reason", "annexation_need_third_reason", "{HERO_KINGDOM} {LOW_FIEFS_DESCRIPTION}.", clickableCondition: new ConversationSentence.OnClickableConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.HeroKingdomLowFiefsClickableCondition));
            starter.AddPlayerLine("annexation_only_one_reason", "annexation_give_second_reason", "lord_pretalk", "{=DK_045}Nevermind.");
            starter.AddDialogLine("annexation_ask_third_reason", "annexation_need_third_reason", "annexation_give_third_reason", "{=DK_046}But why should YOU be the one to rule {HERO_KINGDOM_CULTURE} people?");
            starter.AddPlayerLine("annexation_fiefs_control", "annexation_give_third_reason", "annexation_need_fourth_reason", "{=DK_047}{PLAYER_KINGDOM} is controlling big part of {HERO_KINGDOM_CULTURE} lands.", new ConversationSentence.OnConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.PlayerAnyTraitsCondition), clickableCondition: new ConversationSentence.OnClickableConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.PlayerControllingCultureTownsClickableCondition));
            starter.AddPlayerLine("annexation_fiefs_control_no_traits", "annexation_give_third_reason", "annexation_oath", "{=DK_048}{PLAYER_KINGDOM} is controlling big part of {HERO_KINGDOM_CULTURE} lands.", new ConversationSentence.OnConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.PlayerNoTraitsCondition), clickableCondition: new ConversationSentence.OnClickableConditionDelegate(PlayerRulerAnnexationConversationCampaignBehavior.PlayerControllingCultureTownsClickableCondition));
            starter.AddPlayerLine("annexation_only_two_reasons", "annexation_give_third_reason", "lord_pretalk", "{=DK_049}Nevermind.");
            starter.AddDialogLine("annexation_ask_fourth_reason", "annexation_need_fourth_reason", "annexation_give_fourth_reason", "{=DK_050}Go on.");
            starter.AddPlayerLine("annexation_honor", "annexation_give_fourth_reason", "annexation_oath", "{=DK_051}You can trust my word - I will be taking care of the {HERO_KINGDOM_CULTURE}.", new ConversationSentence.OnConditionDelegate(PlayerTraitCondition.Honorable));
            starter.AddPlayerLine("annexation_cruel", "annexation_give_fourth_reason", "annexation_oath", "{=DK_052}If you don't recognize my authority I will kill you and all your supporters.", new ConversationSentence.OnConditionDelegate(PlayerTraitCondition.Cruel));
            starter.AddPlayerLine("annexation_generous", "annexation_give_fourth_reason", "annexation_oath", "{=DK_053}I will generously reward everyone who will be faithful to me.", new ConversationSentence.OnConditionDelegate(PlayerTraitCondition.Generous));
            starter.AddPlayerLine("annexation_fearless", "annexation_give_fourth_reason", "annexation_oath", "{=DK_054}Many times I have shown my value as a fearless leader on the battlefields.", new ConversationSentence.OnConditionDelegate(PlayerTraitCondition.Fearless));
            starter.AddPlayerLine("annexation_only_three_reasons", "annexation_give_fourth_reason", "lord_pretalk", "{=DK_055}Nevermind.");
            starter.AddDialogLine("annexation_oath_text", "annexation_oath", "annexation_oath_2", "{=DK_056}This is hard decision, but after considering all circumstances that's the only thing that I can do.");
            starter.AddDialogLine("annexation_oath_text_2", "annexation_oath_2", "annexation_oath_3", "{=DK_057}I swear homage to you as lawful liege of mine.");
            starter.AddDialogLine("annexation_oath_text_3", "annexation_oath_3", "annexation_oath_4", "{=DK_058}I will be at your side to fight your enemies should you need my sword.");
            starter.AddDialogLine("annexation_oath_text_4", "annexation_oath_4", "annexation_oath_5", "{=DK_059}And I shall defend your rights and the rights of your legitimate heirs.");
            starter.AddDialogLine("annexation_oath_text_5", "annexation_oath_5", "lord_pretalk", "{=DK_060}You are now my {HERO_KINGDOM_TITLE}.", consequence: new ConversationSentence.OnConsequenceDelegate(KingdomAnnexationAction.ApplyByPlayerConversation));
        }

        private static void SetTextVariables()
        {
            Kingdom kingdom1 = Hero.OneToOneConversationHero.Clan?.Kingdom;
            Kingdom kingdom2 = Hero.MainHero.Clan?.Kingdom;
            if (kingdom1 == null || kingdom2 == null)
                return;
            //kingdom1.Fiefs.IsEmpty<Town>
            string text = kingdom1.Fiefs.IsEmpty<Town>() ? new TextObject("{=DK_061}does not control any fiefs").ToString() : new TextObject("{=DK_062}controls very few fiefs").ToString();
            MBTextManager.SetTextVariable("HERO_KINGDOM_TITLE", PlayerRulerAnnexationConversationCampaignBehavior.GetHeroFactionRulerText(), false);
            MBTextManager.SetTextVariable("PLAYER_KINGDOM", kingdom2.Name, false);
            MBTextManager.SetTextVariable("HERO_KINGDOM", kingdom1.Name, false);
            MBTextManager.SetTextVariable("HERO_KINGDOM_CULTURE", kingdom1.Culture.Name, false);
            MBTextManager.SetTextVariable("LOW_FIEFS_DESCRIPTION", text, false);
        }

        private static bool PlayerNoTraitsCondition() => !PlayerRulerAnnexationConversationCampaignBehavior.PlayerAnyTraitsCondition();

        private static bool PlayerAnyTraitsCondition() => PlayerTraitCondition.Fearless() || PlayerTraitCondition.Generous() || PlayerTraitCondition.Cruel() || PlayerTraitCondition.Honorable();

        private static TextObject GetHeroFactionRulerText()
        {
            string appendant = Hero.MainHero.IsFemale ? "_f" : "";
            Clan clan = Hero.OneToOneConversationHero.Clan;
            string variation;
            if (clan == null)
            {
                variation = (string)null;
            }
            else
            {
                Kingdom kingdom = clan.Kingdom;
                if (kingdom == null)
                {
                    variation = (string)null;
                }
                else
                {
                    CultureObject culture = kingdom.Culture;
                    if (culture == null)
                    {
                        variation = (string)null;
                    }
                    else
                    {
                        string stringId = culture.StringId;
                        variation = stringId != null ? stringId.Add(appendant, false) : (string)null;
                    }
                }
            }
            return GameTexts.FindText("str_faction_ruler", variation);
        }

        private static bool HeroKingdomStrengthClickableCondition(out TextObject? explanation)
        {
            Kingdom kingdom1 = Hero.OneToOneConversationHero?.Clan?.Kingdom;
            Kingdom kingdom2 = Hero.MainHero?.Clan?.Kingdom;
            explanation = (TextObject)null;
            if (kingdom1 == null || kingdom2 == null)
                return false;
            int num = KingdomExtensions.KingdomsStrengthRatio(kingdom1, kingdom2);
            bool flag = num < Settings.AnnexedKingdomMaxStrengthRatio;

            if (num > 100)
                explanation = new TextObject("{=DK_063}{HERO_KINGDOM} is stronger than {PLAYER_KINGDOM}.");
            else
            {
                if (!flag)
                {
                    explanation = new TextObject("{=DK_064}{KINGDOM1} has {NUM}% of {KINGDOM2} strengt. Needs to be less than {ANEX}% ");
                    explanation.SetTextVariable("KINGDOM1", kingdom1.Name.ToString());
                    explanation.SetTextVariable("NUM", num.ToString());
                    explanation.SetTextVariable("KINGDOM2", kingdom2.Name.ToString());
                    explanation.SetTextVariable("ANEX", Settings.AnnexedKingdomMaxStrengthRatio.ToString());
                }
            }
            //explanation = new TextObject(string.Format("{0} has {1}% of {2} strength. Needs to be less than {3}%.", (object) kingdom1.Name, (object) num, (object) kingdom2.Name, (object) Settings.AnnexedKingdomMaxStrengthRatio));
            return flag;
        }

        private static bool PlayerControllingCultureTownsClickableCondition(out TextObject? explanation)
        {
            explanation = (TextObject)null;
            int controlledFiefsPercentage;
            int requiredFiefsPercentage;
            bool flag = KingdomAnnexationCondition.ControllingEnoughCultureLands(Hero.MainHero?.Clan?.Kingdom, Hero.OneToOneConversationHero?.Clan?.Kingdom, out controlledFiefsPercentage, out requiredFiefsPercentage);
            if (!flag)
            {
                TextObject textObject = Hero.OneToOneConversationHero?.Clan?.Kingdom?.Culture?.Name ?? TextObject.Empty;
                explanation = new TextObject("{=DK_065}You are controlling {PORC}% of {NAME} fiefs ({NEED}% required).");
                explanation.SetTextVariable("PORC", controlledFiefsPercentage.ToString());
                explanation.SetTextVariable("NAME", textObject.ToString());
                explanation.SetTextVariable("NEED", requiredFiefsPercentage.ToString());

                //explanation = new TextObject(string.Format("You are controlling {0}% of {1} fiefs ({2}% required).", (object)controlledFiefsPercentage, (object)textObject, (object)requiredFiefsPercentage));
            }
            return flag;
        }

        private static bool HeroKingdomLowFiefsClickableCondition(out TextObject? explanation)
        {
            Kingdom kingdom = Hero.OneToOneConversationHero?.Clan?.Kingdom;
            explanation = (TextObject)null;
            if (kingdom == null)
                return false;
            int count = kingdom.Fiefs.Count;
            int kingdomMaxFiefsAmount = Settings.AnnexedKingdomMaxFiefsAmount;
            bool flag = count <= kingdomMaxFiefsAmount;
            if (!flag)
            {
                explanation = new TextObject("{=DK_066}{KINGDOM1} is controlling {COUNT} fiefs (maximum {MAX}).");
                explanation.SetTextVariable("KINGDOM1", kingdom.Name.ToString());
                explanation.SetTextVariable("COUNT", count.ToString());
                explanation.SetTextVariable("MAX", kingdomMaxFiefsAmount.ToString());

                //explanation = new TextObject(string.Format("{0} is controlling {1} fiefs (maximum {2}).", (object)kingdom.Name, (object)count, (object)kingdomMaxFiefsAmount));
            }

            return flag;

        }

        private static bool PlayerIsRulerAndHeroIsRulerCondition()
        {
            bool flag = Hero.OneToOneConversationHero.IsRulerOfKingdom() && Hero.MainHero.IsRulerOfKingdom();
            if (flag)
                PlayerRulerAnnexationConversationCampaignBehavior.SetTextVariables();
            return flag;
        }
    }
}