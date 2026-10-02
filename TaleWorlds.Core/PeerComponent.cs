using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C3 RID: 195
	public abstract class PeerComponent : IEntityComponent
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00023048 File Offset: 0x00021248
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00023050 File Offset: 0x00021250
		public VirtualPlayer Peer
		{
			get
			{
				return this._peer;
			}
			set
			{
				this._peer = value;
			}
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00023059 File Offset: 0x00021259
		public virtual void Initialize()
		{
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0002305B File Offset: 0x0002125B
		public string Name
		{
			get
			{
				return this.Peer.UserName;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00023068 File Offset: 0x00021268
		public bool IsMine
		{
			get
			{
				return this.Peer.IsMine;
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00023075 File Offset: 0x00021275
		public T GetComponent<T>() where T : PeerComponent
		{
			return this.Peer.GetComponent<T>();
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00023082 File Offset: 0x00021282
		public virtual void OnInitialize()
		{
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00023084 File Offset: 0x00021284
		public virtual void OnFinalize()
		{
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00023086 File Offset: 0x00021286
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x0002308E File Offset: 0x0002128E
		public uint TypeId { get; set; }

		// Token: 0x040005FD RID: 1533
		private VirtualPlayer _peer;
	}
}
