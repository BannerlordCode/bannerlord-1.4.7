using System;
using NetworkMessages.FromServer;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000012 RID: 18
	public class MultiplayerMissionServerStatusVM : ViewModel
	{
		// Token: 0x060000FF RID: 255 RVA: 0x0000572B File Offset: 0x0000392B
		public void UpdatePacketLossRatio(float v)
		{
			if (v >= 0.02f)
			{
				this.PacketLossState = 2;
				return;
			}
			if (v >= 0.01f)
			{
				this.PacketLossState = 1;
				return;
			}
			this.PacketLossState = 0;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005754 File Offset: 0x00003954
		public void UpdatePeerPing(double averagePingInMilliseconds)
		{
			if (averagePingInMilliseconds >= 110.0)
			{
				this.PingState = 2;
				return;
			}
			if (averagePingInMilliseconds >= 90.0)
			{
				this.PingState = 1;
				return;
			}
			this.PingState = 0;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005785 File Offset: 0x00003985
		public void UpdateServerPerformanceState(ServerPerformanceState serverPerformanceState)
		{
			switch (serverPerformanceState)
			{
			default:
				this.ServerPerformanceState = 0;
				return;
			case NetworkMessages.FromServer.ServerPerformanceState.Medium:
				this.ServerPerformanceState = 1;
				return;
			case NetworkMessages.FromServer.ServerPerformanceState.Low:
			case NetworkMessages.FromServer.ServerPerformanceState.Count:
				this.ServerPerformanceState = 2;
				return;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000057B4 File Offset: 0x000039B4
		public void ResetStates()
		{
			this.PacketLossState = 0;
			this.PingState = 0;
			this.ServerPerformanceState = 0;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000057CB File Offset: 0x000039CB
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000057D3 File Offset: 0x000039D3
		[DataSourceProperty]
		public int PacketLossState
		{
			get
			{
				return this._packetLossState;
			}
			set
			{
				if (value != this._packetLossState)
				{
					this._packetLossState = value;
					base.OnPropertyChangedWithValue(value, "PacketLossState");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000057F1 File Offset: 0x000039F1
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000057F9 File Offset: 0x000039F9
		[DataSourceProperty]
		public int PingState
		{
			get
			{
				return this._pingState;
			}
			set
			{
				if (value != this._pingState)
				{
					this._pingState = value;
					base.OnPropertyChangedWithValue(value, "PingState");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005817 File Offset: 0x00003A17
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000581F File Offset: 0x00003A1F
		[DataSourceProperty]
		public int ServerPerformanceState
		{
			get
			{
				return this._serverPerformanceState;
			}
			set
			{
				if (value != this._serverPerformanceState)
				{
					this._serverPerformanceState = value;
					base.OnPropertyChangedWithValue(value, "ServerPerformanceState");
				}
			}
		}

		// Token: 0x04000092 RID: 146
		private int _packetLossState;

		// Token: 0x04000093 RID: 147
		private int _pingState;

		// Token: 0x04000094 RID: 148
		private int _serverPerformanceState;

		// Token: 0x020000BE RID: 190
		private enum StatusTypes
		{
			// Token: 0x040007E1 RID: 2017
			Good,
			// Token: 0x040007E2 RID: 2018
			Average,
			// Token: 0x040007E3 RID: 2019
			Poor
		}
	}
}
