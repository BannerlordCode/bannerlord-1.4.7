using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.CharacterCreation
{
	// Token: 0x0200007D RID: 125
	public abstract class CharacterCreationStageViewBase : ICharacterCreationStageListener
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x0002835C File Offset: 0x0002655C
		protected CharacterCreationStageViewBase(ControlCharacterCreationStage affirmativeAction, ControlCharacterCreationStage negativeAction, ControlCharacterCreationStage refreshAction, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
		{
			this._affirmativeAction = affirmativeAction;
			this._negativeAction = negativeAction;
			this._refreshAction = refreshAction;
			this._getTotalStageCountAction = getTotalStageCountAction;
			this._getCurrentStageIndexAction = getCurrentStageIndexAction;
			this._getFurthestIndexAction = getFurthestIndexAction;
			this._goToIndexAction = goToIndexAction;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000283C3 File Offset: 0x000265C3
		public virtual void SetGenericScene(Scene scene)
		{
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000283C5 File Offset: 0x000265C5
		protected virtual void OnRefresh()
		{
			this._refreshAction();
		}

		// Token: 0x0600055D RID: 1373
		public abstract IEnumerable<ScreenLayer> GetLayers();

		// Token: 0x0600055E RID: 1374
		public abstract void NextStage();

		// Token: 0x0600055F RID: 1375
		public abstract void PreviousStage();

		// Token: 0x06000560 RID: 1376 RVA: 0x000283D2 File Offset: 0x000265D2
		void ICharacterCreationStageListener.OnStageFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000283DA File Offset: 0x000265DA
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000283DC File Offset: 0x000265DC
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06000563 RID: 1379
		public abstract int GetVirtualStageCount();

		// Token: 0x06000564 RID: 1380 RVA: 0x000283DE File Offset: 0x000265DE
		public virtual void GoToIndex(int index)
		{
			this._goToIndexAction(index);
		}

		// Token: 0x06000565 RID: 1381
		public abstract void LoadEscapeMenuMovie();

		// Token: 0x06000566 RID: 1382
		public abstract void ReleaseEscapeMenuMovie();

		// Token: 0x06000567 RID: 1383 RVA: 0x000283EC File Offset: 0x000265EC
		public void HandleEscapeMenu(CharacterCreationStageViewBase view, ScreenLayer screenLayer)
		{
			if (screenLayer.Input.IsHotKeyReleased("ToggleEscapeMenu"))
			{
				if (this._isEscapeOpen)
				{
					this.RemoveEscapeMenu(view);
					return;
				}
				this.OpenEscapeMenu(view);
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00028417 File Offset: 0x00026617
		private void OpenEscapeMenu(CharacterCreationStageViewBase view)
		{
			view.LoadEscapeMenuMovie();
			this._isEscapeOpen = true;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00028426 File Offset: 0x00026626
		private void RemoveEscapeMenu(CharacterCreationStageViewBase view)
		{
			view.ReleaseEscapeMenuMovie();
			this._isEscapeOpen = false;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00028438 File Offset: 0x00026638
		public List<EscapeMenuItemVM> GetEscapeMenuItems(CharacterCreationStageViewBase view)
		{
			TextObject characterCreationDisabledReason = GameTexts.FindText("str_pause_menu_disabled_hint", "CharacterCreation");
			List<EscapeMenuItemVM> list = new List<EscapeMenuItemVM>();
			list.Add(new EscapeMenuItemVM(new TextObject("{=5Saniypu}Resume", null), delegate(object o)
			{
				this.RemoveEscapeMenu(view);
			}, null, () => new Tuple<bool, TextObject>(false, null), true));
			list.Add(new EscapeMenuItemVM(new TextObject("{=PXT6aA4J}Campaign Options", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=NqarFr4P}Options", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=bV75iwKa}Save", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=e0KdfaNe}Save As", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=9NuttOBC}Load", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=AbEh2y8o}Save And Exit", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=RamV6yLM}Exit to Main Menu", null), delegate(object o)
			{
				this.RemoveEscapeMenu(view);
				view.OnFinalize();
				MBGameManager.EndGame();
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			return list;
		}

		// Token: 0x04000279 RID: 633
		protected readonly ControlCharacterCreationStage _affirmativeAction;

		// Token: 0x0400027A RID: 634
		protected readonly ControlCharacterCreationStage _negativeAction;

		// Token: 0x0400027B RID: 635
		protected readonly ControlCharacterCreationStage _refreshAction;

		// Token: 0x0400027C RID: 636
		protected readonly ControlCharacterCreationStageReturnInt _getTotalStageCountAction;

		// Token: 0x0400027D RID: 637
		protected readonly ControlCharacterCreationStageReturnInt _getCurrentStageIndexAction;

		// Token: 0x0400027E RID: 638
		protected readonly ControlCharacterCreationStageReturnInt _getFurthestIndexAction;

		// Token: 0x0400027F RID: 639
		protected readonly ControlCharacterCreationStageWithInt _goToIndexAction;

		// Token: 0x04000280 RID: 640
		protected readonly Vec3 _cameraPosition = new Vec3(6.45f, 4.35f, 1.6f, -1f);

		// Token: 0x04000281 RID: 641
		private bool _isEscapeOpen;
	}
}
