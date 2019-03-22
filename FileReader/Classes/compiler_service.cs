namespace System.Runtime.CompilerServices
{
	// Summary: Allows you to obtain the method or property name of the caller to the method.
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class CallerMemberNameAttribute : Attribute
	{
		// Summary: Initializes a new instance of the System.Runtime.CompilerServices.CallerMemberNameAttribute class.
		public CallerMemberNameAttribute()
		{
		}
	}

	// Summary: Allows you to obtain the full path of the source file that contains the caller.
	// This is the file path at the time of compile.
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class CallerFilePathAttribute : Attribute
	{
		// Summary: Initializes a new instance of the System.Runtime.CompilerServices.CallerFilePathAttribute class.
		public CallerFilePathAttribute()
		{
		}
	}

	// Summary: Allows you to obtain the line number in the source file at which the method is called.
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	internal sealed class CallerLineNumberAttribute : Attribute
	{
		// Summary: Initializes a new instance of the System.Runtime.CompilerServices.CallerLineNumberAttribute class.
		public CallerLineNumberAttribute()
		{
		}
	}
}
