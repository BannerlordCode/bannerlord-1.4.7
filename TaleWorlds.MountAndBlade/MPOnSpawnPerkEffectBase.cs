using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030F RID: 783
	public abstract class MPOnSpawnPerkEffectBase : MPPerkEffectBase, IOnSpawnPerkEffect
	{
		// Token: 0x06002CAD RID: 11437 RVA: 0x000AC384 File Offset: 0x000AA584
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
					XmlAttribute xmlAttribute2 = attributes2["target"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			this.EffectTarget = MPOnSpawnPerkEffectBase.Target.Any;
			if (text4 != null && !Enum.TryParse<MPOnSpawnPerkEffectBase.Target>(text4, true, out this.EffectTarget))
			{
				this.EffectTarget = MPOnSpawnPerkEffectBase.Target.Any;
				Debug.FailedAssert("provided 'target' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\Perks\\MPOnSpawnPerkEffectBase.cs", "Deserialize", 38);
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000AC437 File Offset: 0x000AA637
		public virtual float GetTroopCountMultiplier()
		{
			return 0f;
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000AC43E File Offset: 0x000AA63E
		public virtual int GetExtraTroopCount()
		{
			return 0;
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000AC441 File Offset: 0x000AA641
		public virtual List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAll = false)
		{
			return alternativeEquipments;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000AC444 File Offset: 0x000AA644
		public virtual float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			return 0f;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000AC44B File Offset: 0x000AA64B
		public virtual float GetHitpoints(bool isPlayer)
		{
			return 0f;
		}

		// Token: 0x040011B8 RID: 4536
		protected MPOnSpawnPerkEffectBase.Target EffectTarget;

		// Token: 0x020005F7 RID: 1527
		protected enum Target
		{
			// Token: 0x0400200F RID: 8207
			Player,
			// Token: 0x04002010 RID: 8208
			Troops,
			// Token: 0x04002011 RID: 8209
			Any
		}
	}
}
