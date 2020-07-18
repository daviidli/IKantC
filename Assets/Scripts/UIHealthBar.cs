using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image fullHearts;
    public Image emptyHearts;

    RectTransform full;
    RectTransform empty;

    int currentHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        full = fullHearts.GetComponent<RectTransform>();
        empty = emptyHearts.GetComponent<RectTransform>();

        int maxHealth = GameController.Instance.MaxHealth;
        full.sizeDelta = new Vector2(85 * maxHealth, 85);
        empty.sizeDelta = new Vector2(85 * maxHealth, 85);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.Health != currentHealth)
        {
            currentHealth = GameController.Instance.Health;
            Resize(GameController.Instance.Health);
        }
    }

    void Resize(int health)
    {
        full.sizeDelta = new Vector2(85 * health, 85);
    }
}
