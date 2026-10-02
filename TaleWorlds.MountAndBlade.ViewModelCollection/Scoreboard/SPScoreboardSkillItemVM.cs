using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000013 RID: 19
	public class SPScoreboardSkillItemVM : ViewModel
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public SPScoreboardSkillItemVM(SkillObject skill, int initialValue)
		{
			this.Skill = skill;
			this._initialValue = initialValue;
			this._newValue = initialValue;
			this.SkillId = skill.StringId;
			this.Description = "(" + initialValue + ")";
			this.RefreshValues();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005D49 File Offset: 0x00003F49
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Level = this.Skill.Name.ToString();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005D68 File Offset: 0x00003F68
		public void UpdateSkill(int newValue)
		{
			this._newValue = newValue;
			this.Description = string.Concat(new object[]
			{
				"+",
				newValue - this._initialValue,
				"(",
				newValue,
				")"
			});
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005DBE File Offset: 0x00003FBE
		public bool IsValid()
		{
			return this._newValue > this._initialValue;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00005DCE File Offset: 0x00003FCE
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00005DD6 File Offset: 0x00003FD6
		[DataSourceProperty]
		public string Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue<string>(value, "Level");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00005DF9 File Offset: 0x00003FF9
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00005E01 File Offset: 0x00004001
		[DataSourceProperty]
		public string SkillId
		{
			get
			{
				return this._imagePath;
			}
			set
			{
				if (value != this._imagePath)
				{
					this._imagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillId");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00005E24 File Offset: 0x00004024
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00005E2C File Offset: 0x0000402C
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x040000A8 RID: 168
		public SkillObject Skill;

		// Token: 0x040000A9 RID: 169
		private readonly int _initialValue;

		// Token: 0x040000AA RID: 170
		private int _newValue;

		// Token: 0x040000AB RID: 171
		private string _level;

		// Token: 0x040000AC RID: 172
		private string _imagePath;

		// Token: 0x040000AD RID: 173
		private string _description;
	}
}
