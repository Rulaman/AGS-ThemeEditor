// You only need this file, if you compile against .NET 2.0

namespace System
{
	public delegate void Action();
}

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute { }
}
