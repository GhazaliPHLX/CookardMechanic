using UnityEngine;

public class CardSortingHandler : MonoBehaviour
{
    private int originalOrder;
    private float originalZ;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalOrder = sr.sortingOrder;
        originalZ = transform.position.z;
    }

    public void BringToFront()
    {
        SortingManager.BringToFront(gameObject);
    }

    public void ResetOrder()
    {
        sr.sortingOrder = originalOrder;

        Vector3 pos = transform.position;
        pos.z = originalZ;
        transform.position = pos;
    }
}
