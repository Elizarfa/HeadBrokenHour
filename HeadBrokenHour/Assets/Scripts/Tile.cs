using UnityEngine;
using TMPro; // Если вы используете TextMeshPro, раскомментируйте эту строку

public class Tile : MonoBehaviour
{
    public int number;
    private TextMeshProUGUI tileText; // Если используете TextMeshPro
    // private Text tileText; // Если используете обычный Text
    private GameGrid gameGrid;

    public void Setup(int number, GameGrid grid)
    {
        this.number = number;
        gameGrid = grid;
        if (tileText == null)
        {
            tileText = GetComponentInChildren<TextMeshProUGUI>(); // Если используете TextMeshPro
            // tileText = GetComponentInChildren<Text>(); // Если используете обычный Text
        }
        tileText.text = number.ToString();
    }

    private void OnMouseDown()
    {
        gameGrid.TryMoveTile(this);
    }
}