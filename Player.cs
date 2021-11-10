using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int race;                                        //
    public ProgressBar health;                              //
    public ProgressBarCircle stamina;                       //
    public ProgressBar mana;                                //  Valeurs externes
    public Image hunger;                                    //
    public Animator anim;                                   //
    public CharacterController controller;                  //
    public TMP_Text Message;                                //

    private bool run = false;                               //
    private bool majDown = false;                           //
    private float horizontal = 0f;                          //
    private bool fighting = false;                          //  Valeurs d'états
    public bool ate = false;                                //
    public bool blackOut = false;                           //
    private bool dying = false;                             //
    private bool blackedOut = false;                        //

    private float _staminaPool;                             //
    private float _staminaCurrent;                          //  Valeurs liées à l'endurance
    private float _staminaRegen;                            //

    public float healthPool;                                //
    public float healthCurrent;                             //  Valeurs liées aux points de vie
    private float _healthRegen;                             //

    private float _manaPool;                                //
    private float _manaCurrent;                             //  Valeurs liées à la magie
    private float _manaRegen;                               //

    private int _speedLevel; private float _speed = 2f; private float _speedExp = 0;
    private int _crafterLevel; private float _crafterExp;
    private int _stealthLevel; private float _stealthExp;
    private int _handLevel; private float _handExp;                                         //Préparation pour la partie RPG.
    private int _knifeLevel; private float _knifeExp;                                       //Chaque niveau est plus compliqué
    private int _axeLevel; private float _axeExp;                                           //à obtenir mais accélère et rends
    private int _spearLevel; private float _spearExp;                                       //les taches plus efficientes.
    private int _shovelLevel; private float _shovelExp;
    private int _pickLevel; private float _pickExp;

    // Start is called before the first frame update
    void Start()
    {
        _speedLevel = 1; _shovelLevel = 1; _pickLevel = 1; _axeLevel = 1; _crafterLevel = 1; _handLevel = 1; _knifeLevel = 1; _spearLevel = 1; _stealthLevel = 1;
        healthPool = 100f; _healthRegen = 0.005f; _staminaPool = 100f; _staminaRegen = 0.05f; _manaPool = 50f; _manaRegen = 0.025f;
        switch(race)
        {
            case 0:                                 //Cas joueur humain. A planifié avec un load component
                break;                              //Considérer ajouter un bonus à l'expérience des skills.

            case 1:                                 //Cas joueur gobelin. A planifié avec un load component
                break;                              //Considérer une réduction des chances de poison par nourriture.

            case 2:                                 //Cas joueur orc. Cas de test par défaut.
                _staminaRegen *= 1.2f;
                _staminaPool *= 1.5f;
                break;
            case 3:                                 //Cas joueur elfe. A planifié avec un load component
                _manaRegen *= 1.2f;
                _manaPool *= 1.5f;
                break;
            default:
                break;
        }
        _staminaCurrent = _staminaPool; _manaCurrent = _manaPool; healthCurrent = healthPool;

        GameObject.Find("Canvas/BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, 0); //Ecran noir pour tomber dans les pommes, ici transparent.
    }

    // Update is called once per frame
    void Update()
    {
        health.BarValue = healthCurrent / healthPool * 100f; stamina.BarValue = _staminaCurrent / _staminaPool * 100f; mana.BarValue = _manaCurrent / _manaPool * 100f;
                    //mise à jour des différentes barre dans l'UI
        
        
        if (blackOut)                        //Si perte de connaissance
        {
            if(!blackedOut)
            {
                StopAllCoroutines();
                StartCoroutine(BlackOut());
            }
        } else
        {
            if (Input.GetKeyDown("left shift"))          //A modifier pour un double appui sur la touche avancer.
            {
                majDown = !majDown;
            }

            StartCoroutine(Regeneration());

            if (!fighting)                              //Si l'on est pas en train de donner un coup, on peut marcher ou courir
            {
                StartCoroutine(Moving());
            }
            StartCoroutine(Fight());
            StartCoroutine(Food());

            horizontal += Input.GetAxisRaw("Horizontal");                                                           //
            Quaternion targetRotation = Quaternion.AngleAxis(horizontal, Vector3.up);                               //pour une rotation en fonction de l'appui des touches horizontal
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime);    //
        }
    }

    private IEnumerator BlackOut()                          //Perte de connaissance
    {
        blackedOut = true;
        GameObject.Find("Canvas/BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, 255);
        Message.text = "You blacked out.";
        yield return new WaitForSeconds(10f);
        GameObject.Find("Canvas/BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        Message.text = "";
        blackOut = false;
        blackedOut = false;
    }
    
    private IEnumerator Regeneration()
    {
        if(_staminaCurrent < _staminaPool && !run)              //Régéneration de l'endurance lorsque l'on ne cours pas.
        {
            _staminaCurrent += _staminaRegen;
            if (_staminaCurrent > _staminaPool)
            {
                _staminaCurrent = _staminaPool;
            }
        }

        if (_manaCurrent < _manaPool)                           //Régération de la magie. Inutile pour l'instant.
        {                                                       //Considérer d'autre skill obtenu?
            _manaCurrent += _manaRegen;                         //A voir plus en détail si le temps
            if (_manaCurrent > _manaPool)
            {
                _manaCurrent = _manaPool;
            }
        }

        if(healthCurrent < healthPool && !dying && hunger.fillAmount > 50)        //Régéneration des points de vie lorsque l'on ne perds pas de 
        {                                                                         //vie activement et tant que la faim est remplies à +50%
            healthCurrent += _healthRegen;
            if (healthCurrent > healthPool)
            {
                healthCurrent = healthPool;
            }
        }
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator Moving()                                        //Co-routine de déplacement
    {
        while (Input.GetAxis("Vertical")!=0 && _staminaCurrent > 0)
        {
            if(majDown && hunger.fillAmount != 0)
            {
                run = true;
                if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    anim.Play("Run");                                                       //Lancer l'animation de course. Attention, toutes les nommée "Run"
                }
                var forward = transform.TransformDirection(Vector3.forward * Input.GetAxisRaw("Vertical"));
                controller.SimpleMove(forward * _speed);
                _staminaCurrent -= 0.08f / _speedLevel;
                _speedExp += Mathf.Log(_speedLevel+1)/(250*_speedLevel);
                if(_speedExp>=100&&_speedLevel<10)
                {
                    _speedLevel += 1;
                    _speedExp = 0;
                    Message.text = "Votre compétence \"Courir\" viens de passer au niveau " + _speedLevel;              //Level Up du skill de la course
                    _speed = _speedLevel / (3f * Mathf.Log(_speedLevel));                                               //Augmente la vitesse de course
                    yield return new WaitForSeconds(2f);
                    Message.text = "";
                }
                if(_staminaCurrent<0)
                {
                    _staminaCurrent = 0;
                    hunger.fillAmount -= 0.2f;
                }
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            } else
            {

                run = false;
                anim.Play("Walk");                                                          //Lancer l'animation de marche. Attention, toutes les nommée "Walk"
                var forward = transform.TransformDirection(Vector3.forward * Input.GetAxisRaw("Vertical"));
                controller.SimpleMove(forward);
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        anim.Play("Idle");
        if (_staminaCurrent==0)
        {
            yield return new WaitForSeconds(3.5f);
        }
        run = false;
    }

    private IEnumerator Fight()                                                             //Non fonctionel. Devrait permettre de frapper des cibles pour infliger des dégats
    {                                                                                       //Animation devrait changer en fonction de l'objet équiper
        while (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))                  //Dégâts en fonction du niveau du skill lié à l'objet utile
        {

            fighting = true;
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("Attack");                                                        //Lancer l'animation d'attaque. Attention, toutes les nommée "Attack"
            }
            else if (Input.GetMouseButtonDown(1))
            {
                anim.Play("Block");                                                         //Lancer l'animation de parade. Attention, toutes les nommée "Block"
            }
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        fighting = false;
    }

    private IEnumerator Food()                                      //Co-routine de gestion de la faim
    {
        if(ate)
        {
            dying = false;                                          //Après avoir manger, retire la perte de vie continuelle.
            yield return new WaitForSeconds(120f);
        } else
        {
            if (run)
            {
                hunger.fillAmount -= 0.0003f;                       //Plus grande faim avec la course. Faire de même avec les phases de combats?
            } else
            {
                hunger.fillAmount -= 0.00001f;
            }
        }
        if(hunger.fillAmount == 0 && healthCurrent/healthPool > 0.1f)
        {
            dying = true;                                           //Plus de réserve, commence à mourrir de faim
            healthCurrent -= 0.05f;
            if(healthCurrent / healthPool < 0.1f)
            {
                healthCurrent = 0.1f * healthPool;
            }
        }
    }
}
