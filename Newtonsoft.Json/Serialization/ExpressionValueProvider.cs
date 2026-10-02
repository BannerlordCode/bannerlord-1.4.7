using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public class ExpressionValueProvider : IValueProvider
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x0001B055 File Offset: 0x00019255
		public ExpressionValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			this._memberInfo = memberInfo;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001B070 File Offset: 0x00019270
		public void SetValue(object target, [Nullable(2)] object value)
		{
			try
			{
				if (this._setter == null)
				{
					this._setter = ExpressionReflectionDelegateFactory.Instance.CreateSet<object>(this._memberInfo);
				}
				this._setter(target, value);
			}
			catch (Exception ex)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), ex);
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001B0E4 File Offset: 0x000192E4
		[return: Nullable(2)]
		public object GetValue(object target)
		{
			object obj;
			try
			{
				if (this._getter == null)
				{
					this._getter = ExpressionReflectionDelegateFactory.Instance.CreateGet<object>(this._memberInfo);
				}
				obj = this._getter(target);
			}
			catch (Exception ex)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), ex);
			}
			return obj;
		}

		// Token: 0x0400022A RID: 554
		private readonly MemberInfo _memberInfo;

		// Token: 0x0400022B RID: 555
		[Nullable(new byte[] { 2, 1, 2 })]
		private Func<object, object> _getter;

		// Token: 0x0400022C RID: 556
		[Nullable(new byte[] { 2, 1, 2 })]
		private Action<object, object> _setter;
	}
}
