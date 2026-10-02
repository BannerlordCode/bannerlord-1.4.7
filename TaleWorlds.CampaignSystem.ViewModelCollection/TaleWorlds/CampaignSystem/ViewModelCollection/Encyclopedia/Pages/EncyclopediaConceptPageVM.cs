using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D0 RID: 208
	[EncyclopediaViewModel(typeof(Concept))]
	public class EncyclopediaConceptPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x060013C8 RID: 5064 RVA: 0x0004F8C4 File Offset: 0x0004DAC4
		public EncyclopediaConceptPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._concept = base.Obj as Concept;
			Concept.SetConceptTextLinks();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._concept);
			this.RefreshValues();
			this.Refresh();
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0004F91A File Offset: 0x0004DB1A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = this._concept.Title.ToString();
			this.DescriptionText = this._concept.Description.ToString();
			base.UpdateBookmarkHintText();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004F954 File Offset: 0x0004DB54
		public override void Refresh()
		{
			base.IsLoadingOver = false;
			base.IsLoadingOver = true;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004F964 File Offset: 0x0004DB64
		public override string GetName()
		{
			return this._concept.Title.ToString();
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004F976 File Offset: 0x0004DB76
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004F988 File Offset: 0x0004DB88
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Concept", GameTexts.FindText("str_encyclopedia_concepts", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004F9F0 File Offset: 0x0004DBF0
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._concept);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._concept);
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x0004FA40 File Offset: 0x0004DC40
		// (set) Token: 0x060013D0 RID: 5072 RVA: 0x0004FA48 File Offset: 0x0004DC48
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x0004FA6B File Offset: 0x0004DC6B
		// (set) Token: 0x060013D2 RID: 5074 RVA: 0x0004FA73 File Offset: 0x0004DC73
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x0400090E RID: 2318
		private Concept _concept;

		// Token: 0x0400090F RID: 2319
		private string _titleText;

		// Token: 0x04000910 RID: 2320
		private string _descriptionText;
	}
}
