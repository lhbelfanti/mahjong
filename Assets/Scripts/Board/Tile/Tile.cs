using UnityEngine;

namespace Board.Tile
{
	public class Tile : MonoBehaviour
	{
		public enum States
		{
			Empty = 0,
			Single = 1,
			Double = 2,
			Dummy = 3
		}

		[SerializeField] private Color selected;
		[SerializeField] private Color unselected;
		[SerializeField] private Color hint;
		[SerializeField] private SpriteRenderer sprite;

		private bool _tileSelected;
		private Animator _animator;

		// Animation States
		private static readonly int MatchState = Animator.StringToHash("Match");
		private static readonly int ShakeState = Animator.StringToHash("Shake");
		private static readonly int CreationState = Animator.StringToHash("Create");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			_animator.SetTrigger(CreationState);
			Selected(false);
		}

		public void MatchAnim()
		{
			_animator.SetTrigger(MatchState);
		}

		public void WrongMatchAnim()
		{
			_animator.SetTrigger(ShakeState);
		}

		public void Selected(bool value)
		{
			sprite.color = value ? selected : unselected;
		}

		public void SetHint()
		{
			sprite.color = hint;
		}

		public string Id { get; set; }
		public States State { get; set; }
		public Vector3 Index { get; set; }
		public SpriteRenderer SpriteRenderer => sprite;
	}
}
