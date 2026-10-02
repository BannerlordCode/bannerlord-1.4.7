using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x0200002F RID: 47
	public static class MissionNameMarkerFactory
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003BF RID: 959 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
		// (remove) Token: 0x060003C0 RID: 960 RVA: 0x0000FE24 File Offset: 0x0000E024
		public static event Action OnProvidersChanged;

		// Token: 0x060003C2 RID: 962 RVA: 0x0000FE8C File Offset: 0x0000E08C
		public static MissionNameMarkerFactory.INameMarkerProviderContext PushContext(string name, bool addDefaultProviders)
		{
			MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext = new MissionNameMarkerFactory.NameMarkerProviderContext(false, name, new Action(MissionNameMarkerFactory.FireProvidersChangedEvent));
			if (addDefaultProviders)
			{
				MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext2 = MissionNameMarkerFactory.DefaultContext as MissionNameMarkerFactory.NameMarkerProviderContext;
				for (int i = 0; i < nameMarkerProviderContext2.ProviderTypes.Count; i++)
				{
					nameMarkerProviderContext.AddProvider(nameMarkerProviderContext2.ProviderTypes[i]);
				}
			}
			MissionNameMarkerFactory._registeredContexts.Add(nameMarkerProviderContext);
			return nameMarkerProviderContext;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public static void PopContext(string contextId)
		{
			for (int i = 0; i < MissionNameMarkerFactory._registeredContexts.Count; i++)
			{
				if (MissionNameMarkerFactory._registeredContexts[i].Id == contextId)
				{
					MissionNameMarkerFactory.PopContext(MissionNameMarkerFactory._registeredContexts[i]);
					return;
				}
			}
			Debug.FailedAssert("Trying to pop a name marker context that was not pushed: " + contextId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "PopContext", 54);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000FF57 File Offset: 0x0000E157
		public static void PopContext(MissionNameMarkerFactory.INameMarkerProviderContext context)
		{
			if (context.IsDefaultContext)
			{
				Debug.FailedAssert("Default name marker context cannot be removed", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "PopContext", 61);
				return;
			}
			MissionNameMarkerFactory._registeredContexts.Remove(context);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000FF84 File Offset: 0x0000E184
		private static void FireProvidersChangedEvent()
		{
			Action onProvidersChanged = MissionNameMarkerFactory.OnProvidersChanged;
			if (onProvidersChanged == null)
			{
				return;
			}
			onProvidersChanged();
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000FF98 File Offset: 0x0000E198
		public static List<MissionNameMarkerProvider> CollectProviders()
		{
			List<MissionNameMarkerProvider> list = new List<MissionNameMarkerProvider>();
			MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext = MissionNameMarkerFactory._registeredContexts[MissionNameMarkerFactory._registeredContexts.Count - 1] as MissionNameMarkerFactory.NameMarkerProviderContext;
			for (int i = 0; i < nameMarkerProviderContext.ProviderTypes.Count; i++)
			{
				MissionNameMarkerProvider missionNameMarkerProvider = Activator.CreateInstance(nameMarkerProviderContext.ProviderTypes[i]) as MissionNameMarkerProvider;
				list.Add(missionNameMarkerProvider);
			}
			return list;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		public static void UpdateProviders(MissionNameMarkerProvider[] existingProviders, out List<MissionNameMarkerProvider> addedProviders, out List<MissionNameMarkerProvider> removedProviders)
		{
			addedProviders = new List<MissionNameMarkerProvider>();
			removedProviders = new List<MissionNameMarkerProvider>();
			MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext = MissionNameMarkerFactory._registeredContexts[MissionNameMarkerFactory._registeredContexts.Count - 1] as MissionNameMarkerFactory.NameMarkerProviderContext;
			for (int i = 0; i < existingProviders.Length; i++)
			{
				bool flag = true;
				MissionNameMarkerProvider missionNameMarkerProvider = existingProviders[i];
				for (int j = 0; j < nameMarkerProviderContext.ProviderTypes.Count; j++)
				{
					Type type = nameMarkerProviderContext.ProviderTypes[j];
					if (missionNameMarkerProvider.GetType() == type)
					{
						flag = false;
					}
				}
				if (flag)
				{
					removedProviders.Add(missionNameMarkerProvider);
				}
			}
			for (int k = 0; k < nameMarkerProviderContext.ProviderTypes.Count; k++)
			{
				bool flag2 = true;
				Type type2 = nameMarkerProviderContext.ProviderTypes[k];
				for (int l = 0; l < existingProviders.Length; l++)
				{
					if (existingProviders[l].GetType() == type2)
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					MissionNameMarkerProvider missionNameMarkerProvider2 = Activator.CreateInstance(type2) as MissionNameMarkerProvider;
					addedProviders.Add(missionNameMarkerProvider2);
				}
			}
		}

		// Token: 0x040001E8 RID: 488
		public static readonly MissionNameMarkerFactory.INameMarkerProviderContext DefaultContext = new MissionNameMarkerFactory.NameMarkerProviderContext(true, "DefaultNameMarkerContext", new Action(MissionNameMarkerFactory.FireProvidersChangedEvent));

		// Token: 0x040001EA RID: 490
		private static List<MissionNameMarkerFactory.INameMarkerProviderContext> _registeredContexts = new List<MissionNameMarkerFactory.INameMarkerProviderContext> { MissionNameMarkerFactory.DefaultContext };

		// Token: 0x0200009B RID: 155
		public interface INameMarkerProviderContext
		{
			// Token: 0x170001CB RID: 459
			// (get) Token: 0x06000698 RID: 1688
			string Id { get; }

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x06000699 RID: 1689
			bool IsDefaultContext { get; }

			// Token: 0x0600069A RID: 1690
			void AddProvider<T>() where T : MissionNameMarkerProvider, new();

			// Token: 0x0600069B RID: 1691
			void RemoveProvider<T>() where T : MissionNameMarkerProvider, new();
		}

		// Token: 0x0200009C RID: 156
		private class NameMarkerProviderContext : MissionNameMarkerFactory.INameMarkerProviderContext
		{
			// Token: 0x170001CD RID: 461
			// (get) Token: 0x0600069C RID: 1692 RVA: 0x00016E6A File Offset: 0x0001506A
			// (set) Token: 0x0600069D RID: 1693 RVA: 0x00016E72 File Offset: 0x00015072
			public string Id { get; private set; }

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x0600069E RID: 1694 RVA: 0x00016E7B File Offset: 0x0001507B
			// (set) Token: 0x0600069F RID: 1695 RVA: 0x00016E83 File Offset: 0x00015083
			public bool IsDefaultContext { get; private set; }

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00016E8C File Offset: 0x0001508C
			// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00016E94 File Offset: 0x00015094
			public List<Type> ProviderTypes { get; private set; }

			// Token: 0x060006A2 RID: 1698 RVA: 0x00016E9D File Offset: 0x0001509D
			public NameMarkerProviderContext(bool isDefault, string id, Action onProvidersChanged)
			{
				this._onProvidersChanged = onProvidersChanged;
				this.IsDefaultContext = isDefault;
				this.Id = id;
				this.ProviderTypes = new List<Type>();
			}

			// Token: 0x060006A3 RID: 1699 RVA: 0x00016EC5 File Offset: 0x000150C5
			public void AddProvider<T>() where T : MissionNameMarkerProvider, new()
			{
				this.AddProvider(typeof(T));
			}

			// Token: 0x060006A4 RID: 1700 RVA: 0x00016ED7 File Offset: 0x000150D7
			public void RemoveProvider<T>() where T : MissionNameMarkerProvider, new()
			{
				this.RemoveProvider(typeof(T));
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x00016EEC File Offset: 0x000150EC
			public void AddProvider(Type tProvider)
			{
				for (int i = 0; i < this.ProviderTypes.Count; i++)
				{
					if (this.ProviderTypes[i] == tProvider)
					{
						Debug.FailedAssert("Provider of type: " + tProvider.Name + " was already added to name marker context: " + this.Id, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "AddProvider", 182);
						return;
					}
				}
				this.ProviderTypes.Add(tProvider);
				Action onProvidersChanged = this._onProvidersChanged;
				if (onProvidersChanged == null)
				{
					return;
				}
				onProvidersChanged();
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x00016F70 File Offset: 0x00015170
			public void RemoveProvider(Type tProvider)
			{
				int i = 0;
				while (i < this.ProviderTypes.Count)
				{
					if (this.ProviderTypes[i] == tProvider)
					{
						this.ProviderTypes.Remove(tProvider);
						Action onProvidersChanged = this._onProvidersChanged;
						if (onProvidersChanged == null)
						{
							return;
						}
						onProvidersChanged();
						return;
					}
					else
					{
						i++;
					}
				}
				Debug.FailedAssert("Provider of type: " + tProvider.Name + " was not added to name marker context: " + this.Id, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "RemoveProvider", 203);
			}

			// Token: 0x040003B4 RID: 948
			private Action _onProvidersChanged;
		}
	}
}
