using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x000231D4 File Offset: 0x000213D4
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Deserialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00023235 File Offset: 0x00021435
		public string GetDeserializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00023242 File Offset: 0x00021442
		public override bool Read()
		{
			bool flag = this._innerReader.Read();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00023255 File Offset: 0x00021455
		public override int? ReadAsInt32()
		{
			int? num = this._innerReader.ReadAsInt32();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00023268 File Offset: 0x00021468
		[NullableContext(2)]
		public override string ReadAsString()
		{
			string text = this._innerReader.ReadAsString();
			this.WriteCurrentToken();
			return text;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002327B File Offset: 0x0002147B
		[NullableContext(2)]
		public override byte[] ReadAsBytes()
		{
			byte[] array = this._innerReader.ReadAsBytes();
			this.WriteCurrentToken();
			return array;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002328E File Offset: 0x0002148E
		public override decimal? ReadAsDecimal()
		{
			decimal? num = this._innerReader.ReadAsDecimal();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000232A1 File Offset: 0x000214A1
		public override double? ReadAsDouble()
		{
			double? num = this._innerReader.ReadAsDouble();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000232B4 File Offset: 0x000214B4
		public override bool? ReadAsBoolean()
		{
			bool? flag = this._innerReader.ReadAsBoolean();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000232C7 File Offset: 0x000214C7
		public override DateTime? ReadAsDateTime()
		{
			DateTime? dateTime = this._innerReader.ReadAsDateTime();
			this.WriteCurrentToken();
			return dateTime;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000232DA File Offset: 0x000214DA
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? dateTimeOffset = this._innerReader.ReadAsDateTimeOffset();
			this.WriteCurrentToken();
			return dateTimeOffset;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000232ED File Offset: 0x000214ED
		public void WriteCurrentToken()
		{
			this._textWriter.WriteToken(this._innerReader, false, false, true);
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00023303 File Offset: 0x00021503
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00023310 File Offset: 0x00021510
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0002331D File Offset: 0x0002151D
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0002332A File Offset: 0x0002152A
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00023338 File Offset: 0x00021538
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00023345 File Offset: 0x00021545
		[Nullable(2)]
		public override object Value
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00023352 File Offset: 0x00021552
		[Nullable(2)]
		public override Type ValueType
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002335F File Offset: 0x0002155F
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002336C File Offset: 0x0002156C
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00023390 File Offset: 0x00021590
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x000233B4 File Offset: 0x000215B4
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x040002C7 RID: 711
		private readonly JsonReader _innerReader;

		// Token: 0x040002C8 RID: 712
		private readonly JsonTextWriter _textWriter;

		// Token: 0x040002C9 RID: 713
		private readonly StringWriter _sw;
	}
}
