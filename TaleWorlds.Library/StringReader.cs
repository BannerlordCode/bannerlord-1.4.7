using System;
using System.Globalization;

namespace TaleWorlds.Library
{
	// Token: 0x02000091 RID: 145
	public class StringReader : IReader
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00012649 File Offset: 0x00010849
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x00012651 File Offset: 0x00010851
		public string Data { get; private set; }

		// Token: 0x06000517 RID: 1303 RVA: 0x0001265A File Offset: 0x0001085A
		private string GetNextToken()
		{
			string text = this._tokens[this._currentIndex];
			this._currentIndex++;
			return text;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00012677 File Offset: 0x00010877
		public StringReader(string data)
		{
			this.Data = data;
			this._tokens = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001269E File Offset: 0x0001089E
		public ISerializableObject ReadSerializableObject()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000126A5 File Offset: 0x000108A5
		public int ReadInt()
		{
			return Convert.ToInt32(this.GetNextToken());
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000126B2 File Offset: 0x000108B2
		public short ReadShort()
		{
			return Convert.ToInt16(this.GetNextToken());
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000126C0 File Offset: 0x000108C0
		public string ReadString()
		{
			int num = this.ReadInt();
			int i = 0;
			string text = "";
			while (i < num)
			{
				string nextToken = this.GetNextToken();
				text += nextToken;
				i = text.Length;
				if (i < num)
				{
					text += " ";
				}
			}
			if (text.Length != num)
			{
				throw new Exception("invalid string format, length does not match");
			}
			return text;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00012720 File Offset: 0x00010920
		public Color ReadColor()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			float num3 = this.ReadFloat();
			float num4 = this.ReadFloat();
			return new Color(num, num2, num3, num4);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00012754 File Offset: 0x00010954
		public bool ReadBool()
		{
			string nextToken = this.GetNextToken();
			return nextToken == "1" || (!(nextToken == "0") && Convert.ToBoolean(nextToken));
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001278C File Offset: 0x0001098C
		public float ReadFloat()
		{
			return Convert.ToSingle(this.GetNextToken(), CultureInfo.InvariantCulture);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001279E File Offset: 0x0001099E
		public uint ReadUInt()
		{
			return Convert.ToUInt32(this.GetNextToken());
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x000127AB File Offset: 0x000109AB
		public ulong ReadULong()
		{
			return Convert.ToUInt64(this.GetNextToken());
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000127B8 File Offset: 0x000109B8
		public long ReadLong()
		{
			return Convert.ToInt64(this.GetNextToken());
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000127C5 File Offset: 0x000109C5
		public byte ReadByte()
		{
			return Convert.ToByte(this.GetNextToken());
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000127D2 File Offset: 0x000109D2
		public byte[] ReadBytes(int length)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000127DC File Offset: 0x000109DC
		public Vec2 ReadVec2()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			return new Vec2(num, num2);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000127FC File Offset: 0x000109FC
		public Vec3 ReadVec3()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			float num3 = this.ReadFloat();
			float num4 = this.ReadFloat();
			return new Vec3(num, num2, num3, num4);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001282C File Offset: 0x00010A2C
		public Vec3i ReadVec3Int()
		{
			int num = this.ReadInt();
			int num2 = this.ReadInt();
			int num3 = this.ReadInt();
			return new Vec3i(num, num2, num3);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00012854 File Offset: 0x00010A54
		public sbyte ReadSByte()
		{
			return Convert.ToSByte(this.GetNextToken());
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00012861 File Offset: 0x00010A61
		public ushort ReadUShort()
		{
			return Convert.ToUInt16(this.GetNextToken());
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001286E File Offset: 0x00010A6E
		public double ReadDouble()
		{
			return Convert.ToDouble(this.GetNextToken());
		}

		// Token: 0x04000198 RID: 408
		private string[] _tokens;

		// Token: 0x04000199 RID: 409
		private int _currentIndex;
	}
}
