using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorRandom : MonoBehaviour
{

    public Color primeira = Color.green;
    //public bool alterarAlpha = false;
    public float tempoParaTrocar = 1;
    public float velocidadeCor = 5;
    Color[] colors = new Color[3];

    Material materialObjeto;
    Color corRandom;
    float cronometro;

    
    void Start()
    {
        colors[0] = Color.blue;
        colors[1] = Color.red;
        colors[2] = Color.green;
    

        materialObjeto = GetComponent<MeshRenderer>().material;
        materialObjeto.color = primeira;
        cronometro = 0;
       
        corRandom = colors[Random.Range(0, colors.Length)];
        
    }

    
    void Update()
    {
        materialObjeto.color = Color.Lerp(materialObjeto.color, corRandom, Time.deltaTime * velocidadeCor);
        cronometro += Time.deltaTime;
        if(cronometro > tempoParaTrocar)
        {
            cronometro = 0;
            
            corRandom = colors[Random.Range(0, colors.Length)];
            
          
        }
    }
}
// corRandom = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
//corRandom = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
