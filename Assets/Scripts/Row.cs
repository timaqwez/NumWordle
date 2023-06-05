using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles { get; private set; }

    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }

    public string number
    {
        get
        {
            string number = "";

            for (int i = 0; i < tiles.Length; i++) {
                number += tiles[i].digit;
            }

            return number;
        }
    }

}
