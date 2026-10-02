using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003D RID: 61
	public class PropertyOwnerObject
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x0001013E File Offset: 0x0000E33E
		protected void OnPropertyChanged<T>(T value, [CallerMemberName] string propertyName = null) where T : class
		{
			Action<PropertyOwnerObject, string, object> propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, propertyName, value);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00010158 File Offset: 0x0000E358
		protected void OnPropertyChanged(int value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, int> action = this.intPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001016D File Offset: 0x0000E36D
		protected void OnPropertyChanged(float value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, float> action = this.floatPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010182 File Offset: 0x0000E382
		protected void OnPropertyChanged(bool value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, bool> action = this.boolPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010197 File Offset: 0x0000E397
		protected void OnPropertyChanged(Vec2 value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, Vec2> vec2PropertyChanged = this.Vec2PropertyChanged;
			if (vec2PropertyChanged == null)
			{
				return;
			}
			vec2PropertyChanged(this, propertyName, value);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000101AC File Offset: 0x0000E3AC
		protected void OnPropertyChanged(Vector2 value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, Vector2> vector2PropertyChanged = this.Vector2PropertyChanged;
			if (vector2PropertyChanged == null)
			{
				return;
			}
			vector2PropertyChanged(this, propertyName, value);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000101C1 File Offset: 0x0000E3C1
		protected void OnPropertyChanged(double value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, double> action = this.doublePropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000101D6 File Offset: 0x0000E3D6
		protected void OnPropertyChanged(uint value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, uint> action = this.uintPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000101EB File Offset: 0x0000E3EB
		protected void OnPropertyChanged(Color value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, Color> colorPropertyChanged = this.ColorPropertyChanged;
			if (colorPropertyChanged == null)
			{
				return;
			}
			colorPropertyChanged(this, propertyName, value);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600040F RID: 1039 RVA: 0x00010200 File Offset: 0x0000E400
		// (remove) Token: 0x06000410 RID: 1040 RVA: 0x00010238 File Offset: 0x0000E438
		public event Action<PropertyOwnerObject, string, object> PropertyChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000411 RID: 1041 RVA: 0x00010270 File Offset: 0x0000E470
		// (remove) Token: 0x06000412 RID: 1042 RVA: 0x000102A8 File Offset: 0x0000E4A8
		public event Action<PropertyOwnerObject, string, bool> boolPropertyChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000413 RID: 1043 RVA: 0x000102E0 File Offset: 0x0000E4E0
		// (remove) Token: 0x06000414 RID: 1044 RVA: 0x00010318 File Offset: 0x0000E518
		public event Action<PropertyOwnerObject, string, int> intPropertyChanged;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000415 RID: 1045 RVA: 0x00010350 File Offset: 0x0000E550
		// (remove) Token: 0x06000416 RID: 1046 RVA: 0x00010388 File Offset: 0x0000E588
		public event Action<PropertyOwnerObject, string, float> floatPropertyChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000417 RID: 1047 RVA: 0x000103C0 File Offset: 0x0000E5C0
		// (remove) Token: 0x06000418 RID: 1048 RVA: 0x000103F8 File Offset: 0x0000E5F8
		public event Action<PropertyOwnerObject, string, Vec2> Vec2PropertyChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000419 RID: 1049 RVA: 0x00010430 File Offset: 0x0000E630
		// (remove) Token: 0x0600041A RID: 1050 RVA: 0x00010468 File Offset: 0x0000E668
		public event Action<PropertyOwnerObject, string, Vector2> Vector2PropertyChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600041B RID: 1051 RVA: 0x000104A0 File Offset: 0x0000E6A0
		// (remove) Token: 0x0600041C RID: 1052 RVA: 0x000104D8 File Offset: 0x0000E6D8
		public event Action<PropertyOwnerObject, string, double> doublePropertyChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600041D RID: 1053 RVA: 0x00010510 File Offset: 0x0000E710
		// (remove) Token: 0x0600041E RID: 1054 RVA: 0x00010548 File Offset: 0x0000E748
		public event Action<PropertyOwnerObject, string, uint> uintPropertyChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600041F RID: 1055 RVA: 0x00010580 File Offset: 0x0000E780
		// (remove) Token: 0x06000420 RID: 1056 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public event Action<PropertyOwnerObject, string, Color> ColorPropertyChanged;
	}
}
