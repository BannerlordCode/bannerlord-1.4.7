using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Matchmaking
{
	// Token: 0x020000AD RID: 173
	public class MultiplayerLobbyMatchmakingRegionConnectionQualityTextWidget : TextWidget
	{
		// Token: 0x06000916 RID: 2326 RVA: 0x00019C75 File Offset: 0x00017E75
		public MultiplayerLobbyMatchmakingRegionConnectionQualityTextWidget(UIContext context)
			: base(context)
		{
			base.AddState("PoorQuality");
			base.AddState("AverageQuality");
			base.AddState("GoodQuality");
			this.ConnectionQualityLevelUpdated();
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00019CA8 File Offset: 0x00017EA8
		private void ConnectionQualityLevelUpdated()
		{
			switch (this.ConnectionQualityLevel)
			{
			case 0:
				this.SetState("PoorQuality");
				return;
			case 1:
				this.SetState("AverageQuality");
				return;
			case 2:
				this.SetState("GoodQuality");
				return;
			default:
				this.SetState("Default");
				return;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00019CFF File Offset: 0x00017EFF
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00019D07 File Offset: 0x00017F07
		[Editor(false)]
		public int ConnectionQualityLevel
		{
			get
			{
				return this._connectionQualityLevel;
			}
			set
			{
				if (this._connectionQualityLevel != value)
				{
					this._connectionQualityLevel = value;
					base.OnPropertyChanged(value, "ConnectionQualityLevel");
					this.ConnectionQualityLevelUpdated();
				}
			}
		}

		// Token: 0x0400041C RID: 1052
		private int _connectionQualityLevel;
	}
}
