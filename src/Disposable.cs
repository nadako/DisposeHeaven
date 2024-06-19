using System;
using System.Threading;

namespace DisposeHeaven
{
	public static class Disposable
	{
		public static readonly IDisposable None = new NoneDisposable();

		public static IDisposable From(Action onDispose)
		{
			return new CallbackDisposable(onDispose);
		}

		public static IDisposable From(CancellationTokenSource tokenSource)
		{
			return new CancellationTokenDisposable(tokenSource);
		}

		public static IDisposable Combine(IDisposable first, IDisposable second)
		{
			return new CombinedDisposable(new[] { first, second });
		}

		public static IDisposable Combine(IDisposable first, IDisposable second, params IDisposable[] other)
		{
			var array = new IDisposable[other.Length + 2];
			array[0] = first;
			array[1] = second;
			Array.Copy(other, 0, array, 2, other.Length);
			return new CombinedDisposable(array);
		}

		class NoneDisposable : IDisposable
		{
			public void Dispose()
			{
			}
		}

		class CallbackDisposable : IDisposable
		{
			Action onDispose;

			public CallbackDisposable(Action onDispose)
			{
				this.onDispose = onDispose;
			}

			public void Dispose()
			{
				if (onDispose != null)
				{
					var action = onDispose;
					onDispose = null;
					action();
				}
			}
		}

		class CombinedDisposable : IDisposable
		{
			IDisposable[] disposables;

			public CombinedDisposable(IDisposable[] disposables)
			{
				this.disposables = disposables;
			}

			public void Dispose()
			{
				if (disposables != null)
				{
					var list = disposables;
					disposables = null;
					foreach (var disposable in list) disposable.Dispose();
				}
			}
		}

		class CancellationTokenDisposable : IDisposable
		{
			readonly CancellationTokenSource tokenSource;

			public CancellationTokenDisposable(CancellationTokenSource tokenSource)
			{
				this.tokenSource = tokenSource;
			}

			public void Dispose()
			{
				if (!tokenSource.IsCancellationRequested)
				{
					tokenSource.Cancel();
					tokenSource.Dispose();
				}
			}
		}
	}
}
