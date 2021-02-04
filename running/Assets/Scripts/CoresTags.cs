using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoresTags : MonoBehaviour
{
    public string[] tagsArray = new string[3];
    private int randomIndex;
    string randomTag;
    Transform target;

    public float tempoParaTrocar = 1;
    public float velocidadeCor = 5;
    public Color primeira = Color.green;

    Material materialObjeto;
    Color corRandom;
    float cronometro;

    private UIManager uiManager;

    [HideInInspector]
    public int coins;

  


    void Start()
    {
        materialObjeto = GetComponent<MeshRenderer>().material;
        materialObjeto.color = primeira;
        cronometro = 0;
        GameObject[] verde = GameObject.FindGameObjectsWithTag("Verde");
        
        randomIndex = Random.Range(0, 3);
        randomTag = tagsArray[randomIndex];
        //target = GameObject.FindWithTag(randomTag).transform;
        transform.gameObject.tag = randomTag;

        uiManager = FindObjectOfType<UIManager>();

        

    }
    
    void Update()
    {
        if (randomIndex == 0)
        {
            setColor();
        }
        else if (randomIndex == 1)
        {
            setColor();
        }
        else if (randomIndex == 2)
        {
            setColor();
        }
        
        // materialObjeto.color = Color.Lerp(materialObjeto.color, corRandom, Time.deltaTime * velocidadeCor);

        cronometro += Time.deltaTime;
        if (cronometro > tempoParaTrocar)
        {
            cronometro = 0;
            randomIndex = Random.Range(0, 3);
            randomTag = tagsArray[randomIndex];
            //target = GameObject.FindWithTag(randomTag).transform;
            transform.gameObject.tag = randomTag;
        }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (this.tag == "Azul" && other.CompareTag("Azul"))// se colide  com essa tag
        {
            /*PlayServices.IncrementAchievment(EndlessRunnerServices.achievement_colete_100_peixes, 1);*/
            coins++;
            uiManager.UpdateCoins(coins); // chama essa funçao
            other.transform.parent.gameObject.SetActive(false); // desativa as moedas q pegou
            //esta desativando o pai
        }
        if (this.tag == "Vermelho" && other.CompareTag("Vermelho"))// se colide  com essa tag
        {
            /*PlayServices.IncrementAchievment(EndlessRunnerServices.achievement_colete_100_peixes, 1);*/
            coins++;
            uiManager.UpdateCoins(coins); // chama essa funçao
            other.transform.parent.gameObject.SetActive(false); // desativa as moedas q pegou
            //esta desativando o pai
        }
        if (this.tag == "Verde" && other.CompareTag("Verde"))// se colide  com essa tag
        {
            /*PlayServices.IncrementAchievment(EndlessRunnerServices.achievement_colete_100_peixes, 1);*/
            coins++;
            uiManager.UpdateCoins(coins); // chama essa funçao
            other.transform.parent.gameObject.SetActive(false); // desativa as moedas q pegou
            //esta desativando o pai
        }
    }

    public void CallMenu() // para carregar o menu
    {
        GameManager.gm.coins += coins;//atualizar as moedas q coletou durante a fase
       // GameManager.gm.EndRun();
    }
}
