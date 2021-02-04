using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player; // referencia ao tr do p
    private Vector3 offset; // distancia inicial da camera do player

    // Use this for initialization
    void Start()
    {
                              //procurando pela tag
        player = GameObject.FindGameObjectWithTag("Player").transform; // referencia quem e esse transforme
        offset = transform.position - player.position;//posi inicial - posi do player

    }

    // Update is called once per frame
    void LateUpdate() // chamado depois q o flame termina
    {
        //posiçao do player + o offset = nova posi da camera/////
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
        transform.position = newPosition; //atualiza

    }
}
