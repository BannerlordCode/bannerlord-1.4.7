using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000205 RID: 517
	public sealed class CharacterCreationContent
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x0008FF11 File Offset: 0x0008E111
		// (set) Token: 0x06001FC7 RID: 8135 RVA: 0x0008FF19 File Offset: 0x0008E119
		public string SelectedTitleType { get; set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0008FF22 File Offset: 0x0008E122
		// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x0008FF2A File Offset: 0x0008E12A
		public string SelectedParentOccupation { get; private set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0008FF33 File Offset: 0x0008E133
		// (set) Token: 0x06001FCB RID: 8139 RVA: 0x0008FF3B File Offset: 0x0008E13B
		public string DefaultSelectedTitleType { get; set; }

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x0008FF44 File Offset: 0x0008E144
		// (set) Token: 0x06001FCD RID: 8141 RVA: 0x0008FF4C File Offset: 0x0008E14C
		public TextObject ReviewPageDescription { get; private set; }

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x0008FF55 File Offset: 0x0008E155
		// (set) Token: 0x06001FCF RID: 8143 RVA: 0x0008FF5D File Offset: 0x0008E15D
		public string MainCharacterName { get; private set; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x0008FF66 File Offset: 0x0008E166
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x0008FF6E File Offset: 0x0008E16E
		public CultureObject SelectedCulture { get; private set; }

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0008FF77 File Offset: 0x0008E177
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x0008FF7F File Offset: 0x0008E17F
		public Banner SelectedBanner { get; private set; }

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0008FF88 File Offset: 0x0008E188
		public CharacterCreationContent()
		{
			this.SetMainHeroInitialStats();
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0008FFD5 File Offset: 0x0008E1D5
		public void AddCharacterCreationCulture(CultureObject culture, int focusToAddByCulture, int skillLevelToAddByCulture)
		{
			if (!this._characterCreationCultures.ContainsKey(culture))
			{
				this._characterCreationCultures.Add(culture, new KeyValuePair<int, int>(focusToAddByCulture, skillLevelToAddByCulture));
				return;
			}
			this._characterCreationCultures[culture] = new KeyValuePair<int, int>(focusToAddByCulture, skillLevelToAddByCulture);
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0009000C File Offset: 0x0008E20C
		public int GetFocusToAddByCulture(CultureObject culture)
		{
			return this._characterCreationCultures[culture].Key;
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x00090030 File Offset: 0x0008E230
		public int GetSkillLevelToAddByCulture(CultureObject culture)
		{
			return this._characterCreationCultures[culture].Value;
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x00090051 File Offset: 0x0008E251
		public void ChangeReviewPageDescription(TextObject reviewPageDescription)
		{
			this.ReviewPageDescription = reviewPageDescription;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0009005A File Offset: 0x0008E25A
		public void SetMainCharacterName(string name)
		{
			this.MainCharacterName = name;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00090063 File Offset: 0x0008E263
		public void SetParentOccupation(string occupationType)
		{
			this.SelectedParentOccupation = occupationType;
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0009006C File Offset: 0x0008E26C
		public void ApplySkillAndAttributeEffects(List<SkillObject> skills, int focusToAdd, int skillLevelToAdd, CharacterAttribute attribute, int attributeLevelToAdd, List<TraitObject> traits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocusPoints = 0, int unspentAttributePoints = 0)
		{
			foreach (SkillObject skillObject in skills)
			{
				Hero.MainHero.HeroDeveloper.AddFocus(skillObject, focusToAdd, false);
				if (Hero.MainHero.GetSkillValue(skillObject) == 1)
				{
					Hero.MainHero.HeroDeveloper.ChangeSkillLevel(skillObject, skillLevelToAdd - 1, false);
				}
				else
				{
					Hero.MainHero.HeroDeveloper.ChangeSkillLevel(skillObject, skillLevelToAdd, false);
				}
			}
			Hero.MainHero.HeroDeveloper.UnspentFocusPoints += unspentFocusPoints;
			Hero.MainHero.HeroDeveloper.UnspentAttributePoints += unspentAttributePoints;
			if (attribute != null)
			{
				Hero.MainHero.HeroDeveloper.AddAttribute(attribute, attributeLevelToAdd, false);
			}
			if (traits != null && traitLevelToAdd > 0 && traits.Count > 0)
			{
				foreach (TraitObject traitObject in traits)
				{
					Hero.MainHero.SetTraitLevel(traitObject, Hero.MainHero.GetTraitLevel(traitObject) + traitLevelToAdd);
				}
			}
			if (renownToAdd > 0)
			{
				GainRenownAction.Apply(Hero.MainHero, (float)renownToAdd, true);
			}
			if (goldToAdd > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, goldToAdd, true);
			}
			Hero.MainHero.HeroDeveloper.ResetTotalXpForPlayerCharacter();
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000901DC File Offset: 0x0008E3DC
		public void SetMainClanBanner(Banner banner)
		{
			this.SelectedBanner = banner;
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000901E8 File Offset: 0x0008E3E8
		public void SetSelectedCulture(CultureObject culture, CharacterCreationManager characterCreationManager)
		{
			this.SelectedCulture = culture;
			characterCreationManager.ResetMenuOptions();
			this.SelectedTitleType = this.DefaultSelectedTitleType;
			TextObject textObject = FactionHelper.GenerateClanNameforPlayer();
			Clan.PlayerClan.ChangeClanName(textObject, textObject);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x00090220 File Offset: 0x0008E420
		public void ApplyCulture(CharacterCreationManager characterCreationManager)
		{
			Hero.MainHero.Culture = this.SelectedCulture;
			Clan.PlayerClan.Culture = this.SelectedCulture;
			Clan.PlayerClan.ResetPlayerHomeAndFactionMidSettlement();
			Hero.MainHero.BornSettlement = Clan.PlayerClan.HomeSettlement;
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x00090260 File Offset: 0x0008E460
		public IEnumerable<CultureObject> GetCultures()
		{
			foreach (KeyValuePair<CultureObject, KeyValuePair<int, int>> keyValuePair in this._characterCreationCultures)
			{
				yield return keyValuePair.Key;
			}
			Dictionary<CultureObject, KeyValuePair<int, int>>.Enumerator enumerator = default(Dictionary<CultureObject, KeyValuePair<int, int>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x00090270 File Offset: 0x0008E470
		private void SetMainHeroInitialStats()
		{
			Hero.MainHero.HeroDeveloper.ClearHero();
			Hero.MainHero.HitPoints = 100;
			foreach (SkillObject skillObject in Skills.All)
			{
				Hero.MainHero.HeroDeveloper.InitializeSkillXp(skillObject);
			}
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				Hero.MainHero.HeroDeveloper.AddAttribute(characterAttribute, 2, false);
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00090334 File Offset: 0x0008E534
		public void AddEquipmentToUseGetter(CharacterCreationContent.TryGetEquipmentIdDelegate tryGetEquipmentIdDelegate)
		{
			this._tryGetEquipmentIdDelegates.Add(tryGetEquipmentIdDelegate);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00090344 File Offset: 0x0008E544
		public bool TryGetEquipmentToUse(string occupationId, out string equipmentId)
		{
			for (int i = this._tryGetEquipmentIdDelegates.Count - 1; i >= 0; i--)
			{
				if (this._tryGetEquipmentIdDelegates[i](occupationId, out equipmentId))
				{
					return true;
				}
			}
			equipmentId = null;
			return false;
		}

		// Token: 0x04000951 RID: 2385
		public int FocusToAdd = 1;

		// Token: 0x04000952 RID: 2386
		public int SkillLevelToAdd = 10;

		// Token: 0x04000953 RID: 2387
		public int AttributeLevelToAdd = 1;

		// Token: 0x0400095B RID: 2395
		public int StartingAge = 20;

		// Token: 0x0400095C RID: 2396
		private readonly Dictionary<CultureObject, KeyValuePair<int, int>> _characterCreationCultures = new Dictionary<CultureObject, KeyValuePair<int, int>>();

		// Token: 0x0400095D RID: 2397
		private readonly List<CharacterCreationContent.TryGetEquipmentIdDelegate> _tryGetEquipmentIdDelegates = new List<CharacterCreationContent.TryGetEquipmentIdDelegate>();

		// Token: 0x02000606 RID: 1542
		// (Invoke) Token: 0x0600501C RID: 20508
		public delegate bool TryGetEquipmentIdDelegate(string occupationId, out string equipmentId);
	}
}
