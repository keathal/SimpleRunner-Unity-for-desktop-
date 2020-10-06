using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour {
    public Material colorMaterial;
    public Transform gameOver;
    public Text scoreText, recordText, gameOverScore;
    float speed = 5.0f;
    Vector3 moveVector;
    CharacterController controller;
    float verticalVelocity;
    float gravity = 10.0f;
    float jumpForce = 2.2f;
    float animationDuration = 3.0f;
    bool dead;
    float score = 0.0f;
    int money;
    bool[] lifes;
    int currentLife;
    int amountOfLifes=0;
    float level = 15.0f;
    Animator anim;


    void Start () {
        lifes = new bool[3];
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        anim.SetBool("Ground", true);
        money = PlayerPrefs.GetInt("money");

        //если текущее время меньше последнего посещения - удалить данные об использованных попытках
        if (PlayerPrefs.GetInt("2".ToString()) > System.DateTime.Now.Hour * 60 + System.DateTime.Now.Minute)
        {
            PlayerPrefs.DeleteKey("0");
            PlayerPrefs.DeleteKey("1");
            PlayerPrefs.DeleteKey("2");
        }
        else
        {
            for (currentLife = 2; currentLife > -1; currentLife--)
            {
                //если прошло полчаса с утраты попытки - восполнить попытку
                if ((System.DateTime.Now.Hour * 60 + System.DateTime.Now.Minute) - PlayerPrefs.GetInt(currentLife.ToString()) < 30)
                {
                    lifes[currentLife] = false;
                }
                else
                    lifes[currentLife] = true;
            }
        }
        
        for (int i=0; i<3; i++)
        {
            if (lifes[i]==true)
                amountOfLifes++;
        }
        Debug.Log("amount of lifes: " + amountOfLifes);

        //установить текущий цвет предмета(выбирается в магазине, сохраняется текущий)
        Color one;
        ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("currentColor"), out one);
        colorMaterial.color = one;

    }
	
	void Update () {
        if (dead)
            return;
        
        if(controller.isGrounded)
        {
            verticalVelocity = -gravity*Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
               anim.SetBool("Ground", false);
            }
            else
                anim.SetBool("Ground", true);
        }
        else
        {
            anim.SetBool("Ground", false);
            verticalVelocity -= gravity*Time.deltaTime;
        }
        if(Time.time < animationDuration)
        {
            controller.Move(Vector3.forward * Time.deltaTime*speed);
            return;
        }

        moveVector = Vector3.zero;

        moveVector.x = Input.GetAxisRaw("Horizontal")*speed;
        if (Input.GetMouseButton(0))
        {
            if(Input.mousePosition.x > Screen.width/2)
            {
                moveVector.x = speed;
            }
            else
            {
                moveVector.x = -speed;
            }
        }
        moveVector.y = verticalVelocity*speed;
        moveVector.z = speed;
        controller.Move(moveVector*Time.deltaTime);

        
        score = GameObject.FindGameObjectWithTag("Player").transform.position.z+10;
        scoreText.text = ((int)score).ToString();
        gameOverScore.text = scoreText.text;

        if (transform.position.y < -2.0f)
            Death();

        if (score > level)
        {
            speed += 1.5f;
            level = level * 2;
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.z > transform.position.z + controller.radius && hit.gameObject.tag=="Finish")
            Death();
    }
    void Death()
    {
        gameOver.gameObject.SetActive(true);
        dead = true;
        if(score> PlayerPrefs.GetInt("record"))
            PlayerPrefs.SetInt("record", (int)score);

        PlayerPrefs.SetInt((amountOfLifes-1).ToString(), System.DateTime.Now.Hour * 60 + System.DateTime.Now.Minute);
        money += (int)(score/10);
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.Save();
        recordText.text = (PlayerPrefs.GetInt("record")).ToString();
    }
}

