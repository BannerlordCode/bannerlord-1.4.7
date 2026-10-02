using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000C9 RID: 201
	public class EncyclopediaHomeVM : EncyclopediaPageVM
	{
		// Token: 0x06001338 RID: 4920 RVA: 0x0004DC44 File Offset: 0x0004BE44
		public EncyclopediaHomeVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this.Lists = new MBBindingList<ListTypeVM>();
			foreach (EncyclopediaPage encyclopediaPage in from p in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages()
				orderby p.HomePageOrderIndex
				select p)
			{
				if (encyclopediaPage.IsRelevant())
				{
					this.Lists.Add(new ListTypeVM(encyclopediaPage));
				}
			}
			this.RefreshValues();
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0004DCE8 File Offset: 0x0004BEE8
		public override void Refresh()
		{
			base.Refresh();
			this.RefreshValues();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0004DCF8 File Offset: 0x0004BEF8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._baseName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
			this.HomeTitleText = GameTexts.FindText("str_encyclopedia_name", null).ToString();
			this.Lists.ApplyActionOnAllItems(delegate(ListTypeVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0004DD61 File Offset: 0x0004BF61
		public override string GetNavigationBarURL()
		{
			return GameTexts.FindText("str_encyclopedia_home", null).ToString() + " \\";
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0004DD7D File Offset: 0x0004BF7D
		public override string GetName()
		{
			return this._baseName;
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x0004DD85 File Offset: 0x0004BF85
		// (set) Token: 0x0600133E RID: 4926 RVA: 0x0004DD8D File Offset: 0x0004BF8D
		[DataSourceProperty]
		public bool IsListActive
		{
			get
			{
				return this._isListActive;
			}
			set
			{
				if (value != this._isListActive)
				{
					this._isListActive = value;
					base.OnPropertyChangedWithValue(value, "IsListActive");
				}
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x0004DDAB File Offset: 0x0004BFAB
		// (set) Token: 0x06001340 RID: 4928 RVA: 0x0004DDB3 File Offset: 0x0004BFB3
		[DataSourceProperty]
		public string HomeTitleText
		{
			get
			{
				return this._homeTitleText;
			}
			set
			{
				if (value != this._homeTitleText)
				{
					this._homeTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "HomeTitleText");
				}
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x0004DDD6 File Offset: 0x0004BFD6
		// (set) Token: 0x06001342 RID: 4930 RVA: 0x0004DDDE File Offset: 0x0004BFDE
		[DataSourceProperty]
		public MBBindingList<ListTypeVM> Lists
		{
			get
			{
				return this._lists;
			}
			set
			{
				if (value != this._lists)
				{
					this._lists = value;
					base.OnPropertyChangedWithValue<MBBindingList<ListTypeVM>>(value, "Lists");
				}
			}
		}

		// Token: 0x040008CE RID: 2254
		private string _baseName;

		// Token: 0x040008CF RID: 2255
		private MBBindingList<ListTypeVM> _lists;

		// Token: 0x040008D0 RID: 2256
		private bool _isListActive;

		// Token: 0x040008D1 RID: 2257
		private string _homeTitleText;
	}
}
