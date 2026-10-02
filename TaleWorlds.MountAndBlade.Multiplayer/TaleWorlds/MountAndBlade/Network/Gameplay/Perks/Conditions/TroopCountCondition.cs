using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004F RID: 79
	public class TroopCountCondition : MPPerkCondition
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000B510 File Offset: 0x00009710
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.AliveBotCountChange | MPPerkCondition.PerkEventFlags.SpawnEnd;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000B517 File Offset: 0x00009717
		public override bool IsPeerCondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B51A File Offset: 0x0000971A
		protected TroopCountCondition()
		{
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B524 File Offset: 0x00009724
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
					XmlAttribute xmlAttribute = attributes["is_ratio"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			this._isRatio = ((text2 != null) ? text2.ToLower() : null) == "true";
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
					XmlAttribute xmlAttribute2 = attributes2["min"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null)
			{
				this._min = 0f;
			}
			else if (!float.TryParse(text4, out this._min))
			{
				Debug.FailedAssert("provided 'min' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\TroopCountCondition.cs", "Deserialize", 39);
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
					XmlAttribute xmlAttribute3 = attributes3["max"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			if (text6 == null)
			{
				this._max = (this._isRatio ? 1f : float.MaxValue);
				return;
			}
			if (!float.TryParse(text6, out this._max))
			{
				Debug.FailedAssert("provided 'max' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Conditions\\TroopCountCondition.cs", "Deserialize", 49);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B644 File Offset: 0x00009844
		public override bool Check(MissionPeer peer)
		{
			if (peer == null || MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 || peer.ControlledFormation == null)
			{
				return false;
			}
			int num = (peer.IsControlledAgentActive ? (peer.BotsUnderControlAlive + 1) : peer.BotsUnderControlAlive);
			if (this._isRatio)
			{
				float num2 = (float)num / (float)(peer.BotsUnderControlTotal + 1);
				return num2 >= this._min && num2 <= this._max;
			}
			return (float)num >= this._min && (float)num <= this._max;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B6C8 File Offset: 0x000098C8
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			return this.Check(((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null));
		}

		// Token: 0x040000D4 RID: 212
		protected static string StringType = "TroopCount";

		// Token: 0x040000D5 RID: 213
		private bool _isRatio;

		// Token: 0x040000D6 RID: 214
		private float _min;

		// Token: 0x040000D7 RID: 215
		private float _max;
	}
}
