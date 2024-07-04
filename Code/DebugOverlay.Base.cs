global using Sandbox;
global using System;
global using System.Collections.Generic;

/// <summary>
/// Badass library for rendering debug information.
/// </summary>
public sealed partial class DebugOverlay : GameObjectSystem
{
	private static List<(
		Action Callback,
		TimeSince SinceCreated,
		float Duration
	)> _queue = new();

	public DebugOverlay( Scene scene ) : base( scene )
	{
		_queue.Clear();
		Listen( Stage.FinishUpdate, 0, RenderQueue, "DebugOverlay" );
	}

	private static void RenderQueue()
	{
		for ( int i = 0; i < _queue.Count; i++ )
		{
			var obj = _queue[i];
			if ( obj.SinceCreated >= obj.Duration )
			{
				_queue.RemoveAt( i );
				continue;
			}

			obj.Callback();
		}
	}

	private static void AddToQueue( Action callback, float time )
	{
		if ( time <= 0f )
		{
			callback();
			return;
		}

		_queue.Add( (
			Callback: callback,
			SinceCreated: 0f,
			Duration: time
		) );
	}
}
