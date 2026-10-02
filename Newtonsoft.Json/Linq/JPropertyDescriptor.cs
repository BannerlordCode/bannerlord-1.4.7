using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000A71 RID: 2673 RVA: 0x00029B07 File Offset: 0x00027D07
		public JPropertyDescriptor(string name)
			: base(name, null)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00029B11 File Offset: 0x00027D11
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00029B19 File Offset: 0x00027D19
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00029B1C File Offset: 0x00027D1C
		[return: Nullable(2)]
		public override object GetValue(object component)
		{
			JObject jobject = component as JObject;
			if (jobject == null)
			{
				return null;
			}
			return jobject[this.Name];
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00029B35 File Offset: 0x00027D35
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00029B38 File Offset: 0x00027D38
		public override void SetValue(object component, object value)
		{
			JObject jobject = component as JObject;
			if (jobject != null)
			{
				JToken jtoken = (value as JToken) ?? new JValue(value);
				jobject[this.Name] = jtoken;
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00029B6D File Offset: 0x00027D6D
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00029B70 File Offset: 0x00027D70
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00029B7C File Offset: 0x00027D7C
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00029B7F File Offset: 0x00027D7F
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00029B8B File Offset: 0x00027D8B
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
