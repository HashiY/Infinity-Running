using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] lifeHearts; //vai pegar a propriedade de cor para mudar
    public Text coinText;
    public GameObject gameOverPanel;
    public Text scoreText;
    public AudioClip fishCoin;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateLives(int lives) // quantidade de vidas atuais
    {
        //se tiver 3vidas todos vermelhos , 2vidas o 3 coraçao preto
        //,1vida so o primeiro fica vermeçho
        for (int i = 0; i < lifeHearts.Length; i++) // caso queira aumntar a vida assim e melhor
        {
            if (lives > i)
            {
                lifeHearts[i].color = Color.white; //nenhuma alteraçao
            }
            else
            {
                lifeHearts[i].color = Color.black;
            }
        }
    }

    public void UpdateCoins(int coin) // coin vem do Player
    {
        audioSource.PlayOneShot(fishCoin, 0.1f);
        coinText.text = coin.ToString();//o texto atualizado
    }

    public void UpdateScore(int score) // score vem do Player
    {
        scoreText.text = "Score: " + score + "m";//coloca esse texto, m = metros
    }

}
