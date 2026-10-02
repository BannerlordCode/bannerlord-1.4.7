using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000155 RID: 341
	public class CharacterCreationReviewStageItemVM : ViewModel
	{
		// Token: 0x06002029 RID: 8233 RVA: 0x00075C2E File Offset: 0x00073E2E
		public CharacterCreationReviewStageItemVM(BannerImageIdentifierVM imageIdentifier, string title, string text, string description)
			: this(title, text, description)
		{
			this.HasImage = true;
			this.ImageIdentifier = imageIdentifier;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00075C48 File Offset: 0x00073E48
		public CharacterCreationReviewStageItemVM(string title, string text, string description)
		{
			this.Title = title;
			this.Text = text;
			this.Description = description;
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x00075C65 File Offset: 0x00073E65
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x00075C6D File Offset: 0x00073E6D
		[DataSourceProperty]
		public bool HasImage
		{
			get
			{
				return this._hasImage;
			}
			set
			{
				if (value != this._hasImage)
				{
					this._hasImage = value;
					base.OnPropertyChangedWithValue(value, "HasImage");
				}
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x00075C8B File Offset: 0x00073E8B
		// (set) Token: 0x0600202E RID: 8238 RVA: 0x00075C93 File Offset: 0x00073E93
		[DataSourceProperty]
		public BannerImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x00075CB1 File Offset: 0x00073EB1
		// (set) Token: 0x06002030 RID: 8240 RVA: 0x00075CB9 File Offset: 0x00073EB9
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x00075CDC File Offset: 0x00073EDC
		// (set) Token: 0x06002032 RID: 8242 RVA: 0x00075CE4 File Offset: 0x00073EE4
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x00075D07 File Offset: 0x00073F07
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x00075D0F File Offset: 0x00073F0F
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x04000EF8 RID: 3832
		private bool _hasImage;

		// Token: 0x04000EF9 RID: 3833
		private BannerImageIdentifierVM _imageIdentifier;

		// Token: 0x04000EFA RID: 3834
		private string _title;

		// Token: 0x04000EFB RID: 3835
		private string _text;

		// Token: 0x04000EFC RID: 3836
		private string _description;
	}
}
