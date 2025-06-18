using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BackendSystem.Core;
using CoreSystem;
using Cysharp.Threading.Tasks;
using PlayerSystem.TeleportSystem;
using R3;
using ScoreSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.LeaderboardSystem
{
    public class NewPlayerFormView : MonoBehaviour
    {
        [SerializeField] private GameObject formPanel;
        [SerializeField] private TMP_Text mainText;
        [SerializeField] private TMP_InputField inputFieldName;
        [SerializeField] private TMP_InputField inputFieldNumber;
        [SerializeField] private TMP_InputField inputFieldEmail;
        [SerializeField] private Sprite enabledIcon;
        [SerializeField] private Sprite disabledIcon;
        private bool _isChangedIcon;
        [Inject] private PlayerScoreController _playerScoreController;

        public void Init()
        {
            transform.GetComponent<Image>().sprite = disabledIcon;
            gameObject.GetComponent<Button>().enabled = false;
            _isChangedIcon = false;
            TeleportPlayer.NewPlayer
                .Subscribe(ShowForm).AddTo(this);
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(inputFieldName.text)
                || string.IsNullOrEmpty(inputFieldNumber.text) || string.IsNullOrEmpty(inputFieldEmail.text))
            {
                if (_isChangedIcon)
                {
                    transform.GetComponent<Image>().sprite = disabledIcon;
                    gameObject.GetComponent<Button>().enabled = false;
                    _isChangedIcon = false;
                }

                return;
            }

            if (!_isChangedIcon)
            {
                transform.GetComponent<Image>().sprite = enabledIcon;
                gameObject.GetComponent<Button>().enabled = true;
                _isChangedIcon = true;
            }

        }

        void ShowForm(Unit unit)
        {
            formPanel.SetActive(true);
        }

        public async void SendData()
        {
            var name = inputFieldName.text;
            var number = inputFieldNumber.text;
            var email = inputFieldEmail.text;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(number) || string.IsNullOrEmpty(email))
            {
                mainText.color = Color.red;
                 mainText.text = "Заполни все поля";
                return;
            }

            if (!await ValidateForm(name, email, number))
            {
                mainText.color = Color.red;
                
                return;
            }
            
            var playerData = new PlayerData(name, Score.ScoreCount,number, email);

            _playerScoreController.SendRecord(Score.ScoreCount,playerData);
            formPanel.SetActive(false);


        }

       

        public  async UniTask<bool> ValidateForm(string name, string email, string phone)
        {
            List<string> bannedWords = new List<string> {
                // Английские маты и оскорбления
                "fuck", "fuk", "f*ck", "f.u.c.k", "shit", "sh1t", "sh!t",
                "bitch", "bi7ch", "b!tch", "asshole", "a55hole", "dick", "d1ck",
                "cock", "c0ck", "cunt", "whore", "slut", "faggot", "fag", "nigger", "nigga",

                // Русские маты и оскорбления (включая транслит)
                "сука", "сучка", "су4ка", "пизда", "пездa", "пидор", "пидр", "педик", "петух",
                "блять", "бля", "бл@", "бл*дь", "ебать", "ебан", "ёб", "ебло", "еблан",
                "хуй", "хер", "xyй", "xуй", "х*й", "хрен", "мудак", "мудила", "гандон", "гондон", "жопа",
                "долбоёб", "долбаеб", "дебил", "даун", "идиот", "кретин", "обосса", "ссаный",

                // Транслит и сокращения
                "blyat", "suka", "pidr", "pidor", "ebat", "eblan", "mudak", "gandon", "govno", "zhopa", "loh", "lox", "looser", "loser",

                // Дополнительные обходы
                "f.u.c.k", "f u c k", "s u c k", "c u n t", "c u c k", "d i c k", "porn", "porno"
            };
            // Валидация имени
            if (string.IsNullOrWhiteSpace(name))
            {
                mainText.text  = "Имя не может быть пустым";
                return false;
            }

            if (name.Length < 2 || name.Length > 8)
            {
                mainText.text = "Имя должно содержать от 2 до 10 символов";
                return false;
            }

            string lowered = name.ToLower();

            foreach (var word in bannedWords)
            {
                if (lowered.Contains(word))
                {
                    mainText.text = "Имя содержит недопустимые слова";
                    return false;
                }
            }

            if (await _playerScoreController.CheckName(name))
            {
                mainText.text = "Такое имя уже есть. Придумай другое!";
                return false;
            }
            // Валидация email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                mainText.text = "Некорректный адрес электронной почты";
                return false;
            }

            // Валидация телефона (например: +7 999 123-45-67 или +375 29 1234567)
            if (!Regex.IsMatch(phone, @"^\+?\d{7,15}$"))
            {
                mainText.text = "Неверный формат номера телефона";
                return false;
            }
            return true;
        }
    }
}
