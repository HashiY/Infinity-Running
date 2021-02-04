using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//para carregar as cenas
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random; // pq tem na biblioteca da unity e da system esse random

[Serializable] //converte em matriz de bytes
public class PlayerData //
{
    public int coins;
    public int[] max; // o valor max do objetivo de cada mission
    public int[] progress;//de cada m
    public int[] currentProgress;
    public int[] reward;
    public string[] missionType; // tipo de m, para adicionar o componente certo
    public int[] characterCost;
}

public class GameManager : MonoBehaviour
{//percistir entre uma cena e outra, quando ir para outra faze vai manter, passando de uma cena para outra


    public static GameManager gm;//acessa tudo que tem aqui dentro , por outras classes
    public int coins;
    public int[] characterCost; // para quando ter + q 1 e um vetor

    public int characterIndex;

    private MissionBase[] missions; //referencia vai ter agora 2 missions para aparecer no panel
    private string filePath; // caminho do arquivo
    
    private void Awake()//antes do strat ele chamado
    {
        if (gm == null) // se for nulo
        {
            gm = this; //esse obj se trnsforma em gm
        }
        else if (gm != this) // se gm nao for esse obj
        {
            Destroy(gameObject); //para garantir q tenha 1 gm na cena, ce colocar em +q 1 cena, apaga 1 
        }
        DontDestroyOnLoad(gameObject);//para nao destruir ao carregar

        filePath = Application.persistentDataPath + "/save.sav"; // onde o arquivo e salvo
        //c - user - nome - appdata - localow - nomeDaCampany - nomeDoJogo

        missions = new MissionBase[2];//instanciar as missions, vai ter 2 missions
        
        if (File.Exists(filePath)) //verifica se existe o arquivo nesse caminho
        {
            Load();
        }

        else //se nao faz tudo pela primeira vez
        {
            for (int i = 0; i < missions.Length; i++) // total de tamanho de missions
            {
                GameObject newMission = new GameObject("Mission" + i);//instanciar por novo obj
                newMission.transform.SetParent(transform);//definir o obj do GameManager como o pai desse novo obj
                MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.FishesSingleRun };//para sortear
                int randomType = Random.Range(0, missionType.Length);// esta sorteando
                if (randomType == (int)MissionType.SingleRun) // se for 
                {
                    missions[i] = newMission.AddComponent<SingleRun>();//missions vai ser esse componente

                }
                else if (randomType == (int)MissionType.TotalMeter)
                {
                    missions[i] = newMission.AddComponent<TotalMeters>();

                }
                else if (randomType == (int)MissionType.FishesSingleRun)
                {
                    missions[i] = newMission.AddComponent<FishesSingleRun>();

                }

                missions[i].Created();//para setar os valores iniciais da missions
            }
        }
        
    }
    
    public void Save() // vai acessar de outros obj
    {
        BinaryFormatter bf = new BinaryFormatter(); // para usar o formato binario
        FileStream file = File.Create(filePath); //arquivo, se ja estiver criado ele sobreescreve

        PlayerData data = new PlayerData(); // armazena oa dados pegando daqui

        data.coins = coins;

        data.max = new int[2]; // 2 pq tem 2 missions
        data.progress = new int[2];
        data.currentProgress = new int[2];
        data.reward = new int[2];
        data.missionType = new string[2];
        data.characterCost = new int[characterCost.Length];//para quando acrescentar + personagens

        for (int i = 0; i < 2; i++) //2 missions, armazena cada valor individualmente
        {
            data.max[i] = missions[i].max;
            data.progress[i] = missions[i].progress;
            data.currentProgress[i] = missions[i].currentProgress;
            data.reward[i] = missions[i].reward;
            data.missionType[i] = missions[i].missionType.ToString();
        }

        for (int i = 0; i < characterCost.Length; i++) // 0 ate total de p
        {
            data.characterCost[i] = characterCost[i]; // salva
        }

        bf.Serialize(file, data);//passa tudo que ta em data para dentro do arquivo
        file.Close(); //fecha o arquivo
    }

    void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);//abri , caminho,  modoAbrir

        PlayerData data = (PlayerData)bf.Deserialize(file); //passa convertendo o que tem no arquivo para dados de jogo
        file.Close();

        coins = data.coins;

        for (int i = 0; i < 2; i++)//2 missions,
        {
            GameObject newMission = new GameObject("Mission" + i);
            newMission.transform.SetParent(transform);
            if (data.missionType[i] == MissionType.SingleRun.ToString()) // se for essa mission type
            {
                missions[i] = newMission.AddComponent<SingleRun>(); //adiciona esse componente
                missions[i].missionType = MissionType.SingleRun; 
            }
            else if (data.missionType[i] == MissionType.TotalMeter.ToString())
            {
                missions[i] = newMission.AddComponent<TotalMeters>();
                missions[i].missionType = MissionType.TotalMeter;
            }
            else if (data.missionType[i] == MissionType.FishesSingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<FishesSingleRun>();
                missions[i].missionType = MissionType.FishesSingleRun;
            }
            //agora passa cada valor
            missions[i].max = data.max[i];
            missions[i].progress = data.progress[i];
            missions[i].currentProgress = data.currentProgress[i];
            missions[i].reward = data.reward[i];
        }

        for (int i = 0; i < data.characterCost.Length; i++) // 0 ate total de p
        {
            characterCost[i] = data.characterCost[i]; // load
        }
    }
    /*
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    */
    public void StartRun(int charIndex)//chama a cena de jogo , index do personagem
    {
        characterIndex = charIndex; // para saber qual persogem foi selecionado no menu
        SceneManager.LoadScene("Running"); // carrega a cena
    }
    
    public void EndRun()//carrega o menu
    {
        SceneManager.LoadScene("Menu");
    }
    
    public MissionBase GetMission(int index)//para o menu pegar a mission e saber as informaçoes da mission
    {                                   //0 ou 1
        return missions[index];
    }
    
    public void StartMissions()//quando iniciar a cena e chamada pelo player
    {
        for (int i = 0; i < 2; i++)
        {
            missions[i].RunStart();
        }
    }
    
    public void GenerateMission(int i) // i=0,1  , para ver qual vai substituir quando completar a mission
    {
        Destroy(missions[i].gameObject);//destroi a mission 

        GameObject newMission = new GameObject("Mission" + i); // cria uma nova mission
        newMission.transform.SetParent(transform);
        MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.FishesSingleRun };
        int randomType = Random.Range(0, missionType.Length);
        if (randomType == (int)MissionType.SingleRun)
        {
            missions[i] = newMission.AddComponent<SingleRun>();

        }
        else if (randomType == (int)MissionType.TotalMeter)
        {
            missions[i] = newMission.AddComponent<TotalMeters>();

        }
        else if (randomType == (int)MissionType.FishesSingleRun)
        {
            missions[i] = newMission.AddComponent<FishesSingleRun>();

        }

        missions[i].Created();

        FindObjectOfType<Menu>().SetMission();//procura pelo menu e usa a funçao q atualiza os valores das missions na tela
    }
    
}
