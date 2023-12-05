using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakePiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Wall" || other.tag == "Player") {
            GameObject.FindObjectOfType<GameManager>().TimerC = 0;
            GameObject.FindObjectOfType<GameManager>().game_over = true;
        }

        else if (other.tag == "Food") {
            GameObject.FindObjectOfType<GameManager>().EatFood(transform.position);
            //GameObject.FindObjectOfType<GameManager>().CoinC++;
            Destroy(other.gameObject);
        }
    }
}
