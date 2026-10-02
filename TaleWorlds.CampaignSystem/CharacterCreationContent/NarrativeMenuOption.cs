using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000218 RID: 536
	public sealed class NarrativeMenuOption
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x00091176 File Offset: 0x0008F376
		public TextObject PositiveEffectText
		{
			get
			{
				return this.Args.PositiveEffectText;
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00091184 File Offset: 0x0008F384
		public NarrativeMenuOption(string stringId, TextObject text, TextObject descriptionText, GetNarrativeMenuOptionArgsDelegate getNarrativeMenuOptionArgs, NarrativeMenuOptionOnConditionDelegate onCondition, NarrativeMenuOptionOnSelectDelegate onSelect, NarrativeMenuOptionOnConsequenceDelegate onConsequence)
		{
			this.StringId = stringId;
			this.Text = text;
			this.DescriptionText = descriptionText;
			this._onConditionInternal = onCondition;
			this._onSelectInternal = onSelect;
			this._onConsequenceInternal = onConsequence;
			this._getNarrativeMenuOptionArgs = getNarrativeMenuOptionArgs;
			this.Args = new NarrativeMenuOptionArgs();
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000911D7 File Offset: 0x0008F3D7
		public bool OnCondition(CharacterCreationManager characterCreationManager)
		{
			return this._onConditionInternal == null || this._onConditionInternal(characterCreationManager);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000911F0 File Offset: 0x0008F3F0
		public void OnSelect(CharacterCreationManager characterCreationManager)
		{
			GetNarrativeMenuOptionArgsDelegate getNarrativeMenuOptionArgs = this._getNarrativeMenuOptionArgs;
			if (getNarrativeMenuOptionArgs != null)
			{
				getNarrativeMenuOptionArgs(this.Args);
			}
			foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
			{
				if (narrativeMenuCharacter.IsHuman)
				{
					narrativeMenuCharacter.SetRightHandItem("");
					narrativeMenuCharacter.SetLeftHandItem("");
					narrativeMenuCharacter.EquipLeftHandItemWithEquipmentIndex(EquipmentIndex.WeaponItemBeginSlot);
					narrativeMenuCharacter.EquipRightHandItemWithEquipmentIndex(EquipmentIndex.Weapon1);
				}
			}
			NarrativeMenuOptionOnSelectDelegate onSelectInternal = this._onSelectInternal;
			if (onSelectInternal == null)
			{
				return;
			}
			onSelectInternal(characterCreationManager);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00091298 File Offset: 0x0008F498
		public void OnConsequence(CharacterCreationManager characterCreationManager)
		{
			NarrativeMenuOptionOnConsequenceDelegate onConsequenceInternal = this._onConsequenceInternal;
			if (onConsequenceInternal == null)
			{
				return;
			}
			onConsequenceInternal(characterCreationManager);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000912AB File Offset: 0x0008F4AB
		public void SetOnCondition(NarrativeMenuOptionOnConditionDelegate onCondition)
		{
			this._onConditionInternal = onCondition;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000912B4 File Offset: 0x0008F4B4
		public void SetOnSelect(NarrativeMenuOptionOnSelectDelegate onSelect)
		{
			this._onSelectInternal = onSelect;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000912BD File Offset: 0x0008F4BD
		public void SetOnConsequence(NarrativeMenuOptionOnConsequenceDelegate onConsequence)
		{
			this._onConsequenceInternal = onConsequence;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000912C8 File Offset: 0x0008F4C8
		public void ApplyFinalEffects(CharacterCreationContent characterCreationContent)
		{
			characterCreationContent.ApplySkillAndAttributeEffects(this.Args.AffectedSkills.ToList<SkillObject>(), this.Args.FocusToAdd, this.Args.SkillLevelToAdd, this.Args.EffectedAttribute, this.Args.AttributeLevelToAdd, this.Args.AffectedTraits.ToList<TraitObject>(), this.Args.TraitLevelToAdd, this.Args.RenownToAdd, this.Args.GoldToAdd, this.Args.UnspentFocusToAdd, this.Args.UnspentAttributeToAdd);
		}

		// Token: 0x0400098B RID: 2443
		public readonly string StringId;

		// Token: 0x0400098C RID: 2444
		public readonly TextObject Text;

		// Token: 0x0400098D RID: 2445
		public readonly TextObject DescriptionText;

		// Token: 0x0400098E RID: 2446
		private NarrativeMenuOptionOnConditionDelegate _onConditionInternal;

		// Token: 0x0400098F RID: 2447
		private NarrativeMenuOptionOnSelectDelegate _onSelectInternal;

		// Token: 0x04000990 RID: 2448
		private NarrativeMenuOptionOnConsequenceDelegate _onConsequenceInternal;

		// Token: 0x04000991 RID: 2449
		private readonly GetNarrativeMenuOptionArgsDelegate _getNarrativeMenuOptionArgs;

		// Token: 0x04000992 RID: 2450
		public readonly NarrativeMenuOptionArgs Args;
	}
}
