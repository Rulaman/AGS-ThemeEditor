namespace Notify
{
	public class NotifyBase : System.ComponentModel.INotifyPropertyChanged, System.Windows.Forms.IBindableComponent
	{
		#region INotifyPropertyChanged

		private System.Collections.Generic.Dictionary<string, object> PropertyDict = new System.Collections.Generic.Dictionary<string, object>();

		/// <summary>Gets the value of a property</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		protected T Get<T>(T defaultvalue, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
		{
			System.Diagnostics.Debug.Assert(name != null, "name != null");

			if ( PropertyDict.TryGetValue(name, out object value) )
			{
				if ( value == null )
				{
					PropertyDict.Add(name, defaultvalue);
				}

				return value == null ? defaultvalue : (T)value;
			}

			else
			{
				PropertyDict.Add(name, defaultvalue);
			}

			return defaultvalue;
		}

		/// <summary>Sets the value of a property</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <remarks>Use this overload when implicitly naming the property</remarks>
		protected bool Notify<T>(T value, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
		{
			System.Diagnostics.Debug.Assert(name != null, "name != null");

			if ( PropertyDict.TryGetValue(name, out object o) )
			{
				if ( Equals(value, Get<T>(default(T), name)) )
				{
					return false;
				}

				PropertyDict[name] = value;
			}
			else
			{
				PropertyDict.Add(name, value);
			}

			OnPropertyChanged(name);

			return true;
		}

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] private System.Windows.Forms.Form MainForm = null;

		protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
		{
			System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;

			if ( handler != null )
			{
				if ( MainForm == null )
				{
					if ( System.Windows.Forms.Application.OpenForms.Count > 0 )
					{
						MainForm = System.Windows.Forms.Application.OpenForms[0];
					}
				}

				if ( MainForm != null )
				{
					if ( MainForm.InvokeRequired )
					{
						// We are not in UI Thread now
						MainForm.Invoke(handler, new object[] { this, new System.ComponentModel.PropertyChangedEventArgs(propertyName) });
					}
					else
					{
						handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
					}
				}
			}
		}

		#endregion INotifyPropertyChanged

		#region IBindableComponent

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private System.Windows.Forms.BindingContext bindingContext;

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private System.Windows.Forms.ControlBindingsCollection dataBindings;

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never), System.ComponentModel.Browsable(false)]
		public System.Windows.Forms.BindingContext BindingContext
		{
			get
			{
				if ( bindingContext == null )
				{
					bindingContext = new System.Windows.Forms.BindingContext();
				}
				return bindingContext;
			}
			set
			{
				bindingContext = value;
			}
		}

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never), System.ComponentModel.Browsable(false)]
		public System.Windows.Forms.ControlBindingsCollection DataBindings
		{
			get
			{
				if ( dataBindings == null )
				{
					dataBindings = new System.Windows.Forms.ControlBindingsCollection(this);
				}
				return dataBindings;
			}
		}

		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never), System.ComponentModel.Browsable(false)]
		public System.ComponentModel.ISite Site
		{
			get { return null; }
			set { }

		}

		public event System.EventHandler Disposed;

		public void Dispose()
		{
			Disposed?.Invoke(this, new System.EventArgs());
		}

		#endregion IBindableComponent
	}
}
