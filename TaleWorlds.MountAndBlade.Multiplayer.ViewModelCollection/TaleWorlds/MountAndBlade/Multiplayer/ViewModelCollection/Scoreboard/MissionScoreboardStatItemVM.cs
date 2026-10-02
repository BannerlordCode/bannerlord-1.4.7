using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x02000020 RID: 32
	public class MissionScoreboardStatItemVM : ViewModel
	{
		// Token: 0x06000214 RID: 532 RVA: 0x00008496 File Offset: 0x00006696
		public MissionScoreboardStatItemVM(MissionScoreboardPlayerVM belongedPlayer, string headerID, string item)
		{
			this.Item = item;
			this.HeaderID = headerID;
			this.BelongedPlayer = belongedPlayer;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000084BE File Offset: 0x000066BE
		// (set) Token: 0x06000216 RID: 534 RVA: 0x000084C6 File Offset: 0x000066C6
		[DataSourceProperty]
		public string Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue<string>(value, "Item");
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000084E9 File Offset: 0x000066E9
		// (set) Token: 0x06000218 RID: 536 RVA: 0x000084F1 File Offset: 0x000066F1
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00008514 File Offset: 0x00006714
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0000851C File Offset: 0x0000671C
		[DataSourceProperty]
		public MissionScoreboardPlayerVM BelongedPlayer
		{
			get
			{
				return this._belongedPlayer;
			}
			set
			{
				if (value != this._belongedPlayer)
				{
					this._belongedPlayer = value;
					base.OnPropertyChangedWithValue<MissionScoreboardPlayerVM>(value, "BelongedPlayer");
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000853A File Offset: 0x0000673A
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardMVPItemVM> MVPBadges
		{
			get
			{
				return this.BelongedPlayer.MVPBadges;
			}
		}

		// Token: 0x0400011E RID: 286
		private string _item;

		// Token: 0x0400011F RID: 287
		private string _headerID = "";

		// Token: 0x04000120 RID: 288
		private MissionScoreboardPlayerVM _belongedPlayer;
	}
}
