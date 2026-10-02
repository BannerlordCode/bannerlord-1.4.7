using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005F RID: 95
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class HasTableauCache : Attribute
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00008C50 File Offset: 0x00006E50
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x00008C58 File Offset: 0x00006E58
		public Type TableauCacheType { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00008C61 File Offset: 0x00006E61
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x00008C69 File Offset: 0x00006E69
		public Type MaterialCacheIDGetType { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00008C72 File Offset: 0x00006E72
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x00008C79 File Offset: 0x00006E79
		internal static Dictionary<Type, MaterialCacheIDGetMethodDelegate> TableauCacheTypes { get; private set; }

		// Token: 0x06000964 RID: 2404 RVA: 0x00008C81 File Offset: 0x00006E81
		public HasTableauCache(Type tableauCacheType, Type materialCacheIDGetType)
		{
			this.TableauCacheType = tableauCacheType;
			this.MaterialCacheIDGetType = materialCacheIDGetType;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00008C98 File Offset: 0x00006E98
		public static void CollectTableauCacheTypes()
		{
			HasTableauCache.TableauCacheTypes = new Dictionary<Type, MaterialCacheIDGetMethodDelegate>();
			HasTableauCache.CollectTableauCacheTypesFrom(typeof(HasTableauCache).Assembly);
			Assembly[] referencingAssembliesSafe = typeof(HasTableauCache).Assembly.GetReferencingAssembliesSafe(null);
			for (int i = 0; i < referencingAssembliesSafe.Length; i++)
			{
				HasTableauCache.CollectTableauCacheTypesFrom(referencingAssembliesSafe[i]);
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00008CF0 File Offset: 0x00006EF0
		private static void CollectTableauCacheTypesFrom(Assembly assembly)
		{
			object[] customAttributesSafe = assembly.GetCustomAttributesSafe(typeof(HasTableauCache), true);
			if (customAttributesSafe.Length != 0)
			{
				foreach (HasTableauCache hasTableauCache in customAttributesSafe)
				{
					MethodInfo method = hasTableauCache.MaterialCacheIDGetType.GetMethod("GetMaterialCacheID", BindingFlags.Static | BindingFlags.Public);
					MaterialCacheIDGetMethodDelegate materialCacheIDGetMethodDelegate = (MaterialCacheIDGetMethodDelegate)Delegate.CreateDelegate(typeof(MaterialCacheIDGetMethodDelegate), method);
					HasTableauCache.TableauCacheTypes.Add(hasTableauCache.TableauCacheType, materialCacheIDGetMethodDelegate);
				}
			}
		}
	}
}
