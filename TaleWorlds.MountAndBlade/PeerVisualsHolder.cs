using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CA RID: 714
	public class PeerVisualsHolder
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x0009AD90 File Offset: 0x00098F90
		// (set) Token: 0x06002940 RID: 10560 RVA: 0x0009AD98 File Offset: 0x00098F98
		public MissionPeer Peer { get; private set; }

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002941 RID: 10561 RVA: 0x0009ADA1 File Offset: 0x00098FA1
		// (set) Token: 0x06002942 RID: 10562 RVA: 0x0009ADA9 File Offset: 0x00098FA9
		public int VisualsIndex { get; private set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002943 RID: 10563 RVA: 0x0009ADB2 File Offset: 0x00098FB2
		// (set) Token: 0x06002944 RID: 10564 RVA: 0x0009ADBA File Offset: 0x00098FBA
		public IAgentVisual AgentVisuals { get; private set; }

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002945 RID: 10565 RVA: 0x0009ADC3 File Offset: 0x00098FC3
		// (set) Token: 0x06002946 RID: 10566 RVA: 0x0009ADCB File Offset: 0x00098FCB
		public IAgentVisual MountAgentVisuals { get; private set; }

		// Token: 0x06002947 RID: 10567 RVA: 0x0009ADD4 File Offset: 0x00098FD4
		public PeerVisualsHolder(MissionPeer peer, int index, IAgentVisual agentVisuals, IAgentVisual mountVisuals)
		{
			this.Peer = peer;
			this.VisualsIndex = index;
			this.AgentVisuals = agentVisuals;
			this.MountAgentVisuals = mountVisuals;
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x0009ADF9 File Offset: 0x00098FF9
		public void SetMountVisuals(IAgentVisual mountAgentVisuals)
		{
			this.MountAgentVisuals = mountAgentVisuals;
		}
	}
}
