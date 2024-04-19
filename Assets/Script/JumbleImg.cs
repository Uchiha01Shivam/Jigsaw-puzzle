using System.Collections.Generic;
using UnityEngine;

public class JumbleImg : MonoBehaviour
{
    private List<Transform> objectsToShuffle;
    [SerializeField] private GameObject image;
    [SerializeField] private Sprite[] pieces;
    [SerializeField] private Transform[] pos;
    private void Start()
    {
        objectsToShuffle = new();
        foreach (Transform item in pos)
        {
            objectsToShuffle.Add(item);
        }
        Shuffle(objectsToShuffle);

        CreateObj();
    }

    private void CreateObj()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            GameObject clone = Instantiate(image);
            clone.name = $" ({i+1})";
            clone.GetComponent<SpriteRenderer>().sprite = pieces[i];
            clone.transform.position = pos[i].position;
            clone.GetComponent<ID>().Id = i + 1;
            GameManager.instance.imgs.Insert(i,clone.GetComponent<imagePieceManager>());
        }
    }

    void Shuffle(List<Transform> list)
    {
        // Create a random number generator
        System.Random rng = new();

        // Iterate through the list in reverse order
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Generate a random index
            int j = rng.Next(i + 1);

            // Swap the position of the objects at index i and j
            (list[j].position, list[i].position) = (list[i].position, list[j].position);
        }
    }
}

