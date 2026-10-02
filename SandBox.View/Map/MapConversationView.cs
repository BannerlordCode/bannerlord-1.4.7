using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x0200004A RID: 74
	public class MapConversationView : MapView
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00017403 File Offset: 0x00015603
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0001740B File Offset: 0x0001560B
		public bool IsConversationActive { get; protected set; }

		// Token: 0x0600027C RID: 636 RVA: 0x00017414 File Offset: 0x00015614
		protected internal virtual void InitializeConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00017416 File Offset: 0x00015616
		protected internal override void OnFinalize()
		{
			base.OnFinalize();
			this.DestroyConversationMission();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00017424 File Offset: 0x00015624
		protected internal virtual void FinalizeConversation()
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00017428 File Offset: 0x00015628
		protected void CreateConversationMissionIfMissing()
		{
			MapConversationView.MapConversationMission mapConversationMission;
			if ((mapConversationMission = CampaignMission.Current as MapConversationView.MapConversationMission) != null)
			{
				this.ConversationMission = mapConversationMission;
				return;
			}
			this.ConversationMission = new MapConversationView.MapConversationMission();
			CampaignMission.Current = this.ConversationMission;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00017461 File Offset: 0x00015661
		protected void DestroyConversationMission()
		{
			MapConversationView.MapConversationMission conversationMission = this.ConversationMission;
			if (conversationMission != null)
			{
				conversationMission.OnFinalize();
			}
			this.ConversationMission = null;
		}

		// Token: 0x0400015B RID: 347
		public MapConversationView.MapConversationMission ConversationMission;

		// Token: 0x020000A3 RID: 163
		public class MapConversationMission : ICampaignMission
		{
			// Token: 0x170000AE RID: 174
			// (get) Token: 0x060005AF RID: 1455 RVA: 0x00028BD5 File Offset: 0x00026DD5
			GameState ICampaignMission.State
			{
				get
				{
					return GameStateManager.Current.ActiveState;
				}
			}

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00028BE1 File Offset: 0x00026DE1
			IMissionTroopSupplier ICampaignMission.AgentSupplier
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00028BE4 File Offset: 0x00026DE4
			// (set) Token: 0x060005B2 RID: 1458 RVA: 0x00028BEC File Offset: 0x00026DEC
			Location ICampaignMission.Location { get; set; }

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00028BF5 File Offset: 0x00026DF5
			// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00028BFD File Offset: 0x00026DFD
			Alley ICampaignMission.LastVisitedAlley { get; set; }

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00028C06 File Offset: 0x00026E06
			MissionMode ICampaignMission.Mode
			{
				get
				{
					return MissionMode.Conversation;
				}
			}

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00028C09 File Offset: 0x00026E09
			// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00028C11 File Offset: 0x00026E11
			public MapConversationTableau ConversationTableau { get; private set; }

			// Token: 0x060005B8 RID: 1464 RVA: 0x00028C1A File Offset: 0x00026E1A
			public MapConversationMission()
			{
				CampaignMission.Current = this;
				this._conversationPlayQueue = new Queue<MapConversationView.MapConversationMission.ConversationPlayArgs>();
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x00028C33 File Offset: 0x00026E33
			public void SetConversationTableau(MapConversationTableau tableau)
			{
				this.ConversationTableau = tableau;
				this.PlayCachedConversations();
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x00028C42 File Offset: 0x00026E42
			public void Tick(float dt)
			{
				this.PlayCachedConversations();
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x00028C4A File Offset: 0x00026E4A
			public void OnFinalize()
			{
				this.ConversationTableau = null;
				this._conversationPlayQueue = null;
				CampaignMission.Current = null;
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x00028C60 File Offset: 0x00026E60
			private void PlayCachedConversations()
			{
				if (this.ConversationTableau != null)
				{
					while (this._conversationPlayQueue.Count > 0)
					{
						MapConversationView.MapConversationMission.ConversationPlayArgs conversationPlayArgs = this._conversationPlayQueue.Dequeue();
						this.ConversationTableau.OnConversationPlay(conversationPlayArgs.IdleActionId, conversationPlayArgs.IdleFaceAnimId, conversationPlayArgs.ReactionId, conversationPlayArgs.ReactionFaceAnimId, conversationPlayArgs.SoundPath);
					}
				}
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x00028CBA File Offset: 0x00026EBA
			void ICampaignMission.OnConversationPlay(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath)
			{
				if (this.ConversationTableau != null)
				{
					this.ConversationTableau.OnConversationPlay(idleActionId, idleFaceAnimId, reactionId, reactionFaceAnimId, soundPath);
					return;
				}
				this._conversationPlayQueue.Enqueue(new MapConversationView.MapConversationMission.ConversationPlayArgs(idleActionId, idleFaceAnimId, reactionId, reactionFaceAnimId, soundPath));
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x00028CEE File Offset: 0x00026EEE
			void ICampaignMission.AddAgentFollowing(IAgent agent)
			{
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x00028CF0 File Offset: 0x00026EF0
			bool ICampaignMission.AgentLookingAtAgent(IAgent agent1, IAgent agent2)
			{
				return false;
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x00028CF3 File Offset: 0x00026EF3
			bool ICampaignMission.CheckIfAgentCanFollow(IAgent agent)
			{
				return false;
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x00028CF6 File Offset: 0x00026EF6
			bool ICampaignMission.CheckIfAgentCanUnFollow(IAgent agent)
			{
				return false;
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x00028CF9 File Offset: 0x00026EF9
			void ICampaignMission.EndMission()
			{
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x00028CFB File Offset: 0x00026EFB
			void ICampaignMission.OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation)
			{
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x00028CFD File Offset: 0x00026EFD
			void ICampaignMission.OnCloseEncounterMenu()
			{
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x00028CFF File Offset: 0x00026EFF
			void ICampaignMission.OnConversationContinue()
			{
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x00028D01 File Offset: 0x00026F01
			void ICampaignMission.OnConversationEnd(IAgent agent)
			{
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x00028D03 File Offset: 0x00026F03
			void ICampaignMission.OnConversationStart(IAgent agent, bool setActionsInstantly)
			{
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x00028D05 File Offset: 0x00026F05
			void ICampaignMission.OnProcessSentence()
			{
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x00028D07 File Offset: 0x00026F07
			void ICampaignMission.RemoveAgentFollowing(IAgent agent)
			{
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x00028D09 File Offset: 0x00026F09
			void ICampaignMission.SetMissionMode(MissionMode newMode, bool atStart)
			{
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x00028D0B File Offset: 0x00026F0B
			void ICampaignMission.FadeOutCharacter(CharacterObject characterObject)
			{
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x00028D0D File Offset: 0x00026F0D
			void ICampaignMission.OnGameStateChanged()
			{
				MapConversationTableau conversationTableau = this.ConversationTableau;
				if (conversationTableau != null)
				{
					conversationTableau.RemovePreviousAgentsSoundEvent();
				}
				MapConversationTableau conversationTableau2 = this.ConversationTableau;
				if (conversationTableau2 == null)
				{
					return;
				}
				conversationTableau2.StopConversationSoundEvent();
			}

			// Token: 0x04000341 RID: 833
			private Queue<MapConversationView.MapConversationMission.ConversationPlayArgs> _conversationPlayQueue;

			// Token: 0x020000CE RID: 206
			public struct ConversationPlayArgs
			{
				// Token: 0x0600067B RID: 1659 RVA: 0x0002A0FF File Offset: 0x000282FF
				public ConversationPlayArgs(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath)
				{
					this.IdleActionId = idleActionId;
					this.IdleFaceAnimId = idleFaceAnimId;
					this.ReactionId = reactionId;
					this.ReactionFaceAnimId = reactionFaceAnimId;
					this.SoundPath = soundPath;
				}

				// Token: 0x040003E0 RID: 992
				public readonly string IdleActionId;

				// Token: 0x040003E1 RID: 993
				public readonly string IdleFaceAnimId;

				// Token: 0x040003E2 RID: 994
				public readonly string ReactionId;

				// Token: 0x040003E3 RID: 995
				public readonly string ReactionFaceAnimId;

				// Token: 0x040003E4 RID: 996
				public readonly string SoundPath;
			}
		}
	}
}
