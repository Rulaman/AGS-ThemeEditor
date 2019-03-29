// You only need this file, if you compile against .NET 2.0

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	internal sealed class ExtensionAttribute : Attribute { }
}

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class CallerMemberNameAttribute : Attribute
	{
		public CallerMemberNameAttribute()
		{
		}
	}

	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class CallerFilePathAttribute : Attribute
	{
		public CallerFilePathAttribute()
		{
		}
	}

	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class CallerLineNumberAttribute : Attribute
	{
		public CallerLineNumberAttribute()
		{
		}
	}
}
