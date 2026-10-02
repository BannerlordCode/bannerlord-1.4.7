using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic
{
	// Token: 0x020003DD RID: 989
	public class AmmoSupplyLogic : MissionLogic
	{
		// Token: 0x060036AA RID: 13994 RVA: 0x000E2A82 File Offset: 0x000E0C82
		public AmmoSupplyLogic(List<BattleSideEnum> sideList)
		{
			this._sideList = sideList;
			this._checkTimer = new BasicMissionTimer();
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000E2A9C File Offset: 0x000E0C9C
		public bool IsAgentEligibleForAmmoSupply(Agent agent)
		{
			if (agent.IsAIControlled && this._sideList.Contains(agent.Team.Side))
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					if (!agent.Equipment[equipmentIndex].IsEmpty && agent.Equipment[equipmentIndex].IsAnyAmmo())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000E2B04 File Offset: 0x000E0D04
		public override void OnMissionTick(float dt)
		{
			if (this._checkTimer.ElapsedTime > 3f)
			{
				this._checkTimer.Reset();
				foreach (Team team in base.Mission.Teams)
				{
					if (this._sideList.IndexOf(team.Side) >= 0)
					{
						foreach (Agent agent in team.ActiveAgents)
						{
							for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
							{
								if (agent.IsAIControlled && !agent.Equipment[equipmentIndex].IsEmpty && agent.Equipment[equipmentIndex].IsAnyAmmo())
								{
									short modifiedMaxAmount = agent.Equipment[equipmentIndex].ModifiedMaxAmount;
									short amount = agent.Equipment[equipmentIndex].Amount;
									short num = modifiedMaxAmount;
									if (modifiedMaxAmount > 1)
									{
										num = modifiedMaxAmount - 1;
									}
									if (amount < num)
									{
										agent.SetWeaponAmountInSlot(equipmentIndex, num, false);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400178E RID: 6030
		private const float CheckTimePeriod = 3f;

		// Token: 0x0400178F RID: 6031
		private readonly List<BattleSideEnum> _sideList;

		// Token: 0x04001790 RID: 6032
		private readonly BasicMissionTimer _checkTimer;
	}
}
