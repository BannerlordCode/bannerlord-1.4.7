using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.DamageFeed
{
	// Token: 0x02000065 RID: 101
	public class MissionAgentDamageFeedItemVM : ViewModel
	{
		// Token: 0x060007FD RID: 2045 RVA: 0x0001C100 File Offset: 0x0001A300
		public MissionAgentDamageFeedItemVM(string feedText, Action<MissionAgentDamageFeedItemVM> onRemove)
		{
			this._onRemove = onRemove;
			this.FeedText = feedText;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001C116 File Offset: 0x0001A316
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001C124 File Offset: 0x0001A324
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0001C12C File Offset: 0x0001A32C
		[DataSourceProperty]
		public string FeedText
		{
			get
			{
				return this._feedText;
			}
			set
			{
				if (value != this._feedText)
				{
					this._feedText = value;
					base.OnPropertyChangedWithValue<string>(value, "FeedText");
				}
			}
		}

		// Token: 0x04000392 RID: 914
		private readonly Action<MissionAgentDamageFeedItemVM> _onRemove;

		// Token: 0x04000393 RID: 915
		private string _feedText;
	}
}
