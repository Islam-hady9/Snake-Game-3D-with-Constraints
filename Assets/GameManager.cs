using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject snake_piece;
    public GameObject food;
    public GameObject obstacle;
    public TextMeshProUGUI Collection;
    public TextMeshProUGUI Timer;
    public int TimerC = 10;
    public int CoinC = 0;

    List<Vector3> positions = new List<Vector3>();
    List<GameObject> snake = new List<GameObject>();
    List<Vector3> extensions = new List<Vector3>();

    Vector3 direction = new Vector3(0, 0, .15f);

    int level_width = 29;
    int level_height = 38;

    public bool game_over = false;

    bool is_locked = false;
    int starting_count = 30;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < starting_count; i++){
            positions.Add(new Vector3(0, 0, (i - starting_count) * 0.10f));

            GameObject new_snake_piece = Instantiate(snake_piece);
            new_snake_piece.transform.position = positions[i];

            if (i == starting_count - 1) {
                new_snake_piece.AddComponent<SnakePiece>();

                Camera.main.transform.parent = new_snake_piece.transform;
                Camera.main.transform.eulerAngles = new Vector3(35, 0, 0);
                Camera.main.transform.localPosition = new Vector3(0, 5, -8);
            }
            else if (i > starting_count - 20) {
                new_snake_piece.tag = "Untagged";
            }

            snake.Add(new_snake_piece);
        }

        for (int i = 0; i < 20; i++) {
            GameObject new_obstacle = Instantiate(obstacle);

            int x, z;

            bool valid = true;

            do {
                valid = true;
                x = Random.Range(-level_width / 2, level_width / 2);
                z = Random.Range(-level_height / 2, level_height / 2);

                if (x > -level_width / 4 && x < level_width / 4 && z > -level_height / 4 && z < level_height / 4) {
                    valid = false;
                }
            }
            while(valid == false);
            new_obstacle.transform.position = new Vector3(x, 0, z);
            new_obstacle.transform.localScale = new Vector3(Random.Range(1, 4), 1, Random.Range(1, 4));
        }

        StartCoroutine(MoveSnake());
        StartCoroutine(CreateFood());
    }

    // Update is called once per frame
    void Update()
    {
        Timer.text = $"{TimerC}";
        //Collection.text = $"{CoinC}";
        //Collection.text = $"{CoinC} / {5}";
        if (Collection != null){
            if (CoinC == 5){
                Collection.text = "Success";
                GameObject.FindObjectOfType<GameManager>().game_over = true;
            }
            else{
                Collection.text = $"{CoinC} / {5}";
            }
            Collection.ForceMeshUpdate();
        }
        if(TimerC == 0) {
            Timer.text = "TimeOut";
            game_over = true;
        }
        /*
        if (is_locked == false){
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction.z == 0) { direction = new Vector3(0, 0, .15f); is_locked = true;}
            else if (Input.GetKeyDown(KeyCode.DownArrow) && direction.z == 0) {direction = new Vector3(0, 0, -.15f); is_locked = true;}
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction.x == 0) {direction = new Vector3(-.15f, 0, 0); is_locked = true;}
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction.x == 0) {direction = new Vector3(.15f, 0, 0); is_locked = true;}
        }
        */
        if (Input.GetKey(KeyCode.LeftArrow)) {
            snake[snake.Count - 1].transform.Rotate(new Vector3(0, -Time.deltaTime * 260, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            snake[snake.Count - 1].transform.Rotate(new Vector3(0, Time.deltaTime * 260, 0));
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene("SampleScene");
    }

    IEnumerator MoveSnake(){
        yield return new WaitForSeconds(.02f);

        if (game_over) yield break;

        bool grow_snake = false;
        if (extensions.Count > 0 && extensions[0] == positions[0]) {
            grow_snake = true;
        }

        positions.RemoveAt(0);
        positions.Add(positions[positions.Count - 1] + snake[snake.Count - 1].transform.forward * 0.10f);

        for(int i = 0; i < positions.Count; i++){
            snake[i].transform.position = positions[i];
        }

        if (grow_snake) {
            positions.Insert(0, extensions[0]);

            GameObject new_snake_piece = Instantiate(snake_piece);
            new_snake_piece.transform.position = positions[0];

            snake.Insert(0, new_snake_piece);

            extensions.RemoveAt(0);
        }

        is_locked = false;

        StartCoroutine(MoveSnake());
    }

    IEnumerator CreateFood() {
        yield return new WaitForSeconds(2.5f);
         if(TimerC > 0) {
            TimerC--;
         }

        //bool valid_location = true;
        int x, z;
        x = Random.Range(-level_width / 2, level_width / 2);
        z = Random.Range(-level_height / 2, level_height / 2);

        /*
        do {
            valid_location = true;

            x = Random.Range(-level_width / 2, level_width / 2);
            z = Random.Range(-level_height / 2, level_height / 2);

            for (int i = 0; i < positions.Count; i++) {
                if (positions[i].x == x && positions[i].z == z) {
                    valid_location = false;
                }
            }
        }
        while (valid_location == false);
        */

        GameObject new_food = Instantiate(food);
        new_food.transform.position = new Vector3(x, 0, z);

        if (!game_over) StartCoroutine(CreateFood()); //Food always appears randomly
    }

    public void EatFood(Vector3 p){
        extensions.Add(p);
        CoinC++;
        if (Collection != null){
            if (CoinC == 5){
                Collection.text = "Success";
                GameObject.FindObjectOfType<GameManager>().game_over = true;
            }
            else{
                Collection.text = $"{CoinC} / {5}";
            }
            Collection.ForceMeshUpdate();
        }
        // When I eat one, the other appears
        //StartCoroutine(CreateFood());
    }
}
