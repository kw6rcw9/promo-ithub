using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenKeyboard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_InputField inputField;
    private TouchScreenKeyboard keyboard;
    private string currentText = "";

  
    public void OnPointerClick(PointerEventData eventData)
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        StartCoroutine(CheckKeyboardStatus());
        currentText = "";
    }

    private IEnumerator CheckKeyboardStatus()
    {
        while (keyboard != null)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Visible)
            {
                currentText = keyboard.text; // Обновляем текущий текст
                Debug.LogError("KeyBoard Text (during input): " + currentText);
                inputField.text = currentText;
                Debug.LogError("INPUT TEXT AAAAAAA: " + inputField.text);
            }
            else if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                Debug.LogError("KeyBoard Text (on Done): " + currentText);
                inputField.text = currentText;
                Debug.LogError("INPUT TEXT AAAAAAA: " + inputField.text);
                keyboard = null;
                break; // Завершаем корутину
                
            }
            else if (keyboard.status == TouchScreenKeyboard.Status.Canceled)
            {
                Debug.LogError("KeyBoard Text (on Cancel): " + currentText);
                keyboard = null;
                break; // Завершаем корутину
                
            }
            yield return new WaitForSeconds(0.1f); // Интервал проверки 0.1 секунд
        }
    }

}
