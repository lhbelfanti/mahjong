using UnityEngine;
using TileStates = Board.Tile.TileCreator.TileStates;

namespace Board.Tile
{
	public class Tile : MonoBehaviour
	{
		[SerializeField] private Color selected;
		[SerializeField] private Color unselected;
		[SerializeField] private Color hint;
		[SerializeField] private GameObject card;

		private bool _tileSelected;
		private Animator _animator;
		private Material _material;
		private Renderer _renderer;

		// Animation States
		private static readonly int MatchState = Animator.StringToHash("Match");
		private static readonly int ShakeState = Animator.StringToHash("Shake");
		private static readonly int CreationState = Animator.StringToHash("Create");

		// Shader Properties
		private static readonly int SelectedColor = Shader.PropertyToID("SelectedColor");
		private static readonly int Texture = Shader.PropertyToID("Texture");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_material = card.GetComponent<Renderer>().material;
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
			_material.SetColor(SelectedColor, value ? selected : unselected);
		}

		public void SetHint()
		{
			_material.SetColor(SelectedColor, hint);
		}

		public void SetTexture(Texture2D texture)
		{
			_material.SetTexture(Texture, texture);
		}

		public string Id { get; set; }
		public TileStates State { get; set; }
		public Vector3 Index { get; set; }
	}
}
