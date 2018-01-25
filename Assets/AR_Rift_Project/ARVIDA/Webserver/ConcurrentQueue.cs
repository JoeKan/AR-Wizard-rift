using System;
using System.Collections.Generic;


public class ConcurrentQueue<tvalue>
{
	private readonly object syncLock = new object ();
	private Queue<tvalue> queue = new Queue<tvalue> ();
	
	public ConcurrentQueue ()
	{
	}
	
	public void Enqueue(tvalue val)
	{
		lock (syncLock) 
		{
			queue.Enqueue (val);
		}
	}

	public int Count
	{
		get {return queue.Count;}
	}
	
	public tvalue Dequeue()
	{
		lock (syncLock)
		{
			return queue.Dequeue();
		}
	}
}

