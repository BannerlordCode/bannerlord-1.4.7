using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006E RID: 110
	public abstract class GameManagerComponent : IEntityComponent
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0001A330 File Offset: 0x00018530
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0001A338 File Offset: 0x00018538
		public GameManagerBase GameManager { get; internal set; }

		// Token: 0x060007DB RID: 2011 RVA: 0x0001A341 File Offset: 0x00018541
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001A349 File Offset: 0x00018549
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001A34B File Offset: 0x0001854B
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001A353 File Offset: 0x00018553
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001A355 File Offset: 0x00018555
		protected internal virtual void OnTick()
		{
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001A357 File Offset: 0x00018557
		protected internal virtual void OnPlayerDisconnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001A359 File Offset: 0x00018559
		protected internal virtual void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001A35B File Offset: 0x0001855B
		protected internal virtual void OnPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001A35D File Offset: 0x0001855D
		protected internal virtual void OnGameNetworkBegin()
		{
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001A35F File Offset: 0x0001855F
		protected internal virtual void OnGameNetworkEnd()
		{
		}
	}
}
