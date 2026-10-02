using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000AB RID: 171
	public class MultiplayerLobbyRankItemButtonWidget : ButtonWidget
	{
		// Token: 0x060008F2 RID: 2290 RVA: 0x0001971D File Offset: 0x0001791D
		public MultiplayerLobbyRankItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00019728 File Offset: 0x00017928
		private void UpdateSprite()
		{
			string text = "unranked";
			if (this.RankID != string.Empty)
			{
				text = this.RankID;
			}
			base.Brush.DefaultLayer.Sprite = base.Context.SpriteData.GetSprite("MPGeneral\\MPRanks\\" + text);
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0001977F File Offset: 0x0001797F
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00019787 File Offset: 0x00017987
		[Editor(false)]
		public string RankID
		{
			get
			{
				return this._rankID;
			}
			set
			{
				if (value != this._rankID)
				{
					this._rankID = value;
					base.OnPropertyChanged<string>(value, "RankID");
					this.UpdateSprite();
				}
			}
		}

		// Token: 0x0400040C RID: 1036
		private const string _defaultRankID = "unranked";

		// Token: 0x0400040D RID: 1037
		private string _rankID;
	}
}
