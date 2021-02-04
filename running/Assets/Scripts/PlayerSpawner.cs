using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] players;//referencia aos prefabs dos jogadores

    void Awake()
    {
        //instanceia um dos jogadores , indexado no gm, na propria posiçao do obj, sem rotaçao 
        Instantiate(players[GameManager.gm.characterIndex], transform.position, Quaternion.identity);

    }
}
