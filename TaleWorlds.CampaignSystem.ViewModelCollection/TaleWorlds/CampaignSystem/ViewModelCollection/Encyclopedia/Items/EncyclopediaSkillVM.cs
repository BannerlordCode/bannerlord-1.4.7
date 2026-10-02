using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EB RID: 235
	public class EncyclopediaSkillVM : ViewModel
	{
		// Token: 0x060015C3 RID: 5571 RVA: 0x00055808 File Offset: 0x00053A08
		public EncyclopediaSkillVM(SkillObject skill, int skillValue)
		{
			this._skill = skill;
			this.SkillValue = skillValue;
			this.SkillId = skill.StringId;
			this.RefreshValues();
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00055830 File Offset: 0x00053A30
		public override void RefreshValues()
		{
			base.RefreshValues();
			string name = this._skill.Name.ToString();
			string desc = this._skill.Description.ToString();
			this.Hint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("STR1", name);
				GameTexts.SetVariable("STR2", desc);
				return GameTexts.FindText("str_string_newline_string", null).ToString();
			});
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0005588C File Offset: 0x00053A8C
		// (set) Token: 0x060015C6 RID: 5574 RVA: 0x00055894 File Offset: 0x00053A94
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x000558B2 File Offset: 0x00053AB2
		// (set) Token: 0x060015C8 RID: 5576 RVA: 0x000558BA File Offset: 0x00053ABA
		[DataSourceProperty]
		public int SkillValue
		{
			get
			{
				return this._skillValue;
			}
			set
			{
				if (value != this._skillValue)
				{
					this._skillValue = value;
					base.OnPropertyChangedWithValue(value, "SkillValue");
				}
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x000558D8 File Offset: 0x00053AD8
		// (set) Token: 0x060015CA RID: 5578 RVA: 0x000558E0 File Offset: 0x00053AE0
		[DataSourceProperty]
		public string SkillId
		{
			get
			{
				return this._skillId;
			}
			set
			{
				if (value != this._skillId)
				{
					this._skillId = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillId");
				}
			}
		}

		// Token: 0x040009E4 RID: 2532
		private readonly SkillObject _skill;

		// Token: 0x040009E5 RID: 2533
		private string _skillId;

		// Token: 0x040009E6 RID: 2534
		private int _skillValue;

		// Token: 0x040009E7 RID: 2535
		private BasicTooltipViewModel _hint;
	}
}
