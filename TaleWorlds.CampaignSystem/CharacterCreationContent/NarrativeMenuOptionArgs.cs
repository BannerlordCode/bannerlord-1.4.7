using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000219 RID: 537
	public class NarrativeMenuOptionArgs
	{
		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x0009135E File Offset: 0x0008F55E
		// (set) Token: 0x06002064 RID: 8292 RVA: 0x00091366 File Offset: 0x0008F566
		public MBList<SkillObject> AffectedSkills { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x0009136F File Offset: 0x0008F56F
		// (set) Token: 0x06002066 RID: 8294 RVA: 0x00091377 File Offset: 0x0008F577
		public int SkillLevelToAdd { get; private set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x00091380 File Offset: 0x0008F580
		// (set) Token: 0x06002068 RID: 8296 RVA: 0x00091388 File Offset: 0x0008F588
		public MBList<TraitObject> AffectedTraits { get; private set; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x00091391 File Offset: 0x0008F591
		// (set) Token: 0x0600206A RID: 8298 RVA: 0x00091399 File Offset: 0x0008F599
		public int TraitLevelToAdd { get; private set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x000913A2 File Offset: 0x0008F5A2
		// (set) Token: 0x0600206C RID: 8300 RVA: 0x000913AA File Offset: 0x0008F5AA
		public int FocusToAdd { get; private set; }

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x000913B3 File Offset: 0x0008F5B3
		// (set) Token: 0x0600206E RID: 8302 RVA: 0x000913BB File Offset: 0x0008F5BB
		public int UnspentFocusToAdd { get; private set; }

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x000913C4 File Offset: 0x0008F5C4
		// (set) Token: 0x06002070 RID: 8304 RVA: 0x000913CC File Offset: 0x0008F5CC
		public CharacterAttribute EffectedAttribute { get; private set; }

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x000913D5 File Offset: 0x0008F5D5
		// (set) Token: 0x06002072 RID: 8306 RVA: 0x000913DD File Offset: 0x0008F5DD
		public int AttributeLevelToAdd { get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x000913E6 File Offset: 0x0008F5E6
		// (set) Token: 0x06002074 RID: 8308 RVA: 0x000913EE File Offset: 0x0008F5EE
		public int UnspentAttributeToAdd { get; private set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x000913F7 File Offset: 0x0008F5F7
		// (set) Token: 0x06002076 RID: 8310 RVA: 0x000913FF File Offset: 0x0008F5FF
		public int RenownToAdd { get; private set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00091408 File Offset: 0x0008F608
		// (set) Token: 0x06002078 RID: 8312 RVA: 0x00091410 File Offset: 0x0008F610
		public int GoldToAdd { get; private set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x0009141C File Offset: 0x0008F61C
		public TextObject PositiveEffectText
		{
			get
			{
				return this.GetPositiveEffectText(this.AffectedSkills.ToMBList<SkillObject>(), this.EffectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, this.AffectedTraits.ToMBList<TraitObject>(), this.TraitLevelToAdd, this.RenownToAdd, this.GoldToAdd, this.UnspentFocusToAdd, this.UnspentAttributeToAdd);
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x0009147B File Offset: 0x0008F67B
		public NarrativeMenuOptionArgs()
		{
			this.AffectedSkills = new MBList<SkillObject>();
			this.AffectedTraits = new MBList<TraitObject>();
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00091499 File Offset: 0x0008F699
		public void SetAffectedSkills(SkillObject[] affectedSkills)
		{
			this.AffectedSkills = affectedSkills.ToMBList<SkillObject>();
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000914A7 File Offset: 0x0008F6A7
		public void SetFocusToSkills(int focusToAdd)
		{
			this.FocusToAdd = focusToAdd;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000914B0 File Offset: 0x0008F6B0
		public void SetLevelToSkills(int levelToAdd)
		{
			this.SkillLevelToAdd = levelToAdd;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000914B9 File Offset: 0x0008F6B9
		public void SetAffectedTraits(TraitObject[] affectedTraits)
		{
			this.AffectedTraits = affectedTraits.ToMBList<TraitObject>();
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000914C7 File Offset: 0x0008F6C7
		public void SetLevelToTraits(int levelToAdd)
		{
			this.TraitLevelToAdd = levelToAdd;
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000914D0 File Offset: 0x0008F6D0
		public void SetLevelToAttribute(CharacterAttribute characterAttribute, int levelToAdd)
		{
			this.EffectedAttribute = characterAttribute;
			this.AttributeLevelToAdd = levelToAdd;
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000914E0 File Offset: 0x0008F6E0
		public void SetRenownToAdd(int value)
		{
			this.RenownToAdd = value;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x000914E9 File Offset: 0x0008F6E9
		public void SetUnspentFocusToAdd(int value)
		{
			this.UnspentFocusToAdd = value;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000914F2 File Offset: 0x0008F6F2
		public void SetUnspentAttributeToAdd(int value)
		{
			this.UnspentAttributeToAdd = value;
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000914FC File Offset: 0x0008F6FC
		private TextObject GetPositiveEffectText(MBList<SkillObject> skills, CharacterAttribute attribute, int focusToAdd = 0, int skillLevelToAdd = 0, int attributeLevelToAdd = 0, MBList<TraitObject> traits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocustoAdd = 0, int unspentAttributeToAdd = 0)
		{
			TextObject textObject;
			if (skills.Count == 3)
			{
				textObject = new TextObject("{=jeWV2uV3}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}, {SKILL_TWO} and {SKILL_THREE}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt<SkillObject>(0).Name);
				textObject.SetTextVariable("SKILL_TWO", skills.ElementAt<SkillObject>(1).Name);
				textObject.SetTextVariable("SKILL_THREE", skills.ElementAt<SkillObject>(2).Name);
			}
			else if (skills.Count == 2)
			{
				textObject = new TextObject("{=5JTEvvaO}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE} and {SKILL_TWO}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt<SkillObject>(0).Name);
				textObject.SetTextVariable("SKILL_TWO", skills.ElementAt<SkillObject>(1).Name);
			}
			else if (skills.Count == 1)
			{
				textObject = new TextObject("{=uw2kKrQk}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}{NEWLINE}{ATTR_VALUE} Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?} to {ATTR_NAME}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}", null);
				textObject.SetTextVariable("SKILL_ONE", skills.ElementAt<SkillObject>(0).Name);
			}
			else
			{
				textObject = new TextObject("{=NDWdnpI5}{UNSPENT_FOCUS_VALUE} unspent Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?}{NEWLINE}{UNSPENT_ATTR_VALUE} unspent Attribute {?IS_PLURAL_ATR}Points{?}Point{\\?}", null);
			}
			if (skills.Count > 0)
			{
				textObject.SetTextVariable("FOCUS_VALUE", focusToAdd);
				textObject.SetTextVariable("EXP_VALUE", skillLevelToAdd);
				textObject.SetTextVariable("ATTR_VALUE", attributeLevelToAdd);
				textObject.SetTextVariable("IS_PLURAL_SKILL", (skillLevelToAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_FOCUS", (focusToAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_ATR", (attributeLevelToAdd > 1) ? 1 : 0);
			}
			else
			{
				textObject.SetTextVariable("IS_PLURAL_FOCUS", (unspentFocustoAdd > 1) ? 1 : 0);
				textObject.SetTextVariable("IS_PLURAL_ATR", (unspentAttributeToAdd > 1) ? 1 : 0);
			}
			if (attribute != null)
			{
				textObject.SetTextVariable("ATTR_NAME", attribute.Name);
			}
			textObject.SetTextVariable("UNSPENT_FOCUS_VALUE", unspentFocustoAdd);
			textObject.SetTextVariable("UNSPENT_ATTR_VALUE", unspentAttributeToAdd);
			if (traits != null && traits.Count > 0 && traits.Count < 4)
			{
				TextObject textObject2 = TextObject.GetEmpty();
				if (traits.Count == 1)
				{
					textObject2 = new TextObject("{=DuQvj7zd}{newline}+{VALUE} to {TRAIT_NAME}", null);
					textObject2.SetTextVariable("TRAIT_NAME", traits.ElementAt<TraitObject>(0).Name);
				}
				else if (traits.Count == 2)
				{
					textObject2 = new TextObject("{=F1syZDs4}{newline}+{VALUE} to {TRAIT_NAME_ONE} and {TRAIT_NAME_TWO}", null);
					textObject2.SetTextVariable("TRAIT_NAME_ONE", traits.ElementAt<TraitObject>(0).Name);
					textObject2.SetTextVariable("TRAIT_NAME_TWO", traits.ElementAt<TraitObject>(1).Name);
				}
				else if (traits.Count == 3)
				{
					textObject2 = new TextObject("{=i20baAus}{newline}+{VALUE} to {TRAIT_NAME_ONE}, {TRAIT_NAME_TWO} and {TRAIT_NAME_THREE}", null);
					textObject2.SetTextVariable("TRAIT_NAME_ONE", traits.ElementAt<TraitObject>(0).Name);
					textObject2.SetTextVariable("TRAIT_NAME_TWO", traits.ElementAt<TraitObject>(1).Name);
					textObject2.SetTextVariable("TRAIT_NAME_THREE", traits.ElementAt<TraitObject>(2).Name);
				}
				if (!textObject2.IsEmpty())
				{
					textObject.SetTextVariable("TRAIT_DESC", textObject2);
					textObject2.SetTextVariable("VALUE", traitLevelToAdd);
				}
			}
			else
			{
				textObject.SetTextVariable("TRAIT_DESC", TextObject.GetEmpty());
			}
			if (renownToAdd > 0)
			{
				TextObject textObject3 = new TextObject("{=KXtaJNo4}{newline}+{VALUE} renown", null);
				textObject3.SetTextVariable("VALUE", renownToAdd);
				textObject.SetTextVariable("RENOWN_DESC", textObject3);
			}
			else
			{
				textObject.SetTextVariable("RENOWN_DESC", TextObject.GetEmpty());
			}
			if (goldToAdd > 0)
			{
				TextObject textObject4 = new TextObject("{=YBqmnNGv}{newline}+{VALUE} gold", null);
				textObject4.SetTextVariable("VALUE", goldToAdd);
				textObject.SetTextVariable("GOLD_DESC", textObject4);
			}
			else
			{
				textObject.SetTextVariable("GOLD_DESC", TextObject.GetEmpty());
			}
			return textObject;
		}
	}
}
