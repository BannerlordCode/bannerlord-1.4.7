using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A6 RID: 166
	public class MultiplayerLobbyGameTypeItemButtonWidget : ButtonWidget
	{
		// Token: 0x060008C9 RID: 2249 RVA: 0x000192F7 File Offset: 0x000174F7
		public MultiplayerLobbyGameTypeItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00019300 File Offset: 0x00017500
		private void UpdateSprite()
		{
			base.Brush.DefaultLayer.Sprite = base.Context.SpriteData.GetSprite("MPLobby\\GameTypes\\" + this.GameTypeID);
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00019332 File Offset: 0x00017532
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0001933A File Offset: 0x0001753A
		[Editor(false)]
		public string GameTypeID
		{
			get
			{
				return this._gameTypeID;
			}
			set
			{
				if (value != this._gameTypeID)
				{
					this._gameTypeID = value;
					base.OnPropertyChanged<string>(value, "GameTypeID");
					this.UpdateSprite();
				}
			}
		}

		// Token: 0x040003FD RID: 1021
		private string _gameTypeID;
	}
}
