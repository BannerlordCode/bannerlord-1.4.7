using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000E RID: 14
	public class GauntletFullScreenNoticeView : GlobalLayer
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004AC1 File Offset: 0x00002CC1
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00004AC8 File Offset: 0x00002CC8
		public static GauntletFullScreenNoticeView Current { get; private set; }

		// Token: 0x06000075 RID: 117 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public GauntletFullScreenNoticeView()
		{
			this._dataSource = new FullScreenNoticeVM();
			GauntletLayer gauntletLayer = new GauntletLayer("FullScreenNotice", 15010, false);
			gauntletLayer.LoadMovie("FullScreenNotice", this._dataSource);
			base.Layer = gauntletLayer;
			base.Layer.IsFocusLayer = true;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004B6A File Offset: 0x00002D6A
		public static void Initialize()
		{
			if (GauntletFullScreenNoticeView.Current == null && !BannerlordConfig.IAPNoticeConfirmed)
			{
				GauntletFullScreenNoticeView.Current = new GauntletFullScreenNoticeView();
				ScreenManager.AddGlobalLayer(GauntletFullScreenNoticeView.Current, false);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004B8F File Offset: 0x00002D8F
		public static void SkipNotice()
		{
			GauntletFullScreenNoticeView gauntletFullScreenNoticeView = GauntletFullScreenNoticeView.Current;
			if (gauntletFullScreenNoticeView == null)
			{
				return;
			}
			FullScreenNoticeVM dataSource = gauntletFullScreenNoticeView._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.ExecuteCloseNotice();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004BAC File Offset: 0x00002DAC
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			GauntletFullScreenNoticeView gauntletFullScreenNoticeView = GauntletFullScreenNoticeView.Current;
			if (((gauntletFullScreenNoticeView != null) ? gauntletFullScreenNoticeView._dataSource : null) != null)
			{
				if (GauntletFullScreenNoticeView.Current._dataSource.IsNoticeActive)
				{
					ScreenManager.TrySetFocus(base.Layer);
					if (base.Layer.Input.IsHotKeyReleased("Confirm"))
					{
						GauntletFullScreenNoticeView.SkipNotice();
						UISoundsHelper.PlayUISound("event:/ui/default");
						return;
					}
				}
				else
				{
					ScreenManager.RemoveGlobalLayer(GauntletFullScreenNoticeView.Current);
					GauntletFullScreenNoticeView.Current._dataSource.OnFinalize();
					GauntletFullScreenNoticeView.Current = null;
				}
			}
		}

		// Token: 0x04000054 RID: 84
		private readonly FullScreenNoticeVM _dataSource;
	}
}
