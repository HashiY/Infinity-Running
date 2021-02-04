using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a : MonoBehaviour
{
    void setColor()
    {
        GameObject[] azul = GameObject.FindGameObjectsWithTag("Azul");
        GameObject[] vermelho = GameObject.FindGameObjectsWithTag("Vermelho");
        GameObject[] verde = GameObject.FindGameObjectsWithTag("Verde");

        foreach (GameObject novoAzul in azul)
        {
            novoAzul.GetComponent<Renderer>().material.color = Color.blue;
        }
        foreach (GameObject novoVermelho in vermelho)
        {
            novoVermelho.GetComponent<Renderer>().material.color = Color.red;
        }
        foreach (GameObject novoVerde in verde)
        {
            novoVerde.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
