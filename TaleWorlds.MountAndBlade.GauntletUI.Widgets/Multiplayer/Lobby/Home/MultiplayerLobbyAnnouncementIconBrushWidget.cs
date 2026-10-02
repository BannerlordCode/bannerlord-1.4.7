using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Home
{
	// Token: 0x020000AF RID: 175
	public class MultiplayerLobbyAnnouncementIconBrushWidget : BrushWidget
	{
		// Token: 0x0600092D RID: 2349 RVA: 0x00019FC0 File Offset: 0x000181C0
		public MultiplayerLobbyAnnouncementIconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00019FCC File Offset: 0x000181CC
		private void UpdateIcon()
		{
			if (this.AnnouncementType == null)
			{
				return;
			}
			Brush iconBrush = this.IconBrush;
			Sprite sprite;
			if (iconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = iconBrush.GetLayer(this.AnnouncementType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			Sprite sprite2 = sprite;
			if (base.Brush != null)
			{
				base.Brush.Sprite = sprite2;
				foreach (BrushLayer brushLayer in base.Brush.Layers)
				{
					brushLayer.Sprite = sprite2;
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0001A064 File Offset: 0x00018264
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0001A06C File Offset: 0x0001826C
		public string AnnouncementType
		{
			get
			{
				return this._announcementType;
			}
			set
			{
				if (value != this._announcementType)
				{
					this._announcementType = value;
					base.OnPropertyChanged<string>(value, "AnnouncementType");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0001A095 File Offset: 0x00018295
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x0001A09D File Offset: 0x0001829D
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (value != this._iconBrush)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x0400042A RID: 1066
		private string _announcementType;

		// Token: 0x0400042B RID: 1067
		private Brush _iconBrush;
	}
}
