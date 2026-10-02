using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000010 RID: 16
	public class MultiplayerDuelVM : ViewModel
	{
		// Token: 0x060000BA RID: 186 RVA: 0x0000452C File Offset: 0x0000272C
		public MultiplayerDuelVM(Camera missionCamera, MissionMultiplayerGameModeDuelClient client)
		{
			this._missionCamera = missionCamera;
			this._client = client;
			MissionMultiplayerGameModeDuelClient client2 = this._client;
			client2.OnMyRepresentativeAssigned = (Action)Delegate.Combine(client2.OnMyRepresentativeAssigned, new Action(this.OnMyRepresentativeAssigned));
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.PlayerDuelMatch = new DuelMatchVM();
			this.OngoingDuels = new MBBindingList<DuelMatchVM>();
			this._duelArenaProperties = new List<MultiplayerDuelVM.DuelArenaProperties>();
			List<GameEntity> list = new List<GameEntity>();
			list.AddRange(Mission.Current.Scene.FindEntitiesWithTagExpression("area_flag(_\\d+)*"));
			foreach (GameEntity gameEntity in list)
			{
				MultiplayerDuelVM.DuelArenaProperties arenaPropertiesOfFlagEntity = this.GetArenaPropertiesOfFlagEntity(gameEntity);
				this._duelArenaProperties.Add(arenaPropertiesOfFlagEntity);
			}
			this.Markers = new MissionDuelMarkersVM(missionCamera, this._client);
			this.KillNotifications = new MBBindingList<MPDuelKillNotificationItemVM>();
			this._scoreWithSeparatorText = new TextObject("{=J5rb5YVV}/ {SCORE}", null);
			this.RefreshValues();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000464C File Offset: 0x0000284C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PlayerDuelMatch.RefreshValues();
			this.Markers.RefreshValues();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000466C File Offset: 0x0000286C
		private void OnMyRepresentativeAssigned()
		{
			DuelMissionRepresentative myRepresentative = this._client.MyRepresentative;
			myRepresentative.OnDuelPrepStartedEvent = (Action<MissionPeer, int>)Delegate.Combine(myRepresentative.OnDuelPrepStartedEvent, new Action<MissionPeer, int>(this.OnDuelPrepStarted));
			DuelMissionRepresentative myRepresentative2 = this._client.MyRepresentative;
			myRepresentative2.OnAgentSpawnedWithoutDuelEvent = (Action)Delegate.Combine(myRepresentative2.OnAgentSpawnedWithoutDuelEvent, new Action(this.OnAgentSpawnedWithoutDuel));
			DuelMissionRepresentative myRepresentative3 = this._client.MyRepresentative;
			myRepresentative3.OnDuelPreparationStartedForTheFirstTimeEvent = (Action<MissionPeer, MissionPeer, int>)Delegate.Combine(myRepresentative3.OnDuelPreparationStartedForTheFirstTimeEvent, new Action<MissionPeer, MissionPeer, int>(this.OnDuelStarted));
			DuelMissionRepresentative myRepresentative4 = this._client.MyRepresentative;
			myRepresentative4.OnDuelEndedEvent = (Action<MissionPeer>)Delegate.Combine(myRepresentative4.OnDuelEndedEvent, new Action<MissionPeer>(this.OnDuelEnded));
			DuelMissionRepresentative myRepresentative5 = this._client.MyRepresentative;
			myRepresentative5.OnDuelRoundEndedEvent = (Action<MissionPeer>)Delegate.Combine(myRepresentative5.OnDuelRoundEndedEvent, new Action<MissionPeer>(this.OnDuelRoundEnded));
			DuelMissionRepresentative myRepresentative6 = this._client.MyRepresentative;
			myRepresentative6.OnMyPreferredZoneChanged = (Action<TroopType>)Delegate.Combine(myRepresentative6.OnMyPreferredZoneChanged, new Action<TroopType>(this.OnPlayerPreferredZoneChanged));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			this.Markers.RegisterEvents();
			this.UpdatePlayerScore();
			this._isMyRepresentativeAssigned = true;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000047BC File Offset: 0x000029BC
		public void Tick(float dt)
		{
			int num;
			int num2;
			if (this._gameMode.CheckTimer(out num, out num2, false))
			{
				this.RemainingRoundTime = TimeSpan.FromSeconds((double)num).ToString("mm':'ss");
			}
			this.Markers.Tick(dt);
			if (this.PlayerDuelMatch.IsEnabled)
			{
				this.PlayerDuelMatch.Tick(dt);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000481C File Offset: 0x00002A1C
		[Conditional("DEBUG")]
		private void DebugTick()
		{
			if (Input.IsKeyReleased(InputKey.Numpad3))
			{
				this._showSpawnPoints = !this._showSpawnPoints;
			}
			if (this._showSpawnPoints)
			{
				string text = "spawnpoint_area(_\\d+)*";
				foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTagExpression(text))
				{
					Vec3 vec = new Vec3(gameEntity.GlobalPosition.x, gameEntity.GlobalPosition.y, gameEntity.GlobalPosition.z, -1f);
					Vec3 vec2 = this._missionCamera.WorldPointToViewPortPoint(ref vec);
					vec2.y = 1f - vec2.y;
					if (vec2.z < 0f)
					{
						vec2.x = 1f - vec2.x;
						vec2.y = 1f - vec2.y;
						vec2.z = 0f;
						float num = 0f;
						num = ((vec2.x > num) ? vec2.x : num);
						num = ((vec2.y > num) ? vec2.y : num);
						num = ((vec2.z > num) ? vec2.z : num);
						vec2 /= num;
					}
					if (float.IsPositiveInfinity(vec2.x))
					{
						vec2.x = 1f;
					}
					else if (float.IsNegativeInfinity(vec2.x))
					{
						vec2.x = 0f;
					}
					if (float.IsPositiveInfinity(vec2.y))
					{
						vec2.y = 1f;
					}
					else if (float.IsNegativeInfinity(vec2.y))
					{
						vec2.y = 0f;
					}
					vec2.x = MathF.Clamp(vec2.x, 0f, 1f) * Screen.RealScreenResolutionWidth;
					vec2.y = MathF.Clamp(vec2.y, 0f, 1f) * Screen.RealScreenResolutionHeight;
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004A48 File Offset: 0x00002C48
		public override void OnFinalize()
		{
			base.OnFinalize();
			MissionMultiplayerGameModeDuelClient client = this._client;
			client.OnMyRepresentativeAssigned = (Action)Delegate.Remove(client.OnMyRepresentativeAssigned, new Action(this.OnMyRepresentativeAssigned));
			if (this._isMyRepresentativeAssigned)
			{
				DuelMissionRepresentative myRepresentative = this._client.MyRepresentative;
				myRepresentative.OnDuelPrepStartedEvent = (Action<MissionPeer, int>)Delegate.Remove(myRepresentative.OnDuelPrepStartedEvent, new Action<MissionPeer, int>(this.OnDuelPrepStarted));
				DuelMissionRepresentative myRepresentative2 = this._client.MyRepresentative;
				myRepresentative2.OnAgentSpawnedWithoutDuelEvent = (Action)Delegate.Remove(myRepresentative2.OnAgentSpawnedWithoutDuelEvent, new Action(this.OnAgentSpawnedWithoutDuel));
				DuelMissionRepresentative myRepresentative3 = this._client.MyRepresentative;
				myRepresentative3.OnDuelPreparationStartedForTheFirstTimeEvent = (Action<MissionPeer, MissionPeer, int>)Delegate.Remove(myRepresentative3.OnDuelPreparationStartedForTheFirstTimeEvent, new Action<MissionPeer, MissionPeer, int>(this.OnDuelStarted));
				DuelMissionRepresentative myRepresentative4 = this._client.MyRepresentative;
				myRepresentative4.OnDuelEndedEvent = (Action<MissionPeer>)Delegate.Remove(myRepresentative4.OnDuelEndedEvent, new Action<MissionPeer>(this.OnDuelEnded));
				DuelMissionRepresentative myRepresentative5 = this._client.MyRepresentative;
				myRepresentative5.OnDuelRoundEndedEvent = (Action<MissionPeer>)Delegate.Remove(myRepresentative5.OnDuelRoundEndedEvent, new Action<MissionPeer>(this.OnDuelRoundEnded));
				DuelMissionRepresentative myRepresentative6 = this._client.MyRepresentative;
				myRepresentative6.OnMyPreferredZoneChanged = (Action<TroopType>)Delegate.Remove(myRepresentative6.OnMyPreferredZoneChanged, new Action<TroopType>(this.OnPlayerPreferredZoneChanged));
				ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
				this.Markers.UnregisterEvents();
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004BC0 File Offset: 0x00002DC0
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableGenericNames)
			{
				this._ongoingDuels.ApplyActionOnAllItems(delegate(DuelMatchVM d)
				{
					d.RefreshNames(true);
				});
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004BF1 File Offset: 0x00002DF1
		private void OnDuelPrepStarted(MissionPeer opponentPeer, int duelStartTime)
		{
			this.PlayerDuelMatch.OnDuelPrepStarted(opponentPeer, duelStartTime);
			this.AreOngoingDuelsActive = false;
			this.Markers.IsEnabled = false;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004C13 File Offset: 0x00002E13
		private void OnAgentSpawnedWithoutDuel()
		{
			this.Markers.OnAgentSpawnedWithoutDuel();
			this.AreOngoingDuelsActive = true;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004C28 File Offset: 0x00002E28
		private void OnPlayerPreferredZoneChanged(TroopType zoneType)
		{
			if (zoneType != (TroopType)this.PlayerPrefferedArenaType)
			{
				this.PlayerPrefferedArenaType = (int)zoneType;
				this.Markers.OnPlayerPreferredZoneChanged((int)zoneType);
				this._hasPlayerChangedArenaPreferrence = true;
				GameTexts.SetVariable("ARENA_TYPE", this.GetArenaTypeLocalizedName(zoneType));
				InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=nLdQvaRK}Arena preference updated to {ARENA_TYPE}.", null).ToString()));
				return;
			}
			InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=YLZV7dxI}This arena type is already the preferred one.", null).ToString()));
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004CA0 File Offset: 0x00002EA0
		private void OnDuelStarted(MissionPeer firstPeer, MissionPeer secondPeer, int flagIndex)
		{
			this.Markers.OnDuelStarted(firstPeer, secondPeer);
			MultiplayerDuelVM.DuelArenaProperties duelArenaProperties = this._duelArenaProperties.First<MultiplayerDuelVM.DuelArenaProperties>((MultiplayerDuelVM.DuelArenaProperties f) => f.Index == flagIndex);
			if (firstPeer == this._client.MyRepresentative.MissionPeer || secondPeer == this._client.MyRepresentative.MissionPeer)
			{
				this.AreOngoingDuelsActive = false;
				this.IsPlayerInDuel = true;
				this.PlayerDuelMatch.OnDuelStarted(firstPeer, secondPeer, (int)duelArenaProperties.ArenaTroopType);
				return;
			}
			DuelMatchVM duelMatchVM = new DuelMatchVM();
			duelMatchVM.OnDuelStarted(firstPeer, secondPeer, (int)duelArenaProperties.ArenaTroopType);
			this.OngoingDuels.Add(duelMatchVM);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004D48 File Offset: 0x00002F48
		private void OnDuelEnded(MissionPeer winnerPeer)
		{
			if (this.PlayerDuelMatch.FirstPlayerPeer == winnerPeer || this.PlayerDuelMatch.SecondPlayerPeer == winnerPeer)
			{
				this.AreOngoingDuelsActive = true;
				this.IsPlayerInDuel = false;
				this.Markers.IsEnabled = true;
				this.Markers.SetMarkerOfPeerEnabled(this.PlayerDuelMatch.FirstPlayerPeer, true);
				this.Markers.SetMarkerOfPeerEnabled(this.PlayerDuelMatch.SecondPlayerPeer, true);
				this.PlayerDuelMatch.OnDuelEnded();
				this.PlayerBounty = this._client.MyRepresentative.Bounty;
				this.UpdatePlayerScore();
			}
			DuelMatchVM duelMatchVM = this.OngoingDuels.FirstOrDefault<DuelMatchVM>((DuelMatchVM d) => d.FirstPlayerPeer == winnerPeer || d.SecondPlayerPeer == winnerPeer);
			if (duelMatchVM != null)
			{
				this.Markers.SetMarkerOfPeerEnabled(duelMatchVM.FirstPlayerPeer, true);
				this.Markers.SetMarkerOfPeerEnabled(duelMatchVM.SecondPlayerPeer, true);
				this.OngoingDuels.Remove(duelMatchVM);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004E44 File Offset: 0x00003044
		private void OnDuelRoundEnded(MissionPeer winnerPeer)
		{
			if (this.PlayerDuelMatch.FirstPlayerPeer == winnerPeer || this.PlayerDuelMatch.SecondPlayerPeer == winnerPeer)
			{
				this.PlayerDuelMatch.OnPeerScored(winnerPeer);
				this.KillNotifications.Add(new MPDuelKillNotificationItemVM(this.PlayerDuelMatch.FirstPlayerPeer, this.PlayerDuelMatch.SecondPlayerPeer, this.PlayerDuelMatch.FirstPlayerScore, this.PlayerDuelMatch.SecondPlayerScore, (TroopType)this.PlayerDuelMatch.ArenaType, new Action<MPDuelKillNotificationItemVM>(this.RemoveKillNotification)));
				return;
			}
			DuelMatchVM duelMatchVM = this.OngoingDuels.FirstOrDefault<DuelMatchVM>((DuelMatchVM d) => d.FirstPlayerPeer == winnerPeer || d.SecondPlayerPeer == winnerPeer);
			if (duelMatchVM != null)
			{
				duelMatchVM.OnPeerScored(winnerPeer);
				this.KillNotifications.Add(new MPDuelKillNotificationItemVM(duelMatchVM.FirstPlayerPeer, duelMatchVM.SecondPlayerPeer, duelMatchVM.FirstPlayerScore, duelMatchVM.SecondPlayerScore, (TroopType)duelMatchVM.ArenaType, new Action<MPDuelKillNotificationItemVM>(this.RemoveKillNotification)));
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004F4A File Offset: 0x0000314A
		private void UpdatePlayerScore()
		{
			GameTexts.SetVariable("SCORE", this._client.MyRepresentative.Score);
			this.PlayerScoreText = this._scoreWithSeparatorText.ToString();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004F77 File Offset: 0x00003177
		private void RemoveKillNotification(MPDuelKillNotificationItemVM item)
		{
			this.KillNotifications.Remove(item);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004F86 File Offset: 0x00003186
		public void OnScreenResolutionChanged()
		{
			this.Markers.UpdateScreenCenter();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004F93 File Offset: 0x00003193
		public void OnMainAgentRemoved()
		{
			if (!this.PlayerDuelMatch.IsEnabled)
			{
				this.Markers.IsEnabled = false;
				this.AreOngoingDuelsActive = false;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004FB8 File Offset: 0x000031B8
		public void OnMainAgentBuild()
		{
			if (!this.PlayerDuelMatch.IsEnabled)
			{
				this.Markers.IsEnabled = true;
				this.AreOngoingDuelsActive = true;
			}
			string stringId = MultiplayerClassDivisions.GetMPHeroClassForPeer(this._client.MyRepresentative.MissionPeer, false).StringId;
			if (this._isAgentBuiltForTheFirstTime || (stringId != this._cachedPlayerClassID && !this._hasPlayerChangedArenaPreferrence))
			{
				this.PlayerPrefferedArenaType = (int)MultiplayerDuelVM.GetAgentDefaultPreferredArenaType(Agent.Main);
				this.Markers.OnAgentBuiltForTheFirstTime();
				this._isAgentBuiltForTheFirstTime = false;
				this._cachedPlayerClassID = stringId;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005048 File Offset: 0x00003248
		private string GetArenaTypeName(TroopType duelArenaType)
		{
			switch (duelArenaType)
			{
			case TroopType.Infantry:
				return "infantry";
			case TroopType.Ranged:
				return "archery";
			case TroopType.Cavalry:
				return "cavalry";
			default:
				Debug.FailedAssert("Invalid duel arena type!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MultiplayerDuelVM.cs", "GetArenaTypeName", 363);
				return "";
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000509C File Offset: 0x0000329C
		private TextObject GetArenaTypeLocalizedName(TroopType duelArenaType)
		{
			switch (duelArenaType)
			{
			case TroopType.Infantry:
				return new TextObject("{=1Bm1Wk1v}Infantry", null);
			case TroopType.Ranged:
				return new TextObject("{=OJbpmlXu}Ranged", null);
			case TroopType.Cavalry:
				return new TextObject("{=YVGtcLHF}Cavalry", null);
			default:
				Debug.FailedAssert("Invalid duel arena type!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MultiplayerDuelVM.cs", "GetArenaTypeLocalizedName", 379);
				return TextObject.GetEmpty();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005100 File Offset: 0x00003300
		private MultiplayerDuelVM.DuelArenaProperties GetArenaPropertiesOfFlagEntity(GameEntity flagEntity)
		{
			MultiplayerDuelVM.DuelArenaProperties duelArenaProperties;
			duelArenaProperties.FlagEntity = flagEntity;
			string text = flagEntity.Tags.FirstOrDefault<string>((string t) => t.StartsWith("area_flag"));
			if (!text.IsEmpty<char>())
			{
				duelArenaProperties.Index = int.Parse(text.Substring(text.LastIndexOf('_') + 1)) - 1;
			}
			else
			{
				duelArenaProperties.Index = 0;
				Debug.FailedAssert("Flag has duel_area Tag Missing!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MultiplayerDuelVM.cs", "GetArenaPropertiesOfFlagEntity", 397);
			}
			duelArenaProperties.ArenaTroopType = TroopType.Infantry;
			for (TroopType troopType = TroopType.Infantry; troopType < TroopType.NumberOfTroopTypes; troopType++)
			{
				if (flagEntity.HasTag("flag_" + this.GetArenaTypeName(troopType)))
				{
					duelArenaProperties.ArenaTroopType = troopType;
				}
			}
			return duelArenaProperties;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000051BF File Offset: 0x000033BF
		public static TroopType GetAgentDefaultPreferredArenaType(Agent agent)
		{
			return agent.Character.DefaultFormationClass.GetTroopTypeForRegularFormation();
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000051D1 File Offset: 0x000033D1
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000051D9 File Offset: 0x000033D9
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000051F7 File Offset: 0x000033F7
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000051FF File Offset: 0x000033FF
		[DataSourceProperty]
		public bool AreOngoingDuelsActive
		{
			get
			{
				return this._areOngoingDuelsActive;
			}
			set
			{
				if (value != this._areOngoingDuelsActive)
				{
					this._areOngoingDuelsActive = value;
					base.OnPropertyChangedWithValue(value, "AreOngoingDuelsActive");
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000521D File Offset: 0x0000341D
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00005225 File Offset: 0x00003425
		[DataSourceProperty]
		public bool IsPlayerInDuel
		{
			get
			{
				return this._isPlayerInDuel;
			}
			set
			{
				if (value != this._isPlayerInDuel)
				{
					this._isPlayerInDuel = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerInDuel");
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005243 File Offset: 0x00003443
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000524B File Offset: 0x0000344B
		[DataSourceProperty]
		public int PlayerBounty
		{
			get
			{
				return this._playerBounty;
			}
			set
			{
				if (value != this._playerBounty)
				{
					this._playerBounty = value;
					base.OnPropertyChangedWithValue(value, "PlayerBounty");
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005269 File Offset: 0x00003469
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00005271 File Offset: 0x00003471
		[DataSourceProperty]
		public int PlayerPrefferedArenaType
		{
			get
			{
				return this._playerPreferredArenaType;
			}
			set
			{
				if (value != this._playerPreferredArenaType)
				{
					this._playerPreferredArenaType = value;
					base.OnPropertyChangedWithValue(value, "PlayerPrefferedArenaType");
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000528F File Offset: 0x0000348F
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00005297 File Offset: 0x00003497
		[DataSourceProperty]
		public string PlayerScoreText
		{
			get
			{
				return this._playerScoreText;
			}
			set
			{
				if (value != this._playerScoreText)
				{
					this._playerScoreText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerScoreText");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000052BA File Offset: 0x000034BA
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000052C2 File Offset: 0x000034C2
		[DataSourceProperty]
		public string RemainingRoundTime
		{
			get
			{
				return this._remainingRoundTime;
			}
			set
			{
				if (value != this._remainingRoundTime)
				{
					this._remainingRoundTime = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingRoundTime");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000052E5 File Offset: 0x000034E5
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000052ED File Offset: 0x000034ED
		[DataSourceProperty]
		public MissionDuelMarkersVM Markers
		{
			get
			{
				return this._markers;
			}
			set
			{
				if (value != this._markers)
				{
					this._markers = value;
					base.OnPropertyChangedWithValue<MissionDuelMarkersVM>(value, "Markers");
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000530B File Offset: 0x0000350B
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00005313 File Offset: 0x00003513
		[DataSourceProperty]
		public DuelMatchVM PlayerDuelMatch
		{
			get
			{
				return this._playerDuelMatch;
			}
			set
			{
				if (value != this._playerDuelMatch)
				{
					this._playerDuelMatch = value;
					base.OnPropertyChangedWithValue<DuelMatchVM>(value, "PlayerDuelMatch");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005331 File Offset: 0x00003531
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00005339 File Offset: 0x00003539
		[DataSourceProperty]
		public MBBindingList<DuelMatchVM> OngoingDuels
		{
			get
			{
				return this._ongoingDuels;
			}
			set
			{
				if (value != this._ongoingDuels)
				{
					this._ongoingDuels = value;
					base.OnPropertyChangedWithValue<MBBindingList<DuelMatchVM>>(value, "OngoingDuels");
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005357 File Offset: 0x00003557
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000535F File Offset: 0x0000355F
		[DataSourceProperty]
		public MBBindingList<MPDuelKillNotificationItemVM> KillNotifications
		{
			get
			{
				return this._killNotifications;
			}
			set
			{
				if (value != this._killNotifications)
				{
					this._killNotifications = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPDuelKillNotificationItemVM>>(value, "KillNotifications");
				}
			}
		}

		// Token: 0x0400006E RID: 110
		private const string ArenaFlagTag = "area_flag";

		// Token: 0x0400006F RID: 111
		private const string AremaTypeFlagTagBase = "flag_";

		// Token: 0x04000070 RID: 112
		private readonly MissionMultiplayerGameModeDuelClient _client;

		// Token: 0x04000071 RID: 113
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x04000072 RID: 114
		private bool _isMyRepresentativeAssigned;

		// Token: 0x04000073 RID: 115
		private List<MultiplayerDuelVM.DuelArenaProperties> _duelArenaProperties;

		// Token: 0x04000074 RID: 116
		private TextObject _scoreWithSeparatorText;

		// Token: 0x04000075 RID: 117
		private bool _isAgentBuiltForTheFirstTime = true;

		// Token: 0x04000076 RID: 118
		private bool _hasPlayerChangedArenaPreferrence;

		// Token: 0x04000077 RID: 119
		private string _cachedPlayerClassID;

		// Token: 0x04000078 RID: 120
		private bool _showSpawnPoints;

		// Token: 0x04000079 RID: 121
		private Camera _missionCamera;

		// Token: 0x0400007A RID: 122
		private bool _isEnabled;

		// Token: 0x0400007B RID: 123
		private bool _areOngoingDuelsActive;

		// Token: 0x0400007C RID: 124
		private bool _isPlayerInDuel;

		// Token: 0x0400007D RID: 125
		private int _playerBounty;

		// Token: 0x0400007E RID: 126
		private int _playerPreferredArenaType;

		// Token: 0x0400007F RID: 127
		private string _playerScoreText;

		// Token: 0x04000080 RID: 128
		private string _remainingRoundTime;

		// Token: 0x04000081 RID: 129
		private MissionDuelMarkersVM _markers;

		// Token: 0x04000082 RID: 130
		private DuelMatchVM _playerDuelMatch;

		// Token: 0x04000083 RID: 131
		private MBBindingList<DuelMatchVM> _ongoingDuels;

		// Token: 0x04000084 RID: 132
		private MBBindingList<MPDuelKillNotificationItemVM> _killNotifications;

		// Token: 0x020000B8 RID: 184
		public struct DuelArenaProperties
		{
			// Token: 0x060010CE RID: 4302 RVA: 0x00034211 File Offset: 0x00032411
			public DuelArenaProperties(GameEntity flagEntity, int index, TroopType arenaTroopType)
			{
				this.FlagEntity = flagEntity;
				this.Index = index;
				this.ArenaTroopType = arenaTroopType;
			}

			// Token: 0x040007D5 RID: 2005
			public GameEntity FlagEntity;

			// Token: 0x040007D6 RID: 2006
			public int Index;

			// Token: 0x040007D7 RID: 2007
			public TroopType ArenaTroopType;
		}
	}
}
