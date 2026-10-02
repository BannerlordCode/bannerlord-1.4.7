using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008A RID: 138
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x0001C2AD File Offset: 0x0001A4AD
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, [Nullable(2)] JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001C2E0 File Offset: 0x0001A4E0
		private T GetTokenValue<[Nullable(2)] T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			return (T)((object)global::System.Convert.ChangeType(((JValue)value).Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001C314 File Offset: 0x0001A514
		[return: Nullable(2)]
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(jtoken, type, this._contract, this._member);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001C360 File Offset: 0x0001A560
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue = value as JValue;
			return global::System.Convert.ChangeType((jvalue != null) ? jvalue.Value : value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001C396 File Offset: 0x0001A596
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001C39F File Offset: 0x0001A59F
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001C3A8 File Offset: 0x0001A5A8
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001C3B1 File Offset: 0x0001A5B1
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001C3BA File Offset: 0x0001A5BA
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001C3C3 File Offset: 0x0001A5C3
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001C3CC File Offset: 0x0001A5CC
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001C3D5 File Offset: 0x0001A5D5
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001C3DE File Offset: 0x0001A5DE
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001C3E7 File Offset: 0x0001A5E7
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001C3F0 File Offset: 0x0001A5F0
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001C3F9 File Offset: 0x0001A5F9
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001C402 File Offset: 0x0001A602
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001C40B File Offset: 0x0001A60B
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001C414 File Offset: 0x0001A614
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x04000270 RID: 624
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x04000271 RID: 625
		private readonly JsonISerializableContract _contract;

		// Token: 0x04000272 RID: 626
		[Nullable(2)]
		private readonly JsonProperty _member;
	}
}
