using System;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x02000092 RID: 146
	public class StringWriter : IWriter
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001287B File Offset: 0x00010A7B
		public string Data
		{
			get
			{
				return this._stringBuilder.ToString();
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00012888 File Offset: 0x00010A88
		public StringWriter()
		{
			this._stringBuilder = new StringBuilder();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001289B File Offset: 0x00010A9B
		private void AddToken(string token)
		{
			this._stringBuilder.Append(token);
			this._stringBuilder.Append(" ");
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000128BB File Offset: 0x00010ABB
		public void WriteSerializableObject(ISerializableObject serializableObject)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000128C2 File Offset: 0x00010AC2
		public void WriteByte(byte value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000128D0 File Offset: 0x00010AD0
		public void WriteBytes(byte[] bytes)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000128D7 File Offset: 0x00010AD7
		public void WriteInt(int value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000128E5 File Offset: 0x00010AE5
		public void WriteShort(short value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000128F3 File Offset: 0x00010AF3
		public void WriteString(string value)
		{
			this.WriteInt(value.Length);
			this.AddToken(value);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00012908 File Offset: 0x00010B08
		public void WriteColor(Color value)
		{
			this.WriteFloat(value.Red);
			this.WriteFloat(value.Green);
			this.WriteFloat(value.Blue);
			this.WriteFloat(value.Alpha);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001293A File Offset: 0x00010B3A
		public void WriteBool(bool value)
		{
			this.AddToken(value ? "1" : "0");
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00012951 File Offset: 0x00010B51
		public void WriteFloat(float value)
		{
			this.AddToken((value == 0f) ? "0" : Convert.ToString(value));
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001296E File Offset: 0x00010B6E
		public void WriteUInt(uint value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001297C File Offset: 0x00010B7C
		public void WriteULong(ulong value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001298A File Offset: 0x00010B8A
		public void WriteLong(long value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00012998 File Offset: 0x00010B98
		public void WriteVec2(Vec2 vec2)
		{
			this.WriteFloat(vec2.x);
			this.WriteFloat(vec2.y);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000129B2 File Offset: 0x00010BB2
		public void WriteVec3(Vec3 vec3)
		{
			this.WriteFloat(vec3.x);
			this.WriteFloat(vec3.y);
			this.WriteFloat(vec3.z);
			this.WriteFloat(vec3.w);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000129E4 File Offset: 0x00010BE4
		public void WriteVec3Int(Vec3i vec3)
		{
			this.WriteInt(vec3.X);
			this.WriteInt(vec3.Y);
			this.WriteInt(vec3.Z);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00012A0A File Offset: 0x00010C0A
		public void WriteSByte(sbyte value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00012A18 File Offset: 0x00010C18
		public void WriteUShort(ushort value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00012A26 File Offset: 0x00010C26
		public void WriteDouble(double value)
		{
			this.AddToken((value == 0.0) ? "0" : Convert.ToString(value));
		}

		// Token: 0x0400019A RID: 410
		private StringBuilder _stringBuilder;
	}
}
