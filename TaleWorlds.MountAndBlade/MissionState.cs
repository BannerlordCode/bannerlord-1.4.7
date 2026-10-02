using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000241 RID: 577
	public class MissionState : GameState
	{
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x00074C84 File Offset: 0x00072E84
		// (set) Token: 0x06002146 RID: 8518 RVA: 0x00074C8C File Offset: 0x00072E8C
		public IMissionSystemHandler Handler { get; set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002147 RID: 8519 RVA: 0x00074C95 File Offset: 0x00072E95
		// (set) Token: 0x06002148 RID: 8520 RVA: 0x00074C9C File Offset: 0x00072E9C
		public static MissionState Current { get; private set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x00074CA4 File Offset: 0x00072EA4
		// (set) Token: 0x0600214A RID: 8522 RVA: 0x00074CAC File Offset: 0x00072EAC
		public Mission CurrentMission { get; private set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x00074CB5 File Offset: 0x00072EB5
		// (set) Token: 0x0600214C RID: 8524 RVA: 0x00074CBD File Offset: 0x00072EBD
		public string MissionName { get; private set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x00074CC6 File Offset: 0x00072EC6
		// (set) Token: 0x0600214E RID: 8526 RVA: 0x00074CCE File Offset: 0x00072ECE
		public bool FirstMissionTickAfterLoading { get; private set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x00074CD7 File Offset: 0x00072ED7
		// (set) Token: 0x06002150 RID: 8528 RVA: 0x00074CDF File Offset: 0x00072EDF
		public bool Paused { get; set; }

		// Token: 0x06002151 RID: 8529 RVA: 0x00074CE8 File Offset: 0x00072EE8
		protected override void OnInitialize()
		{
			base.OnInitialize();
			MissionState.Current = this;
			this.FirstMissionTickAfterLoading = true;
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00074D02 File Offset: 0x00072F02
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this.CurrentMission.OnMissionStateFinalize(this.CurrentMission.NeedsMemoryCleanup);
			this.CurrentMission = null;
			MissionState.Current = null;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00074D2D File Offset: 0x00072F2D
		protected override void OnActivate()
		{
			base.OnActivate();
			this.CurrentMission.OnMissionStateActivate();
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00074D40 File Offset: 0x00072F40
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.CurrentMission.OnMissionStateDeactivate();
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00074D53 File Offset: 0x00072F53
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			if (this.CurrentMission != null && this.CurrentMission.CurrentState == Mission.State.Continuing)
			{
				this.CurrentMission.IdleTick(dt);
			}
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00074D80 File Offset: 0x00072F80
		protected override void OnTick(float realDt)
		{
			base.OnTick(realDt);
			if (this._isDelayedDisconnecting && this.CurrentMission != null && this.CurrentMission.CurrentState == Mission.State.Continuing)
			{
				BannerlordNetwork.EndMultiplayerLobbyMission();
			}
			if (this.CurrentMission == null)
			{
				return;
			}
			if (this.CurrentMission.CurrentState == Mission.State.NewlyCreated || this.CurrentMission.CurrentState == Mission.State.Initializing)
			{
				if (this.CurrentMission.CurrentState == Mission.State.NewlyCreated)
				{
					this.CurrentMission.ClearUnreferencedResources(this.CurrentMission.NeedsMemoryCleanup);
				}
				this.TickLoading(realDt);
			}
			else if (this.CurrentMission.CurrentState == Mission.State.Continuing || this.CurrentMission.MissionEnded)
			{
				if (this.MissionReplayStartTime != 0f)
				{
					this.CurrentMission.SkipForwardMissionReplay(this.MissionReplayStartTime, 0.033f);
					this.MissionReplayStartTime = 0f;
				}
				bool flag = false;
				if (this.MissionEndTime != 0f && this.CurrentMission.CurrentTime > this.MissionEndTime)
				{
					this.CurrentMission.EndMission();
					flag = true;
				}
				if (!flag && (this.Handler == null || this.Handler.RenderIsReady()))
				{
					this.TickMission(realDt);
				}
				if (flag && MBEditor._isEditorMissionOn)
				{
					MBEditor.LeaveEditMissionMode();
					this.TickMission(realDt);
				}
			}
			if (this.CurrentMission.CurrentState == Mission.State.Over)
			{
				if (MBGameManager.Current.IsEnding)
				{
					Game.Current.GameStateManager.CleanStates(0);
					return;
				}
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00074EF8 File Offset: 0x000730F8
		private void TickMission(float realDt)
		{
			if (this.FirstMissionTickAfterLoading && this.CurrentMission != null && this.CurrentMission.CurrentState == Mission.State.Continuing && GameNetwork.IsClient)
			{
				int currentBattleIndex = GameNetwork.GetNetworkComponent<BaseNetworkComponentData>().CurrentBattleIndex;
				MBDebug.Print(string.Format("Client: I finished loading battle with index: {0}. Sending confirmation to server.", currentBattleIndex), 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new FinishedLoading(currentBattleIndex));
				GameNetwork.EndModuleEventAsClient();
				GameNetwork.SyncRelevantGameOptionsToServer();
			}
			IMissionSystemHandler handler = this.Handler;
			if (handler != null)
			{
				handler.BeforeMissionTick(this.CurrentMission, realDt);
			}
			this.CurrentMission.PauseAITick = false;
			if (GameNetwork.IsSessionActive && this.CurrentMission.ClearSceneTimerElapsedTime < 0f)
			{
				this.CurrentMission.PauseAITick = true;
			}
			float num = realDt;
			if (this.Paused || MBCommon.IsPaused)
			{
				num = 0f;
			}
			else if (this.CurrentMission.FixedDeltaTimeMode)
			{
				num = this.CurrentMission.FixedDeltaTime;
			}
			if (!GameNetwork.IsSessionActive)
			{
				this.CurrentMission.UpdateSceneTimeSpeed();
				float timeSpeed = this.CurrentMission.Scene.TimeSpeed;
				num *= timeSpeed;
			}
			if (this.CurrentMission.ClearSceneTimerElapsedTime < -0.3f && !GameNetwork.IsClientOrReplay)
			{
				this.CurrentMission.ClearAgentActions();
			}
			if (this.CurrentMission.CurrentState == Mission.State.Continuing || this.CurrentMission.MissionEnded)
			{
				if (this.CurrentMission.IsFastForward)
				{
					float num2 = num * 9f;
					while (num2 > 1E-06f)
					{
						if (num2 > 0.1f)
						{
							this.TickMissionAux(0.1f, 0.1f, false, false);
							if (this.CurrentMission.CurrentState == Mission.State.Over)
							{
								break;
							}
							num2 -= 0.1f;
						}
						else
						{
							if (num2 > 0.0033333334f)
							{
								this.TickMissionAux(num2, num2, false, false);
							}
							num2 = 0f;
						}
					}
					if (this.CurrentMission.CurrentState != Mission.State.Over)
					{
						this.TickMissionAux(num, realDt, true, false);
					}
				}
				else
				{
					this.TickMissionAux(num, realDt, true, true);
				}
			}
			if (this.Handler != null)
			{
				this.Handler.AfterMissionTick(this.CurrentMission, realDt);
			}
			this.FirstMissionTickAfterLoading = false;
			this._missionTickCount++;
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00075113 File Offset: 0x00073313
		private void TickMissionAux(float dt, float realDt, bool updateCamera, bool asyncAITick)
		{
			this.CurrentMission.Tick(dt);
			if (this._missionTickCount > 2)
			{
				this.CurrentMission.OnTick(dt, realDt, updateCamera, asyncAITick);
			}
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x0007513C File Offset: 0x0007333C
		private void TickLoading(float realDt)
		{
			this._tickCountBeforeLoad++;
			if (!this._missionInitializing && this._tickCountBeforeLoad > 0)
			{
				this.LoadMission();
				Utilities.SetLoadingScreenPercentage(0.01f);
				return;
			}
			if (this._missionInitializing && this.CurrentMission.IsLoadingFinished)
			{
				this.FinishMissionLoading();
			}
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00075194 File Offset: 0x00073394
		private void LoadMission()
		{
			foreach (MissionBehavior missionBehavior in this.CurrentMission.MissionBehaviors)
			{
				missionBehavior.OnMissionScreenPreLoad();
			}
			Utilities.ClearOldResourcesAndObjects();
			this._missionInitializing = true;
			this.CurrentMission.Initialize();
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00075200 File Offset: 0x00073400
		private void CreateMission(MissionInitializerRecord rec, bool needsMemoryCleanup)
		{
			this.CurrentMission = new Mission(rec, this, needsMemoryCleanup);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00075210 File Offset: 0x00073410
		protected Mission HandleOpenNew(string missionName, MissionInitializerRecord rec, InitializeMissionBehaviorsDelegate handler, bool addDefaultMissionBehaviors, bool needsMemoryCleanup)
		{
			this.MissionName = missionName;
			this.CreateMission(rec, needsMemoryCleanup);
			IEnumerable<MissionBehavior> enumerable = handler(this.CurrentMission);
			enumerable = enumerable.Where<MissionBehavior>((MissionBehavior behavior) => behavior != null);
			if (addDefaultMissionBehaviors)
			{
				enumerable = MissionState.AddDefaultMissionBehaviorsTo(this.CurrentMission, enumerable);
			}
			foreach (MissionBehavior missionBehavior in enumerable)
			{
				missionBehavior.OnAfterMissionCreated();
			}
			this.AddBehaviorsToMission(enumerable);
			if (this.Handler != null)
			{
				enumerable = new MissionBehavior[0];
				enumerable = this.Handler.OnAddBehaviors(enumerable, this.CurrentMission, missionName, addDefaultMissionBehaviors);
				this.AddBehaviorsToMission(enumerable);
			}
			if (GameNetwork.IsDedicatedServer)
			{
				GameNetwork.SetServerFrameRate((double)Module.CurrentModule.StartupInfo.ServerTickRate);
			}
			return this.CurrentMission;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00075300 File Offset: 0x00073500
		private void AddBehaviorsToMission(IEnumerable<MissionBehavior> behaviors)
		{
			MissionLogic[] array = (from behavior in behaviors.OfType<MissionLogic>()
				where !(behavior is MissionNetwork)
				select behavior).ToArray<MissionLogic>();
			MissionBehavior[] array2 = behaviors.Where<MissionBehavior>((MissionBehavior behavior) => behavior != null && !(behavior is MissionNetwork) && !(behavior is MissionLogic)).ToArray<MissionBehavior>();
			MissionNetwork[] array3 = behaviors.OfType<MissionNetwork>().ToArray<MissionNetwork>();
			this.CurrentMission.InitializeStartingBehaviors(array, array2, array3);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x00075382 File Offset: 0x00073582
		protected static bool IsRecordingActive()
		{
			if (GameNetwork.IsServer)
			{
				return MultiplayerOptions.OptionType.EnableMissionRecording.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			return MissionState.RecordMission && Game.Current.GameType.IsCoreOnlyGameMode;
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000753AC File Offset: 0x000735AC
		public static Mission OpenNew(string missionName, MissionInitializerRecord rec, InitializeMissionBehaviorsDelegate handler, bool addDefaultMissionBehaviors = true, bool needsMemoryCleanup = true)
		{
			Debug.Print(string.Concat(new string[] { "Opening new mission ", missionName, " ", rec.SceneLevels, ".\n" }), 0, Debug.DebugColor.White, 17592186044416UL);
			if (!GameNetwork.IsClientOrReplay && !GameNetwork.IsServer)
			{
				MBCommon.CurrentGameType = (MissionState.IsRecordingActive() ? MBCommon.GameType.SingleRecord : MBCommon.GameType.Single);
			}
			Game.Current.OnMissionIsStarting(missionName, rec);
			MissionState missionState = Game.Current.GameStateManager.CreateState<MissionState>();
			Mission mission = missionState.HandleOpenNew(missionName, rec, handler, addDefaultMissionBehaviors, needsMemoryCleanup);
			Game.Current.GameStateManager.PushState(missionState, 0);
			return mission;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00075454 File Offset: 0x00073654
		private static IEnumerable<MissionBehavior> AddDefaultMissionBehaviorsTo(Mission mission, IEnumerable<MissionBehavior> behaviors)
		{
			List<MissionBehavior> list = new List<MissionBehavior>();
			if (GameNetwork.IsSessionActive || GameNetwork.IsReplay)
			{
				list.Add(new MissionNetworkComponent());
			}
			if (MissionState.IsRecordingActive() && !GameNetwork.IsReplay)
			{
				list.Add(new RecordMissionLogic());
			}
			list.Add(new BasicMissionHandler());
			list.Add(new CasualtyHandler());
			list.Add(new AgentCommonAILogic());
			return list.Concat<MissionBehavior>(behaviors);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000754C4 File Offset: 0x000736C4
		private void FinishMissionLoading()
		{
			this._missionInitializing = false;
			this.CurrentMission.Scene.SetOwnerThread();
			Utilities.SetLoadingScreenPercentage(0.4f);
			for (int i = 0; i < 2; i++)
			{
				this.CurrentMission.Tick(0.001f);
			}
			Utilities.SetLoadingScreenPercentage(0.42f);
			IMissionSystemHandler handler = this.Handler;
			if (handler != null)
			{
				handler.OnMissionAfterStarting(this.CurrentMission);
			}
			Utilities.SetLoadingScreenPercentage(0.48f);
			this.CurrentMission.AfterStart();
			Utilities.SetLoadingScreenPercentage(0.56f);
			IMissionSystemHandler handler2 = this.Handler;
			if (handler2 != null)
			{
				handler2.OnMissionLoadingFinished(this.CurrentMission);
			}
			Utilities.SetLoadingScreenPercentage(0.62f);
			this.CurrentMission.Scene.ResumeLoadingRenderings();
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x0007557F File Offset: 0x0007377F
		public void BeginDelayedDisconnectFromMission()
		{
			this._isDelayedDisconnecting = true;
		}

		// Token: 0x04000CC2 RID: 3266
		private const int MissionFastForwardSpeedMultiplier = 10;

		// Token: 0x04000CC3 RID: 3267
		private bool _missionInitializing;

		// Token: 0x04000CC4 RID: 3268
		private int _tickCountBeforeLoad;

		// Token: 0x04000CC5 RID: 3269
		public static bool RecordMission;

		// Token: 0x04000CC7 RID: 3271
		public float MissionReplayStartTime;

		// Token: 0x04000CC8 RID: 3272
		public float MissionEndTime;

		// Token: 0x04000CCE RID: 3278
		private bool _isDelayedDisconnecting;

		// Token: 0x04000CCF RID: 3279
		private int _missionTickCount;
	}
}
