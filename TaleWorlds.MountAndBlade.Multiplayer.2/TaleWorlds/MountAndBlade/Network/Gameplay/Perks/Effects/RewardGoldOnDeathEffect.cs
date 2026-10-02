using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003E RID: 62
	public class RewardGoldOnDeathEffect : MPPerkEffect
	{
		// Token: 0x06000233 RID: 563 RVA: 0x00009F30 File Offset: 0x00008130
		protected RewardGoldOnDeathEffect()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009F38 File Offset: 0x00008138
		protected override void Deserialize(XmlNode node)
		{
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_disabled_in_warmup"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			base.IsDisabledInWarmup = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["value"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null || !int.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnDeathEffect.cs", "Deserialize", 41);
			}
			string text5;
			if (node == null)
			{
				text5 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text5 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["number_of_receivers"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			if (text6 == null || !int.TryParse(text6, out this._count))
			{
				Debug.FailedAssert("provided 'number_of_receivers' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnDeathEffect.cs", "Deserialize", 47);
			}
			string text7;
			if (node == null)
			{
				text7 = null;
			}
			else
			{
				XmlAttributeCollection attributes4 = node.Attributes;
				if (attributes4 == null)
				{
					text7 = null;
				}
				else
				{
					XmlAttribute xmlAttribute4 = attributes4["order_by"];
					text7 = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
				}
			}
			string text8 = text7;
			this._orderBy = RewardGoldOnDeathEffect.OrderBy.Random;
			if (text8 != null && !Enum.TryParse<RewardGoldOnDeathEffect.OrderBy>(text8, true, out this._orderBy))
			{
				this._orderBy = RewardGoldOnDeathEffect.OrderBy.Random;
				Debug.FailedAssert("provided 'order_by' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnDeathEffect.cs", "Deserialize", 55);
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A08D File Offset: 0x0000828D
		public override bool GetIsTeamRewardedOnDeath()
		{
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000A090 File Offset: 0x00008290
		public override void CalculateRewardedGoldOnDeath(Agent agent, List<ValueTuple<MissionPeer, int>> teamMembers)
		{
			Agent agent2 = agent;
			if (((agent2 != null) ? agent2.MissionPeer : null) == null)
			{
				Agent agent3 = agent;
				if (agent3 != null)
				{
					MissionPeer owningAgentMissionPeer = agent3.OwningAgentMissionPeer;
				}
			}
			switch (this._orderBy)
			{
			case RewardGoldOnDeathEffect.OrderBy.WealthAscending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => a.Item1.Representative.Gold.CompareTo(b.Item1.Representative.Gold));
				break;
			case RewardGoldOnDeathEffect.OrderBy.WealthDescending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => b.Item1.Representative.Gold.CompareTo(a.Item1.Representative.Gold));
				break;
			case RewardGoldOnDeathEffect.OrderBy.DistanceAscending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => this.SortByDistance(agent, a.Item1.Representative.ControlledAgent, b.Item1.Representative.ControlledAgent));
				break;
			case RewardGoldOnDeathEffect.OrderBy.DistanceDescending:
				teamMembers.Sort((ValueTuple<MissionPeer, int> a, ValueTuple<MissionPeer, int> b) => this.SortByDistance(agent, b.Item1.Representative.ControlledAgent, a.Item1.Representative.ControlledAgent));
				break;
			}
			int num = this._count;
			int num2 = 0;
			while (num2 < teamMembers.Count && num > 0)
			{
				bool flag;
				if (this._orderBy >= RewardGoldOnDeathEffect.OrderBy.DistanceAscending)
				{
					MissionRepresentativeBase representative = teamMembers[num2].Item1.Representative;
					if (representative == null)
					{
						flag = false;
					}
					else
					{
						Agent controlledAgent = representative.ControlledAgent;
						bool? flag2 = ((controlledAgent != null) ? new bool?(controlledAgent.IsActive()) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					ValueTuple<MissionPeer, int> valueTuple = teamMembers[num2];
					valueTuple.Item2 += this._value;
					teamMembers[num2] = valueTuple;
					num--;
				}
				num2++;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000A214 File Offset: 0x00008414
		private int SortByDistance(Agent from, Agent a, Agent b)
		{
			if (a == null || !a.IsActive())
			{
				if (b != null && b.IsActive())
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (b == null || !b.IsActive())
				{
					return -1;
				}
				return a.Position.DistanceSquared(from.Position).CompareTo(b.Position.DistanceSquared(from.Position));
			}
		}

		// Token: 0x040000A9 RID: 169
		protected static string StringType = "RewardGoldOnDeath";

		// Token: 0x040000AA RID: 170
		private int _value;

		// Token: 0x040000AB RID: 171
		private int _count;

		// Token: 0x040000AC RID: 172
		private RewardGoldOnDeathEffect.OrderBy _orderBy;

		// Token: 0x020000A5 RID: 165
		private enum OrderBy
		{
			// Token: 0x040001AF RID: 431
			Random,
			// Token: 0x040001B0 RID: 432
			WealthAscending,
			// Token: 0x040001B1 RID: 433
			WealthDescending,
			// Token: 0x040001B2 RID: 434
			DistanceAscending,
			// Token: 0x040001B3 RID: 435
			DistanceDescending,
			// Token: 0x040001B4 RID: 436
			DeadCanReceiveEnd = 3
		}
	}
}
