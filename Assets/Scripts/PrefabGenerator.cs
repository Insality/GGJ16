using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public GameObject ActionPrefab;

    public Action GenerateAction(Transform parent)
    {
        Action action = Instantiate(ActionPrefab).GetComponent<Action>();
        action.transform.parent = parent;
        return action;
    }
}
