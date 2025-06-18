using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [Header("UI")]
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image iconImage;

    private bool isMuted;

    private const string PlayerPrefsKey = "SoundMuted";

    void Start()
    {
        // Загружаем предыдущее состояние
        isMuted = PlayerPrefs.GetInt(PlayerPrefsKey, 0) == 1;
        ApplySoundState();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt(PlayerPrefsKey, isMuted ? 1 : 0);
        ApplySoundState();
    }

    private void ApplySoundState()
    {
        AudioListener.volume = isMuted ? 0f : 1f;
        if (iconImage != null)
            iconImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
