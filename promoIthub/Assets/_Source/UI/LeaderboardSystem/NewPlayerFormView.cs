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
        [SerializeField] private Sprite enabledIcon;
        [SerializeField] private Sprite disabledIcon;
        private bool _isChangedIcon;
        [Inject] private PlayerScoreController _playerScoreController;

        public void Init()
        {
            transform.GetComponent<Image>().sprite = disabledIcon;
            gameObject.GetComponent<Button>().enabled = false;
            _isChangedIcon = false;
            /*TeleportPlayer.NewPlayer
                .Subscribe(ShowForm).AddTo(this);*/
        }

        /*private void Update()
        {
            if (string.IsNullOrEmpty(inputFieldName.text))
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

        }*/

        void ShowForm(Unit unit)
        {
            formPanel.SetActive(true);
        }

        public async void SendData()
        {
            var name = inputFieldName.text;
            if (string.IsNullOrEmpty(name))
            {
                mainText.color = Color.red;
                 mainText.text = "Заполни все поля";
                return;
            }

            if (!await ValidateForm(name))
            {
                mainText.color = Color.red;
                
                return;
            }
            
            var playerData = new PlayerData(name, Score.ScoreCount);

            _playerScoreController.SendRecord(Score.ScoreCount,playerData);
            formPanel.SetActive(false);


        }

       

        public  async UniTask<bool> ValidateForm(string name)
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
                "долбоёб", "долбаеб", "дебил", "даун", "идиот", "кретин", "обосса", "ссаный", "минет","minet",
                "м1нет", "минэт",

                // Транслит и сокращения
                "blyat", "suka", "pidr", "pidor", "ebat", "eblan", "mudak", "gandon", "govno", "zhopa", "loh", "lox", "looser", "loser",

                // Дополнительные обходы
                "f.u.c.k", "f u c k", "s u c k", "c u n t", "c u c k", "d i c k", "porn", "porno",
                
                "n+([ehiy]+|ay|ey|io|[il]+)[bgq$]+h?(a+|aer|a+h+|a+r+|e+|ea|eoa|e+r+|ie|ier|let|lit|o|or|r+|u|uh|uhr|u+r+|ward|y+)s*",
                "f[ae]y?g+[oeiu]+t+s?", "даунская", "пидорасы", "ching\\W*chongs?", "niggers", "re+t+a?r[dt]+s?",
                "towel\\W*heads?", "пидорка", "pac?k(i|ie|y)", "ting\\W*tongs?", "trann(ie|y)s?",
                "white\\W*trash", "ni:gg\\w*:\\w*", "chin(c|k)s?", "n+i+[gq]+s?", "yam\\W*yams?",
                "\\w*niggas?", "chinamans?", "polacke?s?", "neekeris?", "nigfhers?", "redskins?",
                "beaners?", "coolies?", "f+a+g+s*", "fenians?", "gringos?", "newfags?", "ni gg er",
                "nigga\\w*", "niggles?", "nignogs?", "rus+kis?", "wiggers?", "Ϝaggоt", "gypsies", "injuns?", "jiggas?", "nikkas?", "wiggas?", "\\w*fag",
                "coons?", "dykes?", "fa66ot", "fagts?", "gooks?", "gypos?", "homos?", "kikes?", "spics?",
                "tards?", "abos?", "fgts?", "fаgs", "gypsy", "japs?", "wops?", "fagz", "fаg", "f@g",
                "\\w*NIGGER\\w*", "\\w*ɴɪɢɢᴇʀ\\w*", "\\w*nіggеr\\w*", "\\w*NÍGGER\\w*", "\\w*n\\\\\\|gger\\w*", "\\w*niggеr\\w*",
                "\\w*F4GG0T\\w*", "\\w*N1GG3R\\w*", "\\w*nigger\\w*", "\\w*nigg4\\w*", "\\w*niggz\\w*",
                @"\bпедераст(ический|ина|ия|и|ы)?\b",
                @"\bоднодырочники\b",
                @"\bпиндостан(цы|ов|цы)?\b",
                @"\bговносерка\b",
                @"\bживоглотка\b",
                @"\bускоглазый\b",
                @"\bчерножоп(ые|ый|ы)?\b",
                @"\bчернозад(ый|ы)?\b",
                @"\bчерномаз(ый|ы)?\b",
                @"\базуроеб(ам|ы)?\b",
                @"\bпидораска\b",
                @"\bпиндос(ы|ов|а)?\b",
                @"\bговномес\b",
                @"\bзигхайль\b",
                @"\bпедераси\b",
                @"\bпедераст\b",
                @"\bпедеруга\b",
                @"\bпидарасы\b",
                @"\bпедиков\b",
                @"\bпедобир\b",
                @"\bпедрила\b",
                @"\bпедрило\b",
                @"\bпидарас\b",
                @"\bпидрило\b",
                @"\bциганин\b",
                @"\bпидор\\w*\b",
                @"\bволоеб\b",
                @"\bдаунит\b",
                @"\bжид(яра|ов|овня|ах|ам|е|у|ы)?\b",
                @"\bопездал\b",
                @"\bсучара\b",
                @"\bсучары\b",
                @"\bуебан(ы|ы)?\b",
                @"\bхач(ами|ах|ам|ей|ек|ом|ик|ил)?\b",
                @"\bгомик\b",
                @"\bгомос\b",
                @"\bкалич\b",
                @"\bкурва\b",
                @"\bнигг(а|ер|ро)?\b",
                @"\bпедик(и|ов|а|у)?\b",
                @"\bпидер\b",
                @"\bсуко(й|й)?\b",
                @"\bуебак\b",
                @"\bуебищ\b",
                @"\bуебк(и|а)?\b",
                @"\bчурка\b",
                @"\bчурк\b",
                @"\bп+дар\b",
                @"\beблaн\b",
                @"\bжид(а|е|у)?\b",
                @"\bсука\b",
                @"\bсуки\b",
                @"\bхач(а|е|и)?\b",
                @"\bpidaras\b",
                @"\bpidoras\b",
                @"\bpedik\b",
                @"\bpidar\b",
                @"\bpidor\b",
                @"\bpido\b",
                @"\bвыблядк\\w*\b",
                @"\bдалбаеб\\w*\b",
                @"\bдолба[её]б\\w*\b",
                @"\bдoлбaёб\\w*\b", // латинская 'o'
                @"\bослаеб\\w*\b",
                @"\bпидар\\w*\b",
                @"\bуeбaн\\w*\b",
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


            if (await _playerScoreController.CheckName(name))
            {
                mainText.text = "Такое имя уже есть. Придумай другое!";
                return false;
            }
            
            return NotContainsBannedWord(name,bannedWords);
            
        }

        private static readonly Regex[] BannedPatterns = new Regex[]
        {
            new Regex(@"\bn[i1!]+g+[e3]+r+\b", RegexOptions.IgnoreCase), // nigger вариации
            new Regex(@"\bn[i1!]+g+[gq]+a+[sz]?\b", RegexOptions.IgnoreCase), // nigga, niggaz, nigas
            new Regex(@"\bn\s*[\W_]*i\s*[\W_]*g\s*[\W_]*g\s*[\W_]*e\s*[\W_]*r\b",
                RegexOptions.IgnoreCase), // n i g g e r

            new Regex(@"\bf[a@]+g+[o0]+t+\b", RegexOptions.IgnoreCase), // faggot
            new Regex(@"\bf[a@]+g+[s$]+\b", RegexOptions.IgnoreCase), // fags
            new Regex(@"\bf[a@]+g+z+\b", RegexOptions.IgnoreCase), // fagz
            new Regex(@"\bf[a@]+g+\b", RegexOptions.IgnoreCase), // fag

            new Regex(@"\bfa66ot\b", RegexOptions.IgnoreCase), // fa66ot obfuscation
            new Regex(@"\bfagts?\b", RegexOptions.IgnoreCase), // fagts

            new Regex(@"\btrann?(y|ie)s?\b", RegexOptions.IgnoreCase), // tranny, trannies

            new Regex(@"\bpid[o0]+r[a-z]*\b", RegexOptions.IgnoreCase), // пидор и вариации
            new Regex(@"\bпидорасы\b", RegexOptions.IgnoreCase),
            new Regex(@"\bпидорка\b", RegexOptions.IgnoreCase),

            new Regex(@"\bдаун(ская)?\b", RegexOptions.IgnoreCase), // даун/даунская

            new Regex(@"\bre+t+a?r[dt]+s?\b", RegexOptions.IgnoreCase), // retard
            new Regex(@"\btard(s)?\b", RegexOptions.IgnoreCase), // tards

            new Regex(@"\btowel\W*heads?\b", RegexOptions.IgnoreCase), // towelhead
            new Regex(@"\bwhite\W*trash\b", RegexOptions.IgnoreCase), // white trash
            new Regex(@"\bbeaners?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bcoons?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bspics?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bjaps?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bkikes?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bgooks?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bgyps(y|ies)\b", RegexOptions.IgnoreCase),
            new Regex(@"\binjuns?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bchinamans?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bchin(c|k)s?\b", RegexOptions.IgnoreCase),
            new Regex(@"\brus+kis?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bpolacke?s?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bneekeris?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bnigfhers?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bnignogs?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bniggles?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bdykes?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bgringos?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bcoolies?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bfenians?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bnewfags?\b", RegexOptions.IgnoreCase),
            new Regex(@"\babos?\b", RegexOptions.IgnoreCase),
            new Regex(@"\bfgts?\b", RegexOptions.IgnoreCase),

            new Regex(@"\bnigg[ae4z]+\w*", RegexOptions.IgnoreCase), // nigg4, niggaz, nigga123
            new Regex(@"\bɴɪɢɢᴇʀ\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant), // Unicode variants
            new Regex(@"\bnіggеr\b", RegexOptions.IgnoreCase), // кириллические подмены (i/e)
            new Regex(@"\bn\\\|gger\b", RegexOptions.IgnoreCase),
            new Regex(@"\bF4GG0T\b", RegexOptions.IgnoreCase),
            new Regex(@"\bN1GG3R\b", RegexOptions.IgnoreCase),
            new Regex(@"\bNÍGGER\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
        };
        public  bool NotContainsBannedWord(string message, List<string> patternList)
        {
            foreach (string pattern in patternList)
            {
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch(message))
                {
                    mainText.text = "Имя содержит недопустимые слова";
                    return false;
                }
            }
            return true;
        }

    }
    
}
