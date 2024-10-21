using System;
using UnityEngine;

namespace Akela.Tools
{
	public class RunForSeconds : CustomYieldInstruction
	{
		private readonly Action _action;
		private readonly float _duration;
		private readonly float _startTime;

		public override bool keepWaiting
		{
			get
			{
				_action();

				return Time.time - _startTime < _duration;
			}
		}

		public RunForSeconds(Action action, float duration)
		{
			_action = action;
			_duration = duration;
			_startTime = Time.time;
		}
	}
}
