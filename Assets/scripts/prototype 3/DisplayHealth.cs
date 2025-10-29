using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayHealth : MonoBehaviour
{

    public Combatant playerHealth;
    public Combatant enemyHealth;
    public TMP_Text healthDisplay;

    public string pHealth;
    public string eHealth;

    // Update is called once per frame
    void Update()
    {
        pHealth = GameObject.FindWithTag("Player").GetComponent<Combatant>().Health.ToString();
        eHealth = GameObject.FindWithTag("Enemy").GetComponent<Combatant>().Health.ToString();

        healthDisplay.text = "player health:" + pHealth + "\r\nenemy health:" + eHealth;
    }
}
