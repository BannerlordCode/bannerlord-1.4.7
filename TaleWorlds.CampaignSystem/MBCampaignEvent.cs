using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003B RID: 59
	public class MBCampaignEvent
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0001E8AE File Offset: 0x0001CAAE
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0001E8B6 File Offset: 0x0001CAB6
		public CampaignTime TriggerPeriod { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001E8BF File Offset: 0x0001CABF
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0001E8C7 File Offset: 0x0001CAC7
		public CampaignTime InitialWait { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001E8D0 File Offset: 0x0001CAD0
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x0001E8D8 File Offset: 0x0001CAD8
		public bool isEventDeleted { get; set; }

		// Token: 0x060003E5 RID: 997 RVA: 0x0001E8E1 File Offset: 0x0001CAE1
		public MBCampaignEvent(string eventName)
		{
			this.description = eventName;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001E8FB File Offset: 0x0001CAFB
		public MBCampaignEvent(CampaignTime triggerPeriod, CampaignTime initialWait)
		{
			this.TriggerPeriod = triggerPeriod;
			this.InitialWait = initialWait;
			this.NextTriggerTime = CampaignTime.Now + this.InitialWait;
			this.isEventDeleted = false;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001E939 File Offset: 0x0001CB39
		public void AddHandler(MBCampaignEvent.CampaignEventDelegate gameEventDelegate)
		{
			this.handlers.Add(gameEventDelegate);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001E948 File Offset: 0x0001CB48
		public void RunHandlers(params object[] delegateParams)
		{
			for (int i = 0; i < this.handlers.Count; i++)
			{
				this.handlers[i](this, delegateParams);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001E980 File Offset: 0x0001CB80
		public void Unregister(object instance)
		{
			for (int i = 0; i < this.handlers.Count; i++)
			{
				if (this.handlers[i].Target == instance)
				{
					this.handlers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
		public void CheckUpdate()
		{
			while (this.NextTriggerTime.IsPast && !this.isEventDeleted)
			{
				this.RunHandlers(new object[] { CampaignTime.Now });
				this.NextTriggerTime += this.TriggerPeriod;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001EA1C File Offset: 0x0001CC1C
		public void DeletePeriodicEvent()
		{
			this.isEventDeleted = true;
		}

		// Token: 0x04000185 RID: 389
		public string description;

		// Token: 0x04000186 RID: 390
		protected List<MBCampaignEvent.CampaignEventDelegate> handlers = new List<MBCampaignEvent.CampaignEventDelegate>();

		// Token: 0x04000187 RID: 391
		[CachedData]
		protected CampaignTime NextTriggerTime;

		// Token: 0x02000507 RID: 1287
		// (Invoke) Token: 0x06004BE3 RID: 19427
		public delegate void CampaignEventDelegate(MBCampaignEvent campaignEvent, params object[] delegateParams);
	}
}
