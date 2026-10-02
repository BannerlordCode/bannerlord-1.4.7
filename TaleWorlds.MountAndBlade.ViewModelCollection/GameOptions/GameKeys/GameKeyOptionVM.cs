using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys
{
	// Token: 0x02000079 RID: 121
	public class GameKeyOptionVM : KeyOptionVM
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00021674 File Offset: 0x0001F874
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0002167C File Offset: 0x0001F87C
		public GameKey CurrentGameKey { get; private set; }

		// Token: 0x060009A7 RID: 2471 RVA: 0x00021688 File Offset: 0x0001F888
		public GameKeyOptionVM(GameKey gameKey, Action<KeyOptionVM> onKeybindRequest, Action<GameKeyOptionVM, InputKey> onKeySet, Func<GameKeyOptionVM, string> getExtraInformation)
			: base(gameKey.GroupId, ((GameKeyDefinition)gameKey.Id).ToString(), onKeybindRequest)
		{
			this._onKeySet = onKeySet;
			this._getExtraInformation = getExtraInformation;
			this.CurrentGameKey = gameKey;
			base.Key = (Input.IsGamepadActive ? this.CurrentGameKey.ControllerKey : this.CurrentGameKey.KeyboardKey);
			if (base.Key == null)
			{
				base.Key = new Key(InputKey.Invalid);
			}
			this._initalKey = base.Key.InputKey;
			base.CurrentKey = new Key(base.Key.InputKey);
			this.RefreshValues();
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00021738 File Offset: 0x0001F938
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Name = Module.CurrentModule.GlobalTextManager.FindText("str_key_name", this._groupId + "_" + this._id).ToString();
			base.Description = Module.CurrentModule.GlobalTextManager.FindText("str_key_description", this._groupId + "_" + this._id).ToString();
			base.OptionValueText = Module.CurrentModule.GlobalTextManager.GetHotKeyGameTextFromKeyID(base.CurrentKey.ToString().ToLower()).ToString();
			Func<GameKeyOptionVM, string> getExtraInformation = this._getExtraInformation;
			base.ExtraInformationText = ((getExtraInformation != null) ? getExtraInformation(this) : null);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x000217F8 File Offset: 0x0001F9F8
		private void ExecuteKeybindRequest()
		{
			this._onKeybindRequest(this);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00021806 File Offset: 0x0001FA06
		public override void Set(InputKey newKey)
		{
			this._onKeySet(this, newKey);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00021818 File Offset: 0x0001FA18
		public override void Update()
		{
			base.Key = (Input.IsGamepadActive ? this.CurrentGameKey.ControllerKey : this.CurrentGameKey.KeyboardKey);
			if (base.Key == null)
			{
				base.Key = new Key(InputKey.Invalid);
			}
			base.CurrentKey = new Key(base.Key.InputKey);
			base.OptionValueText = Module.CurrentModule.GlobalTextManager.GetHotKeyGameTextFromKeyID(base.CurrentKey.ToString().ToLower()).ToString();
			this.UpdateIsChanged();
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x000218AA File Offset: 0x0001FAAA
		public override void OnDone()
		{
			Key key = base.Key;
			if (key != null)
			{
				key.ChangeKey(base.CurrentKey.InputKey);
			}
			this._initalKey = base.CurrentKey.InputKey;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000218DC File Offset: 0x0001FADC
		internal override void UpdateIsChanged()
		{
			Key currentKey = base.CurrentKey;
			InputKey? inputKey = ((currentKey != null) ? new InputKey?(currentKey.InputKey) : null);
			InputKey initalKey = this._initalKey;
			base.IsChanged = !((inputKey.GetValueOrDefault() == initalKey) & (inputKey != null));
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002192B File Offset: 0x0001FB2B
		public override void ExecuteRevert()
		{
			this.Set(this._initalKey);
			this.Update();
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002193F File Offset: 0x0001FB3F
		public void Apply()
		{
			this.OnDone();
			base.CurrentKey = base.Key;
		}

		// Token: 0x04000444 RID: 1092
		private InputKey _initalKey;

		// Token: 0x04000446 RID: 1094
		private readonly Action<GameKeyOptionVM, InputKey> _onKeySet;

		// Token: 0x04000447 RID: 1095
		private readonly Func<GameKeyOptionVM, string> _getExtraInformation;
	}
}
