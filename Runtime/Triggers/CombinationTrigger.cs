//using SOL.Utilities;
//using System;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class CombinationTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate;
//		public event Action OnDeactivate;

//		#region Component Fields
//		public int[] expectedCombination;
//		public bool bothWays;

//		[SerializeField] UltEvent _onCombinationCorrect;

//		[SerializeField] UltEvent _onCombinationIncorrect;
//		#endregion

//		private bool _active;
//		private int[] _currentCombination;
//		private int _currentMember;

//		public bool IsActive() => _active;

//		public void EnterNewMember(int member)
//		{
//			_currentCombination[_currentMember] = member;

//			if (++_currentMember == expectedCombination.Length)
//			{
//				var ok = true;

//				for (var i = 0; i < expectedCombination.Length; i++)
//				{
//					if (_currentCombination[i] != expectedCombination[i])
//					{
//						ok = false;
//						break;
//					}
//				}

//				if (!ok && bothWays)
//				{
//					ok = true;

//					for (var i = 0; i < expectedCombination.Length; i++)
//					{
//						if (_currentCombination[_currentCombination.Length - i - 1] != expectedCombination[i])
//						{
//							ok = false;
//							break;
//						}
//					}
//				}

//				if (ok)
//				{
//					_active = true;
//					_onCombinationCorrect.Invoke();
//					OnActivate?.Invoke();
//				}
//				else
//				{
//					_active = false;
//					_onCombinationIncorrect.Invoke();
//					OnDeactivate?.Invoke();
//				}
//			}
//		}

//		public void ResetCombination()
//		{
//			_currentCombination = new int[expectedCombination.Length];
//			_currentMember = 0;
//			_active = false;
//		}

//		#region Component Messages
//		private void Awake()
//		{
//			ResetCombination();
//		}
//		#endregion
//	}
//}
