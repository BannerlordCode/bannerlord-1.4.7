using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000044 RID: 68
	public class TroopCountEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0000A800 File Offset: 0x00008A00
		protected TroopCountEffect()
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A808 File Offset: 0x00008A08
		protected override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
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
					XmlAttribute xmlAttribute = attributes["value"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null || !int.TryParse(text2, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\TroopCountEffect.cs", "Deserialize", 20);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A86D File Offset: 0x00008A6D
		public override int GetExtraTroopCount()
		{
			return this._value;
		}

		// Token: 0x040000B9 RID: 185
		protected static string StringType = "TroopCountOnSpawn";

		// Token: 0x040000BA RID: 186
		private int _value;
	}
}
