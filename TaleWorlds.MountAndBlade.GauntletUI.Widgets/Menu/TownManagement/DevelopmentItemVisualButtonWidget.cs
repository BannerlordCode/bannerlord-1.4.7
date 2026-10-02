using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000109 RID: 265
	public class DevelopmentItemVisualButtonWidget : ButtonWidget
	{
		// Token: 0x06000E27 RID: 3623 RVA: 0x00026B52 File Offset: 0x00024D52
		public DevelopmentItemVisualButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00026B5C File Offset: 0x00024D5C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._changedVisualToSmallVariant)
			{
				string text = this.DetermineSpriteImageFromSpriteCode(this.SpriteCode, this.UseSmallVariant);
				base.Sprite = base.Context.SpriteData.GetSprite((!string.IsNullOrEmpty(text)) ? text : "building_default");
				if (!this.IsDaily && this.DevelopmentFrontVisualWidget != null)
				{
					this.DevelopmentFrontVisualWidget.Sprite = base.Sprite;
				}
				this._changedVisualToSmallVariant = true;
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00026BDC File Offset: 0x00024DDC
		private string DetermineSpriteImageFromSpriteCode(string spriteCode, bool useSmallVariant)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(spriteCode);
			string text;
			if (num <= 1824052176U)
			{
				if (num > 997573159U)
				{
					if (num <= 1477360017U)
					{
						if (num <= 1010357983U)
						{
							if (num != 1007477469U)
							{
								if (num != 1010357983U)
								{
									goto IL_05A5;
								}
								if (!(spriteCode == "building_settlement_siege_workshop"))
								{
									goto IL_05A5;
								}
							}
							else
							{
								if (!(spriteCode == "building_castle_daily_irrigation"))
								{
									goto IL_05A5;
								}
								goto IL_0585;
							}
						}
						else if (num != 1014636139U)
						{
							if (num != 1477360017U)
							{
								goto IL_05A5;
							}
							if (!(spriteCode == "building_settlement_waterworks"))
							{
								goto IL_05A5;
							}
							text = "building_waterworks";
							goto IL_05AB;
						}
						else
						{
							if (!(spriteCode == "building_settlement_courthouse"))
							{
								goto IL_05A5;
							}
							text = "building_courthouse";
							goto IL_05AB;
						}
					}
					else if (num <= 1738509422U)
					{
						if (num != 1498294180U)
						{
							if (num != 1738509422U)
							{
								goto IL_05A5;
							}
							if (!(spriteCode == "building_castle_siege_workshop"))
							{
								goto IL_05A5;
							}
						}
						else
						{
							if (!(spriteCode == "building_settlement_tax_office"))
							{
								goto IL_05A5;
							}
							text = "building_tax_office";
							goto IL_05AB;
						}
					}
					else if (num != 1791108000U)
					{
						if (num != 1824052176U)
						{
							goto IL_05A5;
						}
						if (!(spriteCode == "building_settlement_daily_irrigation"))
						{
							goto IL_05A5;
						}
						goto IL_0585;
					}
					else
					{
						if (!(spriteCode == "building_settlement_roads_and_paths"))
						{
							goto IL_05A5;
						}
						goto IL_0565;
					}
					text = "building_siege_workshop";
					goto IL_05AB;
					IL_0585:
					text = "building_daily_irrigation";
					goto IL_05AB;
				}
				if (num <= 180479552U)
				{
					if (num <= 96547887U)
					{
						if (num != 14721173U)
						{
							if (num != 96547887U)
							{
								goto IL_05A5;
							}
							if (!(spriteCode == "building_castle_roads_and_paths"))
							{
								goto IL_05A5;
							}
						}
						else
						{
							if (!(spriteCode == "building_castle_granary"))
							{
								goto IL_05A5;
							}
							goto IL_0535;
						}
					}
					else if (num != 166629318U)
					{
						if (num != 180479552U)
						{
							goto IL_05A5;
						}
						if (!(spriteCode == "building_settlement_mason"))
						{
							goto IL_05A5;
						}
						goto IL_054D;
					}
					else
					{
						if (!(spriteCode == "building_settlement_fortifications"))
						{
							goto IL_05A5;
						}
						goto IL_04E3;
					}
				}
				else if (num <= 387747708U)
				{
					if (num != 209632957U)
					{
						if (num != 387747708U)
						{
							goto IL_05A5;
						}
						if (!(spriteCode == "building_shipyard"))
						{
							goto IL_05A5;
						}
						text = "building_shipyard";
						goto IL_05AB;
					}
					else
					{
						if (!(spriteCode == "building_settlement_barracks"))
						{
							goto IL_05A5;
						}
						goto IL_04EE;
					}
				}
				else if (num != 921898011U)
				{
					if (num != 997573159U)
					{
						goto IL_05A5;
					}
					if (!(spriteCode == "building_castle_training_fields"))
					{
						goto IL_05A5;
					}
					goto IL_04F9;
				}
				else
				{
					if (!(spriteCode == "building_castle_daily_drills"))
					{
						goto IL_05A5;
					}
					text = "building_daily_train_militia";
					goto IL_05AB;
				}
				IL_0565:
				text = "building_settlement_roads_and_paths";
				goto IL_05AB;
			}
			if (num <= 3267612449U)
			{
				if (num <= 2589055984U)
				{
					if (num <= 2168957445U)
					{
						if (num != 1962163594U)
						{
							if (num != 2168957445U)
							{
								goto IL_05A5;
							}
							if (!(spriteCode == "building_settlement_marketplace"))
							{
								goto IL_05A5;
							}
							text = "building_marketplace";
							goto IL_05AB;
						}
						else
						{
							if (!(spriteCode == "building_castle_craftmans_quarters"))
							{
								goto IL_05A5;
							}
							text = "building_workshop";
							goto IL_05AB;
						}
					}
					else if (num != 2512466137U)
					{
						if (num != 2589055984U)
						{
							goto IL_05A5;
						}
						if (!(spriteCode == "building_settlement_guard_house"))
						{
							goto IL_05A5;
						}
					}
					else
					{
						if (!(spriteCode == "building_castle_daily_raise_troops"))
						{
							goto IL_05A5;
						}
						text = "building_daily_train_militia";
						goto IL_05AB;
					}
				}
				else if (num <= 2764915242U)
				{
					if (num != 2731049836U)
					{
						if (num != 2764915242U)
						{
							goto IL_05A5;
						}
						if (!(spriteCode == "building_castle_castallans_office"))
						{
							goto IL_05A5;
						}
						text = "building_wardens_office";
						goto IL_05AB;
					}
					else
					{
						if (!(spriteCode == "building_settlement_training_fields"))
						{
							goto IL_05A5;
						}
						goto IL_04F9;
					}
				}
				else if (num != 3169381627U)
				{
					if (num != 3267612449U)
					{
						goto IL_05A5;
					}
					if (!(spriteCode == "building_settlement_warehouse"))
					{
						goto IL_05A5;
					}
					goto IL_0535;
				}
				else
				{
					if (!(spriteCode == "building_castle_mason"))
					{
						goto IL_05A5;
					}
					goto IL_054D;
				}
			}
			else if (num <= 3859901263U)
			{
				if (num <= 3565251534U)
				{
					if (num != 3328885644U)
					{
						if (num != 3565251534U)
						{
							goto IL_05A5;
						}
						if (!(spriteCode == "building_settlement_daily_festival_and_games"))
						{
							goto IL_05A5;
						}
						text = "building_daily_festivals_and_games";
						goto IL_05AB;
					}
					else
					{
						if (!(spriteCode == "building_settlement_daily_train_militia"))
						{
							goto IL_05A5;
						}
						text = "building_daily_train_militia";
						goto IL_05AB;
					}
				}
				else if (num != 3635436511U)
				{
					if (num != 3859901263U)
					{
						goto IL_05A5;
					}
					if (!(spriteCode == "building_castle_farmlands"))
					{
						goto IL_05A5;
					}
					text = "building_gardens";
					goto IL_05AB;
				}
				else
				{
					if (!(spriteCode == "building_settlement_daily_housing"))
					{
						goto IL_05A5;
					}
					text = "building_daily_build_house";
					goto IL_05AB;
				}
			}
			else if (num <= 4086265432U)
			{
				if (num != 4045395471U)
				{
					if (num != 4086265432U)
					{
						goto IL_05A5;
					}
					if (!(spriteCode == "building_castle_barracks"))
					{
						goto IL_05A5;
					}
					goto IL_04EE;
				}
				else if (!(spriteCode == "building_castle_guard_house"))
				{
					goto IL_05A5;
				}
			}
			else if (num != 4091954564U)
			{
				if (num != 4120188879U)
				{
					goto IL_05A5;
				}
				if (!(spriteCode == "building_castle_fortifications"))
				{
					goto IL_05A5;
				}
				goto IL_04E3;
			}
			else
			{
				if (!(spriteCode == "building_castle_daily_slacken_garrison"))
				{
					goto IL_05A5;
				}
				text = "building_daily_train_militia";
				goto IL_05AB;
			}
			text = "building_guard_house";
			goto IL_05AB;
			IL_04E3:
			text = "building_fortifications";
			goto IL_05AB;
			IL_04EE:
			text = "building_barracks";
			goto IL_05AB;
			IL_04F9:
			text = "building_training_fields";
			goto IL_05AB;
			IL_0535:
			text = "building_granary";
			goto IL_05AB;
			IL_054D:
			text = "building_masonry";
			goto IL_05AB;
			IL_05A5:
			return "";
			IL_05AB:
			if (useSmallVariant)
			{
				text += "_t";
			}
			return text;
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x000271A4 File Offset: 0x000253A4
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x000271AC File Offset: 0x000253AC
		[Editor(false)]
		public bool UseSmallVariant
		{
			get
			{
				return this._useSmallVariant;
			}
			set
			{
				if (this._useSmallVariant != value)
				{
					this._useSmallVariant = value;
					base.OnPropertyChanged(value, "UseSmallVariant");
				}
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x000271CA File Offset: 0x000253CA
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x000271D2 File Offset: 0x000253D2
		[Editor(false)]
		public bool IsDaily
		{
			get
			{
				return this._isDaily;
			}
			set
			{
				if (this._isDaily != value)
				{
					this._isDaily = value;
					base.OnPropertyChanged(value, "IsDaily");
				}
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x000271F0 File Offset: 0x000253F0
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x000271F8 File Offset: 0x000253F8
		[Editor(false)]
		public string SpriteCode
		{
			get
			{
				return this._spriteCode;
			}
			set
			{
				if (this._spriteCode != value)
				{
					this._spriteCode = value;
					base.OnPropertyChanged<string>(value, "SpriteCode");
					this._changedVisualToSmallVariant = false;
				}
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00027222 File Offset: 0x00025422
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0002722A File Offset: 0x0002542A
		[Editor(false)]
		public Widget DevelopmentFrontVisualWidget
		{
			get
			{
				return this._developmentFrontVisualWidget;
			}
			set
			{
				if (this._developmentFrontVisualWidget != value)
				{
					this._developmentFrontVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentFrontVisualWidget");
				}
			}
		}

		// Token: 0x04000669 RID: 1641
		private const string _defaultBuildingSpriteName = "building_default";

		// Token: 0x0400066A RID: 1642
		private bool _changedVisualToSmallVariant;

		// Token: 0x0400066B RID: 1643
		private bool _useSmallVariant;

		// Token: 0x0400066C RID: 1644
		private bool _isDaily;

		// Token: 0x0400066D RID: 1645
		private string _spriteCode;

		// Token: 0x0400066E RID: 1646
		private Widget _developmentFrontVisualWidget;
	}
}
