using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects
{
	// Token: 0x0200003A RID: 58
	public class PassageUsePoint : StandingPoint
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000CEDC File Offset: 0x0000B0DC
		public MBReadOnlyList<Agent> MovingAgents
		{
			get
			{
				return this._movingAgents;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000CEE4 File Offset: 0x0000B0E4
		public override Agent MovingAgent
		{
			get
			{
				if (this._movingAgents.Count <= 0)
				{
					return null;
				}
				return this._movingAgents[0];
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000CF02 File Offset: 0x0000B102
		public PassageUsePoint()
		{
			base.IsInstantUse = true;
			this._movingAgents = new MBList<Agent>();
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000CF27 File Offset: 0x0000B127
		public Location ToLocation
		{
			get
			{
				if (!this._initialized)
				{
					this.InitializeLocation();
				}
				return this._toLocation;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000CF3D File Offset: 0x0000B13D
		public override bool HasAIMovingTo
		{
			get
			{
				return this._movingAgents.Count > 0;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000CF4D File Offset: 0x0000B14D
		public override FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Door;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000CF50 File Offset: 0x0000B150
		public override bool IsDisabledForAgent(Agent agent)
		{
			return agent.MountAgent != null || base.IsDeactivated || (this.ToLocation == null && !this.IsMissionExit) || base.IsDisabled || (agent.IsAIControlled && (this.IsMissionExit || !this.ToLocation.CanAIEnter(CampaignMission.Current.Location.GetLocationCharacter(agent.Origin))));
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		public override void AfterMissionStart()
		{
			this.DescriptionMessage = GameTexts.FindText(this.IsMissionExit ? "str_mission_exit" : "str_ui_door", null);
			this.ActionMessage = GameTexts.FindText("str_ui_default_door", null);
			if (this.ToLocation != null || this.IsMissionExit)
			{
				this.ActionMessage = GameTexts.FindText("str_key_action", null);
				this.ActionMessage.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				this.ActionMessage.SetTextVariable("ACTION", (this.ToLocation == null) ? GameTexts.FindText("str_ui_default_door", null) : this.ToLocation.DoorName);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000D077 File Offset: 0x0000B277
		public override bool DisableCombatActionsOnUse
		{
			get
			{
				return !base.IsInstantUse;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000D082 File Offset: 0x0000B282
		protected override void OnInit()
		{
			base.OnInit();
			this.LockUserPositions = true;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000D094 File Offset: 0x0000B294
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign || userAgent.IsAIControlled)
			{
				base.OnUse(userAgent, agentBoneIndex);
				bool flag = false;
				if (this.ToLocation != null)
				{
					if (base.UserAgent.IsMainAgent)
					{
						if (!this.ToLocation.CanPlayerEnter())
						{
							InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=ILnr9eCQ}Door is locked!", null).ToString()));
						}
						else
						{
							flag = true;
							Campaign.Current.GameMenuManager.NextLocation = this.ToLocation;
							Campaign.Current.GameMenuManager.PreviousLocation = CampaignMission.Current.Location;
							Mission.Current.EndMission();
						}
					}
					else if (base.UserAgent.IsAIControlled)
					{
						LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(base.UserAgent.Origin);
						if (!this.ToLocation.CanAIEnter(locationCharacter))
						{
							MBDebug.ShowWarning("AI should not try to use passage ");
						}
						else
						{
							flag = true;
							LocationComplex.Current.ChangeLocation(locationCharacter, CampaignMission.Current.Location, this.ToLocation);
							base.UserAgent.FadeOut(false, true);
						}
					}
				}
				else if (this.IsMissionExit)
				{
					flag = true;
					Mission.Current.EndMission();
				}
				if (flag)
				{
					Mission.Current.MakeSound(MiscSoundContainer.SoundCodeMovementFoleyDoorOpen, base.GameEntity.GetGlobalFrame().origin, true, false, -1, -1);
				}
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000D1F1 File Offset: 0x0000B3F1
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (this.LockUserFrames || this.LockUserPositions)
			{
				userAgent.ClearTargetFrame();
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000D214 File Offset: 0x0000B414
		public override bool IsUsableByAgent(Agent userAgent)
		{
			bool flag = true;
			if (userAgent.IsAIControlled && (this.InteractionEntity.GetGlobalFrame().origin.AsVec2 - userAgent.Position.AsVec2).LengthSquared > 0.25f)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000D26C File Offset: 0x0000B46C
		private void InitializeLocation()
		{
			if (string.IsNullOrEmpty(this.ToLocationId) || this.IsMissionExit)
			{
				this._toLocation = null;
				this._initialized = this.IsMissionExit;
				return;
			}
			if (Mission.Current != null && Campaign.Current != null)
			{
				if (PlayerEncounter.LocationEncounter != null && CampaignMission.Current.Location != null)
				{
					this._toLocation = CampaignMission.Current.Location.GetPassageToLocation(this.ToLocationId);
				}
				this._initialized = true;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000D2E5 File Offset: 0x0000B4E5
		public override int GetMovingAgentCount()
		{
			return this._movingAgents.Count;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000D2F2 File Offset: 0x0000B4F2
		public override Agent GetMovingAgentWithIndex(int index)
		{
			return this._movingAgents[index];
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000D300 File Offset: 0x0000B500
		public override void AddMovingAgent(Agent movingAgent)
		{
			this._movingAgents.Add(movingAgent);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000D30E File Offset: 0x0000B50E
		public override void RemoveMovingAgent(Agent movingAgent)
		{
			this._movingAgents.Remove(movingAgent);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000D31D File Offset: 0x0000B51D
		public override bool IsAIMovingTo(Agent agent)
		{
			return this._movingAgents.Contains(agent);
		}

		// Token: 0x040000C1 RID: 193
		public string ToLocationId = "";

		// Token: 0x040000C2 RID: 194
		public bool IsMissionExit;

		// Token: 0x040000C3 RID: 195
		private bool _initialized;

		// Token: 0x040000C4 RID: 196
		private readonly MBList<Agent> _movingAgents;

		// Token: 0x040000C5 RID: 197
		private Location _toLocation;

		// Token: 0x040000C6 RID: 198
		private const float InteractionDistanceForAI = 0.5f;
	}
}
