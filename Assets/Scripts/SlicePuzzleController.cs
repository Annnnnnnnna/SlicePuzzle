using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlicePuzzleController : MonoBehaviour {

    public PiceOfPuzzle piceOfPuzzlePrefab;
    public Sprite[] images;
    static int empty_card_position = 8;
    const int grid = 3;
    const float offsetX = 3.4f;
    const float offsetY = 3.0f;
    int countPuzzleSetProperly = 0;
    Vector3 startPos;

    void Start()
    {
        startPos = piceOfPuzzlePrefab.transform.position;
        int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7 };
        numbers = ShuffleArray(numbers);
        int rows = grid;
        for (int i = 0; i < grid; i++)
        {
            if (grid - i == 1)
                rows = grid - 1;

            for (int j = 0; j < rows; j++)
            {
                PiceOfPuzzle puzzle;
                if (i == 0 && j == 0)
                    puzzle = piceOfPuzzlePrefab;
                else
                    puzzle = Instantiate(piceOfPuzzlePrefab) as PiceOfPuzzle;

                int index = j * grid + i;
                puzzle.position_id = index;

                int id = numbers[index];
                puzzle.ChangeSprite(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = startPos.y - (offsetY * j);
                puzzle.transform.position = new Vector3(posX, posY, startPos.z);

                if (index == id)
                    countPuzzleSetProperly++;

            }
        }
    }
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = UnityEngine.Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
    public void CanMoved(PiceOfPuzzle piceOfPuzzle)
    {
        bool moved = true;
        int dif = empty_card_position - piceOfPuzzle.position_id;

        switch (dif)
        {
            case -1: piceOfPuzzle.transform.position = new Vector3(piceOfPuzzle.transform.position.x - offsetX, piceOfPuzzle.transform.position.y, piceOfPuzzle.transform.position.z); break;
            case 1: piceOfPuzzle.transform.position = new Vector3(piceOfPuzzle.transform.position.x + offsetX, piceOfPuzzle.transform.position.y, piceOfPuzzle.transform.position.z); break;
            case 3: piceOfPuzzle.transform.position = new Vector3(piceOfPuzzle.transform.position.x, piceOfPuzzle.transform.position.y - offsetY, piceOfPuzzle.transform.position.z); break;
            case -3: piceOfPuzzle.transform.position = new Vector3(piceOfPuzzle.transform.position.x, piceOfPuzzle.transform.position.y + offsetY, piceOfPuzzle.transform.position.z); break;
            default: moved = false; break;
        }
        if (moved)
        {
            int temp = piceOfPuzzle.position_id;
            piceOfPuzzle.position_id = empty_card_position;
            empty_card_position = temp;
            StartCoroutine(CheckMatch(piceOfPuzzle, empty_card_position));

        }


    }

    private IEnumerator CheckMatch(PiceOfPuzzle piceOfPuzzle, int previous_position)
    {
        if (piceOfPuzzle.id == piceOfPuzzle.position_id)
            countPuzzleSetProperly++;

        else if (piceOfPuzzle.id == previous_position)
            countPuzzleSetProperly--;

        if (countPuzzleSetProperly == images.Length - 1)
        {
            PiceOfPuzzle puzzle = Instantiate(piceOfPuzzlePrefab) as PiceOfPuzzle;
            puzzle.position_id = empty_card_position;
            int id = empty_card_position;
            puzzle.ChangeSprite(id, images[id]);
            float posX = (offsetX * (grid - 1)) + startPos.x;
            float posY = startPos.y - (offsetY * (grid - 1));
            puzzle.transform.position = new Vector3(posX, posY, startPos.z);

            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(1);
        }
        else
            yield return null;
    }
}