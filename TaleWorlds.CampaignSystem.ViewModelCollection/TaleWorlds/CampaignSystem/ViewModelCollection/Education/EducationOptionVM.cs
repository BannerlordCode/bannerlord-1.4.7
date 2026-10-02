using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F7 RID: 247
	public class EducationOptionVM : StringItemWithActionVM
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x00057937 File Offset: 0x00055B37
		// (set) Token: 0x0600165C RID: 5724 RVA: 0x0005793F File Offset: 0x00055B3F
		public string OptionEffect { get; private set; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x00057948 File Offset: 0x00055B48
		// (set) Token: 0x0600165E RID: 5726 RVA: 0x00057950 File Offset: 0x00055B50
		public string OptionDescription { get; private set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x0600165F RID: 5727 RVA: 0x00057959 File Offset: 0x00055B59
		// (set) Token: 0x06001660 RID: 5728 RVA: 0x00057961 File Offset: 0x00055B61
		public EducationCampaignBehavior.EducationCharacterProperties[] CharacterProperties { get; private set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x0005796A File Offset: 0x00055B6A
		// (set) Token: 0x06001662 RID: 5730 RVA: 0x00057972 File Offset: 0x00055B72
		public string ActionID { get; private set; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x0005797B File Offset: 0x00055B7B
		// (set) Token: 0x06001664 RID: 5732 RVA: 0x00057983 File Offset: 0x00055B83
		public ValueTuple<CharacterAttribute, int>[] OptionAttributes { get; private set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x0005798C File Offset: 0x00055B8C
		// (set) Token: 0x06001666 RID: 5734 RVA: 0x00057994 File Offset: 0x00055B94
		public ValueTuple<SkillObject, int>[] OptionSkills { get; private set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001667 RID: 5735 RVA: 0x0005799D File Offset: 0x00055B9D
		// (set) Token: 0x06001668 RID: 5736 RVA: 0x000579A5 File Offset: 0x00055BA5
		public ValueTuple<SkillObject, int>[] OptionFocusPoints { get; private set; }

		// Token: 0x06001669 RID: 5737 RVA: 0x000579B0 File Offset: 0x00055BB0
		public EducationOptionVM(Action<object> onExecute, string optionId, TextObject optionText, TextObject optionDescription, TextObject optionEffect, bool isSelected, ValueTuple<CharacterAttribute, int>[] optionAttributes, ValueTuple<SkillObject, int>[] optionSkills, ValueTuple<SkillObject, int>[] optionFocusPoints, EducationCampaignBehavior.EducationCharacterProperties[] characterProperties)
			: base(onExecute, optionText.ToString(), optionId)
		{
			this.IsSelected = isSelected;
			this.CharacterProperties = characterProperties;
			this._optionTextObject = optionText;
			this._optionDescriptionObject = optionDescription;
			this._optionEffectObject = optionEffect;
			this.OptionAttributes = optionAttributes;
			this.OptionSkills = optionSkills;
			this.OptionFocusPoints = optionFocusPoints;
			this.RefreshValues();
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00057A10 File Offset: 0x00055C10
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OptionEffect = this._optionEffectObject.ToString();
			this.OptionDescription = this._optionDescriptionObject.ToString();
			base.ActionText = this._optionTextObject.ToString();
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x00057A4B File Offset: 0x00055C4B
		// (set) Token: 0x0600166C RID: 5740 RVA: 0x00057A53 File Offset: 0x00055C53
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x04000A3C RID: 2620
		private readonly TextObject _optionTextObject;

		// Token: 0x04000A3D RID: 2621
		private readonly TextObject _optionDescriptionObject;

		// Token: 0x04000A3E RID: 2622
		private readonly TextObject _optionEffectObject;

		// Token: 0x04000A3F RID: 2623
		private bool _isSelected;
	}
}
