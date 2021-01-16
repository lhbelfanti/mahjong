using UnityEngine;

public class BoardMatcher : MonoBehaviour
{
	private BoardCreator _boardCreator;

	private void Awake()
	{
		_boardCreator = GetComponent<BoardCreator>();
	}
}
