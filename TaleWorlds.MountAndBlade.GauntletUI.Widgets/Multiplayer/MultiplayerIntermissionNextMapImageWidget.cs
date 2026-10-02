using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008B RID: 139
	public class MultiplayerIntermissionNextMapImageWidget : Widget
	{
		// Token: 0x060007B1 RID: 1969 RVA: 0x0001658E File Offset: 0x0001478E
		public MultiplayerIntermissionNextMapImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00016597 File Offset: 0x00014797
		private void UpdateMapImage()
		{
			if (string.IsNullOrEmpty(this.MapID))
			{
				return;
			}
			base.Sprite = base.Context.SpriteData.GetSprite(this.MapID);
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000165C3 File Offset: 0x000147C3
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x000165CB File Offset: 0x000147CB
		[DataSourceProperty]
		public string MapID
		{
			get
			{
				return this._mapID;
			}
			set
			{
				if (value != this._mapID)
				{
					this._mapID = value;
					base.OnPropertyChanged<string>(value, "MapID");
					this.UpdateMapImage();
				}
			}
		}

		// Token: 0x04000360 RID: 864
		private string _mapID;
	}
}
