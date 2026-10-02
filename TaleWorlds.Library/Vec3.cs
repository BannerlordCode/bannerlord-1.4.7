using System;
using System.Numerics;
using System.Xml.Serialization;

namespace TaleWorlds.Library
{
	// Token: 0x020000A2 RID: 162
	[Serializable]
	public struct Vec3
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00014503 File Offset: 0x00012703
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001450B File Offset: 0x0001270B
		public float Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00014513 File Offset: 0x00012713
		public float Z
		{
			get
			{
				return this.z;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001451B File Offset: 0x0001271B
		public Vec3(float x = 0f, float y = 0f, float z = 0f, float w = -1f)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001453A File Offset: 0x0001273A
		public Vec3(Vec3 c, float w = -1f)
		{
			this.x = c.x;
			this.y = c.y;
			this.z = c.z;
			this.w = w;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00014567 File Offset: 0x00012767
		public Vec3(Vec2 xy, float z = 0f, float w = -1f)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001458F File Offset: 0x0001278F
		public Vec3(Vector3 vector3)
		{
			this = new Vec3(vector3.X, vector3.Y, vector3.Z, -1f);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000145AE File Offset: 0x000127AE
		public static Vec3 Abs(Vec3 vec)
		{
			return new Vec3(MathF.Abs(vec.x), MathF.Abs(vec.y), MathF.Abs(vec.z), -1f);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000145DB File Offset: 0x000127DB
		public static explicit operator Vector3(Vec3 vec3)
		{
			return new Vector3(vec3.x, vec3.y, vec3.z);
		}

		// Token: 0x170000A4 RID: 164
		public float this[int i]
		{
			get
			{
				switch (i)
				{
				case 0:
					return this.x;
				case 1:
					return this.y;
				case 2:
					return this.z;
				case 3:
					return this.w;
				default:
					throw new IndexOutOfRangeException("Vec3 out of bounds.");
				}
			}
			set
			{
				switch (i)
				{
				case 0:
					this.x = value;
					return;
				case 1:
					this.y = value;
					return;
				case 2:
					this.z = value;
					return;
				case 3:
					this.w = value;
					return;
				default:
					throw new IndexOutOfRangeException("Vec3 out of bounds.");
				}
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00014683 File Offset: 0x00012883
		public static float DotProduct(Vec3 v1, Vec3 v2)
		{
			return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000146AE File Offset: 0x000128AE
		public static Vec3 Lerp(Vec3 v1, Vec3 v2, float alpha)
		{
			return v1 * (1f - alpha) + v2 * alpha;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000146CC File Offset: 0x000128CC
		public static Vec3 Slerp(Vec3 start, Vec3 end, float percent)
		{
			float num = Vec3.DotProduct(start, end);
			num = MBMath.ClampFloat(num, -1f, 1f);
			float num2 = MathF.Acos(num) * percent;
			Vec3 vec = end - start * num;
			vec.Normalize();
			return start * MathF.Cos(num2) + vec * MathF.Sin(num2);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001472E File Offset: 0x0001292E
		public static Vec3 Vec3Max(Vec3 v1, Vec3 v2)
		{
			return new Vec3(MathF.Max(v1.x, v2.x), MathF.Max(v1.y, v2.y), MathF.Max(v1.z, v2.z), -1f);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001476D File Offset: 0x0001296D
		public static Vec3 Vec3Min(Vec3 v1, Vec3 v2)
		{
			return new Vec3(MathF.Min(v1.x, v2.x), MathF.Min(v1.y, v2.y), MathF.Min(v1.z, v2.z), -1f);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000147AC File Offset: 0x000129AC
		public static Vec3 CrossProduct(Vec3 va, Vec3 vb)
		{
			return new Vec3(va.y * vb.z - va.z * vb.y, va.z * vb.x - va.x * vb.z, va.x * vb.y - va.y * vb.x, -1f);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00014814 File Offset: 0x00012A14
		public static Vec3 ElementWiseProduct(Vec3 va, Vec3 vb)
		{
			return new Vec3(va.x * vb.x, va.y * vb.y, va.z * vb.z, -1f);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00014847 File Offset: 0x00012A47
		public static Vec3 ElementWiseDivision(Vec3 va, Vec3 vb)
		{
			return new Vec3(va.x / vb.x, va.y / vb.y, va.z / vb.z, -1f);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001487A File Offset: 0x00012A7A
		public static Vec3 operator -(Vec3 v)
		{
			return new Vec3(-v.x, -v.y, -v.z, -1f);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001489B File Offset: 0x00012A9B
		public static Vec3 operator +(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, -1f);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000148CE File Offset: 0x00012ACE
		public static Vec3 operator -(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, -1f);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00014901 File Offset: 0x00012B01
		public static Vec3 operator *(Vec3 v, float f)
		{
			return new Vec3(v.x * f, v.y * f, v.z * f, -1f);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00014925 File Offset: 0x00012B25
		public static Vec3 operator *(float f, Vec3 v)
		{
			return new Vec3(v.x * f, v.y * f, v.z * f, -1f);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001494C File Offset: 0x00012B4C
		public static Vec3 operator *(Vec3 v, MatrixFrame frame)
		{
			return new Vec3(frame.rotation.s.x * v.x + frame.rotation.f.x * v.y + frame.rotation.u.x * v.z + frame.origin.x * v.w, frame.rotation.s.y * v.x + frame.rotation.f.y * v.y + frame.rotation.u.y * v.z + frame.origin.y * v.w, frame.rotation.s.z * v.x + frame.rotation.f.z * v.y + frame.rotation.u.z * v.z + frame.origin.z * v.w, frame.rotation.s.w * v.x + frame.rotation.f.w * v.y + frame.rotation.u.w * v.z + frame.origin.w * v.w);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00014AC6 File Offset: 0x00012CC6
		public static Vec3 operator /(Vec3 v, float f)
		{
			f = 1f / f;
			return new Vec3(v.x * f, v.y * f, v.z * f, -1f);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00014AF3 File Offset: 0x00012CF3
		public static bool operator ==(Vec3 v1, Vec3 v2)
		{
			return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00014B21 File Offset: 0x00012D21
		public static bool operator !=(Vec3 v1, Vec3 v2)
		{
			return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00014B52 File Offset: 0x00012D52
		public float Length
		{
			get
			{
				return MathF.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00014B82 File Offset: 0x00012D82
		public float LengthSquared
		{
			get
			{
				return this.x * this.x + this.y * this.y + this.z * this.z;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00014BB0 File Offset: 0x00012DB0
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && (((Vec3)obj).x == this.x && ((Vec3)obj).y == this.y) && ((Vec3)obj).z == this.z;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00014C1A File Offset: 0x00012E1A
		public override int GetHashCode()
		{
			return (int)(1001f * this.x + 10039f * this.y + 117f * this.z);
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00014C44 File Offset: 0x00012E44
		public bool IsValid
		{
			get
			{
				return !float.IsNaN(this.x) && !float.IsNaN(this.y) && !float.IsNaN(this.z) && !float.IsInfinity(this.x) && !float.IsInfinity(this.y) && !float.IsInfinity(this.z);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00014CA4 File Offset: 0x00012EA4
		public bool IsValidXYZW
		{
			get
			{
				return !float.IsNaN(this.x) && !float.IsNaN(this.y) && !float.IsNaN(this.z) && !float.IsNaN(this.w) && !float.IsInfinity(this.x) && !float.IsInfinity(this.y) && !float.IsInfinity(this.z) && !float.IsInfinity(this.w);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00014D1C File Offset: 0x00012F1C
		public bool IsUnit
		{
			get
			{
				float lengthSquared = this.LengthSquared;
				return lengthSquared > 0.98010004f && lengthSquared < 1.0201f;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00014D42 File Offset: 0x00012F42
		public bool IsNonZero
		{
			get
			{
				return this.x != 0f || this.y != 0f || this.z != 0f;
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00014D70 File Offset: 0x00012F70
		public Vec3 NormalizedCopy()
		{
			Vec3 vec = this;
			vec.Normalize();
			return vec;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00014D90 File Offset: 0x00012F90
		public float Normalize()
		{
			float length = this.Length;
			if (length > 1E-05f)
			{
				float num = 1f / length;
				this.x *= num;
				this.y *= num;
				this.z *= num;
			}
			else
			{
				this.x = 0f;
				this.y = 1f;
				this.z = 0f;
			}
			return length;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00014E04 File Offset: 0x00013004
		public void ClampMagnitude(float min, float max)
		{
			float num = this.Normalize();
			this *= MathF.Clamp(num, min, max);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00014E34 File Offset: 0x00013034
		public Vec3 ClampedCopy(float min, float max)
		{
			Vec3 vec = this;
			vec.x = MathF.Clamp(vec.x, min, max);
			vec.y = MathF.Clamp(vec.y, min, max);
			vec.z = MathF.Clamp(vec.z, min, max);
			return vec;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00014E88 File Offset: 0x00013088
		public Vec3 ClampedCopy(float min, float max, out bool valueClamped)
		{
			Vec3 vec = this;
			valueClamped = false;
			if (vec.x < min)
			{
				vec.x = min;
				valueClamped = true;
			}
			else if (vec.x > max)
			{
				vec.x = max;
				valueClamped = true;
			}
			if (vec.y < min)
			{
				vec.y = min;
				valueClamped = true;
			}
			else if (vec.y > max)
			{
				vec.y = max;
				valueClamped = true;
			}
			if (vec.z < min)
			{
				vec.z = min;
				valueClamped = true;
			}
			else if (vec.z > max)
			{
				vec.z = max;
				valueClamped = true;
			}
			return vec;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00014F20 File Offset: 0x00013120
		public void NormalizeWithoutChangingZ()
		{
			this.z = MBMath.ClampFloat(this.z, -0.99999f, 0.99999f);
			float length = this.AsVec2.Length;
			float num = MathF.Sqrt(1f - this.z * this.z);
			if (length < num - 1E-07f || length > num + 1E-07f)
			{
				if (length > 1E-09f)
				{
					float num2 = num / length;
					this.x *= num2;
					this.y *= num2;
					return;
				}
				this.x = 0f;
				this.y = num;
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00014FBF File Offset: 0x000131BF
		public Vec3 CrossProductWithUp()
		{
			return new Vec3(this.y, -this.x, 0f, -1f);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00014FDD File Offset: 0x000131DD
		public Vec3 CrossProductWithUpAsLeftParameter()
		{
			return new Vec3(-this.y, this.x, 0f, -1f);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00014FFC File Offset: 0x000131FC
		public bool NearlyEquals(in Vec3 v, float epsilon = 1E-05f)
		{
			return MathF.Abs(this.x - v.x) < epsilon && MathF.Abs(this.y - v.y) < epsilon && MathF.Abs(this.z - v.z) < epsilon;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001504C File Offset: 0x0001324C
		public void RotateAboutX(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			float num3 = this.y * num2 - this.z * num;
			this.z = this.z * num2 + this.y * num;
			this.y = num3;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00015094 File Offset: 0x00013294
		public void RotateAboutY(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			float num3 = this.x * num2 + this.z * num;
			this.z = this.z * num2 - this.x * num;
			this.x = num3;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000150DC File Offset: 0x000132DC
		public void RotateAboutZ(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			float num3 = this.x * num2 - this.y * num;
			this.y = this.y * num2 + this.x * num;
			this.x = num3;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00015124 File Offset: 0x00013324
		public Vec3 RotateAboutAnArbitraryVector(Vec3 vec, float a)
		{
			float num = vec.x;
			float num2 = vec.y;
			float num3 = vec.z;
			float num4 = num * this.x;
			float num5 = num * this.y;
			float num6 = num * this.z;
			float num7 = num2 * this.x;
			float num8 = num2 * this.y;
			float num9 = num2 * this.z;
			float num10 = num3 * this.x;
			float num11 = num3 * this.y;
			float num12 = num3 * this.z;
			float num13;
			float num14;
			MathF.SinCos(a, out num13, out num14);
			return new Vec3
			{
				x = num * (num4 + num8 + num12) + (this.x * (num2 * num2 + num3 * num3) - num * (num8 + num12)) * num14 + (-num11 + num9) * num13,
				y = num2 * (num4 + num8 + num12) + (this.y * (num * num + num3 * num3) - num2 * (num4 + num12)) * num14 + (num10 - num6) * num13,
				z = num3 * (num4 + num8 + num12) + (this.z * (num * num + num2 * num2) - num3 * (num4 + num8)) * num14 + (-num7 + num5) * num13
			};
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001524C File Offset: 0x0001344C
		public Vec3 Reflect(Vec3 normal)
		{
			return this - normal * (2f * Vec3.DotProduct(this, normal));
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00015271 File Offset: 0x00013471
		public Vec3 ProjectOnUnitVector(Vec3 ov)
		{
			return ov * (this.x * ov.x + this.y * ov.y + this.z * ov.z);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000152A4 File Offset: 0x000134A4
		public float DistanceSquared(Vec3 v)
		{
			return (v.x - this.x) * (v.x - this.x) + (v.y - this.y) * (v.y - this.y) + (v.z - this.z) * (v.z - this.z);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00015304 File Offset: 0x00013504
		public float Distance(Vec3 v)
		{
			return MathF.Sqrt((v.x - this.x) * (v.x - this.x) + (v.y - this.y) * (v.y - this.y) + (v.z - this.z) * (v.z - this.z));
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001536C File Offset: 0x0001356C
		public Vec3 RotateVectorToXYPlane()
		{
			float length = this.Length;
			Vec3 vec = this;
			vec.z = 0f;
			vec.Normalize();
			return vec * length;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000153A2 File Offset: 0x000135A2
		public static float AngleBetweenTwoVectors(Vec3 v1, Vec3 v2)
		{
			return MathF.Acos(MathF.Clamp(Vec3.DotProduct(v1, v2) / (v1.Length * v2.Length), -1f, 1f));
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x000153CF File Offset: 0x000135CF
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x000153E2 File Offset: 0x000135E2
		public Vec2 AsVec2
		{
			get
			{
				return new Vec2(this.x, this.y);
			}
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000153FC File Offset: 0x000135FC
		public override string ToString()
		{
			return string.Concat(new object[] { "(", this.x, ", ", this.y, ", ", this.z, ")" });
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00015460 File Offset: 0x00013660
		public string ToString(string format)
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(format),
				", ",
				this.y.ToString(format),
				", ",
				this.z.ToString(format),
				")"
			});
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000154C8 File Offset: 0x000136C8
		public uint ToARGB
		{
			get
			{
				uint num = (uint)(this.w * 256f);
				uint num2 = (uint)(this.x * 256f);
				uint num3 = (uint)(this.y * 256f);
				uint num4 = (uint)(this.z * 256f);
				return (MathF.Min(num, 255U) << 24) | (MathF.Min(num2, 255U) << 16) | (MathF.Min(num3, 255U) << 8) | MathF.Min(num4, 255U);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00015542 File Offset: 0x00013742
		public float RotationZ
		{
			get
			{
				return MathF.Atan2(-this.x, this.y);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00015556 File Offset: 0x00013756
		public float RotationX
		{
			get
			{
				return MathF.Atan2(this.z, MathF.Sqrt(this.x * this.x + this.y * this.y));
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00015584 File Offset: 0x00013784
		public static Vec3 Parse(string input)
		{
			input = input.Replace(" ", "");
			string[] array = input.Split(new char[] { ',' });
			if (array.Length < 3 || array.Length > 4)
			{
				throw new ArgumentOutOfRangeException();
			}
			float num = float.Parse(array[0]);
			float num2 = float.Parse(array[1]);
			float num3 = float.Parse(array[2]);
			float num4 = ((array.Length == 4) ? float.Parse(array[3]) : (-1f));
			return new Vec3(num, num2, num3, num4);
		}

		// Token: 0x040001C5 RID: 453
		[XmlAttribute]
		public float x;

		// Token: 0x040001C6 RID: 454
		[XmlAttribute]
		public float y;

		// Token: 0x040001C7 RID: 455
		[XmlAttribute]
		public float z;

		// Token: 0x040001C8 RID: 456
		[XmlAttribute]
		public float w;

		// Token: 0x040001C9 RID: 457
		public static readonly Vec3 Side = new Vec3(1f, 0f, 0f, -1f);

		// Token: 0x040001CA RID: 458
		public static readonly Vec3 Forward = new Vec3(0f, 1f, 0f, -1f);

		// Token: 0x040001CB RID: 459
		public static readonly Vec3 Up = new Vec3(0f, 0f, 1f, -1f);

		// Token: 0x040001CC RID: 460
		public static readonly Vec3 One = new Vec3(1f, 1f, 1f, -1f);

		// Token: 0x040001CD RID: 461
		public static readonly Vec3 Zero = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x040001CE RID: 462
		public static readonly Vec3 Invalid = new Vec3(float.NaN, float.NaN, float.NaN, -1f);

		// Token: 0x020000EE RID: 238
		public struct StackArray8Vec3
		{
			// Token: 0x17000108 RID: 264
			public Vec3 this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._element0;
					case 1:
						return this._element1;
					case 2:
						return this._element2;
					case 3:
						return this._element3;
					case 4:
						return this._element4;
					case 5:
						return this._element5;
					case 6:
						return this._element6;
					case 7:
						return this._element7;
					default:
						Debug.FailedAssert("Index out of range.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Vec3.cs", "Item", 40);
						return Vec3.Zero;
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._element0 = value;
						return;
					case 1:
						this._element1 = value;
						return;
					case 2:
						this._element2 = value;
						return;
					case 3:
						this._element3 = value;
						return;
					case 4:
						this._element4 = value;
						return;
					case 5:
						this._element5 = value;
						return;
					case 6:
						this._element6 = value;
						return;
					case 7:
						this._element7 = value;
						return;
					default:
						Debug.FailedAssert("Index out of range.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Vec3.cs", "Item", 58);
						return;
					}
				}
			}

			// Token: 0x040002FF RID: 767
			private Vec3 _element0;

			// Token: 0x04000300 RID: 768
			private Vec3 _element1;

			// Token: 0x04000301 RID: 769
			private Vec3 _element2;

			// Token: 0x04000302 RID: 770
			private Vec3 _element3;

			// Token: 0x04000303 RID: 771
			private Vec3 _element4;

			// Token: 0x04000304 RID: 772
			private Vec3 _element5;

			// Token: 0x04000305 RID: 773
			private Vec3 _element6;

			// Token: 0x04000306 RID: 774
			private Vec3 _element7;

			// Token: 0x04000307 RID: 775
			public const int Length = 8;
		}
	}
}
