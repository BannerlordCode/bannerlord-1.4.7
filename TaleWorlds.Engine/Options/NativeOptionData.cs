using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000AA RID: 170
	public abstract class NativeOptionData : IOptionData
	{
		// Token: 0x06000F51 RID: 3921 RVA: 0x0001206F File Offset: 0x0001026F
		protected NativeOptionData(NativeOptions.NativeOptionsType type)
		{
			this.Type = type;
			this._value = NativeOptions.GetConfig(type);
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0001208A File Offset: 0x0001028A
		public virtual float GetDefaultValue()
		{
			return NativeOptions.GetDefaultConfig(this.Type);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00012097 File Offset: 0x00010297
		public void Commit()
		{
			NativeOptions.SetConfig(this.Type, this._value);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x000120AA File Offset: 0x000102AA
		public float GetValue(bool forceRefresh)
		{
			if (forceRefresh)
			{
				this._value = NativeOptions.GetConfig(this.Type);
			}
			return this._value;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x000120C6 File Offset: 0x000102C6
		public void SetValue(float value)
		{
			this._value = value;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x000120CF File Offset: 0x000102CF
		public object GetOptionType()
		{
			return this.Type;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x000120DC File Offset: 0x000102DC
		public bool IsNative()
		{
			return true;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x000120DF File Offset: 0x000102DF
		public bool IsAction()
		{
			return false;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x000120E4 File Offset: 0x000102E4
		public ValueTuple<string, bool> GetIsDisabledAndReasonID()
		{
			NativeOptions.NativeOptionsType type = this.Type;
			if (type <= NativeOptions.NativeOptionsType.ResolutionScale)
			{
				if (type != NativeOptions.NativeOptionsType.GyroAimSensitivity)
				{
					if (type == NativeOptions.NativeOptionsType.ResolutionScale)
					{
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DLSS) != 0f)
						{
							return new ValueTuple<string, bool>("str_dlss_enabled", true);
						}
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DynamicResolution) != 0f)
						{
							return new ValueTuple<string, bool>("str_dynamic_resolution_enabled", true);
						}
					}
				}
				else if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.EnableGyroAssistedAim) != 1f)
				{
					return new ValueTuple<string, bool>("str_gyro_disabled", true);
				}
			}
			else if (type != NativeOptions.NativeOptionsType.DLSS)
			{
				if (type != NativeOptions.NativeOptionsType.DynamicResolution)
				{
					if (type == NativeOptions.NativeOptionsType.DynamicResolutionTarget)
					{
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DynamicResolution) == 0f)
						{
							return new ValueTuple<string, bool>("str_dynamic_resolution_disabled", true);
						}
						if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DLSS) != 0f)
						{
							return new ValueTuple<string, bool>("str_dlss_enabled", true);
						}
					}
				}
				else if (NativeOptions.GetConfig(NativeOptions.NativeOptionsType.DLSS) != 0f)
				{
					return new ValueTuple<string, bool>("str_dlss_enabled", true);
				}
			}
			else if (!NativeOptions.GetIsDLSSAvailable())
			{
				return new ValueTuple<string, bool>("str_dlss_not_available", true);
			}
			return new ValueTuple<string, bool>(string.Empty, false);
		}

		// Token: 0x0400021E RID: 542
		public readonly NativeOptions.NativeOptionsType Type;

		// Token: 0x0400021F RID: 543
		private float _value;
	}
}
