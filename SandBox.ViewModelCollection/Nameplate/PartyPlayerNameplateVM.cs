using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001B RID: 27
	public class PartyPlayerNameplateVM : PartyNameplateVM
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000B82B File Offset: 0x00009A2B
		public PartyPlayerNameplateVM()
		{
			this.IsMainParty = true;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B848 File Offset: 0x00009A48
		public void InitializePlayerNameplate(Action resetCamera)
		{
			this._isPartyHeroVisualDirty = true;
			this._resetCamera = resetCamera;
			bool flag;
			if (this.IsMainParty && base.Party.LeaderHero == null)
			{
				Hero mainHero = Hero.MainHero;
				flag = mainHero != null && mainHero.IsAlive;
			}
			else
			{
				flag = false;
			}
			this._isPrisonerBind = flag;
			this.MainHeroVisual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(Hero.MainHero.CharacterObject, false));
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B8AD File Offset: 0x00009AAD
		public override void Clear()
		{
			base.Clear();
			base.IsInSettlement = true;
			base.IsVisibleOnMap = false;
			this.MainHeroVisual = null;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B8CC File Offset: 0x00009ACC
		public override void RefreshDynamicProperties(bool forceUpdate)
		{
			base.RefreshDynamicProperties(forceUpdate);
			if ((this.IsMainParty && MathF.Abs(Hero.MainHero.Age - this._latestMainHeroAge) >= 1f) || forceUpdate)
			{
				this._latestMainHeroAge = Hero.MainHero.Age;
				this._isPartyHeroVisualDirty = true;
			}
			if (this._isPartyHeroVisualDirty || forceUpdate)
			{
				this._mainHeroVisualBind = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(Hero.MainHero.CharacterObject, false));
				this._isPartyHeroVisualDirty = false;
			}
			bool flag;
			if (this.IsMainParty && base.Party.LeaderHero == null)
			{
				Hero mainHero = Hero.MainHero;
				flag = mainHero != null && mainHero.IsAlive;
			}
			else
			{
				flag = false;
			}
			this._isPrisonerBind = flag;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000B981 File Offset: 0x00009B81
		public override void RefreshBinding()
		{
			base.RefreshBinding();
			this.IsPrisoner = this._isPrisonerBind;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000B998 File Offset: 0x00009B98
		public override void RefreshPosition()
		{
			Vec3 vec = (base.Party.Position + base.Party.EventPositionAdder).AsVec3();
			Vec3 vec2 = vec + new Vec3(0f, 0f, 0.8f, -1f);
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec, ref this._latestX, ref this._latestY, ref this._latestW);
			this._partyPositionBind = new Vec2(this._latestX, this._latestY);
			this._isHighBind = this._mapCamera.Position.Distance(vec) >= 110f;
			this._isBehindBind = this._latestW < 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec2, ref this._latestX, ref this._latestY, ref this._latestW);
			this._headPositionBind = new Vec2(this._latestX, this._latestY);
			base.DistanceToCamera = vec.Distance(this._mapCamera.Position);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000BAC5 File Offset: 0x00009CC5
		public void ExecuteSetCameraPosition()
		{
			this._resetCamera();
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000BAD2 File Offset: 0x00009CD2
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000BADA File Offset: 0x00009CDA
		[DataSourceProperty]
		public bool IsMainParty
		{
			get
			{
				return this._isMainParty;
			}
			set
			{
				if (value != this._isMainParty)
				{
					this._isMainParty = value;
					base.OnPropertyChangedWithValue(value, "IsMainParty");
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000BB00 File Offset: 0x00009D00
		[DataSourceProperty]
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (value != this._isPrisoner)
				{
					this._isPrisoner = value;
					base.OnPropertyChangedWithValue(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000BB1E File Offset: 0x00009D1E
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000BB26 File Offset: 0x00009D26
		[DataSourceProperty]
		public CharacterImageIdentifierVM MainHeroVisual
		{
			get
			{
				return this._mainHeroVisual;
			}
			set
			{
				if (value != this._mainHeroVisual)
				{
					this._mainHeroVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "MainHeroVisual");
				}
			}
		}

		// Token: 0x0400014F RID: 335
		private float _latestMainHeroAge = -1f;

		// Token: 0x04000150 RID: 336
		private bool _isPartyHeroVisualDirty;

		// Token: 0x04000151 RID: 337
		private Action _resetCamera;

		// Token: 0x04000152 RID: 338
		private CharacterImageIdentifierVM _mainHeroVisualBind;

		// Token: 0x04000153 RID: 339
		private bool _isPrisonerBind;

		// Token: 0x04000154 RID: 340
		private bool _isMainParty;

		// Token: 0x04000155 RID: 341
		private bool _isPrisoner;

		// Token: 0x04000156 RID: 342
		private CharacterImageIdentifierVM _mainHeroVisual;
	}
}
