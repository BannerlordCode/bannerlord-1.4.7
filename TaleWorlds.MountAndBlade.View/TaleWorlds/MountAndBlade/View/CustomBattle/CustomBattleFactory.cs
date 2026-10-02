using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.CustomBattle
{
	// Token: 0x020000AB RID: 171
	public static class CustomBattleFactory
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x0002A628 File Offset: 0x00028828
		public static void RegisterProvider<T>() where T : ICustomBattleProvider, new()
		{
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < CustomBattleFactory._providers.Count; i++)
			{
				if (CustomBattleFactory._providers[i].GetType() == typeFromHandle)
				{
					Debug.FailedAssert("Custom battle provider was already registered: " + typeFromHandle.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\CustomBattle\\CustomBattleFactory.cs", "RegisterProvider", 18);
					return;
				}
			}
			if (typeFromHandle.Name.ToLowerInvariant().Contains("naval"))
			{
				CustomBattleFactory._providers.Insert(0, typeFromHandle);
				return;
			}
			CustomBattleFactory._providers.Add(typeFromHandle);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0002A6BE File Offset: 0x000288BE
		public static void StartCustomBattleWithProvider<T>() where T : ICustomBattleProvider, new()
		{
			(Activator.CreateInstance(typeof(T)) as ICustomBattleProvider).StartCustomBattle();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0002A6D9 File Offset: 0x000288D9
		public static void StartCustomBattle()
		{
			if (CustomBattleFactory._providers.Count > 0)
			{
				(Activator.CreateInstance(CustomBattleFactory._providers[0]) as ICustomBattleProvider).StartCustomBattle();
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002A702 File Offset: 0x00028902
		public static int GetProviderCount()
		{
			return CustomBattleFactory._providers.Count;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002A710 File Offset: 0x00028910
		public static List<ICustomBattleProvider> CollectProviders()
		{
			List<ICustomBattleProvider> list = new List<ICustomBattleProvider>();
			for (int i = 0; i < CustomBattleFactory._providers.Count; i++)
			{
				ICustomBattleProvider customBattleProvider = Activator.CreateInstance(CustomBattleFactory._providers[i]) as ICustomBattleProvider;
				list.Add(customBattleProvider);
			}
			return list;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0002A758 File Offset: 0x00028958
		public static ICustomBattleProvider CollectNextProvider(Type currentProviderType)
		{
			int num = (CustomBattleFactory._providers.IndexOf(currentProviderType) + 1) % CustomBattleFactory._providers.Count;
			return Activator.CreateInstance(CustomBattleFactory._providers[num]) as ICustomBattleProvider;
		}

		// Token: 0x04000340 RID: 832
		private static readonly List<Type> _providers = new List<Type>();
	}
}
