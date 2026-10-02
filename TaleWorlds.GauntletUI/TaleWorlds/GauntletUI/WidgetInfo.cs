using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003C RID: 60
	public class WidgetInfo
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000FEC1 File Offset: 0x0000E0C1
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x0000FEC9 File Offset: 0x0000E0C9
		public string Name { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000FED2 File Offset: 0x0000E0D2
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x0000FEDA File Offset: 0x0000E0DA
		public Type Type { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000FEE3 File Offset: 0x0000E0E3
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x0000FEEB File Offset: 0x0000E0EB
		public bool GotCustomUpdate { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000FEF4 File Offset: 0x0000E0F4
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x0000FEFC File Offset: 0x0000E0FC
		public bool GotCustomLateUpdate { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000FF05 File Offset: 0x0000E105
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0000FF0D File Offset: 0x0000E10D
		public bool GotCustomParallelUpdate { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0000FF16 File Offset: 0x0000E116
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0000FF1E File Offset: 0x0000E11E
		public bool GotUpdateBrushes { get; private set; }

		// Token: 0x060003FF RID: 1023 RVA: 0x0000FF28 File Offset: 0x0000E128
		public WidgetInfo(Type type)
		{
			this.Name = type.Name;
			this.Type = type;
			this.GotCustomUpdate = this.IsMethodOverridden("OnUpdate");
			this.GotCustomLateUpdate = this.IsMethodOverridden("OnLateUpdate");
			this.GotCustomParallelUpdate = this.IsMethodOverridden("OnParallelUpdate");
			this.GotUpdateBrushes = this.IsMethodOverridden("UpdateBrushes");
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000FF92 File Offset: 0x0000E192
		public static void Refresh()
		{
			WidgetInfo._widgetInfos = new Dictionary<Type, WidgetInfo>();
			WidgetInfo.CollectWidgetTypes();
			TextureProviderFactory.RefreshProviderTypes();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000FFA8 File Offset: 0x0000E1A8
		public static WidgetInfo GetWidgetInfo(Type type)
		{
			return WidgetInfo._widgetInfos[type];
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000FFB5 File Offset: 0x0000E1B5
		public static WidgetInfo[] GetWidgetInfos()
		{
			return WidgetInfo._widgetInfos.Values.ToArray<WidgetInfo>();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		private static bool CheckAssemblyReferencesThis(Assembly assembly)
		{
			Assembly assembly2 = typeof(Widget).Assembly;
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].Name == assembly2.GetName().Name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00010018 File Offset: 0x0000E218
		private static void CollectWidgetTypes()
		{
			new List<Type>();
			Assembly assembly = typeof(Widget).Assembly;
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (WidgetInfo.CheckAssemblyReferencesThis(assembly2) || assembly2 == assembly)
				{
					foreach (Type type in assembly2.GetTypesSafe(null))
					{
						if (typeof(Widget).IsAssignableFrom(type))
						{
							WidgetInfo._widgetInfos.Add(type, new WidgetInfo(type));
						}
					}
				}
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000100D4 File Offset: 0x0000E2D4
		private bool IsMethodOverridden(string methodName)
		{
			MethodInfo method = this.Type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag;
			if (method == null)
			{
				flag = false;
			}
			else
			{
				Type type = this.Type;
				Type type2 = this.Type;
				while (type2 != null)
				{
					if (type2.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
					{
						type = type2;
					}
					type2 = type2.BaseType;
				}
				flag = method.DeclaringType != type;
			}
			return flag;
		}

		// Token: 0x040001F1 RID: 497
		private static Dictionary<Type, WidgetInfo> _widgetInfos;
	}
}
