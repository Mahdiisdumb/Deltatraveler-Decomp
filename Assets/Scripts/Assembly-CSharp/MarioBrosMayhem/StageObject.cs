using UnityEngine;

namespace MarioBrosMayhem
{
	public class StageObject : MonoBehaviour
	{
		protected Animator animator;

		protected SpriteRenderer sprite;

		protected AudioSource aud;

		protected CharacterController2D controller;

		protected virtual void Awake()
		{
			animator = GetComponent<Animator>();
			sprite = GetComponent<SpriteRenderer>();
			aud = GetComponent<AudioSource>();
			controller = GetComponent<CharacterController2D>();
		}
	}
}
