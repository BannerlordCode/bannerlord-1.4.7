using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000317 RID: 791
	public abstract class MPRandomOnSpawnPerkEffect : MPOnSpawnPerkEffectBase
	{
		// Token: 0x06002D2C RID: 11564 RVA: 0x000AF458 File Offset: 0x000AD658
		static MPRandomOnSpawnPerkEffect()
		{
			foreach (Type type in from t in PerkAssemblyCollection.GetPerkAssemblyTypes()
				where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MPRandomOnSpawnPerkEffect))
				select t)
			{
				FieldInfo field = type.GetField("StringType", BindingFlags.Static | BindingFlags.NonPublic);
				string text = (string)((field != null) ? field.GetValue(null) : null);
				MPRandomOnSpawnPerkEffect.Registered.Add(text, type);
			}
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000AF4E8 File Offset: 0x000AD6E8
		public static MPRandomOnSpawnPerkEffect CreateFrom(XmlNode node)
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
			MPRandomOnSpawnPerkEffect mprandomOnSpawnPerkEffect = (MPRandomOnSpawnPerkEffect)Activator.CreateInstance(MPRandomOnSpawnPerkEffect.Registered[text2], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
			mprandomOnSpawnPerkEffect.Deserialize(node);
			return mprandomOnSpawnPerkEffect;
		}

		// Token: 0x040011D0 RID: 4560
		protected static Dictionary<string, Type> Registered = new Dictionary<string, Type>();
	}
}
