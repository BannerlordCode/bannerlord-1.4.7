using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000037 RID: 55
	public class GauntletMapEventVisual : IMapEventVisual
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000FB81 File Offset: 0x0000DD81
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000FB89 File Offset: 0x0000DD89
		public MapEvent MapEvent { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000FB92 File Offset: 0x0000DD92
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000FB9A File Offset: 0x0000DD9A
		public Vec2 WorldPosition { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000FBA3 File Offset: 0x0000DDA3
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000FBAB File Offset: 0x0000DDAB
		public bool IsVisible { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000FBB4 File Offset: 0x0000DDB4
		private Scene MapScene
		{
			get
			{
				if (this._mapScene == null)
				{
					Campaign campaign = Campaign.Current;
					if (((campaign != null) ? campaign.MapSceneWrapper : null) != null)
					{
						this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
					}
				}
				return this._mapScene;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000FC02 File Offset: 0x0000DE02
		public GauntletMapEventVisual(MapEvent mapEvent, Action<GauntletMapEventVisual> onInitialized, Action<GauntletMapEventVisual> onVisibilityChanged, Action<GauntletMapEventVisual> onDeactivate)
		{
			this._onDeactivate = onDeactivate;
			this._onInitialized = onInitialized;
			this._onVisibilityChanged = onVisibilityChanged;
			this.MapEvent = mapEvent;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000FC28 File Offset: 0x0000DE28
		public void Initialize(CampaignVec2 position, bool isVisible)
		{
			this.WorldPosition = position.ToVec2();
			this.IsVisible = isVisible;
			Action<GauntletMapEventVisual> onInitialized = this._onInitialized;
			if (onInitialized != null)
			{
				onInitialized(this);
			}
			int num = -1;
			int num2 = 4;
			if (this.MapEvent.IsNavalMapEvent || this.MapEvent.IsBlockade || this.MapEvent.IsBlockadeSallyOut)
			{
				num = GauntletMapEventVisual._navalBattleSoundEventIndex;
			}
			else if (this.MapEvent.IsFieldBattle || this.MapEvent.IsSallyOut)
			{
				num = GauntletMapEventVisual._battleSoundEventIndex;
				num2 = this.GetBattleSizeValue();
			}
			else if (this.MapEvent.IsSiegeAssault || this.MapEvent.IsSiegeOutside || this.MapEvent.IsSiegeAmbush)
			{
				num = GauntletMapEventVisual._siegeSoundEventIndex;
			}
			else if (this.MapEvent.IsRaid)
			{
				num = GauntletMapEventVisual._raidSoundEventIndex;
			}
			else if (this.MapEvent.IsHideoutBattle)
			{
				num = GauntletMapEventVisual._hideoutBattleSoundEventIndex;
			}
			if (num != -1)
			{
				float num3 = 0f;
				Settlement mapEventSettlement = this.MapEvent.MapEventSettlement;
				CampaignVec2 campaignVec = ((mapEventSettlement != null) ? mapEventSettlement.Position : this.MapEvent.Position);
				Campaign.Current.MapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num3);
				this._mapEventSoundEvent = SoundEvent.CreateEvent(num, this.MapScene);
				this._mapEventSoundEvent.SetParameter("battle_size", (float)num2);
				this._mapEventSoundEvent.PlayInPosition(new Vec3(position.X, position.Y, num3 + 2f, -1f));
				if (!isVisible)
				{
					this._mapEventSoundEvent.Pause();
				}
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		private int GetBattleSizeValue()
		{
			if (this.MapEvent.IsSiegeAssault)
			{
				return 4;
			}
			int numberOfInvolvedMen = this.MapEvent.GetNumberOfInvolvedMen();
			if (numberOfInvolvedMen < 30)
			{
				return 0;
			}
			if (numberOfInvolvedMen < 80)
			{
				return 1;
			}
			if (numberOfInvolvedMen >= 120)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000FDF2 File Offset: 0x0000DFF2
		public void OnMapEventEnd()
		{
			Action<GauntletMapEventVisual> onDeactivate = this._onDeactivate;
			if (onDeactivate != null)
			{
				onDeactivate(this);
			}
			if (this._mapEventSoundEvent != null)
			{
				this._mapEventSoundEvent.Stop();
				this._mapEventSoundEvent = null;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000FE20 File Offset: 0x0000E020
		public void SetVisibility(bool isVisible)
		{
			this.IsVisible = isVisible;
			Action<GauntletMapEventVisual> onVisibilityChanged = this._onVisibilityChanged;
			if (onVisibilityChanged != null)
			{
				onVisibilityChanged(this);
			}
			SoundEvent mapEventSoundEvent = this._mapEventSoundEvent;
			if (mapEventSoundEvent != null && mapEventSoundEvent.IsValid)
			{
				if (isVisible && this._mapEventSoundEvent.IsPaused())
				{
					this._mapEventSoundEvent.Resume();
					return;
				}
				if (!isVisible && !this._mapEventSoundEvent.IsPaused())
				{
					this._mapEventSoundEvent.Pause();
				}
			}
		}

		// Token: 0x040000EC RID: 236
		private static int _battleSoundEventIndex = SoundManager.GetEventGlobalIndex("event:/map/ambient/node/battle");

		// Token: 0x040000ED RID: 237
		private static int _navalBattleSoundEventIndex = SoundManager.GetEventGlobalIndex("event:/map/ambient/node/naval_battle_loop");

		// Token: 0x040000EE RID: 238
		private static int _raidSoundEventIndex = SoundManager.GetEventGlobalIndex("event:/map/ambient/node/battle_raid");

		// Token: 0x040000EF RID: 239
		private static int _siegeSoundEventIndex = SoundManager.GetEventGlobalIndex("event:/map/ambient/node/battle_siege");

		// Token: 0x040000F0 RID: 240
		private static int _hideoutBattleSoundEventIndex = SoundManager.GetEventGlobalIndex("event:/map/ambient/node/battle_hideout");

		// Token: 0x040000F1 RID: 241
		private SoundEvent _mapEventSoundEvent;

		// Token: 0x040000F5 RID: 245
		private readonly Action<GauntletMapEventVisual> _onDeactivate;

		// Token: 0x040000F6 RID: 246
		private readonly Action<GauntletMapEventVisual> _onInitialized;

		// Token: 0x040000F7 RID: 247
		private readonly Action<GauntletMapEventVisual> _onVisibilityChanged;

		// Token: 0x040000F8 RID: 248
		private Scene _mapScene;
	}
}
