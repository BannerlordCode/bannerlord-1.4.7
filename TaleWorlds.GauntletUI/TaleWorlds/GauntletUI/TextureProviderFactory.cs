using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000032 RID: 50
	public static class TextureProviderFactory
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public static TextureProvider CreateInstance(string textureProviderName)
		{
			Type type;
			if (TextureProviderFactory._textureProvidertypes.TryGetValue(textureProviderName, out type))
			{
				try
				{
					TextureProvider textureProvider;
					if ((textureProvider = Activator.CreateInstance(type) as TextureProvider) != null)
					{
						return textureProvider;
					}
				}
				catch
				{
				}
			}
			Debug.FailedAssert("Unable to create instance for texture provider with name: " + textureProviderName, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\TextureProviderFactory.cs", "CreateInstance", 36);
			return null;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000F154 File Offset: 0x0000D354
		public static void RefreshProviderTypes()
		{
			TextureProviderFactory._textureProvidertypes.Clear();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				List<Type> typesSafe = assemblies[i].GetTypesSafe(null);
				for (int j = 0; j < typesSafe.Count; j++)
				{
					Type type = typesSafe[j];
					if (type == null)
					{
						Debug.FailedAssert("(RefreshProviderTypes): Null type while iterating assemblies", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\TextureProviderFactory.cs", "RefreshProviderTypes", 53);
					}
					else if (typeof(TextureProvider).IsAssignableFrom(type) && !type.IsAbstract)
					{
						TextureProviderFactory._textureProvidertypes.Add(type.Name, type);
					}
				}
			}
		}

		// Token: 0x040001AA RID: 426
		private static Dictionary<string, Type> _textureProvidertypes = new Dictionary<string, Type>();
	}
}
