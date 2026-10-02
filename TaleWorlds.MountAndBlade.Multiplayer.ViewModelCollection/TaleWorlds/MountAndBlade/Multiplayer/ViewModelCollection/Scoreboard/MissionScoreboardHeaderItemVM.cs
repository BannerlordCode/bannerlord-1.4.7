using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x0200001B RID: 27
	public class MissionScoreboardHeaderItemVM : BindingListStringItem
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00007305 File Offset: 0x00005505
		public MissionScoreboardHeaderItemVM(MissionScoreboardSideVM side, string headerID, string value, bool isAvatarStat, bool isIrregularStat)
			: base(value)
		{
			this._side = side;
			this.HeaderID = headerID;
			this.IsAvatarStat = isAvatarStat;
			this.IsIrregularStat = isIrregularStat;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007337 File Offset: 0x00005537
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000733F File Offset: 0x0000553F
		[DataSourceProperty]
		public string HeaderID
		{
			get
			{
				return this._headerID;
			}
			set
			{
				if (value != this._headerID)
				{
					this._headerID = value;
					base.OnPropertyChangedWithValue<string>(value, "HeaderID");
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007362 File Offset: 0x00005562
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000736A File Offset: 0x0000556A
		[DataSourceProperty]
		public bool IsIrregularStat
		{
			get
			{
				return this._isIrregularStat;
			}
			set
			{
				if (value != this._isIrregularStat)
				{
					this._isIrregularStat = value;
					base.OnPropertyChangedWithValue(value, "IsIrregularStat");
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007388 File Offset: 0x00005588
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00007390 File Offset: 0x00005590
		[DataSourceProperty]
		public bool IsAvatarStat
		{
			get
			{
				return this._isAvatarStat;
			}
			set
			{
				if (value != this._isAvatarStat)
				{
					this._isAvatarStat = value;
					base.OnPropertyChangedWithValue(value, "IsAvatarStat");
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000073AE File Offset: 0x000055AE
		[DataSourceProperty]
		public MissionScoreboardPlayerSortControllerVM PlayerSortController
		{
			get
			{
				return this._side.PlayerSortController;
			}
		}

		// Token: 0x040000E4 RID: 228
		private readonly MissionScoreboardSideVM _side;

		// Token: 0x040000E5 RID: 229
		private string _headerID = "";

		// Token: 0x040000E6 RID: 230
		private bool _isIrregularStat;

		// Token: 0x040000E7 RID: 231
		private bool _isAvatarStat;
	}
}
