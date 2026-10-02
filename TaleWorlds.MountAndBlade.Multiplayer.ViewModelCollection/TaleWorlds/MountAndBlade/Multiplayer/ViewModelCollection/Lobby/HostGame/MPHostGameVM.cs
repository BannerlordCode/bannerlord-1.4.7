using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame
{
	// Token: 0x02000046 RID: 70
	public class MPHostGameVM : ViewModel
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x000152B0 File Offset: 0x000134B0
		public MPHostGameVM(LobbyState lobbyState, MPCustomGameVM.CustomGameMode customGameMode)
		{
			this._lobbyState = lobbyState;
			this._customGameMode = customGameMode;
			this.HostGameOptions = new MPHostGameOptionsVM(false, this._customGameMode);
			this.RefreshValues();
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000152DE File Offset: 0x000134DE
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CreateText = new TextObject("{=aRzlp5XH}CREATE", null).ToString();
			this.HostGameOptions.RefreshValues();
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00015307 File Offset: 0x00013507
		public void ExecuteStart()
		{
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this._lobbyState.HostGame();
				return;
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this._lobbyState.CreatePremadeGame();
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00015331 File Offset: 0x00013531
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x00015339 File Offset: 0x00013539
		[DataSourceProperty]
		public MPHostGameOptionsVM HostGameOptions
		{
			get
			{
				return this._hostGameOptions;
			}
			set
			{
				if (value != this._hostGameOptions)
				{
					this._hostGameOptions = value;
					base.OnPropertyChangedWithValue<MPHostGameOptionsVM>(value, "HostGameOptions");
				}
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00015357 File Offset: 0x00013557
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0001535F File Offset: 0x0001355F
		[DataSourceProperty]
		public string CreateText
		{
			get
			{
				return this._createText;
			}
			set
			{
				if (value != this._createText)
				{
					this._createText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateText");
				}
			}
		}

		// Token: 0x0400030E RID: 782
		private LobbyState _lobbyState;

		// Token: 0x0400030F RID: 783
		private MPCustomGameVM.CustomGameMode _customGameMode;

		// Token: 0x04000310 RID: 784
		private MPHostGameOptionsVM _hostGameOptions;

		// Token: 0x04000311 RID: 785
		private string _createText;
	}
}
