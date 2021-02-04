using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstacles;//referencia aos obstaculos
    public Vector2 numberOfObstacles;//armazenar valor minimo e max para ser a quantidades de obstaculos
    /*
    public GameObject coin; // prefab das moedas
    public Vector2 numberOfCoins;//armazenar valor minimo e max
    */
    public GameObject[] lixoPontos;
    public Vector2 numberOfTrash;

    public List<GameObject> newObstacles; 
    public List<GameObject> newCoins;

    public List<GameObject> newTrash;



    // Start is called before the first frame update
    void Start()
    {   //sortear os obstaculos e coins, onde aparece
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y);
        //valor que vai ser instanciada sorteada
        //int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);
        //para os lixos
        int newNumberOfTrash = (int)Random.Range(numberOfTrash.x, numberOfTrash.y);

        for (int i = 0; i < newNumberOfObstacles; i++)
        {   //adiciona na lista um dos prefabs , 0 - total vai sortear qual obs colocar , instancia como um filho de obj
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newObstacles[i].SetActive(false);//desativado no inicio
        }
        /*
        for (int i = 0; i < newNumberOfCoins; i++)
        {   //instancia a moeda do prefab e a posiçao que sera a pista como o pai
            newCoins.Add(Instantiate(coin, transform));
            newCoins[i].SetActive(false);
        }
        */
        //lixos
        for(int i = 0; i < newNumberOfTrash; i++)
        {
            newTrash.Add(Instantiate(lixoPontos[Random.Range(0, lixoPontos.Length)],transform));
            newTrash[i].SetActive(false);
        }

        PositionateObstacles();//para posicionar os obstaculos
        //PositionateCoins();

        //
        PositionateTrash();

    }

    void PositionateObstacles() //posicionar os obstaculos
    {   
        for (int i = 0; i < newObstacles.Count; i++)
        {   //posilao minima onde vai instanciar os obj, tamanho pega na unity/quantidadeDeObstaculos * index
            float posZMin = (296.97f / newObstacles.Count) + (296.97f / newObstacles.Count) * i;
            float posZMax = (296.97f / newObstacles.Count) + (296.97f / newObstacles.Count) * i + 1;
            //chama o obstaculo da lista, pega a posiçaoLocal , e coloca em algum lugar de z
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax));
            newObstacles[i].SetActive(true);//ativa
            if (newObstacles[i].GetComponent<ChangeLane>() != null) // se possui o componente(so a lixeira tem)
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();//muda
                
        }
    }
    /*
    void PositionateCoins()
    {
        float minZPos = 10f; // o valor minimo que vai ser instanciado a moeda para nao aparecer perto do player
        for (int i = 0; i < newCoins.Count; i++) //para poder posicionar tudo
        {
            float maxZPos = minZPos + 5f; // posiçao maxima , a cada +5 de distancia posiciona o coin
            float randomZPos = Random.Range(minZPos, maxZPos); //vai sortear onde vai posicionar == (n,n+5)
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);//posiciona
            newCoins[i].SetActive(true);
            newCoins[i].GetComponent<ChangeLane>().PositionLane();//pega o componente e a funçao
            minZPos = randomZPos + 1; // a proxima moeda vai ter no minimo +1 de distancia da anterior
        }
    }*/
    //aaaaaaaaaaaaaaaaaaaaaaaaa
    void PositionateTrash()
    {
        float minZPos = 10f;
        for (int i = 0; i < newTrash.Count; i++)
        {
            float maxZPos = minZPos + 5f;
            float randomZPos = Random.Range(minZPos, maxZPos);
            newTrash[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
            newTrash[i].SetActive(true);
            if (newTrash[i].GetComponent<ChangeLane>() != null) // se possui o componente(todos tem),pode tirar
                newTrash[i].GetComponent<ChangeLane>().PositionLane();//muda
            minZPos = randomZPos + 1;

        }
    }

    private void OnTriggerEnter(Collider other)//reposiciona tudo
    {
        if (other.CompareTag("Player")) // se colidir com essa tag
        {
            other.GetComponent<Player>().IncreaseSpeed();//quando colidir com a parede aumenta a velocidade
            transform.position = new Vector3(0, 0, transform.position.z + 296.97f * 2);//mudar a profundidade somente,pois e o track chao e a cidade
            PositionateObstacles();
            //PositionateCoins();
            //
            PositionateTrash();
        }
    }





}
