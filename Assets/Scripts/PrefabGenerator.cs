using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public GameObject ActionPrefab;
    public GameObject ShamanPrefab;

    public Action GenerateAction(Transform parent)
    {
        Action action = Instantiate(ActionPrefab).GetComponent<Action>();
        action.transform.parent = parent;
        return action;
    }

    public Shaman GenerateShaman(Transform parent) {
        Shaman shaman = Instantiate(ShamanPrefab).GetComponent<Shaman>();
        shaman.transform.parent = parent;
        return shaman;
    }

}
