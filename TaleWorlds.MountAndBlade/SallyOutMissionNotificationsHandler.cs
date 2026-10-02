using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000298 RID: 664
	public class SallyOutMissionNotificationsHandler
	{
		// Token: 0x060024C2 RID: 9410 RVA: 0x000856F4 File Offset: 0x000838F4
		public SallyOutMissionNotificationsHandler(DefaultBattleMissionAgentSpawnLogic spawnLogic, SallyOutMissionController sallyOutController)
		{
			this._spawnLogic = spawnLogic;
			this._sallyOutController = sallyOutController;
			this._spawnLogic.OnReinforcementsSpawned += this.OnReinforcementsSpawned;
			this._spawnLogic.OnInitialTroopsSpawned += this.OnInitialTroopsSpawned;
			this._besiegerSpawnedTroopCount = 0;
			this._notificationTimer = new BasicMissionTimer();
			this._notificationsQueue = new Queue<SallyOutMissionNotificationsHandler.NotificationType>();
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x00085768 File Offset: 0x00083968
		public void OnBesiegedSideFallsbackToKeep()
		{
			if (this._isPlayerBesieged)
			{
				if (Mission.Current.PlayerTeam.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.IsAIControlled && f.CountOfUnits > 0))
				{
					this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat);
					if (Mission.Current.MainAgent != null && Mission.Current.MainAgent.IsActive())
					{
						this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegedSidePlayerPullbackRequest);
						return;
					}
				}
			}
			else
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat);
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000857F4 File Offset: 0x000839F4
		public void OnAfterStart()
		{
			this._isPlayerBesieged = Mission.Current.PlayerTeam.Side == BattleSideEnum.Defender;
			this.SetNotificationTimerEnabled(false, true);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00085816 File Offset: 0x00083A16
		public void OnMissionEnd()
		{
			this._spawnLogic.OnReinforcementsSpawned -= this.OnReinforcementsSpawned;
			this._spawnLogic.OnInitialTroopsSpawned -= this.OnInitialTroopsSpawned;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x00085846 File Offset: 0x00083A46
		public void OnDeploymentFinished()
		{
			this.SetNotificationTimerEnabled(true, true);
			this._besiegerSiegeEngines = this._sallyOutController.BesiegerSiegeEngines;
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00085864 File Offset: 0x00083A64
		public void OnMissionTick(float dt)
		{
			if (this._notificationTimerEnabled && this._notificationTimer.ElapsedTime >= 5f)
			{
				this.CheckPeriodicNotifications();
				if (!this._notificationsQueue.IsEmpty<SallyOutMissionNotificationsHandler.NotificationType>())
				{
					SallyOutMissionNotificationsHandler.NotificationType notificationType = this._notificationsQueue.Dequeue();
					this.SendNotification(notificationType);
				}
				this._notificationTimer.Reset();
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000858BC File Offset: 0x00083ABC
		private void SetNotificationTimerEnabled(bool value, bool resetTimer = true)
		{
			this._notificationTimerEnabled = value;
			if (resetTimer)
			{
				this._notificationTimer.Reset();
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000858D4 File Offset: 0x00083AD4
		private void CheckPeriodicNotifications()
		{
			if (!this._objectiveMessageSent)
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.SallyOutObjective);
				this._objectiveMessageSent = true;
			}
			if (!this._siegeEnginesDestroyedMessageSent && this.IsSiegeEnginesDestroyed())
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.SiegeEnginesDestroyed);
				this._siegeEnginesDestroyedMessageSent = true;
			}
			if (!this._besiegersStrengtheningMessageSent && this._spawnLogic.NumberOfRemainingDefenderTroops == 0 && this._besiegerSpawnedTroopCount >= this._spawnLogic.NumberOfActiveDefenderTroops)
			{
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideStrenghtening);
				this._besiegersStrengtheningMessageSent = true;
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0008595C File Offset: 0x00083B5C
		private void SendNotification(SallyOutMissionNotificationsHandler.NotificationType type)
		{
			int num = -1;
			if (this._isPlayerBesieged)
			{
				switch (type)
				{
				case SallyOutMissionNotificationsHandler.NotificationType.SallyOutObjective:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_besieged_objective_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideStrenghtening:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_enemy_becoming_strong_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/retreat");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideReinforcementsSpawned:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_enemy_reinforcements_arrived", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/reinforcements");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_allied_troops_tactical_retreat_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/retreat");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegedSidePlayerPullbackRequest:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_allied_pullback_or_take_command_message", null), 0, null, null, "");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.SiegeEnginesDestroyed:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_enemy_siege_engines_destroyed_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				}
			}
			else
			{
				switch (type)
				{
				case SallyOutMissionNotificationsHandler.NotificationType.SallyOutObjective:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_besieger_objective_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideReinforcementsSpawned:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_allied_reinforcements_arrived", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/reinforcements");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.BesiegedSideTacticalRetreat:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_enemy_troops_fall_back_to_keep_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				case SallyOutMissionNotificationsHandler.NotificationType.SiegeEnginesDestroyed:
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_sally_out_allied_siege_engines_destroyed_message", null), 0, null, null, "");
					num = SoundEvent.GetEventIdFromString("event:/alerts/horns/move");
					break;
				}
			}
			if (num >= 0)
			{
				this.PlayNotificationSound(num);
			}
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00085B40 File Offset: 0x00083D40
		private void PlayNotificationSound(int soundId)
		{
			MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
			Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
			MBSoundEvent.PlaySound(soundId, vec);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00085B77 File Offset: 0x00083D77
		private void OnInitialTroopsSpawned(BattleSideEnum battleSide, int numberOfTroopsSpawned)
		{
			if (battleSide == BattleSideEnum.Attacker)
			{
				this._besiegerSpawnedTroopCount += numberOfTroopsSpawned;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00085B8B File Offset: 0x00083D8B
		private void OnReinforcementsSpawned(BattleSideEnum battleSide, int numberOfTroopsSpawned)
		{
			if (battleSide == BattleSideEnum.Attacker)
			{
				this._besiegerSpawnedTroopCount += numberOfTroopsSpawned;
				this._notificationsQueue.Enqueue(SallyOutMissionNotificationsHandler.NotificationType.BesiegerSideReinforcementsSpawned);
			}
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00085BAB File Offset: 0x00083DAB
		private bool IsSiegeEnginesDestroyed()
		{
			if (this._besiegerSiegeEngines != null)
			{
				return this._besiegerSiegeEngines.All<SiegeWeapon>((SiegeWeapon siegeEngine) => siegeEngine.DestructionComponent.IsDestroyed);
			}
			return false;
		}

		// Token: 0x04000E39 RID: 3641
		private const float NotificationCheckInterval = 5f;

		// Token: 0x04000E3A RID: 3642
		private DefaultBattleMissionAgentSpawnLogic _spawnLogic;

		// Token: 0x04000E3B RID: 3643
		private SallyOutMissionController _sallyOutController;

		// Token: 0x04000E3C RID: 3644
		private bool _isPlayerBesieged;

		// Token: 0x04000E3D RID: 3645
		private MBReadOnlyList<SiegeWeapon> _besiegerSiegeEngines;

		// Token: 0x04000E3E RID: 3646
		private Queue<SallyOutMissionNotificationsHandler.NotificationType> _notificationsQueue;

		// Token: 0x04000E3F RID: 3647
		private BasicMissionTimer _notificationTimer;

		// Token: 0x04000E40 RID: 3648
		private bool _notificationTimerEnabled = true;

		// Token: 0x04000E41 RID: 3649
		private bool _objectiveMessageSent;

		// Token: 0x04000E42 RID: 3650
		private bool _siegeEnginesDestroyedMessageSent;

		// Token: 0x04000E43 RID: 3651
		private bool _besiegersStrengtheningMessageSent;

		// Token: 0x04000E44 RID: 3652
		private int _besiegerSpawnedTroopCount;

		// Token: 0x0200056B RID: 1387
		private enum NotificationType
		{
			// Token: 0x04001E27 RID: 7719
			SallyOutObjective,
			// Token: 0x04001E28 RID: 7720
			BesiegerSideStrenghtening,
			// Token: 0x04001E29 RID: 7721
			BesiegerSideReinforcementsSpawned,
			// Token: 0x04001E2A RID: 7722
			BesiegedSideTacticalRetreat,
			// Token: 0x04001E2B RID: 7723
			BesiegedSidePlayerPullbackRequest,
			// Token: 0x04001E2C RID: 7724
			SiegeEnginesDestroyed
		}
	}
}
