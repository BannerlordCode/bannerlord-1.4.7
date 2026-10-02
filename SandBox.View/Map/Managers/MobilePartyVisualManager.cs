using System;
using System.Collections.Generic;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000075 RID: 117
	public class MobilePartyVisualManager : EntityVisualManagerBase<PartyBase>
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00026818 File Offset: 0x00024A18
		public override int Priority
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0002681C File Offset: 0x00024A1C
		public static MobilePartyVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<MobilePartyVisualManager>();
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00026828 File Offset: 0x00024A28
		public override void OnTick(float realDt, float dt)
		{
			this._dirtyPartyVisualCount = -1;
			TWParallel.For(0, this._visualsFlattened.Count, delegate(int startInclusive, int endExclusive)
			{
				for (int k = startInclusive; k < endExclusive; k++)
				{
					this._visualsFlattened[k].Tick(dt, realDt, ref this._dirtyPartyVisualCount, ref this._dirtyPartiesList);
				}
			}, 16);
			for (int i = 0; i < this._dirtyPartyVisualCount + 1; i++)
			{
				this._dirtyPartiesList[i].ValidateIsDirty();
			}
			for (int j = this._fadingPartiesFlatten.Count - 1; j >= 0; j--)
			{
				this._fadingPartiesFlatten[j].TickFadingState(realDt, dt);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000268CC File Offset: 0x00024ACC
		public override void ClearVisualMemory()
		{
			foreach (MobilePartyVisual mobilePartyVisual in this._visualsFlattened)
			{
				mobilePartyVisual.ClearVisualMemory();
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0002691C File Offset: 0x00024B1C
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			base.OnVisualTick(screen, realDt, dt);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00026928 File Offset: 0x00024B28
		public override bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			for (int i = entityCount - 1; i >= 0; i--)
			{
				UIntPtr uintPtr = intersectedEntityIDs[i];
				MapEntityVisual mapEntityVisual;
				MobilePartyVisual mobilePartyVisual;
				if (uintPtr != UIntPtr.Zero && MapScreen.VisualsOfEntities.TryGetValue(uintPtr, out mapEntityVisual) && (mobilePartyVisual = mapEntityVisual as MobilePartyVisual) != null && mapEntityVisual.IsVisibleOrFadingOut() && (!mobilePartyVisual.MapEntity.IsMobile || mobilePartyVisual.MapEntity.MobileParty.IsMainParty || !mobilePartyVisual.MapEntity.MobileParty.IsInRaftState))
				{
					if (!mobilePartyVisual.IsMainEntity || !Hero.MainHero.IsPrisoner)
					{
						hoveredVisual = mapEntityVisual.AttachedTo ?? mapEntityVisual;
					}
					if (!mapEntityVisual.IsMainEntity && (mapEntityVisual.AttachedTo == null || !mapEntityVisual.AttachedTo.IsMainEntity))
					{
						selectedVisual = mapEntityVisual.AttachedTo ?? mapEntityVisual;
					}
				}
			}
			return selectedVisual != null;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00026A0C File Offset: 0x00024C0C
		public override MapEntityVisual<PartyBase> GetVisualOfEntity(PartyBase partyBase)
		{
			MobileParty mobileParty = partyBase.MobileParty;
			if (mobileParty != null && !mobileParty.IsCurrentlyAtSea)
			{
				MobilePartyVisual mobilePartyVisual;
				this._partiesAndVisuals.TryGetValue(partyBase, out mobilePartyVisual);
				return mobilePartyVisual;
			}
			return null;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00026A44 File Offset: 0x00024C44
		protected override void OnFinalize()
		{
			foreach (MobilePartyVisual mobilePartyVisual in this._partiesAndVisuals.Values)
			{
				mobilePartyVisual.ReleaseResources();
			}
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00026AA4 File Offset: 0x00024CA4
		protected override void OnInitialize()
		{
			base.OnInitialize();
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				this.AddNewPartyVisualForParty(mobileParty, true);
			}
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00026B2C File Offset: 0x00024D2C
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			this.RemovePartyVisualForParty(mobileParty);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00026B35 File Offset: 0x00024D35
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			this.AddNewPartyVisualForParty(mobileParty, false);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00026B3F File Offset: 0x00024D3F
		public MobilePartyVisual GetPartyVisual(PartyBase partyBase)
		{
			return this._partiesAndVisuals[partyBase];
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00026B4D File Offset: 0x00024D4D
		internal void RegisterFadingVisual(MobilePartyVisual visual)
		{
			if (!this._fadingPartiesSet.Contains(visual))
			{
				this._fadingPartiesFlatten.Add(visual);
				this._fadingPartiesSet.Add(visual);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00026B78 File Offset: 0x00024D78
		internal void UnRegisterFadingVisual(MobilePartyVisual visual)
		{
			if (this._fadingPartiesSet.Contains(visual))
			{
				int num = this._fadingPartiesFlatten.IndexOf(visual);
				this._fadingPartiesFlatten[num] = this._fadingPartiesFlatten[this._fadingPartiesFlatten.Count - 1];
				this._fadingPartiesFlatten.Remove(this._fadingPartiesFlatten[this._fadingPartiesFlatten.Count - 1]);
				this._fadingPartiesSet.Remove(visual);
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00026BF8 File Offset: 0x00024DF8
		private void AddNewPartyVisualForParty(MobileParty mobileParty, bool shouldTick = false)
		{
			if (!mobileParty.IsGarrison && !mobileParty.IsMilitia && !this._partiesAndVisuals.ContainsKey(mobileParty.Party))
			{
				MobilePartyVisual mobilePartyVisual = new MobilePartyVisual(mobileParty.Party);
				mobilePartyVisual.OnStartup();
				this._partiesAndVisuals.Add(mobileParty.Party, mobilePartyVisual);
				this._visualsFlattened.Add(mobilePartyVisual);
				if (shouldTick)
				{
					mobilePartyVisual.Tick(0.1f, 0.1f, ref this._dirtyPartyVisualCount, ref this._dirtyPartiesList);
				}
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00026C78 File Offset: 0x00024E78
		private void RemovePartyVisualForParty(MobileParty mobileParty)
		{
			MobilePartyVisual mobilePartyVisual;
			if (this._partiesAndVisuals.TryGetValue(mobileParty.Party, out mobilePartyVisual))
			{
				mobilePartyVisual.OnPartyRemoved();
				this._visualsFlattened.Remove(mobilePartyVisual);
				this._partiesAndVisuals.Remove(mobileParty.Party);
			}
		}

		// Token: 0x04000244 RID: 580
		private readonly Dictionary<PartyBase, MobilePartyVisual> _partiesAndVisuals = new Dictionary<PartyBase, MobilePartyVisual>();

		// Token: 0x04000245 RID: 581
		private readonly List<MobilePartyVisual> _visualsFlattened = new List<MobilePartyVisual>();

		// Token: 0x04000246 RID: 582
		private int _dirtyPartyVisualCount;

		// Token: 0x04000247 RID: 583
		private MobilePartyVisual[] _dirtyPartiesList = new MobilePartyVisual[2500];

		// Token: 0x04000248 RID: 584
		private readonly List<MobilePartyVisual> _fadingPartiesFlatten = new List<MobilePartyVisual>();

		// Token: 0x04000249 RID: 585
		private readonly HashSet<MobilePartyVisual> _fadingPartiesSet = new HashSet<MobilePartyVisual>();
	}
}
