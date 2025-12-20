using UnityEngine;

public class PortalActivation : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.IsName("Activate"))
		{
			animator.SetBool("activated", value: true);
		}
	}
}
