using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip selectionSound;
    private RectTransform rectTransworm;
    private int currentPos;

    private void Awake()
    {
        rectTransworm = GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        else if(Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
            
    }


    private void Interact()
    {
        SoundManager.instance.PlaySound(selectionSound);

        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }


    private void ChangePosition(int _change)
    {
        currentPos += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPos < 0)
            currentPos = options.Length - 1;
        else if(currentPos >= options.Length)
            currentPos = 0;

        rectTransworm.position = new Vector3(rectTransworm.position.x, options[currentPos].position.y, 0);
    }
}
