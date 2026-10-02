using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.DamageFeed
{
	// Token: 0x02000066 RID: 102
	public class MissionAgentDamageFeedVM : ViewModel
	{
		// Token: 0x06000801 RID: 2049 RVA: 0x0001C14F File Offset: 0x0001A34F
		public MissionAgentDamageFeedVM()
		{
			this._takenDamageText = new TextObject("{=meFS5F4V}-{DAMAGE}", null);
			this.FeedList = new MBBindingList<MissionAgentDamageFeedItemVM>();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001C174 File Offset: 0x0001A374
		public void OnMainAgentHit(float damage)
		{
			if (damage > 0f)
			{
				this._takenDamageText.SetTextVariable("DAMAGE", damage, 2);
				MissionAgentDamageFeedItemVM missionAgentDamageFeedItemVM = new MissionAgentDamageFeedItemVM(this._takenDamageText.ToString(), new Action<MissionAgentDamageFeedItemVM>(this.RemoveItem));
				this.FeedList.Add(missionAgentDamageFeedItemVM);
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001C1C5 File Offset: 0x0001A3C5
		private void RemoveItem(MissionAgentDamageFeedItemVM item)
		{
			this.FeedList.Remove(item);
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001C1D4 File Offset: 0x0001A3D4
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x0001C1DC File Offset: 0x0001A3DC
		[DataSourceProperty]
		public MBBindingList<MissionAgentDamageFeedItemVM> FeedList
		{
			get
			{
				return this._feedList;
			}
			set
			{
				if (value != this._feedList)
				{
					this._feedList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentDamageFeedItemVM>>(value, "FeedList");
				}
			}
		}

		// Token: 0x04000394 RID: 916
		private readonly TextObject _takenDamageText;

		// Token: 0x04000395 RID: 917
		private MBBindingList<MissionAgentDamageFeedItemVM> _feedList;
	}
}
