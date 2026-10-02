using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000063 RID: 99
	public class MultiplayerSubModule : MBSubModuleBase
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000D7F0 File Offset: 0x0000B9F0
		protected internal override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("TeamDeathmatch"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Duel"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Siege"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Captain"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Skirmish"));
			Module.CurrentModule.AddMultiplayerGameMode(new MissionBasedMultiplayerGameMode("Battle"));
			TextObject coreContentDisabledReason = new TextObject("{=V8BXjyYq}Disabled during installation.", null);
			if (Module.CurrentModule.StartupInfo.StartupType != GameStartupType.Singleplayer)
			{
				Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Multiplayer", new TextObject("{=YDYnuBmC}Multiplayer", null), 9997, new Action(this.StartMultiplayer), () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason), null, null));
			}
			TauntUsageManager.Initialize();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		public override void OnGameLoaded(Game game, object initializerObject)
		{
			base.OnGameLoaded(game, initializerObject);
			MultiplayerMain.Initialize(new GameNetworkHandler());
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D8FC File Offset: 0x0000BAFC
		protected internal override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D905 File Offset: 0x0000BB05
		protected internal override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
			if (GameNetwork.IsDedicatedServer)
			{
				MBGameManager.StartNewGame(new MultiplayerGameManager());
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D920 File Offset: 0x0000BB20
		public override void OnInitialState()
		{
			base.OnInitialState();
			if (Utilities.CommandLineArgumentExists("+connect_lobby"))
			{
				MBGameManager.StartNewGame(new MultiplayerGameManager());
				return;
			}
			if (!Module.CurrentModule.IsOnlyCoreContentEnabled && Module.CurrentModule.MultiplayerRequested)
			{
				MBGameManager.StartNewGame(new MultiplayerGameManager());
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D96C File Offset: 0x0000BB6C
		private async void StartMultiplayer()
		{
			if (!this._isConnectingToMultiplayer)
			{
				this._isConnectingToMultiplayer = true;
				bool flag = NetworkMain.GameClient != null && await NetworkMain.GameClient.CheckConnection();
				bool isConnected = flag;
				PlatformServices.Instance.CheckPrivilege(Privilege.Multiplayer, true, delegate(bool result)
				{
					if (!isConnected || !result)
					{
						string text = new TextObject("{=ksq1IBh3}No connection", null).ToString();
						string text2 = new TextObject("{=5VIbo2Cb}No connection could be established to the lobby server. Check your internet connection and try again.", null).ToString();
						InformationManager.ShowInquiry(new InquiryData(text, text2, false, true, "", new TextObject("{=dismissnotification}Dismiss", null).ToString(), null, delegate
						{
							InformationManager.HideInquiry();
						}, "", 0f, null, null, null), false, false);
						return;
					}
					MBGameManager.StartNewGame(new MultiplayerGameManager());
				});
				this._isConnectingToMultiplayer = false;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000D9A5 File Offset: 0x0000BBA5
		protected internal override void OnNetworkTick(float dt)
		{
			base.OnNetworkTick(dt);
			MultiplayerMain.Tick(dt);
			InternetAvailabilityChecker.Tick(dt);
		}

		// Token: 0x040000EE RID: 238
		private bool _isConnectingToMultiplayer;
	}
}
