using System;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.ImageIdentifiers
{
	// Token: 0x02000021 RID: 33
	public abstract class ImageIdentifierVM : ViewModel
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000598D File Offset: 0x00003B8D
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00005998 File Offset: 0x00003B98
		protected ImageIdentifier ImageIdentifier
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
					ImageIdentifier imageIdentifier = this._imageIdentifier;
					this.Id = ((imageIdentifier != null) ? imageIdentifier.Id : null) ?? string.Empty;
					ImageIdentifier imageIdentifier2 = this._imageIdentifier;
					this.AdditionalArgs = ((imageIdentifier2 != null) ? imageIdentifier2.AdditionalArgs : null) ?? string.Empty;
					ImageIdentifier imageIdentifier3 = this._imageIdentifier;
					this.TextureProviderName = ((imageIdentifier3 != null) ? imageIdentifier3.TextureProviderName : null) ?? string.Empty;
				}
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005A18 File Offset: 0x00003C18
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ImageIdentifier = null;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00005A27 File Offset: 0x00003C27
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00005A2F File Offset: 0x00003C2F
		[DataSourceProperty]
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (this._id != value)
				{
					this._id = value;
					base.OnPropertyChangedWithValue<string>(value, "Id");
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00005A52 File Offset: 0x00003C52
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00005A5A File Offset: 0x00003C5A
		[DataSourceProperty]
		public string AdditionalArgs
		{
			get
			{
				return this._additionalArgs;
			}
			set
			{
				if (value != this._additionalArgs)
				{
					this._additionalArgs = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalArgs");
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005A7D File Offset: 0x00003C7D
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00005A85 File Offset: 0x00003C85
		[DataSourceProperty]
		public string TextureProviderName
		{
			get
			{
				return this._textureProviderName;
			}
			set
			{
				if (value != this._textureProviderName)
				{
					this._textureProviderName = value;
					base.OnPropertyChangedWithValue<string>(value, "TextureProviderName");
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00005AA8 File Offset: 0x00003CA8
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return !string.IsNullOrEmpty(this.TextureProviderName) && string.IsNullOrEmpty(this.ImageIdentifier.Id);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00005AC9 File Offset: 0x00003CC9
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return !this.IsEmpty;
			}
		}

		// Token: 0x040000A7 RID: 167
		private ImageIdentifier _imageIdentifier;

		// Token: 0x040000A8 RID: 168
		private string _id;

		// Token: 0x040000A9 RID: 169
		private string _additionalArgs;

		// Token: 0x040000AA RID: 170
		private string _textureProviderName;
	}
}
