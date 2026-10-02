using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B2 RID: 690
	public class MissionMultiplayerSiegeClient : MissionMultiplayerGameModeBaseClient, ICommanderInfo, IMissionBehavior
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x0008FF06 File Offset: 0x0008E106
		public override bool IsGameModeUsingGold
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x0008FF09 File Offset: 0x0008E109
		public override bool IsGameModeTactical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x0008FF0C File Offset: 0x0008E10C
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x0008FF0F File Offset: 0x0008E10F
		public override MultiplayerGameType GameType
		{
			get
			{
				return MultiplayerGameType.Siege;
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x060026F6 RID: 9974 RVA: 0x0008FF14 File Offset: 0x0008E114
		// (remove) Token: 0x060026F7 RID: 9975 RVA: 0x0008FF4C File Offset: 0x0008E14C
		public event Action<BattleSideEnum, float> OnMoraleChangedEvent;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x060026F8 RID: 9976 RVA: 0x0008FF84 File Offset: 0x0008E184
		// (remove) Token: 0x060026F9 RID: 9977 RVA: 0x0008FFBC File Offset: 0x0008E1BC
		public event Action OnFlagNumberChangedEvent;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x060026FA RID: 9978 RVA: 0x0008FFF4 File Offset: 0x0008E1F4
		// (remove) Token: 0x060026FB RID: 9979 RVA: 0x0009002C File Offset: 0x0008E22C
		public event Action<FlagCapturePoint, Team> OnCapturePointOwnerChangedEvent;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x060026FC RID: 9980 RVA: 0x00090064 File Offset: 0x0008E264
		// (remove) Token: 0x060026FD RID: 9981 RVA: 0x0009009C File Offset: 0x0008E29C
		public event Action<GoldGain> OnGoldGainEvent;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x060026FE RID: 9982 RVA: 0x000900D4 File Offset: 0x0008E2D4
		// (remove) Token: 0x060026FF RID: 9983 RVA: 0x0009010C File Offset: 0x0008E30C
		public event Action<int[]> OnCapturePointRemainingMoraleGainsChangedEvent;

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002700 RID: 9984 RVA: 0x00090141 File Offset: 0x0008E341
		public bool AreMoralesIndependent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x00090144 File Offset: 0x0008E344
		// (set) Token: 0x06002702 RID: 9986 RVA: 0x0009014C File Offset: 0x0008E34C
		public IEnumerable<FlagCapturePoint> AllCapturePoints { get; private set; }

		// Token: 0x06002703 RID: 9987 RVA: 0x00090158 File Offset: 0x0008E358
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<SiegeMoraleChangeMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleMoraleChangedMessage));
				registerer.RegisterBaseHandler<SyncGoldsForSkirmish>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdateGold));
				registerer.RegisterBaseHandler<FlagDominationFlagsRemovedMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleFlagsRemovedMessage));
				registerer.RegisterBaseHandler<FlagDominationCapturePointMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPointCapturedMessage));
				registerer.RegisterBaseHandler<GoldGain>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventTDMGoldGain));
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000901C6 File Offset: 0x0008E3C6
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			this._capturePointOwners = new Team[7];
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x00090208 File Offset: 0x0008E408
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (flagCapturePoint.GameEntity.HasTag("keep_capture_point"))
				{
					this._masterFlag = flagCapturePoint;
				}
				else if (flagCapturePoint.FlagIndex == 0)
				{
					MatrixFrame globalFrame = flagCapturePoint.GameEntity.GetGlobalFrame();
					this._retreatHornPosition = globalFrame.origin + globalFrame.rotation.u * 3f;
				}
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000902B8 File Offset: 0x0008E4B8
		private void OnMyClientSynchronized()
		{
			this._myRepresentative = GameNetwork.MyPeer.GetComponent<SiegeMissionRepresentative>();
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000902CA File Offset: 0x0008E4CA
		public override int GetGoldAmount()
		{
			return this._myRepresentative.Gold;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000902D7 File Offset: 0x0008E4D7
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
			if (representative != null && base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				representative.UpdateGold(goldAmount);
				base.ScoreboardComponent.PlayerPropertiesChanged(representative.MissionPeer);
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x00090304 File Offset: 0x0008E504
		public void OnNumberOfFlagsChanged()
		{
			Action onFlagNumberChangedEvent = this.OnFlagNumberChangedEvent;
			if (onFlagNumberChangedEvent != null)
			{
				onFlagNumberChangedEvent();
			}
			SiegeMissionRepresentative myRepresentative = this._myRepresentative;
			bool flag;
			if (myRepresentative == null)
			{
				flag = false;
			}
			else
			{
				Team team = myRepresentative.MissionPeer.Team;
				BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
				BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
				flag = (battleSideEnum.GetValueOrDefault() == battleSideEnum2) & (battleSideEnum != null);
			}
			if (flag)
			{
				Action<GoldGain> onGoldGainEvent = this.OnGoldGainEvent;
				if (onGoldGainEvent == null)
				{
					return;
				}
				onGoldGainEvent(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(512, 35)
				}));
			}
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x00090398 File Offset: 0x0008E598
		public void OnCapturePointOwnerChanged(FlagCapturePoint flagCapturePoint, Team ownerTeam)
		{
			this._capturePointOwners[flagCapturePoint.FlagIndex] = ownerTeam;
			Action<FlagCapturePoint, Team> onCapturePointOwnerChangedEvent = this.OnCapturePointOwnerChangedEvent;
			if (onCapturePointOwnerChangedEvent != null)
			{
				onCapturePointOwnerChangedEvent(flagCapturePoint, ownerTeam);
			}
			if (ownerTeam != null && ownerTeam.Side == BattleSideEnum.Defender && this._remainingTimeForBellSoundToStop > 8f && flagCapturePoint == this._masterFlag)
			{
				this._bellSoundEvent.Stop();
				this._bellSoundEvent = null;
				this._remainingTimeForBellSoundToStop = float.MinValue;
				this._lastBellSoundPercentage += 0.2f;
			}
			if (this._myRepresentative != null && this._myRepresentative.MissionPeer.Team != null)
			{
				MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
				Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
				if (this._myRepresentative.MissionPeer.Team == ownerTeam)
				{
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/flag_captured"), vec);
					return;
				}
				MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/flag_lost"), vec);
			}
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00090488 File Offset: 0x0008E688
		public void OnMoraleChanged(int attackerMorale, int defenderMorale, int[] capturePointRemainingMoraleGains)
		{
			float num = (float)attackerMorale / 360f;
			float num2 = (float)defenderMorale / 360f;
			SiegeMissionRepresentative myRepresentative = this._myRepresentative;
			if (((myRepresentative != null) ? myRepresentative.MissionPeer.Team : null) != null && this._myRepresentative.MissionPeer.Team.Side != BattleSideEnum.None)
			{
				if ((this._capturePointOwners[this._masterFlag.FlagIndex] == null || this._capturePointOwners[this._masterFlag.FlagIndex].Side != BattleSideEnum.Defender) && this._remainingTimeForBellSoundToStop < 0f)
				{
					if (num2 > this._lastBellSoundPercentage)
					{
						this._lastBellSoundPercentage += 0.2f;
					}
					if (num2 <= 0.4f)
					{
						if (this._lastBellSoundPercentage > 0.4f)
						{
							this._remainingTimeForBellSoundToStop = float.MaxValue;
							this._lastBellSoundPercentage = 0.4f;
						}
					}
					else if (num2 <= 0.6f)
					{
						if (this._lastBellSoundPercentage > 0.6f)
						{
							this._remainingTimeForBellSoundToStop = 8f;
							this._lastBellSoundPercentage = 0.6f;
						}
					}
					else if (num2 <= 0.8f && this._lastBellSoundPercentage > 0.8f)
					{
						this._remainingTimeForBellSoundToStop = 4f;
						this._lastBellSoundPercentage = 0.8f;
					}
					if (this._remainingTimeForBellSoundToStop > 0f)
					{
						BattleSideEnum side = this._myRepresentative.MissionPeer.Team.Side;
						if (side != BattleSideEnum.Defender)
						{
							if (side == BattleSideEnum.Attacker)
							{
								this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_attacker", base.Mission.Scene);
							}
						}
						else
						{
							this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_defender", base.Mission.Scene);
						}
						MatrixFrame globalFrame = this._masterFlag.GameEntity.GetGlobalFrame();
						this._bellSoundEvent.PlayInPosition(globalFrame.origin + globalFrame.rotation.u * 3f);
					}
				}
				if (!this._battleEndingNotificationGiven || !this._battleEndingLateNotificationGiven)
				{
					float num3 = ((!this._battleEndingNotificationGiven) ? 0.25f : 0.15f);
					MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
					Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
					if (num <= num3 && num2 > num3)
					{
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString((this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Attacker) ? "event:/alerts/report/battle_losing" : "event:/alerts/report/battle_winning"), vec);
						if (this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Attacker)
						{
							MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/multiplayer/retreat_horn_attacker"), this._retreatHornPosition);
						}
						else if (this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Defender)
						{
							MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/multiplayer/retreat_horn_defender"), this._retreatHornPosition);
						}
						if (this._battleEndingNotificationGiven)
						{
							this._battleEndingLateNotificationGiven = true;
						}
						this._battleEndingNotificationGiven = true;
					}
					if (num2 <= num3 && num > num3)
					{
						MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString((this._myRepresentative.MissionPeer.Team.Side == BattleSideEnum.Defender) ? "event:/alerts/report/battle_losing" : "event:/alerts/report/battle_winning"), vec);
						if (this._battleEndingNotificationGiven)
						{
							this._battleEndingLateNotificationGiven = true;
						}
						this._battleEndingNotificationGiven = true;
					}
				}
			}
			Action<BattleSideEnum, float> onMoraleChangedEvent = this.OnMoraleChangedEvent;
			if (onMoraleChangedEvent != null)
			{
				onMoraleChangedEvent(BattleSideEnum.Attacker, num);
			}
			Action<BattleSideEnum, float> onMoraleChangedEvent2 = this.OnMoraleChangedEvent;
			if (onMoraleChangedEvent2 != null)
			{
				onMoraleChangedEvent2(BattleSideEnum.Defender, num2);
			}
			Action<int[]> onCapturePointRemainingMoraleGainsChangedEvent = this.OnCapturePointRemainingMoraleGainsChangedEvent;
			if (onCapturePointRemainingMoraleGainsChangedEvent == null)
			{
				return;
			}
			onCapturePointRemainingMoraleGainsChangedEvent(capturePointRemainingMoraleGains);
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000907F0 File Offset: 0x0008E9F0
		public Team GetFlagOwner(FlagCapturePoint flag)
		{
			return this._capturePointOwners[flag.FlagIndex];
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000907FF File Offset: 0x0008E9FF
		public override void OnRemoveBehavior()
		{
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			base.OnRemoveBehavior();
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x00090820 File Offset: 0x0008EA20
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._remainingTimeForBellSoundToStop > 0f)
			{
				this._remainingTimeForBellSoundToStop -= dt;
				if (this._remainingTimeForBellSoundToStop <= 0f || base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Playing)
				{
					this._remainingTimeForBellSoundToStop = float.MinValue;
					this._bellSoundEvent.Stop();
					this._bellSoundEvent = null;
				}
			}
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x00090888 File Offset: 0x0008EA88
		public List<ItemObject> GetSiegeMissiles()
		{
			List<ItemObject> list = new List<ItemObject>();
			foreach (WeakGameEntity weakGameEntity in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<RangedSiegeWeapon>())
			{
				RangedSiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<RangedSiegeWeapon>();
				if (!string.IsNullOrEmpty(firstScriptOfType.MissileItemID))
				{
					ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MissileItemID);
					if (!list.Contains(@object))
					{
						list.Add(@object);
					}
				}
				foreach (ItemObject itemObject in new List<ItemObject>
				{
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleFireProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.MultipleFireProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleProjectileFlyingId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleFireProjectileId),
					MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType.SingleFireProjectileFlyingId)
				})
				{
					if (!list.Contains(itemObject))
					{
						list.Add(itemObject);
					}
				}
			}
			foreach (WeakGameEntity weakGameEntity2 in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<StonePile>())
			{
				StonePile firstScriptOfType2 = weakGameEntity2.GetFirstScriptOfType<StonePile>();
				if (!string.IsNullOrEmpty(firstScriptOfType2.GivenItemID))
				{
					ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>(firstScriptOfType2.GivenItemID);
					if (!list.Contains(object2))
					{
						list.Add(object2);
					}
				}
			}
			return list;
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x00090AB0 File Offset: 0x0008ECB0
		private void HandleMoraleChangedMessage(GameNetworkMessage baseMessage)
		{
			SiegeMoraleChangeMessage siegeMoraleChangeMessage = (SiegeMoraleChangeMessage)baseMessage;
			this.OnMoraleChanged(siegeMoraleChangeMessage.AttackerMorale, siegeMoraleChangeMessage.DefenderMorale, siegeMoraleChangeMessage.CapturePointRemainingMoraleGains);
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x00090ADC File Offset: 0x0008ECDC
		private void HandleServerEventUpdateGold(GameNetworkMessage baseMessage)
		{
			SyncGoldsForSkirmish syncGoldsForSkirmish = (SyncGoldsForSkirmish)baseMessage;
			SiegeMissionRepresentative component = syncGoldsForSkirmish.VirtualPlayer.GetComponent<SiegeMissionRepresentative>();
			this.OnGoldAmountChangedForRepresentative(component, syncGoldsForSkirmish.GoldAmount);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00090B09 File Offset: 0x0008ED09
		private void HandleFlagsRemovedMessage(GameNetworkMessage baseMessage)
		{
			this.OnNumberOfFlagsChanged();
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x00090B14 File Offset: 0x0008ED14
		private void HandleServerEventPointCapturedMessage(GameNetworkMessage baseMessage)
		{
			FlagDominationCapturePointMessage flagDominationCapturePointMessage = (FlagDominationCapturePointMessage)baseMessage;
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (flagCapturePoint.FlagIndex == flagDominationCapturePointMessage.FlagIndex)
				{
					this.OnCapturePointOwnerChanged(flagCapturePoint, Mission.MissionNetworkHelper.GetTeamFromTeamIndex(flagDominationCapturePointMessage.OwnerTeamIndex));
					break;
				}
			}
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x00090B84 File Offset: 0x0008ED84
		private void HandleServerEventTDMGoldGain(GameNetworkMessage baseMessage)
		{
			GoldGain goldGain = (GoldGain)baseMessage;
			Action<GoldGain> onGoldGainEvent = this.OnGoldGainEvent;
			if (onGoldGainEvent == null)
			{
				return;
			}
			onGoldGainEvent(goldGain);
		}

		// Token: 0x04000EBF RID: 3775
		private const float DefenderMoraleDropThresholdIncrement = 0.2f;

		// Token: 0x04000EC0 RID: 3776
		private const float DefenderMoraleDropThresholdLow = 0.4f;

		// Token: 0x04000EC1 RID: 3777
		private const float DefenderMoraleDropThresholdMedium = 0.6f;

		// Token: 0x04000EC2 RID: 3778
		private const float DefenderMoraleDropThresholdHigh = 0.8f;

		// Token: 0x04000EC3 RID: 3779
		private const float DefenderMoraleDropMediumDuration = 8f;

		// Token: 0x04000EC4 RID: 3780
		private const float DefenderMoraleDropHighDuration = 4f;

		// Token: 0x04000EC5 RID: 3781
		private const float BattleWinLoseAlertThreshold = 0.25f;

		// Token: 0x04000EC6 RID: 3782
		private const float BattleWinLoseLateAlertThreshold = 0.15f;

		// Token: 0x04000EC7 RID: 3783
		private const string BattleWinningSoundEventString = "event:/alerts/report/battle_winning";

		// Token: 0x04000EC8 RID: 3784
		private const string BattleLosingSoundEventString = "event:/alerts/report/battle_losing";

		// Token: 0x04000EC9 RID: 3785
		private const float IndefiniteDurationThreshold = 8f;

		// Token: 0x04000ECA RID: 3786
		private Team[] _capturePointOwners;

		// Token: 0x04000ECC RID: 3788
		private FlagCapturePoint _masterFlag;

		// Token: 0x04000ECD RID: 3789
		private SiegeMissionRepresentative _myRepresentative;

		// Token: 0x04000ECE RID: 3790
		private SoundEvent _bellSoundEvent;

		// Token: 0x04000ECF RID: 3791
		private float _remainingTimeForBellSoundToStop = float.MinValue;

		// Token: 0x04000ED0 RID: 3792
		private float _lastBellSoundPercentage = 1f;

		// Token: 0x04000ED1 RID: 3793
		private bool _battleEndingNotificationGiven;

		// Token: 0x04000ED2 RID: 3794
		private bool _battleEndingLateNotificationGiven;

		// Token: 0x04000ED3 RID: 3795
		private Vec3 _retreatHornPosition;
	}
}
