using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core
{
	// Token: 0x020000DD RID: 221
	public class VirtualPlayer
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00024D06 File Offset: 0x00022F06
		public static Dictionary<Type, object> PeerComponents
		{
			get
			{
				return VirtualPlayer._peerComponents;
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00024D0D File Offset: 0x00022F0D
		static VirtualPlayer()
		{
			VirtualPlayer.FindPeerComponents();
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00024D20 File Offset: 0x00022F20
		private static void FindPeerComponents()
		{
			Debug.Print("Searching Peer Components", 0, Debug.DebugColor.White, 17592186044416UL);
			VirtualPlayer._peerComponentIds = new Dictionary<Type, uint>();
			VirtualPlayer._peerComponentTypes = new Dictionary<uint, Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (VirtualPlayer.CheckAssemblyForPeerComponent(assembly))
				{
					List<Type> typesSafe = assembly.GetTypesSafe(null);
					list.AddRange(typesSafe.Where<Type>((Type q) => typeof(PeerComponent).IsAssignableFrom(q) && typeof(PeerComponent) != q));
				}
			}
			foreach (Type type in list)
			{
				uint djb = (uint)Common.GetDJB2(type.Name);
				VirtualPlayer._peerComponentIds.Add(type, djb);
				VirtualPlayer._peerComponentTypes.Add(djb, type);
			}
			Debug.Print("Found " + list.Count + " peer components", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00024E48 File Offset: 0x00023048
		private static bool CheckAssemblyForPeerComponent(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(PeerComponent));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00024E9D File Offset: 0x0002309D
		private static void EnsurePeerTypeList<T>() where T : PeerComponent
		{
			if (!VirtualPlayer._peerComponents.ContainsKey(typeof(T)))
			{
				VirtualPlayer._peerComponents.Add(typeof(T), new List<T>());
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00024ED0 File Offset: 0x000230D0
		private static void EnsurePeerTypeList(Type type)
		{
			if (!VirtualPlayer._peerComponents.ContainsKey(type))
			{
				IList list = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type })) as IList;
				VirtualPlayer._peerComponents.Add(type, list);
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00024F1A File Offset: 0x0002311A
		public static List<T> Peers<T>() where T : PeerComponent
		{
			VirtualPlayer.EnsurePeerTypeList<T>();
			return VirtualPlayer._peerComponents[typeof(T)] as List<T>;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00024F3A File Offset: 0x0002313A
		public static void Reset()
		{
			VirtualPlayer._peerComponents = new Dictionary<Type, object>();
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00024F46 File Offset: 0x00023146
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x00024F61 File Offset: 0x00023161
		public string BannerCode
		{
			get
			{
				if (this._bannerCode == null)
				{
					this._bannerCode = "11.8.1.4345.4345.770.774.1.0.0.133.7.5.512.512.784.769.1.0.0";
				}
				return this._bannerCode;
			}
			set
			{
				this._bannerCode = value;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00024F6A File Offset: 0x0002316A
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x00024F72 File Offset: 0x00023172
		public BodyProperties BodyProperties { get; set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x00024F7B File Offset: 0x0002317B
		// (set) Token: 0x06000B5B RID: 2907 RVA: 0x00024F83 File Offset: 0x00023183
		public int Race { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00024F8C File Offset: 0x0002318C
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x00024F94 File Offset: 0x00023194
		public bool IsFemale { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00024F9D File Offset: 0x0002319D
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x00024FA5 File Offset: 0x000231A5
		public PlayerId Id { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00024FAE File Offset: 0x000231AE
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x00024FB6 File Offset: 0x000231B6
		public int Index { get; private set; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00024FBF File Offset: 0x000231BF
		public bool IsMine
		{
			get
			{
				return MBNetwork.MyPeer == this;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00024FC9 File Offset: 0x000231C9
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x00024FD1 File Offset: 0x000231D1
		public string UserName { get; private set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00024FDA File Offset: 0x000231DA
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x00024FE2 File Offset: 0x000231E2
		public int ChosenBadgeIndex { get; set; }

		// Token: 0x06000B67 RID: 2919 RVA: 0x00024FEB File Offset: 0x000231EB
		public VirtualPlayer(int index, string name, PlayerId playerID, ICommunicator communicator)
		{
			this._peerEntitySystem = new EntitySystem<PeerComponent>();
			this.UserName = name;
			this.Index = index;
			this.Id = playerID;
			this.Communicator = communicator;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002501C File Offset: 0x0002321C
		public T AddComponent<T>() where T : PeerComponent, new()
		{
			T t = this._peerEntitySystem.AddComponent<T>();
			t.Peer = this;
			t.TypeId = VirtualPlayer._peerComponentIds[typeof(T)];
			VirtualPlayer.EnsurePeerTypeList<T>();
			(VirtualPlayer._peerComponents[typeof(T)] as List<T>).Add(t);
			this.Communicator.OnAddComponent(t);
			t.Initialize();
			return t;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000250A4 File Offset: 0x000232A4
		public PeerComponent AddComponent(Type peerComponentType)
		{
			PeerComponent peerComponent = this._peerEntitySystem.AddComponent(peerComponentType);
			peerComponent.Peer = this;
			peerComponent.TypeId = VirtualPlayer._peerComponentIds[peerComponentType];
			VirtualPlayer.EnsurePeerTypeList(peerComponentType);
			(VirtualPlayer._peerComponents[peerComponentType] as IList).Add(peerComponent);
			this.Communicator.OnAddComponent(peerComponent);
			peerComponent.Initialize();
			return peerComponent;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00025106 File Offset: 0x00023306
		public PeerComponent AddComponent(uint componentId)
		{
			return this.AddComponent(VirtualPlayer._peerComponentTypes[componentId]);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00025119 File Offset: 0x00023319
		public PeerComponent GetComponent(uint componentId)
		{
			return this.GetComponent(VirtualPlayer._peerComponentTypes[componentId]);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002512C File Offset: 0x0002332C
		public T GetComponent<T>() where T : PeerComponent
		{
			return this._peerEntitySystem.GetComponent<T>();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00025139 File Offset: 0x00023339
		public PeerComponent GetComponent(Type peerComponentType)
		{
			return this._peerEntitySystem.GetComponent(peerComponentType);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00025148 File Offset: 0x00023348
		public void RemoveComponent<T>(bool synched = true) where T : PeerComponent
		{
			T component = this._peerEntitySystem.GetComponent<T>();
			if (component != null)
			{
				this._peerEntitySystem.RemoveComponent(component);
				(VirtualPlayer._peerComponents[typeof(T)] as List<T>).Remove(component);
				if (synched)
				{
					this.Communicator.OnRemoveComponent(component);
				}
			}
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x000251AE File Offset: 0x000233AE
		public void RemoveComponent(PeerComponent component)
		{
			this._peerEntitySystem.RemoveComponent(component);
			(VirtualPlayer._peerComponents[component.GetType()] as IList).Remove(component);
			this.Communicator.OnRemoveComponent(component);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000251E3 File Offset: 0x000233E3
		public void OnDisconnect()
		{
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x000251E8 File Offset: 0x000233E8
		public void SynchronizeComponentsTo(VirtualPlayer peer)
		{
			foreach (PeerComponent peerComponent in this._peerEntitySystem.Components)
			{
				this.Communicator.OnSynchronizeComponentTo(peer, peerComponent);
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00025248 File Offset: 0x00023448
		public void UpdateIndexForReconnectingPlayer(int playerIndex)
		{
			this.Index = playerIndex;
		}

		// Token: 0x0400067A RID: 1658
		private const string DefaultPlayerBannerCode = "11.8.1.4345.4345.770.774.1.0.0.133.7.5.512.512.784.769.1.0.0";

		// Token: 0x0400067B RID: 1659
		private static Dictionary<Type, object> _peerComponents = new Dictionary<Type, object>();

		// Token: 0x0400067C RID: 1660
		private static Dictionary<Type, uint> _peerComponentIds;

		// Token: 0x0400067D RID: 1661
		private static Dictionary<uint, Type> _peerComponentTypes;

		// Token: 0x0400067E RID: 1662
		private string _bannerCode;

		// Token: 0x04000683 RID: 1667
		public readonly ICommunicator Communicator;

		// Token: 0x04000684 RID: 1668
		private EntitySystem<PeerComponent> _peerEntitySystem;

		// Token: 0x04000688 RID: 1672
		public Dictionary<int, List<int>> UsedCosmetics;
	}
}
