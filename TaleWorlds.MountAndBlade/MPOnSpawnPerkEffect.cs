using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030D RID: 781
	public abstract class MPOnSpawnPerkEffect : MPOnSpawnPerkEffectBase
	{
		// Token: 0x06002CA6 RID: 11430 RVA: 0x000AC290 File Offset: 0x000AA490
		static MPOnSpawnPerkEffect()
		{
			foreach (Type type in from t in PerkAssemblyCollection.GetPerkAssemblyTypes()
				where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MPOnSpawnPerkEffect))
				select t)
			{
				FieldInfo field = type.GetField("StringType", BindingFlags.Static | BindingFlags.NonPublic);
				string text = (string)((field != null) ? field.GetValue(null) : null);
				MPOnSpawnPerkEffect.Registered.Add(text, type);
			}
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000AC320 File Offset: 0x000AA520
		public static MPOnSpawnPerkEffect CreateFrom(XmlNode node)
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
					XmlAttribute xmlAttribute = attributes["type"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			MPOnSpawnPerkEffect mponSpawnPerkEffect = (MPOnSpawnPerkEffect)Activator.CreateInstance(MPOnSpawnPerkEffect.Registered[text2], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
			mponSpawnPerkEffect.Deserialize(node);
			return mponSpawnPerkEffect;
		}

		// Token: 0x040011B7 RID: 4535
		protected static Dictionary<string, Type> Registered = new Dictionary<string, Type>();
	}
}
