using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003F RID: 63
	public class ShieldDamageEffect : MPPerkEffect
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000A284 File Offset: 0x00008484
		protected ShieldDamageEffect()
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000A28C File Offset: 0x0000848C
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
			if (text4 == null || !float.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ShieldDamageEffect.cs", "Deserialize", 31);
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
					XmlAttribute xmlAttribute3 = attributes3["block_type"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			this._blockType = ShieldDamageEffect.BlockType.Any;
			if (text6 != null && !Enum.TryParse<ShieldDamageEffect.BlockType>(text6, true, out this._blockType))
			{
				this._blockType = ShieldDamageEffect.BlockType.Any;
				Debug.FailedAssert("provided 'block_type' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ShieldDamageEffect.cs", "Deserialize", 39);
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000A390 File Offset: 0x00008590
		public override float GetShieldDamage(bool isCorrectSideBlock)
		{
			switch (this._blockType)
			{
			case ShieldDamageEffect.BlockType.Any:
				return this._value;
			case ShieldDamageEffect.BlockType.CorrectSide:
				if (!isCorrectSideBlock)
				{
					return 0f;
				}
				return this._value;
			case ShieldDamageEffect.BlockType.WrongSide:
				if (!isCorrectSideBlock)
				{
					return this._value;
				}
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x040000AD RID: 173
		protected static string StringType = "ShieldDamage";

		// Token: 0x040000AE RID: 174
		private float _value;

		// Token: 0x040000AF RID: 175
		private ShieldDamageEffect.BlockType _blockType;

		// Token: 0x020000A8 RID: 168
		private enum BlockType
		{
			// Token: 0x040001BB RID: 443
			Any,
			// Token: 0x040001BC RID: 444
			CorrectSide,
			// Token: 0x040001BD RID: 445
			WrongSide
		}
	}
}
