using System;
using System.Threading;
using DisposeHeaven;

class Program
{
	static void Main()
	{
		var d = Disposable.None;
		d.Dispose();
		d.Dispose();

		d = Disposable.From(() => Console.WriteLine("Action!"));
		d.Dispose();
		d.Dispose();

		var cts = new CancellationTokenSource();
		cts.Token.Register(() => Console.WriteLine("CTS!"));
		d = Disposable.From(cts);
		d.Dispose();
		d.Dispose();

		Func<string, IDisposable> mk = s => Disposable.From(() => Console.WriteLine(s));

		d = Disposable.Combine(mk("a1"), mk("b1"));
		d.Dispose();
		d.Dispose();

		d = Disposable.Combine(mk("a2"), mk("b2"), mk("c2"));
		d.Dispose();
		d.Dispose();

		var r = new DisposableRef();
		r.Dispose();
		r.Dispose();
		r.Current = mk("first");
		r.Dispose();
		Console.WriteLine(r.Current == null);
		r.Current = mk("second");
		Console.WriteLine("setting third");
		r.Current = mk("third");
		r.Dispose();
		r.Dispose();
	}
}
