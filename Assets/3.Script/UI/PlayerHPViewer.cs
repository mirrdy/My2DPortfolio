using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPViewer : MonoBehaviour
{
    private Slider sliderHP;
    [SerializeField] private PlayerControll player;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out sliderHP);
    }

    // Update is called once per frame
    void Update()
    {
        sliderHP.value = (float)player.currentHp / (float)player.maxHp;
    }
}
