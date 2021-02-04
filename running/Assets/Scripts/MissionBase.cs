using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // colocaou pq nao estava chamando o Random

public enum MissionType//enumerador para auxiliar o tipo da mission na hora de sortear e poder salvar
{
    SingleRun, TotalMeter, FishesSingleRun
}

//script de base para todas as missions e outras scripts para cada mission sendo filho dessa classe pai
public abstract class MissionBase : MonoBehaviour//vai servir como base para criar outras classes
{
    public int max; // quanto precisa para completar a mission
    public int progress; // progresso atual, quanto de moeda tem no momento
    public int reward; // recompensa
    public Player player; //referencia do pl

    public CoresTags ct; // colocado por mim (caso nao funcionar)

    public int currentProgress; // progresso atual
    public MissionType missionType;

    //essas funçoes vao ser feitas em scripst q vao erdar dessa base, nao e desse script base
    public abstract void Created(); // criar a missiion chama esse
    public abstract string GetMissionDescription(); // retorna a descriçao da mission
    public abstract void RunStart(); // quando inicia a faze e chamado
    public abstract void Update(); // cada um tem seu proprio update

    public bool GetMissionComplete()//funçao q retorna se a mission foi concluida
    {
        if ((progress + currentProgress) >= max) // se o progresso + progressoAtual e >= max
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

//nessas classes precisa colocar as 4 funcoes q criou na base obrigatoriamente
public class SingleRun : MissionBase //correr tantos metros em uma corrida(cada jogada)
{
    public override void Created()
    {
        missionType = MissionType.SingleRun;
        int[] maxValues = { 1000, 2000, 3000, 4000 };//valores para essa mission, metro
        int randomMaxValue = Random.Range(0, maxValues.Length);//sortea qual aparece como mission
        int[] rewards = { 100, 200, 300, 400 };//recompensa
        reward = rewards[randomMaxValue];//pega a recompensa sorteada
        max = maxValues[randomMaxValue];//tb, os 2 e sorteado com o comprimento entao 1=1 2=2 3=3 4=4
        progress = 0;//inicia com 0
    }

    public override string GetMissionDescription()
    {
        return "Corra " + max + "m em uma corrida"; //descriçao da mission
    }

    public override void RunStart()
    {
        progress = 0;//quando inicia a corrida e zero
        player = FindObjectOfType<Player>();
        
    }

    public override void Update()
    {
        if (player == null) // se nao tem pl
            return;

        //atualiza se tem pl
        progress = (int)player.score; // pega o scoa do  Player(cs), 
    }              //passa para o int por ser float originalmente
}

public class TotalMeters : MissionBase //acumula ate onde foi
{
    public override void Created()
    {
        missionType = MissionType.TotalMeter;
        int[] maxValues = { 10000, 20000, 30000, 40000 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 1000, 2000, 3000, 4000 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription()
    {
        return "Corra " + max + "m no total";
    }

    public override void RunStart()
    {
        progress += currentProgress; // vai acumular o progresso atual
        player = FindObjectOfType<Player>(); // definir o pl
    }

    public override void Update()
    {
        if (player == null)
            return;

        currentProgress = (int)player.score;
    }
}

public class FishesSingleRun : MissionBase // coletar coin em uma unica corrida
{
    public override void Created()
    {
        missionType = MissionType.FishesSingleRun;
        int[] maxValues = { 100, 200, 300, 400, 500 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 100, 200, 300, 400, 500 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription()
    {
        return "Colete " + max + " peixes em uma corrida";
    }

    public override void RunStart()
    {
        progress = 0;
        player = FindObjectOfType<Player>();
        ct = FindObjectOfType<CoresTags>(); // colocado depois
    }

    public override void Update()
    {
        if (player == null)
            return;

        progress = ct.coins; // esse pega do coin e e originalmente int
    }
}
