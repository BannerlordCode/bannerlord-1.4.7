using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle
{
	// Token: 0x02000084 RID: 132
	public class MPAfterBattleBadgeRewardItemVM : MPAfterBattleRewardItemVM
	{
		// Token: 0x06000D06 RID: 3334 RVA: 0x000283FE File Offset: 0x000265FE
		public MPAfterBattleBadgeRewardItemVM(Badge badge)
		{
			base.Type = 1;
			base.Name = badge.Name.ToString();
			this.BadgeID = badge.StringId;
			this.RefreshValues();
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00028430 File Offset: 0x00026630
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00028438 File Offset: 0x00026638
		[DataSourceProperty]
		public string BadgeID
		{
			get
			{
				return this._badgeID;
			}
			set
			{
				if (value != this._badgeID)
				{
					this._badgeID = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeID");
				}
			}
		}

		// Token: 0x040005E5 RID: 1509
		private string _badgeID;
	}
}
