using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000013 RID: 19
	public class MissionAudienceHandler : MissionView
	{
		// Token: 0x0600007A RID: 122 RVA: 0x000048D9 File Offset: 0x00002AD9
		public MissionAudienceHandler(float density)
		{
			this._density = density;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000048E8 File Offset: 0x00002AE8
		public override void EarlyStart()
		{
			this._allOneShotSoundEventsAreDisabled = true;
			this._audienceMidPoints = base.Mission.Scene.FindEntitiesWithTag("audience_mid_point").ToList<GameEntity>();
			this._arenaSoundEntity = base.Mission.Scene.FindEntityWithTag("arena_sound");
			this._audienceList = new List<KeyValuePair<GameEntity, float>>();
			if (this._audienceMidPoints.Count > 0)
			{
				this.OnInit();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004958 File Offset: 0x00002B58
		public void OnInit()
		{
			this._minChance = MathF.Max(this._density - 0.5f, 0f);
			this._maxChance = this._density;
			this.GetAudienceEntities();
			this.SpawnAudienceAgents();
			this._lastOneShotSoundEventStarted = MissionTime.Zero;
			this._allOneShotSoundEventsAreDisabled = false;
			this._ambientSoundEvent = SoundManager.CreateEvent("event:/mission/ambient/detail/arena/arena", base.Mission.Scene);
			this._ambientSoundEvent.Play();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000049D2 File Offset: 0x00002BD2
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent != null && affectorAgent.IsHuman && affectedAgent.IsHuman)
			{
				this.Cheer(false);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000049F0 File Offset: 0x00002BF0
		private void Cheer(bool onEnd = false)
		{
			if (!this._allOneShotSoundEventsAreDisabled)
			{
				string text = null;
				if (onEnd)
				{
					text = "event:/mission/ambient/detail/arena/cheer_big";
					this._allOneShotSoundEventsAreDisabled = true;
				}
				else if (this._lastOneShotSoundEventStarted.ElapsedSeconds > 4f && this._lastOneShotSoundEventStarted.ElapsedSeconds < 10f)
				{
					text = "event:/mission/ambient/detail/arena/cheer_medium";
				}
				else if (this._lastOneShotSoundEventStarted.ElapsedSeconds > 10f)
				{
					text = "event:/mission/ambient/detail/arena/cheer_small";
				}
				if (text != null)
				{
					Vec3 vec = ((this._arenaSoundEntity != null) ? this._arenaSoundEntity.GlobalPosition : (this._audienceMidPoints.Any<GameEntity>() ? this._audienceMidPoints.GetRandomElement<GameEntity>().GlobalPosition : Vec3.Zero));
					SoundManager.StartOneShotEvent(text, in vec);
					this._lastOneShotSoundEventStarted = MissionTime.Now;
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004AB8 File Offset: 0x00002CB8
		private void GetAudienceEntities()
		{
			this._maxDist = 0f;
			this._minDist = float.MaxValue;
			this._maxHeight = 0f;
			this._minHeight = float.MaxValue;
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag("audience"))
			{
				float distanceSquareToArena = this.GetDistanceSquareToArena(gameEntity);
				this._maxDist = ((distanceSquareToArena > this._maxDist) ? distanceSquareToArena : this._maxDist);
				this._minDist = ((distanceSquareToArena < this._minDist) ? distanceSquareToArena : this._minDist);
				float z = gameEntity.GetFrame().origin.z;
				this._maxHeight = ((z > this._maxHeight) ? z : this._maxHeight);
				this._minHeight = ((z < this._minHeight) ? z : this._minHeight);
				this._audienceList.Add(new KeyValuePair<GameEntity, float>(gameEntity, distanceSquareToArena));
				gameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004BD4 File Offset: 0x00002DD4
		private float GetDistanceSquareToArena(GameEntity audienceEntity)
		{
			float num = float.MaxValue;
			foreach (GameEntity gameEntity in this._audienceMidPoints)
			{
				float num2 = gameEntity.GlobalPosition.DistanceSquared(audienceEntity.GlobalPosition);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004C40 File Offset: 0x00002E40
		private CharacterObject GetRandomAudienceCharacterToSpawn()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			CharacterObject characterObject = MBRandom.ChooseWeighted<CharacterObject>(new List<ValueTuple<CharacterObject, float>>
			{
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Townswoman, 0.2f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Townsman, 0.2f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Armorer, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Merchant, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Musician, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Weaponsmith, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.RansomBroker, 0.1f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.Barber, 0.05f),
				new ValueTuple<CharacterObject, float>(currentSettlement.Culture.FemaleDancer, 0.05f)
			});
			if (characterObject == null)
			{
				characterObject = ((MBRandom.RandomFloat < 0.65f) ? currentSettlement.Culture.Townsman : currentSettlement.Culture.Townswoman);
			}
			return characterObject;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004D7C File Offset: 0x00002F7C
		private void SpawnAudienceAgents()
		{
			for (int i = this._audienceList.Count - 1; i >= 0; i--)
			{
				KeyValuePair<GameEntity, float> keyValuePair = this._audienceList[i];
				float num = this._minChance + (1f - (keyValuePair.Value - this._minDist) / (this._maxDist - this._minDist)) * (this._maxChance - this._minChance);
				float num2 = this._minChance + (1f - MathF.Pow((keyValuePair.Key.GetFrame().origin.z - this._minHeight) / (this._maxHeight - this._minHeight), 2f)) * (this._maxChance - this._minChance);
				float num3 = num * 0.4f + num2 * 0.6f;
				if (MBRandom.RandomFloat < num3)
				{
					MatrixFrame globalFrame = keyValuePair.Key.GetGlobalFrame();
					CharacterObject randomAudienceCharacterToSpawn = this.GetRandomAudienceCharacterToSpawn();
					AgentBuildData agentBuildData = new AgentBuildData(randomAudienceCharacterToSpawn).InitialPosition(in globalFrame.origin);
					Vec2 vec = new Vec2(-globalFrame.rotation.f.AsVec2.x, -globalFrame.rotation.f.AsVec2.y);
					AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).TroopOrigin(new SimpleAgentOrigin(randomAudienceCharacterToSpawn, -1, null, default(UniqueTroopDescriptor))).Team(Team.Invalid)
						.ClothingColor1(Settlement.CurrentSettlement.MapFaction.Color)
						.ClothingColor2(Settlement.CurrentSettlement.MapFaction.Color2)
						.CanSpawnOutsideOfMissionBoundary(true);
					Agent agent = Mission.Current.SpawnAgent(agentBuildData2, false);
					MBActionSet actionSetWithIndex = MBActionSet.GetActionSetWithIndex(0);
					AnimationSystemData animationSystemData = agentBuildData2.AgentMonster.FillAnimationSystemData(actionSetWithIndex, randomAudienceCharacterToSpawn.GetStepSize(), false);
					agent.SetActionSet(ref animationSystemData);
					MBAnimation.PrefetchAnimationClip(agent.ActionSet, ActionIndexCache.act_arena_spectator);
					agent.SetActionChannel(0, in ActionIndexCache.act_arena_spectator, true, (AnimFlags)0UL, 0f, MBRandom.RandomFloatRanged(0.75f, 1f), -0.2f, 0.4f, MBRandom.RandomFloatRanged(0.01f, 1f), false, -0.2f, 0, true);
					agent.Controller = AgentControllerType.None;
					agent.ToggleInvulnerable();
				}
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004FA3 File Offset: 0x000031A3
		public override void OnMissionTick(float dt)
		{
			if (this._audienceMidPoints == null)
			{
				return;
			}
			if (base.Mission.MissionEnded)
			{
				this.Cheer(true);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004FC2 File Offset: 0x000031C2
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			if (oldMissionMode == MissionMode.Battle && Mission.Current.Mode == MissionMode.StartUp && Agent.Main != null && Agent.Main.IsActive())
			{
				this.Cheer(true);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004FEE File Offset: 0x000031EE
		public override void OnMissionScreenFinalize()
		{
			SoundEvent ambientSoundEvent = this._ambientSoundEvent;
			if (ambientSoundEvent == null)
			{
				return;
			}
			ambientSoundEvent.Release();
		}

		// Token: 0x04000019 RID: 25
		private const int GapBetweenCheerSmallInSeconds = 10;

		// Token: 0x0400001A RID: 26
		private const int GapBetweenCheerMedium = 4;

		// Token: 0x0400001B RID: 27
		private float _minChance;

		// Token: 0x0400001C RID: 28
		private float _maxChance;

		// Token: 0x0400001D RID: 29
		private float _minDist;

		// Token: 0x0400001E RID: 30
		private float _maxDist;

		// Token: 0x0400001F RID: 31
		private float _minHeight;

		// Token: 0x04000020 RID: 32
		private float _maxHeight;

		// Token: 0x04000021 RID: 33
		private List<GameEntity> _audienceMidPoints;

		// Token: 0x04000022 RID: 34
		private List<KeyValuePair<GameEntity, float>> _audienceList;

		// Token: 0x04000023 RID: 35
		private readonly float _density;

		// Token: 0x04000024 RID: 36
		private GameEntity _arenaSoundEntity;

		// Token: 0x04000025 RID: 37
		private SoundEvent _ambientSoundEvent;

		// Token: 0x04000026 RID: 38
		private MissionTime _lastOneShotSoundEventStarted;

		// Token: 0x04000027 RID: 39
		private bool _allOneShotSoundEventsAreDisabled;
	}
}
