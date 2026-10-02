using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(1)]
	[Nullable(0)]
	internal class BooleanQueryExpression : QueryExpression
	{
		// Token: 0x06000BF7 RID: 3063 RVA: 0x0002F918 File Offset: 0x0002DB18
		public BooleanQueryExpression(QueryOperator @operator, object left, [Nullable(2)] object right)
			: base(@operator)
		{
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002F930 File Offset: 0x0002DB30
		private IEnumerable<JToken> GetResult(JToken root, JToken t, [Nullable(2)] object o)
		{
			JToken jtoken = o as JToken;
			if (jtoken != null)
			{
				return new JToken[] { jtoken };
			}
			List<PathFilter> list = o as List<PathFilter>;
			if (list != null)
			{
				return JPath.Evaluate(list, root, t, null);
			}
			return CollectionUtils.ArrayEmpty<JToken>();
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002F96C File Offset: 0x0002DB6C
		public override bool IsMatch(JToken root, JToken t, [Nullable(2)] JsonSelectSettings settings)
		{
			if (this.Operator == QueryOperator.Exists)
			{
				return this.GetResult(root, t, this.Left).Any<JToken>();
			}
			using (IEnumerator<JToken> enumerator = this.GetResult(root, t, this.Left).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IEnumerable<JToken> result = this.GetResult(root, t, this.Right);
					ICollection<JToken> collection = (result as ICollection<JToken>) ?? result.ToList<JToken>();
					do
					{
						JToken jtoken = enumerator.Current;
						foreach (JToken jtoken2 in collection)
						{
							if (this.MatchTokens(jtoken, jtoken2, settings))
							{
								return true;
							}
						}
					}
					while (enumerator.MoveNext());
				}
			}
			return false;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002FA48 File Offset: 0x0002DC48
		private bool MatchTokens(JToken leftResult, JToken rightResult, [Nullable(2)] JsonSelectSettings settings)
		{
			JValue jvalue = leftResult as JValue;
			if (jvalue != null)
			{
				JValue jvalue2 = rightResult as JValue;
				if (jvalue2 != null)
				{
					switch (this.Operator)
					{
					case QueryOperator.Equals:
						if (BooleanQueryExpression.EqualsWithStringCoercion(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.NotEquals:
						if (!BooleanQueryExpression.EqualsWithStringCoercion(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.Exists:
						return true;
					case QueryOperator.LessThan:
						if (jvalue.CompareTo(jvalue2) < 0)
						{
							return true;
						}
						return false;
					case QueryOperator.LessThanOrEquals:
						if (jvalue.CompareTo(jvalue2) <= 0)
						{
							return true;
						}
						return false;
					case QueryOperator.GreaterThan:
						if (jvalue.CompareTo(jvalue2) > 0)
						{
							return true;
						}
						return false;
					case QueryOperator.GreaterThanOrEquals:
						if (jvalue.CompareTo(jvalue2) >= 0)
						{
							return true;
						}
						return false;
					case QueryOperator.And:
					case QueryOperator.Or:
						return false;
					case QueryOperator.RegexEquals:
						if (BooleanQueryExpression.RegexEquals(jvalue, jvalue2, settings))
						{
							return true;
						}
						return false;
					case QueryOperator.StrictEquals:
						if (BooleanQueryExpression.EqualsWithStrictMatch(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					case QueryOperator.StrictNotEquals:
						if (!BooleanQueryExpression.EqualsWithStrictMatch(jvalue, jvalue2))
						{
							return true;
						}
						return false;
					default:
						return false;
					}
				}
			}
			QueryOperator @operator = this.Operator;
			if (@operator - QueryOperator.NotEquals <= 1)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0002FB2C File Offset: 0x0002DD2C
		private static bool RegexEquals(JValue input, JValue pattern, [Nullable(2)] JsonSelectSettings settings)
		{
			if (input.Type != JTokenType.String || pattern.Type != JTokenType.String)
			{
				return false;
			}
			string text = (string)pattern.Value;
			int num = text.LastIndexOf('/');
			string text2 = text.Substring(1, num - 1);
			string text3 = text.Substring(num + 1);
			TimeSpan timeSpan = ((settings != null) ? settings.RegexMatchTimeout : null) ?? Regex.InfiniteMatchTimeout;
			return Regex.IsMatch((string)input.Value, text2, MiscellaneousUtils.GetRegexOptions(text3), timeSpan);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002FBBC File Offset: 0x0002DDBC
		internal static bool EqualsWithStringCoercion(JValue value, JValue queryValue)
		{
			if (value.Equals(queryValue))
			{
				return true;
			}
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (queryValue.Type != JTokenType.String)
			{
				return false;
			}
			string text = (string)queryValue.Value;
			string text2;
			switch (value.Type)
			{
			case JTokenType.Date:
			{
				using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
				{
					object value2 = value.Value;
					if (value2 is DateTimeOffset)
					{
						DateTimeOffset dateTimeOffset = (DateTimeOffset)value2;
						DateTimeUtils.WriteDateTimeOffsetString(stringWriter, dateTimeOffset, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					else
					{
						DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)value.Value, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
					}
					text2 = stringWriter.ToString();
					goto IL_0122;
				}
				break;
			}
			case JTokenType.Raw:
				return false;
			case JTokenType.Bytes:
				break;
			case JTokenType.Guid:
			case JTokenType.TimeSpan:
				text2 = value.Value.ToString();
				goto IL_0122;
			case JTokenType.Uri:
				text2 = ((Uri)value.Value).OriginalString;
				goto IL_0122;
			default:
				return false;
			}
			text2 = Convert.ToBase64String((byte[])value.Value);
			IL_0122:
			return string.Equals(text2, text, StringComparison.Ordinal);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0002FD04 File Offset: 0x0002DF04
		internal static bool EqualsWithStrictMatch(JValue value, JValue queryValue)
		{
			if ((value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float) || (value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer))
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			return value.Type == queryValue.Type && value.Equals(queryValue);
		}

		// Token: 0x040003C5 RID: 965
		public readonly object Left;

		// Token: 0x040003C6 RID: 966
		[Nullable(2)]
		public readonly object Right;
	}
}
