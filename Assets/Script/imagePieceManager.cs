using UnityEngine;

public class imagePieceManager : MonoBehaviour
{
    private Vector2 initialPos;
    private void Start()
    {
        initialPos = transform.position;
    }

    public void BackToInitial()
    {
        transform.position = initialPos;
    }

    public bool CheckParent()
    {
        if(transform.parent != null)
        {
            return transform.GetComponent<ID>().Id == transform.parent.GetComponent<ID>().Id;
        }
        else
        {
            return false;
        }
    }
}
