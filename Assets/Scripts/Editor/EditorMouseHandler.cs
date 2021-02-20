using LevelEditor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(EditorTile))]
	class EditorMouseHandler : UnityEditor.Editor
	{
		public void OnSceneGUI() {
			Event e = Event.current;
			switch (e.type)
			{
				case EventType.MouseUp:
					if (e.button == 0)
						((EditorTile) target).SetNewPosition();
					break;
				case EventType.MouseDrag:
					if (e.button == 0)
					{
						Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
						if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
							hit.collider.GetComponent<GridTile>().Select();
					}
					break;
			}
		}
	}
}
