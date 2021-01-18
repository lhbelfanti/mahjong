using UnityEngine;

namespace Board.Tile
{
	public class DestroyBehaviour : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.gameObject.SetActive(false);
		}
	}
}
