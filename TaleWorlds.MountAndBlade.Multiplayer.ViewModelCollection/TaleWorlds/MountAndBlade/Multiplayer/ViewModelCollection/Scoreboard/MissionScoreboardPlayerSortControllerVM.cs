using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x0200001D RID: 29
	public class MissionScoreboardPlayerSortControllerVM : ViewModel
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x000073C4 File Offset: 0x000055C4
		public MissionScoreboardPlayerSortControllerVM(ref MBBindingList<MissionScoreboardPlayerVM> listToControl)
		{
			this._listToControl = listToControl;
			this._nameComparer = new MissionScoreboardPlayerSortControllerVM.ItemNameComparer();
			this._scoreComparer = new MissionScoreboardPlayerSortControllerVM.ItemScoreComparer();
			this._killComparer = new MissionScoreboardPlayerSortControllerVM.ItemKillComparer();
			this._assistComparer = new MissionScoreboardPlayerSortControllerVM.ItemAssistComparer();
			this.ExecuteSortByScore();
			this.RefreshValues();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007434 File Offset: 0x00005634
		public override void RefreshValues()
		{
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.ScoreText = GameTexts.FindText("str_scoreboard_header", "score").ToString();
			this.KillText = GameTexts.FindText("str_scoreboard_header", "kill").ToString();
			this.AssistText = GameTexts.FindText("str_scoreboard_header", "assist").ToString();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000074AC File Offset: 0x000056AC
		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsScoreSelected)
			{
				this._listToControl.Sort(this._scoreComparer);
				return;
			}
			if (this.IsKillSelected)
			{
				this._listToControl.Sort(this._killComparer);
				return;
			}
			if (this.IsAssistSelected)
			{
				this._listToControl.Sort(this._assistComparer);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007520 File Offset: 0x00005720
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				int nameState2 = this.NameState;
				this.NameState = nameState2 + 1;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000758C File Offset: 0x0000578C
		public void ExecuteSortByScore()
		{
			int scoreState = this.ScoreState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.ScoreState = (scoreState + 1) % 3;
			if (this.ScoreState == 0)
			{
				int scoreState2 = this.ScoreState;
				this.ScoreState = scoreState2 + 1;
			}
			this._scoreComparer.SetSortMode(this.ScoreState == 1);
			this._listToControl.Sort(this._scoreComparer);
			this.IsScoreSelected = true;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000075F8 File Offset: 0x000057F8
		public void ExecuteSortByKill()
		{
			int killState = this.KillState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.KillState = (killState + 1) % 3;
			if (this.KillState == 0)
			{
				int killState2 = this.KillState;
				this.KillState = killState2 + 1;
			}
			this._killComparer.SetSortMode(this.KillState == 1);
			this._listToControl.Sort(this._killComparer);
			this.IsKillSelected = true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007664 File Offset: 0x00005864
		public void ExecuteSortByAssist()
		{
			int assistState = this.AssistState;
			this.SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState.Default);
			this.AssistState = (assistState + 1) % 3;
			if (this.AssistState == 0)
			{
				int assistState2 = this.AssistState;
				this.AssistState = assistState2 + 1;
			}
			this._assistComparer.SetSortMode(this.AssistState == 1);
			this._listToControl.Sort(this._assistComparer);
			this.IsAssistSelected = true;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000076CE File Offset: 0x000058CE
		private void SetAllStates(MissionScoreboardPlayerSortControllerVM.SortState state)
		{
			this.NameState = (int)state;
			this.ScoreState = (int)state;
			this.KillState = (int)state;
			this.AssistState = (int)state;
			this.IsNameSelected = false;
			this.IsScoreSelected = false;
			this.IsKillSelected = false;
			this.IsAssistSelected = false;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00007708 File Offset: 0x00005908
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00007710 File Offset: 0x00005910
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00007733 File Offset: 0x00005933
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000773B File Offset: 0x0000593B
		[DataSourceProperty]
		public string ScoreText
		{
			get
			{
				return this._scoreText;
			}
			set
			{
				if (value != this._scoreText)
				{
					this._scoreText = value;
					base.OnPropertyChangedWithValue<string>(value, "ScoreText");
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000775E File Offset: 0x0000595E
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00007766 File Offset: 0x00005966
		[DataSourceProperty]
		public string KillText
		{
			get
			{
				return this._killText;
			}
			set
			{
				if (value != this._killText)
				{
					this._killText = value;
					base.OnPropertyChangedWithValue<string>(value, "KillText");
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007789 File Offset: 0x00005989
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00007791 File Offset: 0x00005991
		[DataSourceProperty]
		public string AssistText
		{
			get
			{
				return this._assistText;
			}
			set
			{
				if (value != this._assistText)
				{
					this._assistText = value;
					base.OnPropertyChangedWithValue<string>(value, "AssistText");
				}
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000077B4 File Offset: 0x000059B4
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x000077BC File Offset: 0x000059BC
		[DataSourceProperty]
		public int NameState
		{
			get
			{
				return this._nameState;
			}
			set
			{
				if (value != this._nameState)
				{
					this._nameState = value;
					base.OnPropertyChangedWithValue(value, "NameState");
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000077DA File Offset: 0x000059DA
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x000077E2 File Offset: 0x000059E2
		[DataSourceProperty]
		public int ScoreState
		{
			get
			{
				return this._scoreState;
			}
			set
			{
				if (value != this._scoreState)
				{
					this._scoreState = value;
					base.OnPropertyChangedWithValue(value, "ScoreState");
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00007800 File Offset: 0x00005A00
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00007808 File Offset: 0x00005A08
		[DataSourceProperty]
		public int KillState
		{
			get
			{
				return this._killState;
			}
			set
			{
				if (value != this._killState)
				{
					this._killState = value;
					base.OnPropertyChangedWithValue(value, "KillState");
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00007826 File Offset: 0x00005A26
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000782E File Offset: 0x00005A2E
		[DataSourceProperty]
		public int AssistState
		{
			get
			{
				return this._assistState;
			}
			set
			{
				if (value != this._assistState)
				{
					this._assistState = value;
					base.OnPropertyChangedWithValue(value, "AssistState");
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000784C File Offset: 0x00005A4C
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00007854 File Offset: 0x00005A54
		[DataSourceProperty]
		public bool IsNameSelected
		{
			get
			{
				return this._isNameSelected;
			}
			set
			{
				if (value != this._isNameSelected)
				{
					this._isNameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsNameSelected");
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00007872 File Offset: 0x00005A72
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000787A File Offset: 0x00005A7A
		[DataSourceProperty]
		public bool IsScoreSelected
		{
			get
			{
				return this._isScoreSelected;
			}
			set
			{
				if (value != this._isScoreSelected)
				{
					this._isScoreSelected = value;
					base.OnPropertyChangedWithValue(value, "IsScoreSelected");
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00007898 File Offset: 0x00005A98
		// (set) Token: 0x060001CF RID: 463 RVA: 0x000078A0 File Offset: 0x00005AA0
		[DataSourceProperty]
		public bool IsKillSelected
		{
			get
			{
				return this._isKillSelected;
			}
			set
			{
				if (value != this._isKillSelected)
				{
					this._isKillSelected = value;
					base.OnPropertyChangedWithValue(value, "IsKillSelected");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x000078BE File Offset: 0x00005ABE
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x000078C6 File Offset: 0x00005AC6
		[DataSourceProperty]
		public bool IsAssistSelected
		{
			get
			{
				return this._isAssistSelected;
			}
			set
			{
				if (value != this._isAssistSelected)
				{
					this._isAssistSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAssistSelected");
				}
			}
		}

		// Token: 0x040000E8 RID: 232
		private const string _nameHeaderID = "name";

		// Token: 0x040000E9 RID: 233
		private const string _scoreHeaderID = "score";

		// Token: 0x040000EA RID: 234
		private const string _killHeaderID = "kill";

		// Token: 0x040000EB RID: 235
		private const string _assistHeaderID = "assist";

		// Token: 0x040000EC RID: 236
		private readonly MBBindingList<MissionScoreboardPlayerVM> _listToControl;

		// Token: 0x040000ED RID: 237
		private readonly MissionScoreboardPlayerSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x040000EE RID: 238
		private readonly MissionScoreboardPlayerSortControllerVM.ItemScoreComparer _scoreComparer;

		// Token: 0x040000EF RID: 239
		private readonly MissionScoreboardPlayerSortControllerVM.ItemKillComparer _killComparer;

		// Token: 0x040000F0 RID: 240
		private readonly MissionScoreboardPlayerSortControllerVM.ItemAssistComparer _assistComparer;

		// Token: 0x040000F1 RID: 241
		private string _nameText;

		// Token: 0x040000F2 RID: 242
		private string _scoreText;

		// Token: 0x040000F3 RID: 243
		private string _killText;

		// Token: 0x040000F4 RID: 244
		private string _assistText;

		// Token: 0x040000F5 RID: 245
		private int _nameState = 1;

		// Token: 0x040000F6 RID: 246
		private int _scoreState = 1;

		// Token: 0x040000F7 RID: 247
		private int _killState = 1;

		// Token: 0x040000F8 RID: 248
		private int _assistState = 1;

		// Token: 0x040000F9 RID: 249
		private bool _isNameSelected;

		// Token: 0x040000FA RID: 250
		private bool _isScoreSelected;

		// Token: 0x040000FB RID: 251
		private bool _isKillSelected;

		// Token: 0x040000FC RID: 252
		private bool _isAssistSelected;

		// Token: 0x020000C3 RID: 195
		private enum SortState
		{
			// Token: 0x040007EE RID: 2030
			Default,
			// Token: 0x040007EF RID: 2031
			Ascending,
			// Token: 0x040007F0 RID: 2032
			Descending
		}

		// Token: 0x020000C4 RID: 196
		public abstract class ItemComparerBase : IComparer<MissionScoreboardPlayerVM>
		{
			// Token: 0x060010E9 RID: 4329 RVA: 0x00034372 File Offset: 0x00032572
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060010EA RID: 4330
			public abstract int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y);

			// Token: 0x040007F1 RID: 2033
			protected bool _isAscending;
		}

		// Token: 0x020000C5 RID: 197
		public class ItemNameComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x060010EC RID: 4332 RVA: 0x00034383 File Offset: 0x00032583
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				return y.Name.CompareTo(x.Name) * (this._isAscending ? (-1) : 1);
			}
		}

		// Token: 0x020000C6 RID: 198
		public class ItemScoreComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x060010EE RID: 4334 RVA: 0x000343AC File Offset: 0x000325AC
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				return y.Score.CompareTo(x.Score) * (this._isAscending ? (-1) : 1);
			}
		}

		// Token: 0x020000C7 RID: 199
		public class ItemKillComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x060010F0 RID: 4336 RVA: 0x000343E4 File Offset: 0x000325E4
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				MissionScoreboardStatItemVM missionScoreboardStatItemVM = x.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "kill");
				MissionScoreboardStatItemVM missionScoreboardStatItemVM2 = y.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "kill");
				if (missionScoreboardStatItemVM != null && missionScoreboardStatItemVM2 != null)
				{
					return int.Parse(missionScoreboardStatItemVM2.Item).CompareTo(int.Parse(missionScoreboardStatItemVM.Item)) * (this._isAscending ? (-1) : 1);
				}
				return 0;
			}
		}

		// Token: 0x020000C8 RID: 200
		public class ItemAssistComparer : MissionScoreboardPlayerSortControllerVM.ItemComparerBase
		{
			// Token: 0x060010F2 RID: 4338 RVA: 0x00034484 File Offset: 0x00032684
			public override int Compare(MissionScoreboardPlayerVM x, MissionScoreboardPlayerVM y)
			{
				MissionScoreboardStatItemVM missionScoreboardStatItemVM = x.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "assist");
				MissionScoreboardStatItemVM missionScoreboardStatItemVM2 = y.Stats.FirstOrDefault<MissionScoreboardStatItemVM>((MissionScoreboardStatItemVM s) => s.HeaderID == "assist");
				if (missionScoreboardStatItemVM != null && missionScoreboardStatItemVM2 != null)
				{
					return int.Parse(missionScoreboardStatItemVM2.Item).CompareTo(int.Parse(missionScoreboardStatItemVM.Item)) * (this._isAscending ? (-1) : 1);
				}
				return 0;
			}
		}
	}
}
