using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiceOfPuzzle : MonoBehaviour {
    [SerializeField]
    private SlicePuzzleController controller;

    private int _id;
    public int id
    {
        get { return _id; }
    }

    public int position_id { get; set; }

    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        controller.CanMoved(this);
    }
}
