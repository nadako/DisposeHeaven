using System;

namespace DisposeHeaven
{
	public class DisposableRef : IDisposable
	{
		IDisposable current;

		public IDisposable Current
		{
			get => current;
			set
			{
				current?.Dispose();
				current = value;
			}
		}

		public void Dispose()
		{
			if (current != null)
			{
				var disposable = current;
				current = null;
				disposable.Dispose();
			}
		}
	}
}
