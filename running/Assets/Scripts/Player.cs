using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // velocidade do personagem 
    public float laneSpeed; // quando troca de lane
    public float jumpLength;//distancia total do pulo
    public float jumpHeight;//altura do pulo
    public float slideLength;//distancia
    public int maxLife = 3; // vida total
    public float minSpeed = 10f; // velocidade minima
    public float maxSpeed = 30f; // maxima q pode atingir
    public float invincibleTime; // muteki tempo pela unity
    public GameObject model; // quando estiver usando um outro modelo diferente desse gato usa isso

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private int currentLane = 1;   // lane atual
    private Vector3 verticalTargetPosition; // atualizar o vetor
    private bool jumping = false; //esta pulando?
    private float jumpStart;      // pulo
    private bool sliding = false; //esta escorregando?
    private float slideStart;    //slide
    private Vector3 boxColliderSize; // tamanho inicial 
    private bool isSwipping = false; // se colocou o dedo
    private Vector2 startingTouch; // vai pegar as posiçoes da tela(posiçao inicial)
    private int currentLife; // vida atual
    private bool invincible = false; // muteki
    static int blinkingValue; // isso pode nao dar certo se mudar o modelo E06
    
    private UIManager uiManager;//chamar a funçao para procurar o obj do tipo
    private CoresTags ct;

    //[HideInInspector] // esconde no editor da unity para nao conseguir alterar
    // public int coins; //contagem das moedas
    [HideInInspector]
    public float score; // pontuaçao

    private bool canMove;//quando pode se mover ou nao

    public AudioClip damageSE, jumpSE, slideSE, deadSE, changeLaneSE;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        rb = GetComponent<Rigidbody>(); // esse rig e do proprio obj
        anim = GetComponentInChildren<Animator>();//esta no filho
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        audioSource = GetComponent<AudioSource>();

        currentLife = maxLife;
        
        blinkingValue = Shader.PropertyToID("_BlinkingValue");//pega o shader e a string, se utilizar outro nao vai funcionar
        //outra maneira seria desativar e ativar o modelo dele
        
        uiManager = FindObjectOfType<UIManager>();

        ct = FindObjectOfType<CoresTags>(); // colocado depois

        GameManager.gm.StartMissions();
        
        Invoke("StartRun", 3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)//se for falso
            return;

        score += Time.deltaTime * speed; //tempo atual * velo, esta atualizando
        uiManager.UpdateScore((int)score);//chama essa funçao // float passando para int


        //troca de lane
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Slide();
        }


                // para sumaho
        if (Input.touchCount == 1) //a contagem de toques da tela = 1
        {
            if (isSwipping) // verdadeiro
            {
                Vector2 diff = Input.GetTouch(0).position - startingTouch;//qual posiçao o dedo esta indo
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);//atualizando
                if (diff.magnitude > 0.01f)//se deslizou para algum lugar
                {       //valor absoluto
                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x)) //esta indo para cima ou baixo
                    {
                        if (diff.y < 0) // baixo
                        {
                            Slide();
                        }
                        else // cima
                        {
                            Jump();
                        }
                    }
                    else // direita ou esquerda
                    {
                        if (diff.x < 0) 
                        {
                            ChangeLane(-1);
                        }
                        else
                        {
                            ChangeLane(1);
                        }
                    }

                    isSwipping = false;//depois que chama as funçoes
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began) // se começou o toque na tela
            {
                startingTouch = Input.GetTouch(0).position; //vai pegar a posiçao inicial do toque
                isSwipping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)//se nao existe mais o dedo
            {
                isSwipping = false;
            }
        }




        if (jumping) // verificar o pulo
        { //controla a proporçao do pulo, maior que 1 ela acaba
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if (ratio >= 1f) // maior que um pulo acaba
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            }
            else
            {//atualiza a posiçao na altura, 
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else // se nao tiver em pulo
        {   //atualiza posiçao para baixo por nao ter usado a gravidade
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }

        if (sliding)//verifica      igual ao pulo
        {
            float ratio = (transform.position.z - slideStart) / slideLength;
            if (ratio >= 1f)
            {
                sliding = false;
                anim.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize; // volta ao tamanho inicial
            }
        }

        //posiçao alvo q quer ir , para atualizar, x=ondemuda, y=pula, z=propria posiçao
        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        //atualiza a posiçao , velocidade para ele ir ate essa nova posiçao, ser independente dos flames
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);

    }

    private void FixedUpdate()//chamado a cada tempo fixo = 0,02s
    {                 //atalho x=0,y=0,z=1
        rb.velocity = Vector3.forward * speed;//ter velocidade para mov automaticamente
    }
    
    void StartRun()//para quando começar o player estiver parado e deposi de 3s começar a correr
    {
        //PlayServices.UnlockAnchievment(EndlessRunnerServices.achievement_faa_uma_corrida);
        anim.Play("runStart");
        speed = minSpeed;
        canMove = true;
    }

    void ChangeLane(int direction)
    {
        audioSource.PlayOneShot(changeLaneSE, 0.5f);
        int targetLane = currentLane + direction;//qual a line q vai , atual+direçao
        if (targetLane < 0 || targetLane > 2) // ou
            return; //nao permite isso
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);//pois começa na posiçao 0 mas a line e na 1 
    }

    void Jump()
    {
        if (!jumping) // se esta pulando
        {
            audioSource.PlayOneShot(jumpSE, 0.3f);
            jumpStart = transform.position.z; //posiçao no eixo de z
            anim.SetFloat("JumpSpeed", speed / jumpLength);//velocidade/tamano do pulo
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }
    void Slide()
    {
        if (!jumping && !sliding) // se nao esta pulando e escorregando
        {
            audioSource.PlayOneShot(slideSE, 0.2f);
            slideStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / slideLength);//velocidade/tamano do slide
            anim.SetBool("Sliding", true);
            Vector3 newSize = boxCollider.size;//atualizar o tamanho
            newSize.y = newSize.y / 2; // diminuir a altura do colider
            boxCollider.size = newSize; //para atualizar o vetor inteiro e nao so o eixo dele
            sliding = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Coin"))// se colide  com essa tag
        {
            //PlayServices.IncrementAchievment(EndlessRunnerServices.achievement_colete_100_peixes, 1);
            coins++;
            uiManager.UpdateCoins(coins); // chama essa funçao
            other.transform.parent.gameObject.SetActive(false); // desativa as moedas q pegou
            //esta desativando o pai
        }
        */
        if (invincible) // se esta muteki volta
            return;
            
        if (other.CompareTag("Obstacle")) // se colide  com essa tag
        {
            canMove = false;
            currentLife--; // diminui a vida
            uiManager.UpdateLives(currentLife);//atualizar a imagem da vida no ui
            anim.SetTrigger("Hit");
            audioSource.PlayOneShot(damageSE, 0.3f);
            speed = 0;
            if (currentLife <= 0) // vida = 0 
            {
                audioSource.PlayOneShot(deadSE, 0.3f);
                speed = 0; // zera a velocidade
                anim.SetBool("Dead", true); //anime
                uiManager.gameOverPanel.SetActive(true); // ativa
                
                //if(score > PlayServices.GetPlayerScore(EndlessRunnerServices.leaderboard_ranking))
                //{
                //PlayServices.PostScore((long)score, EndlessRunnerServices.leaderboard_ranking);
                //}

                Invoke("CallMenu", 2f);
                ct.CallMenu();
            }
            else // pisca
            {
                Invoke("CanMove", 0.75f);//como tem vida chama essa funçao
                StartCoroutine(Blinking(invincibleTime));
            }
        }
    }

    void CanMove()//para colocar um tempo com o invoke
    {
        canMove = true;
    }

    IEnumerator Blinking(float time) // time para ficar invencivel
    {
        invincible = true; // muteki
        float timer = 0; // tempo muteki 
        float currentBlink = 1f; //atual
        float lastBlink = 0; // ultimo
        float blinkPeriod = 0.1f; // a cada 0,1s pisca
        bool enabled = false; // para habilitar(desabilitar) o modelo, no inicio e falso
        yield return new WaitForSeconds(1f); // o tempo q vai ficar parado
        speed = minSpeed; // atualizar a velocidade 
        while (timer < time && invincible) // loop para ficar piscando
        {
            model.SetActive(enabled); // pega o modelo e ativa como falso e dessativa o modelo
            //Shader.SetGlobalFloat(blinkingValue, currentBlink); // mexendo com o shader
            yield return null;//vsi retornar um fleime
            timer += Time.deltaTime; // atualiza o time
            lastBlink += Time.deltaTime;//atualiza
            if (blinkPeriod < lastBlink) 
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled; // muda o valor para verdadeiro e depois para falso
                //isso faz com q fique ativando e desativando dentro do loop while 
            }
        }
        model.SetActive(true); // para garantir q esta ativado 
        //Shader.SetGlobalFloat(blinkingValue, 0); // volta ao estado padrao
        invincible = false;
    }
    
    void CallMenu() // para carregar o menu
    {
        //GameManager.gm.coins += coins;//atualizar as moedas q coletou durante a fase
        GameManager.gm.EndRun();
    }
    
    public void IncreaseSpeed() // aumenta um pouca a velocidade
    {
        speed *= 1.15f;
        if (speed >= maxSpeed) // para nao ultrapassar mais que a maxima 
            speed = maxSpeed;
    }
    
}
