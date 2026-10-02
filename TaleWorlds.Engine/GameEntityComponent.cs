using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004B RID: 75
	[EngineClass("rglEntity_component")]
	public abstract class GameEntityComponent : NativeObject
	{
		// Token: 0x060007E2 RID: 2018 RVA: 0x00005D12 File Offset: 0x00003F12
		internal GameEntityComponent(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00005D21 File Offset: 0x00003F21
		public WeakGameEntity GetEntity()
		{
			return new WeakGameEntity(EngineApplicationInterface.IGameEntityComponent.GetEntityPointer(base.Pointer));
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00005D38 File Offset: 0x00003F38
		public virtual MetaMesh GetFirstMetaMesh()
		{
			return EngineApplicationInterface.IGameEntityComponent.GetFirstMetaMesh(this);
		}
	}
}
