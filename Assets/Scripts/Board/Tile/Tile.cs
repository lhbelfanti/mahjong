using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	[SerializeField] private Color _selected;
	[SerializeField] private Color _unselected;
	[SerializeField] private SpriteRenderer _sprite;

	private bool _tileSelected;
	private string _tileId;
	private Vector3 _tileIndex;
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
	}

	public void MatchAnim()
	{
		_animator.SetTrigger(MatchState);
	}

	public void WrongMatchAnim()
	{
		_animator.SetTrigger(ShakeState);
	}

	public string TileId
	{
		get => _tileId;
		set => _tileId = value;
	}

	public Vector3 TileIndex
	{
		get => _tileIndex;
		set => _tileIndex = value;
	}

	public Color Selected => _selected;
	public Color Unselected => _unselected;
	public SpriteRenderer SpriteRenderer => _sprite;
}
