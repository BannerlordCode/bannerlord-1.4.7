using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup
{
	// Token: 0x02000038 RID: 56
	public class MarriageOfferPopupHeroAttributeVM : ViewModel
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x0001DC54 File Offset: 0x0001BE54
		public MarriageOfferPopupHeroAttributeVM(Hero hero, CharacterAttribute attribute)
		{
			this._hero = hero;
			this._attribute = attribute;
			this.FillSkillsList();
			this.RefreshValues();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001DC78 File Offset: 0x0001BE78
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = GameTexts.FindText("str_STR1_space_STR2", null);
			textObject.SetTextVariable("STR1", this._attribute.Name);
			TextObject textObject2 = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject2.SetTextVariable("STR", this._hero.GetAttributeValue(this._attribute));
			textObject.SetTextVariable("STR2", textObject2);
			this._attributeText = textObject.ToString();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001DCF0 File Offset: 0x0001BEF0
		private void FillSkillsList()
		{
			this._attributeSkills = new MBBindingList<EncyclopediaSkillVM>();
			using (List<SkillObject>.Enumerator enumerator = Skills.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (!CampaignUIHelper.GetIsNavalSkill(skill) && skill.Attributes.FirstOrDefault<CharacterAttribute>() == this._attribute && !this._attributeSkills.Any<EncyclopediaSkillVM>((EncyclopediaSkillVM s) => s.SkillId == skill.StringId))
					{
						this._attributeSkills.Add(new EncyclopediaSkillVM(skill, this._hero.GetSkillValue(skill)));
					}
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001DDBC File Offset: 0x0001BFBC
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001DDC4 File Offset: 0x0001BFC4
		[DataSourceProperty]
		public string AttributeText
		{
			get
			{
				return this._attributeText;
			}
			set
			{
				if (value != this._attributeText)
				{
					this._attributeText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttributeText");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001DDE7 File Offset: 0x0001BFE7
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0001DDEF File Offset: 0x0001BFEF
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> AttributeSkills
		{
			get
			{
				return this._attributeSkills;
			}
			set
			{
				if (value != this._attributeSkills)
				{
					this._attributeSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "AttributeSkills");
				}
			}
		}

		// Token: 0x0400025D RID: 605
		private readonly Hero _hero;

		// Token: 0x0400025E RID: 606
		private readonly CharacterAttribute _attribute;

		// Token: 0x0400025F RID: 607
		private string _attributeText;

		// Token: 0x04000260 RID: 608
		private MBBindingList<EncyclopediaSkillVM> _attributeSkills;
	}
}
