using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000E7 RID: 231
	public class EncyclopediaHistoryEventVM : EncyclopediaLinkVM
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x0005540F File Offset: 0x0005360F
		public EncyclopediaHistoryEventVM(IEncyclopediaLog log)
		{
			this._log = log;
			this.RefreshValues();
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00055424 File Offset: 0x00053624
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HistoryEventTimeText = this._log.GameTime.ToString();
			this.HistoryEventText = this._log.GetEncyclopediaText().ToString();
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0005546C File Offset: 0x0005366C
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x0005547E File Offset: 0x0005367E
		// (set) Token: 0x060015A4 RID: 5540 RVA: 0x00055486 File Offset: 0x00053686
		[DataSourceProperty]
		public string HistoryEventTimeText
		{
			get
			{
				return this._historyEventTimeText;
			}
			set
			{
				if (value != this._historyEventTimeText)
				{
					this._historyEventTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "HistoryEventTimeText");
				}
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x000554A9 File Offset: 0x000536A9
		// (set) Token: 0x060015A6 RID: 5542 RVA: 0x000554B1 File Offset: 0x000536B1
		[DataSourceProperty]
		public string HistoryEventText
		{
			get
			{
				return this._historyEventText;
			}
			set
			{
				if (value != this._historyEventText)
				{
					this._historyEventText = value;
					base.OnPropertyChangedWithValue<string>(value, "HistoryEventText");
				}
			}
		}

		// Token: 0x040009D6 RID: 2518
		private readonly IEncyclopediaLog _log;

		// Token: 0x040009D7 RID: 2519
		private string _historyEventText;

		// Token: 0x040009D8 RID: 2520
		private string _historyEventTimeText;
	}
}
