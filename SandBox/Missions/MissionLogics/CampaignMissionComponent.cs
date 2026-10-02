using System;
using System.Collections.Generic;
using Helpers;
using SandBox.Conversation;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000064 RID: 100
	public class CampaignMissionComponent : MissionLogic, ICampaignMission
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x000166D7 File Offset: 0x000148D7
		public GameState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x000166DF File Offset: 0x000148DF
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x000166E7 File Offset: 0x000148E7
		public IMissionTroopSupplier AgentSupplier { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x000166F0 File Offset: 0x000148F0
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x000166F8 File Offset: 0x000148F8
		public Location Location { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00016701 File Offset: 0x00014901
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x00016709 File Offset: 0x00014909
		public Alley LastVisitedAlley { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00016712 File Offset: 0x00014912
		MissionMode ICampaignMission.Mode
		{
			get
			{
				return base.Mission.Mode;
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001671F File Offset: 0x0001491F
		void ICampaignMission.SetMissionMode(MissionMode newMode, bool atStart)
		{
			base.Mission.SetMissionMode(newMode, atStart);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00016730 File Offset: 0x00014930
		public override void OnAgentCreated(Agent agent)
		{
			base.OnAgentCreated(agent);
			agent.AddComponent(new CampaignAgentComponent(agent));
			CharacterObject characterObject = (CharacterObject)agent.Character;
			if (((characterObject != null) ? characterObject.HeroObject : null) != null && characterObject.HeroObject.IsPlayerCompanion)
			{
				agent.AgentRole = new TextObject("{=kPTp6TPT}({AGENT_ROLE})", null);
				agent.AgentRole.SetTextVariable("AGENT_ROLE", GameTexts.FindText("str_companion", null));
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000167A4 File Offset: 0x000149A4
		public override void OnPreDisplayMissionTick(float dt)
		{
			base.OnPreDisplayMissionTick(dt);
			if (this._soundEvent != null && !this._soundEvent.IsPlaying())
			{
				this.RemovePreviousAgentsSoundEvent();
				this._soundEvent.Stop();
				this._soundEvent = null;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000167DA File Offset: 0x000149DA
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (Campaign.Current != null)
			{
				CampaignEventDispatcher.Instance.MissionTick(dt);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000167F8 File Offset: 0x000149F8
		protected override void OnObjectDisabled(DestructableComponent missionObject)
		{
			SiegeWeapon firstScriptOfType = missionObject.GameEntity.GetFirstScriptOfType<SiegeWeapon>();
			if (firstScriptOfType != null && Campaign.Current != null && Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				CampaignSiegeStateHandler missionBehavior = Mission.Current.GetMissionBehavior<CampaignSiegeStateHandler>();
				if (missionBehavior != null && missionBehavior.IsSallyOut)
				{
					ISiegeEventSide siegeEventSide = missionBehavior.Settlement.SiegeEvent.GetSiegeEventSide(firstScriptOfType.Side);
					siegeEventSide.SiegeEvent.BreakSiegeEngine(siegeEventSide, firstScriptOfType.GetSiegeEngineType());
				}
			}
			base.OnObjectDisabled(missionObject);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00016871 File Offset: 0x00014A71
		public override void EarlyStart()
		{
			this._state = Game.Current.GameStateManager.ActiveState as MissionState;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001688D File Offset: 0x00014A8D
		public override void OnCreated()
		{
			CampaignMission.Current = this;
			this._isMainAgentAnimationSet = false;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001689C File Offset: 0x00014A9C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			CampaignEventDispatcher.Instance.OnMissionStarted(base.Mission);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000168B4 File Offset: 0x00014AB4
		public override void AfterStart()
		{
			base.AfterStart();
			CampaignEventDispatcher.Instance.OnAfterMissionStarted(base.Mission);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000168CC File Offset: 0x00014ACC
		private static void SimulateRunningAwayAgents()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				PartyBase ownerParty = agent.GetComponent<CampaignAgentComponent>().OwnerParty;
				if (ownerParty != null && !agent.IsHero && agent.IsRunningAway && MBRandom.RandomFloat < 0.5f)
				{
					CharacterObject characterObject = (CharacterObject)agent.Character;
					ownerParty.MemberRoster.AddToCounts(characterObject, -1, false, 0, 0, true, -1);
				}
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00016968 File Offset: 0x00014B68
		public override void OnMissionResultReady(MissionResult missionResult)
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && PlayerEncounter.IsActive && PlayerEncounter.Battle != null)
			{
				if (missionResult.PlayerVictory)
				{
					PlayerEncounter.SetPlayerVictorious();
				}
				else if (missionResult.BattleState == BattleState.DefenderPullBack)
				{
					PlayerEncounter.SetPlayerSiegeContinueWithDefenderPullBack();
				}
				MissionResult missionResult2 = base.Mission.MissionResult;
				PlayerEncounter.CampaignBattleResult = CampaignBattleResult.GetResult((missionResult2 != null) ? missionResult2.BattleState : BattleState.None, missionResult.EnemyRetreated);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000169D4 File Offset: 0x00014BD4
		protected override void OnEndMission()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				if (PlayerEncounter.Battle != null && (PlayerEncounter.Battle.IsSiegeAssault || PlayerEncounter.Battle.IsSiegeAmbush) && (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege || Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut))
				{
					IEnumerable<IMissionSiegeWeapon> enumerable;
					IEnumerable<IMissionSiegeWeapon> enumerable2;
					Mission.Current.GetMissionBehavior<MissionSiegeEnginesLogic>().GetMissionSiegeWeapons(out enumerable, out enumerable2);
					PlayerEncounter.Battle.GetLeaderParty(BattleSideEnum.Attacker).SiegeEvent.SetSiegeEngineStatesAfterSiegeMission(enumerable2, enumerable);
				}
				if (this._soundEvent != null)
				{
					this.RemovePreviousAgentsSoundEvent();
					this._soundEvent.Stop();
					this._soundEvent = null;
				}
			}
			CampaignEventDispatcher.Instance.OnMissionEnded(base.Mission);
			CampaignMission.Current = null;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00016A8C File Offset: 0x00014C8C
		void ICampaignMission.OnCloseEncounterMenu()
		{
			if (base.Mission.Mode == MissionMode.Conversation)
			{
				Campaign.Current.ConversationManager.EndConversation();
				if (Game.Current.GameStateManager.ActiveState is MissionState)
				{
					Game.Current.GameStateManager.PopState(0);
				}
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00016ADC File Offset: 0x00014CDC
		bool ICampaignMission.AgentLookingAtAgent(IAgent agent1, IAgent agent2)
		{
			return base.Mission.AgentLookingAtAgent((Agent)agent1, (Agent)agent2);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00016AF8 File Offset: 0x00014CF8
		void ICampaignMission.OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation)
		{
			MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			if (toLocation == null)
			{
				missionBehavior.FadeoutExitingLocationCharacter(locationCharacter);
				return;
			}
			missionBehavior.SpawnEnteringLocationCharacter(locationCharacter, fromLocation);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00016B24 File Offset: 0x00014D24
		void ICampaignMission.OnProcessSentence()
		{
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00016B26 File Offset: 0x00014D26
		void ICampaignMission.OnConversationContinue()
		{
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00016B28 File Offset: 0x00014D28
		bool ICampaignMission.CheckIfAgentCanFollow(IAgent agent)
		{
			AgentNavigator agentNavigator = ((Agent)agent).GetComponent<CampaignAgentComponent>().AgentNavigator;
			if (agentNavigator != null)
			{
				DailyBehaviorGroup behaviorGroup = agentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
				return behaviorGroup != null && behaviorGroup.GetBehavior<FollowAgentBehavior>() == null;
			}
			return false;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00016B60 File Offset: 0x00014D60
		void ICampaignMission.AddAgentFollowing(IAgent agent)
		{
			Agent agent2 = (Agent)agent;
			if (agent2.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
			{
				DailyBehaviorGroup behaviorGroup = agent2.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
				behaviorGroup.AddBehavior<FollowAgentBehavior>();
				behaviorGroup.SetScriptedBehavior<FollowAgentBehavior>();
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00016BA0 File Offset: 0x00014DA0
		bool ICampaignMission.CheckIfAgentCanUnFollow(IAgent agent)
		{
			Agent agent2 = (Agent)agent;
			if (agent2.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
			{
				DailyBehaviorGroup behaviorGroup = agent2.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
				return behaviorGroup != null && behaviorGroup.GetBehavior<FollowAgentBehavior>() != null;
			}
			return false;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00016BE4 File Offset: 0x00014DE4
		void ICampaignMission.RemoveAgentFollowing(IAgent agent)
		{
			Agent agent2 = (Agent)agent;
			if (agent2.GetComponent<CampaignAgentComponent>().AgentNavigator != null)
			{
				agent2.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().RemoveBehavior<FollowAgentBehavior>();
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00016C1A File Offset: 0x00014E1A
		void ICampaignMission.EndMission()
		{
			base.Mission.EndMission();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016C28 File Offset: 0x00014E28
		private string GetIdleAnimationId(Agent agent, string selectedId, bool startingConversation)
		{
			Agent.ActionCodeType currentActionType = agent.GetCurrentActionType(0);
			if (currentActionType == Agent.ActionCodeType.Sit)
			{
				return "sit";
			}
			if (currentActionType == Agent.ActionCodeType.SitOnTheFloor)
			{
				return "sit_floor";
			}
			if (currentActionType == Agent.ActionCodeType.SitOnAThrone)
			{
				return "sit_throne";
			}
			if (agent.MountAgent != null)
			{
				ValueTuple<string, ConversationAnimData> animDataForRiderAndMountAgents = this.GetAnimDataForRiderAndMountAgents(agent);
				this.SetMountAgentAnimation(agent.MountAgent, animDataForRiderAndMountAgents.Item2, startingConversation);
				return animDataForRiderAndMountAgents.Item1;
			}
			if (agent == Agent.Main)
			{
				return "normal";
			}
			if (startingConversation)
			{
				CharacterObject characterObject = (CharacterObject)agent.Character;
				PartyBase ownerParty = agent.GetComponent<CampaignAgentComponent>().OwnerParty;
				return CharacterHelper.GetStandingBodyIdle(characterObject, ownerParty);
			}
			return selectedId;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00016CB8 File Offset: 0x00014EB8
		private ValueTuple<string, ConversationAnimData> GetAnimDataForRiderAndMountAgents(Agent riderAgent)
		{
			bool flag = false;
			string text = "";
			bool flag2 = false;
			ConversationAnimData conversationAnimData = null;
			foreach (KeyValuePair<string, ConversationAnimData> keyValuePair in Campaign.Current.ConversationManager.ConversationAnimationManager.ConversationAnims)
			{
				if (keyValuePair.Value != null)
				{
					if (keyValuePair.Value.FamilyType == riderAgent.MountAgent.Monster.FamilyType)
					{
						conversationAnimData = keyValuePair.Value;
						flag2 = true;
					}
					else if (keyValuePair.Value.FamilyType == riderAgent.Monster.FamilyType && keyValuePair.Value.MountFamilyType == riderAgent.MountAgent.Monster.FamilyType)
					{
						text = keyValuePair.Key;
						flag = true;
					}
					if (flag2 && flag)
					{
						break;
					}
				}
			}
			return new ValueTuple<string, ConversationAnimData>(text, conversationAnimData);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00016DA8 File Offset: 0x00014FA8
		private int GetActionChannelNoForConversation(Agent agent)
		{
			if (agent.IsSitting())
			{
				return 0;
			}
			if (agent.MountAgent != null)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00016DC0 File Offset: 0x00014FC0
		private void SetMountAgentAnimation(IAgent agent, ConversationAnimData mountAnimData, bool startingConversation)
		{
			Agent agent2 = (Agent)agent;
			if (mountAnimData != null)
			{
				if (startingConversation)
				{
					this._conversationAgents.Add(new CampaignMissionComponent.AgentConversationState(agent2));
				}
				ActionIndexCache actionIndexCache = (string.IsNullOrEmpty(mountAnimData.IdleAnimStart) ? ActionIndexCache.Create(mountAnimData.IdleAnimLoop) : ActionIndexCache.Create(mountAnimData.IdleAnimStart));
				this.SetConversationAgentActionAtChannel(agent2, in actionIndexCache, this.GetActionChannelNoForConversation(agent2), false, false);
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00016E24 File Offset: 0x00015024
		void ICampaignMission.OnConversationStart(IAgent iAgent, bool setActionsInstantly)
		{
			((Agent)iAgent).AgentVisuals.SetAgentLodZeroOrMax(true);
			Agent.Main.AgentVisuals.SetAgentLodZeroOrMax(true);
			if (!this._isMainAgentAnimationSet)
			{
				this._isMainAgentAnimationSet = true;
				this.StartConversationAnimations(Agent.Main, setActionsInstantly);
			}
			this.StartConversationAnimations(iAgent, setActionsInstantly);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00016E78 File Offset: 0x00015078
		private void StartConversationAnimations(IAgent iAgent, bool setActionsInstantly)
		{
			Agent agent = (Agent)iAgent;
			this._conversationAgents.Add(new CampaignMissionComponent.AgentConversationState(agent));
			string idleAnimationId = this.GetIdleAnimationId(agent, "", true);
			string defaultFaceIdle = CharacterHelper.GetDefaultFaceIdle((CharacterObject)agent.Character);
			int actionChannelNoForConversation = this.GetActionChannelNoForConversation(agent);
			ConversationAnimData conversationAnimData;
			if (Campaign.Current.ConversationManager.ConversationAnimationManager.ConversationAnims.TryGetValue(idleAnimationId, out conversationAnimData))
			{
				ActionIndexCache actionIndexCache = (string.IsNullOrEmpty(conversationAnimData.IdleAnimStart) ? ActionIndexCache.Create(conversationAnimData.IdleAnimLoop) : ActionIndexCache.Create(conversationAnimData.IdleAnimStart));
				this.SetConversationAgentActionAtChannel(agent, in actionIndexCache, actionChannelNoForConversation, setActionsInstantly, false);
				this.SetFaceIdle(agent, defaultFaceIdle);
			}
			if (agent.IsUsingGameObject)
			{
				agent.CurrentlyUsedGameObject.OnUserConversationStart();
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00016F34 File Offset: 0x00015134
		private void EndConversationAnimations(IAgent iAgent, bool ignorePriority = false)
		{
			Agent agent = (Agent)iAgent;
			if (agent.IsHuman)
			{
				agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.High, "", false);
				agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.Mid, "", false);
				if (agent.HasMount)
				{
					this.EndConversationAnimations(agent.MountAgent, true);
				}
			}
			int num = -1;
			int count = this._conversationAgents.Count;
			for (int i = 0; i < count; i++)
			{
				CampaignMissionComponent.AgentConversationState agentConversationState = this._conversationAgents[i];
				if (agentConversationState.Agent == agent)
				{
					for (int j = 0; j < 2; j++)
					{
						if (agentConversationState.IsChannelModified(j))
						{
							agent.SetActionChannel(j, in ActionIndexCache.act_none, ignorePriority, (AnimFlags)((long)Math.Min(agent.GetCurrentActionPriority(j), 73)), 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						}
					}
					if (agent.IsUsingGameObject)
					{
						agent.CurrentlyUsedGameObject.OnUserConversationEnd();
					}
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				this._conversationAgents.RemoveAt(num);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001703C File Offset: 0x0001523C
		void ICampaignMission.OnConversationPlay(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath)
		{
			this._currentAgent = (Agent)Campaign.Current.ConversationManager.SpeakerAgent;
			this.RemovePreviousAgentsSoundEvent();
			this.StopPreviousSound();
			string idleAnimationId = this.GetIdleAnimationId(this._currentAgent, idleActionId, false);
			ConversationAnimData conversationAnimData;
			if (!string.IsNullOrEmpty(idleAnimationId) && Campaign.Current.ConversationManager.ConversationAnimationManager.ConversationAnims.TryGetValue(idleAnimationId, out conversationAnimData))
			{
				if (!string.IsNullOrEmpty(reactionId))
				{
					Agent currentAgent = this._currentAgent;
					ActionIndexCache actionIndexCache = ActionIndexCache.Create(conversationAnimData.Reactions[reactionId]);
					this.SetConversationAgentActionAtChannel(currentAgent, in actionIndexCache, 0, false, true);
				}
				else
				{
					ActionIndexCache actionIndexCache2 = (string.IsNullOrEmpty(conversationAnimData.IdleAnimStart) ? ActionIndexCache.Create(conversationAnimData.IdleAnimLoop) : ActionIndexCache.Create(conversationAnimData.IdleAnimStart));
					this.SetConversationAgentActionAtChannel(this._currentAgent, in actionIndexCache2, this.GetActionChannelNoForConversation(this._currentAgent), false, false);
				}
			}
			if (!string.IsNullOrEmpty(reactionFaceAnimId))
			{
				this._currentAgent.SetAgentFacialAnimation(Agent.FacialAnimChannel.Mid, reactionFaceAnimId, false);
			}
			else if (!string.IsNullOrEmpty(idleFaceAnimId))
			{
				this.SetFaceIdle(this._currentAgent, idleFaceAnimId);
			}
			else
			{
				this._currentAgent.SetAgentFacialAnimation(Agent.FacialAnimChannel.High, "", false);
			}
			if (!string.IsNullOrEmpty(soundPath))
			{
				this.PlayConversationSoundEvent(soundPath);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001716C File Offset: 0x0001536C
		private string GetRhubarbXmlPathFromSoundPath(string soundPath)
		{
			int num = soundPath.LastIndexOf('.');
			return soundPath.Substring(0, num) + ".xml";
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00017194 File Offset: 0x00015394
		public void PlayConversationSoundEvent(string soundPath)
		{
			Vec3 position = ConversationMission.CurrentSpeakerAgent.Position;
			Debug.Print(string.Concat(new object[] { "Conversation sound playing: ", soundPath, ", position: ", position }), 5, Debug.DebugColor.White, 17592186044416UL);
			this._soundEvent = SoundEvent.CreateEventFromExternalFile("event:/Extra/voiceover", soundPath, Mission.Current.Scene, false, false);
			this._soundEvent.SetPosition(position);
			this._soundEvent.Play();
			int soundId = this._soundEvent.GetSoundId();
			this._agentSoundEvents.Add(this._currentAgent, soundId);
			string rhubarbXmlPathFromSoundPath = this.GetRhubarbXmlPathFromSoundPath(soundPath);
			this._currentAgent.AgentVisuals.StartRhubarbRecord(rhubarbXmlPathFromSoundPath, soundId);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00017252 File Offset: 0x00015452
		private void StopPreviousSound()
		{
			if (this._soundEvent != null)
			{
				this._soundEvent.Stop();
				this._soundEvent = null;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00017270 File Offset: 0x00015470
		private void RemovePreviousAgentsSoundEvent()
		{
			if (this._soundEvent != null && this._agentSoundEvents.ContainsValue(this._soundEvent.GetSoundId()))
			{
				Agent agent = null;
				foreach (KeyValuePair<Agent, int> keyValuePair in this._agentSoundEvents)
				{
					if (keyValuePair.Value == this._soundEvent.GetSoundId())
					{
						agent = keyValuePair.Key;
					}
				}
				this._agentSoundEvents.Remove(agent);
				agent.AgentVisuals.StartRhubarbRecord("", -1);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001731C File Offset: 0x0001551C
		void ICampaignMission.OnConversationEnd(IAgent iAgent)
		{
			Agent agent = (Agent)iAgent;
			agent.ResetLookAgent();
			agent.DisableLookToPointOfInterest();
			Agent.Main.ResetLookAgent();
			Agent.Main.DisableLookToPointOfInterest();
			if (Settlement.CurrentSettlement != null && !base.Mission.HasMissionBehavior<ConversationMissionLogic>())
			{
				agent.AgentVisuals.SetAgentLodZeroOrMax(true);
				Agent.Main.AgentVisuals.SetAgentLodZeroOrMax(true);
			}
			if (this._soundEvent != null)
			{
				this.RemovePreviousAgentsSoundEvent();
				this._soundEvent.Stop();
			}
			if (this._isMainAgentAnimationSet)
			{
				this._isMainAgentAnimationSet = false;
				this.EndConversationAnimations(Agent.Main, false);
			}
			this.EndConversationAnimations(iAgent, false);
			this._soundEvent = null;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x000173C3 File Offset: 0x000155C3
		private void SetFaceIdle(Agent agent, string idleFaceAnimId)
		{
			agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.Mid, idleFaceAnimId, true);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000173D0 File Offset: 0x000155D0
		private void SetConversationAgentActionAtChannel(Agent agent, in ActionIndexCache action, int channelNo, bool setInstantly, bool forceFaceMorphRestart)
		{
			agent.SetActionChannel(channelNo, in action, false, (AnimFlags)0UL, 0f, 1f, setInstantly ? 0f : (-0.2f), 0.4f, 0f, false, -0.2f, 0, forceFaceMorphRestart);
			int count = this._conversationAgents.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._conversationAgents[i].Agent == agent)
				{
					this._conversationAgents[i].SetChannelModified(channelNo);
					return;
				}
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00017458 File Offset: 0x00015658
		public void FadeOutCharacter(CharacterObject characterObject)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.Character != null && agent.Character == characterObject)
				{
					agent.FadeOut(true, true);
					break;
				}
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000174C4 File Offset: 0x000156C4
		public void OnGameStateChanged()
		{
			this.RemovePreviousAgentsSoundEvent();
			this.StopPreviousSound();
		}

		// Token: 0x04000208 RID: 520
		private MissionState _state;

		// Token: 0x0400020C RID: 524
		private SoundEvent _soundEvent;

		// Token: 0x0400020D RID: 525
		private Agent _currentAgent;

		// Token: 0x0400020E RID: 526
		private bool _isMainAgentAnimationSet;

		// Token: 0x0400020F RID: 527
		private readonly Dictionary<Agent, int> _agentSoundEvents = new Dictionary<Agent, int>();

		// Token: 0x04000210 RID: 528
		private readonly List<CampaignMissionComponent.AgentConversationState> _conversationAgents = new List<CampaignMissionComponent.AgentConversationState>();

		// Token: 0x0200015F RID: 351
		private class AgentConversationState
		{
			// Token: 0x17000132 RID: 306
			// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00065054 File Offset: 0x00063254
			// (set) Token: 0x06000E3C RID: 3644 RVA: 0x0006505C File Offset: 0x0006325C
			public Agent Agent { get; private set; }

			// Token: 0x06000E3D RID: 3645 RVA: 0x00065065 File Offset: 0x00063265
			public AgentConversationState(Agent agent)
			{
				this.Agent = agent;
				this._actionAtChannelModified = default(StackArray.StackArray2Bool);
				this._actionAtChannelModified[0] = false;
				this._actionAtChannelModified[1] = false;
			}

			// Token: 0x06000E3E RID: 3646 RVA: 0x0006509A File Offset: 0x0006329A
			public bool IsChannelModified(int channelNo)
			{
				return this._actionAtChannelModified[channelNo];
			}

			// Token: 0x06000E3F RID: 3647 RVA: 0x000650A8 File Offset: 0x000632A8
			public void SetChannelModified(int channelNo)
			{
				this._actionAtChannelModified[channelNo] = true;
			}

			// Token: 0x040006E9 RID: 1769
			private StackArray.StackArray2Bool _actionAtChannelModified;
		}
	}
}
