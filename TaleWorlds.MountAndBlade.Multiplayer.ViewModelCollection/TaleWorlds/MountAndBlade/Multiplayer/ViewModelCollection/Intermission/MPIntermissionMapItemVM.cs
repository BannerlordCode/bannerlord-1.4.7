using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission
{
	// Token: 0x0200008F RID: 143
	public class MPIntermissionMapItemVM : ViewModel
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002A053 File Offset: 0x00028253
		public MPIntermissionMapItemVM(string mapID, Action<MPIntermissionMapItemVM> onPlayerVoted)
		{
			this.MapID = mapID;
			this._onPlayerVoted = onPlayerVoted;
			this.RefreshValues();
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002A070 File Offset: 0x00028270
		public override void RefreshValues()
		{
			TextObject textObject;
			if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject, this.MapID))
			{
				this.MapName = textObject.ToString();
				return;
			}
			this.MapName = this.MapID;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002A0AA File Offset: 0x000282AA
		public void ExecuteVote()
		{
			this._onPlayerVoted(this);
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0002A0B8 File Offset: 0x000282B8
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x0002A0C0 File Offset: 0x000282C0
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

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0002A0DE File Offset: 0x000282DE
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0002A0E6 File Offset: 0x000282E6
		[DataSourceProperty]
		public string MapID
		{
			get
			{
				return this._mapID;
			}
			set
			{
				if (value != this._mapID)
				{
					this._mapID = value;
					base.OnPropertyChangedWithValue<string>(value, "MapID");
				}
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0002A109 File Offset: 0x00028309
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0002A111 File Offset: 0x00028311
		[DataSourceProperty]
		public string MapName
		{
			get
			{
				return this._mapName;
			}
			set
			{
				if (value != this._mapName)
				{
					this._mapName = value;
					base.OnPropertyChangedWithValue<string>(value, "MapName");
				}
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0002A134 File Offset: 0x00028334
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x0002A13C File Offset: 0x0002833C
		[DataSourceProperty]
		public int Votes
		{
			get
			{
				return this._votes;
			}
			set
			{
				if (value != this._votes)
				{
					this._votes = value;
					base.OnPropertyChangedWithValue(value, "Votes");
				}
			}
		}

		// Token: 0x0400063D RID: 1597
		private readonly Action<MPIntermissionMapItemVM> _onPlayerVoted;

		// Token: 0x0400063E RID: 1598
		private bool _isSelected;

		// Token: 0x0400063F RID: 1599
		private string _mapID;

		// Token: 0x04000640 RID: 1600
		private string _mapName;

		// Token: 0x04000641 RID: 1601
		private int _votes;
	}
}
