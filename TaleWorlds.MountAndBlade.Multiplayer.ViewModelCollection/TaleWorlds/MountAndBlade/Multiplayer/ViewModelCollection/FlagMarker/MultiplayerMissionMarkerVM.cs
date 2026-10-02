using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker
{
	// Token: 0x02000096 RID: 150
	public class MultiplayerMissionMarkerVM : ViewModel
	{
		// Token: 0x06000ED3 RID: 3795 RVA: 0x0002D99C File Offset: 0x0002BB9C
		public MultiplayerMissionMarkerVM(Camera missionCamera)
		{
			this._missionCamera = missionCamera;
			this.FlagTargets = new MBBindingList<MissionFlagMarkerTargetVM>();
			this.PeerTargets = new MBBindingList<MissionPeerMarkerTargetVM>();
			this.SiegeEngineTargets = new MBBindingList<MissionSiegeEngineMarkerTargetVM>();
			this.AlwaysVisibleTargets = new MBBindingList<MissionAlwaysVisibleMarkerTargetVM>();
			this._teammateDictionary = new Dictionary<MissionPeer, MissionPeerMarkerTargetVM>();
			this._distanceComparer = new MultiplayerMissionMarkerVM.MarkerDistanceComparer();
			this._commanderInfo = Mission.Current.GetMissionBehavior<ICommanderInfo>();
			if (this._commanderInfo != null)
			{
				this._commanderInfo.OnFlagNumberChangedEvent += this.OnFlagNumberChangedEvent;
				this._commanderInfo.OnCapturePointOwnerChangedEvent += this.OnCapturePointOwnerChangedEvent;
				this.OnFlagNumberChangedEvent();
				this._siegeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
				if (this._siegeClient != null)
				{
					this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent += this.OnCapturePointRemainingMoraleGainsChanged;
				}
			}
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
			this._friendIDs = new List<PlayerId>();
			foreach (IFriendListService friendListService in PlatformServices.Instance.GetFriendListServices())
			{
				this._friendIDs.AddRange(friendListService.GetAllFriends());
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._commanderInfo != null)
			{
				this._commanderInfo.OnFlagNumberChangedEvent -= this.OnFlagNumberChangedEvent;
				this._commanderInfo.OnCapturePointOwnerChangedEvent -= this.OnCapturePointOwnerChangedEvent;
				if (this._siegeClient != null)
				{
					this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent -= this.OnCapturePointRemainingMoraleGainsChanged;
				}
			}
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0002DB3C File Offset: 0x0002BD3C
		public void Tick(float dt)
		{
			this.OnRefreshPeerMarkers();
			this.UpdateAlwaysVisibleTargetScreenPosition();
			if (this.IsEnabled)
			{
				this.UpdateTargetScreenPositions();
				this._fadeOutTimerStarted = false;
				this._fadeOutTimer = 0f;
				this._prevEnabledState = this.IsEnabled;
			}
			else
			{
				if (this._prevEnabledState)
				{
					this._fadeOutTimerStarted = true;
				}
				if (this._fadeOutTimerStarted)
				{
					this._fadeOutTimer += dt;
				}
				if (this._fadeOutTimer < 2f)
				{
					this.UpdateTargetScreenPositions();
				}
				else
				{
					this._fadeOutTimerStarted = false;
				}
			}
			this._prevEnabledState = this.IsEnabled;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0002DBD0 File Offset: 0x0002BDD0
		private void OnCapturePointRemainingMoraleGainsChanged(int[] remainingMoraleGainsArr)
		{
			foreach (MissionFlagMarkerTargetVM missionFlagMarkerTargetVM in this.FlagTargets)
			{
				int flagIndex = missionFlagMarkerTargetVM.TargetFlag.FlagIndex;
				if (flagIndex >= 0 && flagIndex < remainingMoraleGainsArr.Length)
				{
					missionFlagMarkerTargetVM.OnRemainingMoraleChanged(remainingMoraleGainsArr[flagIndex]);
				}
			}
			Debug.Print("OnCapturePointRemainingMoraleGainsChanged: " + remainingMoraleGainsArr.Length, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0002DC58 File Offset: 0x0002BE58
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (this._commanderInfo != null)
			{
				this.OnFlagNumberChangedEvent();
			}
			if (peer.IsMine)
			{
				this.SiegeEngineTargets.Clear();
				foreach (WeakGameEntity weakGameEntity in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<SiegeWeapon>())
				{
					SiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SiegeWeapon>();
					if (newTeam.Side == firstScriptOfType.Side)
					{
						this.SiegeEngineTargets.Add(new MissionSiegeEngineMarkerTargetVM(firstScriptOfType));
					}
				}
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0002DCEC File Offset: 0x0002BEEC
		private void UpdateTargetScreenPositions()
		{
			this.PeerTargets.ApplyActionOnAllItems(delegate(MissionPeerMarkerTargetVM pt)
			{
				pt.UpdateScreenPosition(this._missionCamera);
			});
			this.FlagTargets.ApplyActionOnAllItems(delegate(MissionFlagMarkerTargetVM ft)
			{
				ft.UpdateScreenPosition(this._missionCamera);
			});
			this.SiegeEngineTargets.ApplyActionOnAllItems(delegate(MissionSiegeEngineMarkerTargetVM st)
			{
				st.UpdateScreenPosition(this._missionCamera);
			});
			this.PeerTargets.Sort(this._distanceComparer);
			this.FlagTargets.Sort(this._distanceComparer);
			this.SiegeEngineTargets.Sort(this._distanceComparer);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0002DD74 File Offset: 0x0002BF74
		private void UpdateAlwaysVisibleTargetScreenPosition()
		{
			foreach (MissionAlwaysVisibleMarkerTargetVM missionAlwaysVisibleMarkerTargetVM in this.AlwaysVisibleTargets)
			{
				missionAlwaysVisibleMarkerTargetVM.UpdateScreenPosition(this._missionCamera);
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0002DDC4 File Offset: 0x0002BFC4
		private void OnFlagNumberChangedEvent()
		{
			this.ResetCapturePointLists();
			this.InitCapturePoints();
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
		private void InitCapturePoints()
		{
			if (this._commanderInfo != null)
			{
				foreach (FlagCapturePoint flagCapturePoint in this._commanderInfo.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint c) => !c.IsDeactivated).ToArray<FlagCapturePoint>())
				{
					MissionFlagMarkerTargetVM missionFlagMarkerTargetVM = new MissionFlagMarkerTargetVM(flagCapturePoint);
					this.FlagTargets.Add(missionFlagMarkerTargetVM);
					missionFlagMarkerTargetVM.OnOwnerChanged(this._commanderInfo.GetFlagOwner(flagCapturePoint));
					missionFlagMarkerTargetVM.IsEnabled = this.IsEnabled;
				}
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0002DE61 File Offset: 0x0002C061
		private void ResetCapturePointLists()
		{
			this.FlagTargets.Clear();
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002DE70 File Offset: 0x0002C070
		private void OnCapturePointOwnerChangedEvent(FlagCapturePoint flag, Team team)
		{
			foreach (MissionFlagMarkerTargetVM missionFlagMarkerTargetVM in this.FlagTargets)
			{
				if (missionFlagMarkerTargetVM.TargetFlag == flag)
				{
					missionFlagMarkerTargetVM.OnOwnerChanged(team);
				}
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0002DEC8 File Offset: 0x0002C0C8
		private void OnRefreshPeerMarkers()
		{
			if (GameNetwork.MyPeer == null)
			{
				return;
			}
			Agent controlledAgent = GameNetwork.MyPeer.ControlledAgent;
			BattleSideEnum battleSideEnum = ((controlledAgent != null) ? controlledAgent.Team.Side : BattleSideEnum.None);
			List<MissionPeerMarkerTargetVM> list = this.PeerTargets.ToList<MissionPeerMarkerTargetVM>();
			using (List<MissionPeer>.Enumerator enumerator = VirtualPlayer.Peers<MissionPeer>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionPeer missionPeer = enumerator.Current;
					MissionPeer missionPeer2 = missionPeer;
					if (((missionPeer2 != null) ? missionPeer2.Team : null) != null && !missionPeer.IsMine && missionPeer.Team.Side == battleSideEnum)
					{
						IEnumerable<MissionPeerMarkerTargetVM> enumerable = this.PeerTargets.Where<MissionPeerMarkerTargetVM>(delegate(MissionPeerMarkerTargetVM t)
						{
							MissionPeer targetPeer2 = t.TargetPeer;
							return targetPeer2 != null && targetPeer2.Peer.Id.Equals(missionPeer.Peer.Id);
						});
						if (enumerable.Any<MissionPeerMarkerTargetVM>())
						{
							MissionPeerMarkerTargetVM currentMarker = enumerable.First<MissionPeerMarkerTargetVM>();
							IEnumerable<MissionAlwaysVisibleMarkerTargetVM> enumerable2 = this.AlwaysVisibleTargets.Where<MissionAlwaysVisibleMarkerTargetVM>((MissionAlwaysVisibleMarkerTargetVM t) => t.TargetPeer.Peer.Id.Equals(currentMarker.TargetPeer.Peer.Id));
							if (BannerlordConfig.EnableDeathIcon && !missionPeer.IsControlledAgentActive)
							{
								if (enumerable2.Any<MissionAlwaysVisibleMarkerTargetVM>())
								{
									continue;
								}
								MissionPeer targetPeer = enumerable.First<MissionPeerMarkerTargetVM>().TargetPeer;
								if (((targetPeer != null) ? targetPeer.ControlledAgent : null) != null)
								{
									MissionAlwaysVisibleMarkerTargetVM missionAlwaysVisibleMarkerTargetVM = new MissionAlwaysVisibleMarkerTargetVM(currentMarker.TargetPeer, enumerable.First<MissionPeerMarkerTargetVM>().WorldPosition, new Action<MissionAlwaysVisibleMarkerTargetVM>(this.OnRemoveAlwaysVisibleMarker));
									missionAlwaysVisibleMarkerTargetVM.UpdateScreenPosition(this._missionCamera);
									this.AlwaysVisibleTargets.Add(missionAlwaysVisibleMarkerTargetVM);
									continue;
								}
								continue;
							}
						}
						if (!this._teammateDictionary.ContainsKey(missionPeer))
						{
							MissionPeerMarkerTargetVM missionPeerMarkerTargetVM = new MissionPeerMarkerTargetVM(missionPeer, this._friendIDs.Contains(missionPeer.Peer.Id));
							this.PeerTargets.Add(missionPeerMarkerTargetVM);
							this._teammateDictionary.Add(missionPeer, missionPeerMarkerTargetVM);
						}
						else
						{
							list.Remove(this._teammateDictionary[missionPeer]);
						}
					}
				}
			}
			using (List<MissionPeerMarkerTargetVM>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					MissionPeerMarkerTargetVM missionPeerMarkerTargetVM2;
					if ((missionPeerMarkerTargetVM2 = enumerator2.Current) != null)
					{
						this.PeerTargets.Remove(missionPeerMarkerTargetVM2);
						this._teammateDictionary.Remove(missionPeerMarkerTargetVM2.TargetPeer);
					}
				}
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0002E154 File Offset: 0x0002C354
		public void OnRemoveAlwaysVisibleMarker(MissionAlwaysVisibleMarkerTargetVM marker)
		{
			this.AlwaysVisibleTargets.Remove(marker);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0002E164 File Offset: 0x0002C364
		private void UpdateTargetStates(bool state)
		{
			this.PeerTargets.ApplyActionOnAllItems(delegate(MissionPeerMarkerTargetVM pt)
			{
				pt.IsEnabled = state;
			});
			this.FlagTargets.ApplyActionOnAllItems(delegate(MissionFlagMarkerTargetVM ft)
			{
				ft.IsEnabled = state;
			});
			this.SiegeEngineTargets.ApplyActionOnAllItems(delegate(MissionSiegeEngineMarkerTargetVM st)
			{
				st.IsEnabled = state;
			});
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0002E1C3 File Offset: 0x0002C3C3
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x0002E1CB File Offset: 0x0002C3CB
		[DataSourceProperty]
		public MBBindingList<MissionFlagMarkerTargetVM> FlagTargets
		{
			get
			{
				return this._flagTargets;
			}
			set
			{
				if (value != this._flagTargets)
				{
					this._flagTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionFlagMarkerTargetVM>>(value, "FlagTargets");
				}
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0002E1E9 File Offset: 0x0002C3E9
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x0002E1F1 File Offset: 0x0002C3F1
		[DataSourceProperty]
		public MBBindingList<MissionPeerMarkerTargetVM> PeerTargets
		{
			get
			{
				return this._peerTargets;
			}
			set
			{
				if (value != this._peerTargets)
				{
					this._peerTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionPeerMarkerTargetVM>>(value, "PeerTargets");
				}
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0002E20F File Offset: 0x0002C40F
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x0002E217 File Offset: 0x0002C417
		[DataSourceProperty]
		public MBBindingList<MissionSiegeEngineMarkerTargetVM> SiegeEngineTargets
		{
			get
			{
				return this._siegeEngineTargets;
			}
			set
			{
				if (value != this._siegeEngineTargets)
				{
					this._siegeEngineTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionSiegeEngineMarkerTargetVM>>(value, "SiegeEngineTargets");
				}
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0002E235 File Offset: 0x0002C435
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x0002E23D File Offset: 0x0002C43D
		[DataSourceProperty]
		public MBBindingList<MissionAlwaysVisibleMarkerTargetVM> AlwaysVisibleTargets
		{
			get
			{
				return this._alwaysVisibleTargets;
			}
			set
			{
				if (value != this._alwaysVisibleTargets)
				{
					this._alwaysVisibleTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAlwaysVisibleMarkerTargetVM>>(value, "AlwaysVisibleTargets");
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0002E25B File Offset: 0x0002C45B
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x0002E263 File Offset: 0x0002C463
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
					this.UpdateTargetStates(value);
				}
			}
		}

		// Token: 0x040006D4 RID: 1748
		private readonly Camera _missionCamera;

		// Token: 0x040006D5 RID: 1749
		private bool _prevEnabledState;

		// Token: 0x040006D6 RID: 1750
		private bool _fadeOutTimerStarted;

		// Token: 0x040006D7 RID: 1751
		private float _fadeOutTimer;

		// Token: 0x040006D8 RID: 1752
		private MultiplayerMissionMarkerVM.MarkerDistanceComparer _distanceComparer;

		// Token: 0x040006D9 RID: 1753
		private readonly ICommanderInfo _commanderInfo;

		// Token: 0x040006DA RID: 1754
		private readonly Dictionary<MissionPeer, MissionPeerMarkerTargetVM> _teammateDictionary;

		// Token: 0x040006DB RID: 1755
		private readonly MissionMultiplayerSiegeClient _siegeClient;

		// Token: 0x040006DC RID: 1756
		private readonly List<PlayerId> _friendIDs;

		// Token: 0x040006DD RID: 1757
		private MBBindingList<MissionFlagMarkerTargetVM> _flagTargets;

		// Token: 0x040006DE RID: 1758
		private MBBindingList<MissionPeerMarkerTargetVM> _peerTargets;

		// Token: 0x040006DF RID: 1759
		private MBBindingList<MissionSiegeEngineMarkerTargetVM> _siegeEngineTargets;

		// Token: 0x040006E0 RID: 1760
		private MBBindingList<MissionAlwaysVisibleMarkerTargetVM> _alwaysVisibleTargets;

		// Token: 0x040006E1 RID: 1761
		private bool _isEnabled;

		// Token: 0x0200017C RID: 380
		public class MarkerDistanceComparer : IComparer<MissionMarkerTargetVM>
		{
			// Token: 0x060012BE RID: 4798 RVA: 0x0003AC1C File Offset: 0x00038E1C
			public int Compare(MissionMarkerTargetVM x, MissionMarkerTargetVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
