using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapConversation
{
	// Token: 0x02000127 RID: 295
	public class MapConversationTableauWidget : TextureWidget
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x0002ADD5 File Offset: 0x00028FD5
		public MapConversationTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "MapConversationTextureProvider";
			this._isRenderRequestedPreviousFrame = false;
			base.UpdateTextureWidget();
			base.EventManager.AddAfterFinalizedCallback(new Action(this.OnEventManagerIsFinalized));
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0002AE0D File Offset: 0x0002900D
		private void OnEventManagerIsFinalized()
		{
			if (!base.SetForClearNextFrame)
			{
				TextureProvider textureProvider = base.TextureProvider;
				if (textureProvider != null)
				{
					textureProvider.SetProperty("IsReleased", true);
				}
				TextureProvider textureProvider2 = base.TextureProvider;
				if (textureProvider2 == null)
				{
					return;
				}
				textureProvider2.Clear(false);
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0002AE44 File Offset: 0x00029044
		public override void OnClearTextureProvider()
		{
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0002AE46 File Offset: 0x00029046
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0002AE50 File Offset: 0x00029050
		[Editor(false)]
		public object Data
		{
			get
			{
				return this._data;
			}
			set
			{
				if (value != this._data)
				{
					this._data = value;
					base.OnPropertyChanged<object>(value, "Data");
					base.SetTextureProviderProperty("IsEnabled", this._data != null);
					base.SetTextureProviderProperty("Data", value);
					if (this._data != null)
					{
						this._isRenderRequestedPreviousFrame = true;
					}
				}
			}
		}

		// Token: 0x0400070E RID: 1806
		private object _data;
	}
}
