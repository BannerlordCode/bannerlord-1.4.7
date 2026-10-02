using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.MissionRepresentatives
{
	// Token: 0x020003C4 RID: 964
	public class FlagDominationMissionRepresentative : MissionRepresentativeBase
	{
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600360C RID: 13836 RVA: 0x000DEB41 File Offset: 0x000DCD41
		private bool Forfeited
		{
			get
			{
				return base.Gold < 0;
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000DEB4C File Offset: 0x000DCD4C
		public int GetGoldAmountForVisual()
		{
			if (base.Gold < 0)
			{
				return 80;
			}
			return base.Gold;
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000DEB60 File Offset: 0x000DCD60
		public void UpdateSelectedClassServer(Agent agent)
		{
			this._survivedLastRound = agent != null;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000DEB6C File Offset: 0x000DCD6C
		public bool CheckIfSurvivedLastRoundAndReset()
		{
			bool survivedLastRound = this._survivedLastRound;
			this._survivedLastRound = false;
			return survivedLastRound;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000DEB7C File Offset: 0x000DCD7C
		public int GetGoldGainsFromKillData(MPPerkObject.MPPerkHandler killerPerkHandler, MPPerkObject.MPPerkHandler assistingHitterPerkHandler, MultiplayerClassDivisions.MPHeroClass victimClass, bool isAssist, bool isFriendly)
		{
			if (isFriendly || this.Forfeited)
			{
				return 0;
			}
			int num;
			if (isAssist)
			{
				num = ((killerPerkHandler != null) ? killerPerkHandler.GetRewardedGoldOnAssist() : 0) + ((assistingHitterPerkHandler != null) ? assistingHitterPerkHandler.GetGoldOnAssist() : 0);
			}
			else
			{
				int num2 = ((base.ControlledAgent != null) ? MultiplayerClassDivisions.GetMPHeroClassForCharacter(base.ControlledAgent.Character).TroopBattleCost : 0);
				num = ((killerPerkHandler != null) ? killerPerkHandler.GetGoldOnKill((float)num2, (float)victimClass.TroopBattleCost) : 0);
			}
			if (num > 0)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(2048, num)
				}));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000DEC28 File Offset: 0x000DCE28
		public int GetGoldGainFromKillDataAndUpdateFlags(MultiplayerClassDivisions.MPHeroClass victimClass, bool isAssist)
		{
			int num = 0;
			int num2 = 50;
			List<KeyValuePair<ushort, int>> list = new List<KeyValuePair<ushort, int>>();
			if (base.ControlledAgent != null)
			{
				num2 += victimClass.TroopBattleCost - MultiplayerClassDivisions.GetMPHeroClassForCharacter(base.ControlledAgent.Character).TroopBattleCost / 2;
			}
			if (isAssist)
			{
				int num3 = MathF.Max(5, num2 / 10);
				num += num3;
				list.Add(new KeyValuePair<ushort, int>(256, num3));
			}
			else if (base.ControlledAgent != null)
			{
				int num4 = MathF.Max(10, num2 / 5);
				num += num4;
				list.Add(new KeyValuePair<ushort, int>(128, num4));
			}
			if (list.Count > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(list));
				GameNetwork.EndModuleEventAsServer();
			}
			return num;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000DED04 File Offset: 0x000DCF04
		public int GetGoldGainsFromAllyDeathReward(int baseAmount)
		{
			if (this.Forfeited)
			{
				return 0;
			}
			if (baseAmount > 0 && !base.Peer.Communicator.IsServerPeer && base.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginModuleEventAsServer(base.Peer);
				GameNetwork.WriteMessage(new GoldGain(new List<KeyValuePair<ushort, int>>
				{
					new KeyValuePair<ushort, int>(2048, baseAmount)
				}));
				GameNetwork.EndModuleEventAsServer();
			}
			return baseAmount;
		}

		// Token: 0x0400171E RID: 5918
		private bool _survivedLastRound;
	}
}
