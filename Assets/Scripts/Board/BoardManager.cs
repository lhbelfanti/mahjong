using UnityEngine;

public class BoardManager : MonoBehaviour
{
	private BoardCreator _boardCreator;
	private BoardSelector _boardSelector;

	private void Awake()
	{
		_boardCreator = GetComponent<BoardCreator>();
	}

	void Start()
	{
		_boardCreator.CreateBoard("1");
	}


}
