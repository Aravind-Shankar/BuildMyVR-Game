using UnityEngine;
using System.Collections;

public class AnimatorLinker : MonoBehaviour {
	public Animator[] linkedAnimators;

	public void StartLinkedAnimator(int nextAnimatorIndex) {
		linkedAnimators [nextAnimatorIndex].SetTrigger ("startPlaying");
	}
}
