using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLane : MonoBehaviour
{//esse componente e adicionado nos obstaculos ou coins na unity
    //nesse caso seria o osso  de peixe ea lixeira
    public void PositionLane()
    {
        int randomLane = Random.Range(-1, 2);//sortear a lane o 2=exclui e sorteado de -1,0,1
        transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);//atualiza a posi
        //so mudar o eixo x
    }
}
