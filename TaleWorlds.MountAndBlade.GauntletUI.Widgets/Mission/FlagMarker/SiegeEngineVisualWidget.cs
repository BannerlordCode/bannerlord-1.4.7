using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.FlagMarker
{
	// Token: 0x02000100 RID: 256
	public class SiegeEngineVisualWidget : Widget
	{
		// Token: 0x06000DC3 RID: 3523 RVA: 0x00025AE6 File Offset: 0x00023CE6
		public SiegeEngineVisualWidget(UIContext context)
			: base(context)
		{
			this._fallbackSprite = this.GetSprite("BlankWhiteCircle");
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00025B0C File Offset: 0x00023D0C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._hasVisualSet && this.EngineID != string.Empty && this.OutlineWidget != null && this.IconWidget != null)
			{
				string text = string.Empty;
				string engineID = this.EngineID;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(engineID);
				if (num <= 1241455715U)
				{
					if (num <= 712590611U)
					{
						if (num != 6339497U)
						{
							if (num != 695812992U)
							{
								if (num != 712590611U)
								{
									goto IL_01F8;
								}
								if (!(engineID == "siege_tower_level2"))
								{
									goto IL_01F8;
								}
							}
							else if (!(engineID == "siege_tower_level3"))
							{
								goto IL_01F8;
							}
						}
						else
						{
							if (!(engineID == "ladder"))
							{
								goto IL_01F8;
							}
							text = "ladder";
							goto IL_01F8;
						}
					}
					else if (num != 729368230U)
					{
						if (num != 808481256U)
						{
							if (num != 1241455715U)
							{
								goto IL_01F8;
							}
							if (!(engineID == "ram"))
							{
								goto IL_01F8;
							}
							text = "battering_ram";
							goto IL_01F8;
						}
						else
						{
							if (!(engineID == "fire_ballista"))
							{
								goto IL_01F8;
							}
							goto IL_01D2;
						}
					}
					else if (!(engineID == "siege_tower_level1"))
					{
						goto IL_01F8;
					}
					text = "siege_tower";
					goto IL_01F8;
				}
				if (num <= 1839032341U)
				{
					if (num != 1748194790U)
					{
						if (num != 1820818168U)
						{
							if (num != 1839032341U)
							{
								goto IL_01F8;
							}
							if (!(engineID == "trebuchet"))
							{
								goto IL_01F8;
							}
							text = "trebuchet";
							goto IL_01F8;
						}
						else if (!(engineID == "fire_onager"))
						{
							goto IL_01F8;
						}
					}
					else if (!(engineID == "fire_catapult"))
					{
						goto IL_01F8;
					}
				}
				else if (num != 1898442385U)
				{
					if (num != 2806198843U)
					{
						if (num != 4036530155U)
						{
							goto IL_01F8;
						}
						if (!(engineID == "ballista"))
						{
							goto IL_01F8;
						}
						goto IL_01D2;
					}
					else if (!(engineID == "onager"))
					{
						goto IL_01F8;
					}
				}
				else if (!(engineID == "catapult"))
				{
					goto IL_01F8;
				}
				text = "catapult";
				goto IL_01F8;
				IL_01D2:
				text = "ballista";
				IL_01F8:
				this.OutlineWidget.Sprite = ((text == string.Empty) ? this._fallbackSprite : this.GetSprite("MPHud\\SiegeMarkers\\" + text + "_outline"));
				this.IconWidget.Sprite = ((text == string.Empty) ? this._fallbackSprite : this.GetSprite("MPHud\\SiegeMarkers\\" + text));
				this._hasVisualSet = true;
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00025D7F File Offset: 0x00023F7F
		private Sprite GetSprite(string path)
		{
			return base.Context.SpriteData.GetSprite(path);
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00025D92 File Offset: 0x00023F92
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x00025D9A File Offset: 0x00023F9A
		[Editor(false)]
		public string EngineID
		{
			get
			{
				return this._engineID;
			}
			set
			{
				if (value != this._engineID)
				{
					this._engineID = value;
					base.OnPropertyChanged<string>(value, "EngineID");
				}
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00025DBD File Offset: 0x00023FBD
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x00025DC5 File Offset: 0x00023FC5
		public Widget OutlineWidget
		{
			get
			{
				return this._outlineWidget;
			}
			set
			{
				if (this._outlineWidget != value)
				{
					this._outlineWidget = value;
					base.OnPropertyChanged<Widget>(value, "OutlineWidget");
				}
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00025DE3 File Offset: 0x00023FE3
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x00025DEB File Offset: 0x00023FEB
		public Widget IconWidget
		{
			get
			{
				return this._iconWidget;
			}
			set
			{
				if (this._iconWidget != value)
				{
					this._iconWidget = value;
					base.OnPropertyChanged<Widget>(value, "IconWidget");
				}
			}
		}

		// Token: 0x0400063B RID: 1595
		private bool _hasVisualSet;

		// Token: 0x0400063C RID: 1596
		private Sprite _fallbackSprite;

		// Token: 0x0400063D RID: 1597
		private const string SpritePathPrefix = "MPHud\\SiegeMarkers\\";

		// Token: 0x0400063E RID: 1598
		private string _engineID = string.Empty;

		// Token: 0x0400063F RID: 1599
		private Widget _outlineWidget;

		// Token: 0x04000640 RID: 1600
		private Widget _iconWidget;
	}
}
