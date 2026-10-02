using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000016 RID: 22
	public class SPScoreboardUnitVM : ViewModel
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00006770 File Offset: 0x00004970
		public SPScoreboardUnitVM(BasicCharacterObject character)
		{
			this.Character = character;
			this.GainedSkills = new MBBindingList<SPScoreboardSkillItemVM>();
			this._skills = new List<SPScoreboardSkillItemVM>();
			this.Score = new SPScoreboardStatsVM(character.Name);
			CharacterCode.CreateFrom(character);
			this.IsHero = character.IsHero;
			this.Score.IsMainHero = character == Game.Current.PlayerTroop;
			this.IsGainedAnySkills = false;
			if (character.IsHero)
			{
				foreach (SkillObject skillObject in Game.Current.ObjectManager.GetObjectTypeList<SkillObject>())
				{
					this._skills.Add(new SPScoreboardSkillItemVM(skillObject, character.GetSkillValue(skillObject)));
				}
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000684C File Offset: 0x00004A4C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Score.RefreshValues();
			this.GainedSkills.ApplyActionOnAllItems(delegate(SPScoreboardSkillItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006889 File Offset: 0x00004A89
		private void ExecuteActivateGainedSkills()
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000688B File Offset: 0x00004A8B
		private void ExecuteDeactivateGainedSkills()
		{
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000688D File Offset: 0x00004A8D
		public void UpdateScores(int numberRemaining, int numberDead, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
		{
			this.Score.UpdateScores(numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000068A4 File Offset: 0x00004AA4
		public void UpdateHeroSkills(SkillObject gainedSkill, int currentSkill)
		{
			SPScoreboardSkillItemVM spscoreboardSkillItemVM = this._skills.First<SPScoreboardSkillItemVM>((SPScoreboardSkillItemVM s) => s.Skill == gainedSkill);
			spscoreboardSkillItemVM.UpdateSkill(currentSkill);
			if (!this.GainedSkills.Contains(spscoreboardSkillItemVM))
			{
				this.GainedSkills.Add(spscoreboardSkillItemVM);
			}
			this.IsGainedAnySkills = this.GainedSkills.Count > 0;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000690B File Offset: 0x00004B0B
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00006913 File Offset: 0x00004B13
		[DataSourceProperty]
		public bool IsGainedAnySkills
		{
			get
			{
				return this._isGainedAnySkills;
			}
			set
			{
				if (value != this._isGainedAnySkills)
				{
					this._isGainedAnySkills = value;
					base.OnPropertyChangedWithValue(value, "IsGainedAnySkills");
				}
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00006931 File Offset: 0x00004B31
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00006939 File Offset: 0x00004B39
		[DataSourceProperty]
		public MBBindingList<SPScoreboardSkillItemVM> GainedSkills
		{
			get
			{
				return this._gainedSkills;
			}
			set
			{
				if (value != this._gainedSkills)
				{
					this._gainedSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardSkillItemVM>>(value, "GainedSkills");
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00006957 File Offset: 0x00004B57
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000695F File Offset: 0x00004B5F
		[DataSourceProperty]
		public bool IsHero
		{
			get
			{
				return this._isHero;
			}
			set
			{
				if (value != this._isHero)
				{
					this._isHero = value;
					base.OnPropertyChangedWithValue(value, "IsHero");
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000697D File Offset: 0x00004B7D
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00006985 File Offset: 0x00004B85
		[DataSourceProperty]
		public SPScoreboardStatsVM Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue<SPScoreboardStatsVM>(value, "Score");
				}
			}
		}

		// Token: 0x040000CC RID: 204
		public readonly BasicCharacterObject Character;

		// Token: 0x040000CD RID: 205
		private readonly List<SPScoreboardSkillItemVM> _skills;

		// Token: 0x040000CE RID: 206
		private SPScoreboardStatsVM _score;

		// Token: 0x040000CF RID: 207
		private bool _isHero;

		// Token: 0x040000D0 RID: 208
		private bool _isGainedAnySkills;

		// Token: 0x040000D1 RID: 209
		private MBBindingList<SPScoreboardSkillItemVM> _gainedSkills;
	}
}
