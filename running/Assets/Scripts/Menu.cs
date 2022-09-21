using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//para acessar aos textos em cena

public class Menu : MonoBehaviour
{
    public Text[] missionDescription, missionReward, missionProgress;//referencia aos textos das missions
    public GameObject[] rewardButton; //referencia aos botoes
    public Text coinsText;    // pegar o texto dos peixes
    public Text costText;
    public GameObject[] characters;// colocar os chara q vao ser escolhidos para jogar

    private int characterIndex = 0; // index dos perdonagens e começar no 0

    public AudioClip arrowSE, startSE, noFish;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetMission();
        UpdateCoins(GameManager.gm.coins);//atualizar qundo iniciar
       
    }
    /*
    // Update is called once per frame
    void Update()
    {

    }

    public void ShowAchievmentsUI()
    {
        PlayServices.ShowAchievments();
    }
    */
    public void UpdateCoins(int coins) // atualizar a quantidade de coins q tem na tela (peixe) do menu
    {
        coinsText.text = coins.ToString(); // vai pegar o coin q entra converte para string, coloca no coinsText
    }
    
    public void StartRun()
    {                                                     //se da para comprar o personagem
        if (GameManager.gm.characterCost[characterIndex] <= GameManager.gm.coins)//se o custo for menor q o q tem de moedas
        {
            audioSource.PlayOneShot(startSE, 0.2f);
            GameManager.gm.coins -= GameManager.gm.characterCost[characterIndex];//diminui a moeda com ocusto do personagem
            GameManager.gm.characterCost[characterIndex] = 0;//0 = comprado
            GameManager.gm.Save();//salva
            GameManager.gm.StartRun(characterIndex); //para chamar as funçoes dos botoese ir para o startRun
        }                           //qual selecionado
        else
        {
            audioSource.PlayOneShot(noFish, 0.2f);
        }
    }
    
    public void SetMission() //definir as missions na tela
    {
        for (int i = 0; i < 2; i++) //como sao 2
        {
            MissionBase mission = GameManager.gm.GetMission(i);//chama a funçao
            missionDescription[i].text = mission.GetMissionDescription();//vai esse texto na cena
            missionReward[i].text = "Recompensa: " + mission.reward;
            missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;
            if (mission.GetMissionComplete())//se ja foi completada
            {
                rewardButton[i].SetActive(true); // aparece o botao
            }
        }

        GameManager.gm.Save(); // depois que gera uma nova mission salva
    }
    
    public void GetReward(int missionIndex) // para o botao de pegar as recompensas
    {                           //chama a funçao com o index e pega o atributo da recompensa
        GameManager.gm.coins += GameManager.gm.GetMission(missionIndex).reward; //atualiza os valores da moeada
        UpdateCoins(GameManager.gm.coins);//atualiza o numero de moedas na tela
        rewardButton[missionIndex].SetActive(false);//button e desativado
        GameManager.gm.GenerateMission(missionIndex);//cahama a funçao para gerar um new mission
    }
    
    public void ChangeCharacter(int index) //funçao para os botoes q muda de personagem de escolha
    {
        audioSource.PlayOneShot(arrowSE, 0.2f);
        characterIndex += index; // adiciona o index

        if (characterIndex >= characters.Length) // se for maior que o tamanho dos personagens(quantidade)
        {
            characterIndex = 0;//volta para o inicio
        }
        else if (characterIndex < 0) // se for menor q 0
        {
            characterIndex = characters.Length - 1; // passa para o final
        }

        for (int i = 0; i < characters.Length; i++)//0 ate o total de personagens
        {
            if (i == characterIndex)   // se o index do looop = ao charcterIndex
                characters[i].SetActive(true); // o modelo em cena e ativado
            else
                characters[i].SetActive(false); // desativado
        }
        
        string cost = ""; // custo do personagem
        if (GameManager.gm.characterCost[characterIndex] != 0)//se o custo for diferente de zero
        {
            cost = GameManager.gm.characterCost[characterIndex].ToString();//atualiza a string com o preço
        }
        costText.text = cost; // como o custo e 0 continua nulo
    }
    /*
    public void ShowLeaderBoardUI()
    {
        PlayServices.ShowLeaderboard(EndlessRunnerServices.leaderboard_ranking);
    }
    */
}
