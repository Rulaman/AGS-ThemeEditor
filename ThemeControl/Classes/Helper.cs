using System;

namespace ThemeControl.Classes
{
	namespace System
	{
		public delegate void Action();
	}

	namespace System.Runtime.CompilerServices
	{
		[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
		public sealed class ExtensionAttribute : Attribute { }
	}
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
