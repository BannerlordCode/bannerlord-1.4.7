using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000034 RID: 52
	public class HitpointsEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x06000207 RID: 519 RVA: 0x00009526 File Offset: 0x00007726
		protected HitpointsEffect()
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009530 File Offset: 0x00007730
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
			if (text2 == null || !float.TryParse(text2, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\HitpointsEffect.cs", "Deserialize", 20);
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009595 File Offset: 0x00007795
		public override float GetHitpoints(bool isPlayer)
		{
			if (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Any || (isPlayer ? (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Player) : (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Troops)))
			{
				return this._value;
			}
			return 0f;
		}

		// Token: 0x04000094 RID: 148
		protected static string StringType = "HitpointsOnSpawn";

		// Token: 0x04000095 RID: 149
		private float _value;
	}
}
