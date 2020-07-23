using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLocalization
{
    #region Short Manual
    /*
        This singleton is designed to automatically translate text values of all Text components in the scene.
        Using braces {..}, put localization strings in all the places where automatic translation is required. 
        For instance, instead of the text "Pause" in the UI component use localization string {PAUSE}, 
        then add translations for desired languages.

        Call method TranslateAll() from any component in the scene.
        Example:

        void Awake() {
           SimpleLocalization.Translate ();
        }

        If you need to get a translation for a particular localization string use method Translate()
        Example:

        void ShowBalloon() {
            string title = SimpleLocalization.Translate("POW");
        }
    */
    #endregion

    /// <summary>
    /// This language will be used for "unfound" words.
    /// </summary>
    static SystemLanguage fallbackLanguage = SystemLanguage.English;

    /// <summary>
    /// Set localization strings as in example. Using upper case is not necessarily.
    /// </summary>
    static void SetupLanguages()
    {
        #region English
        init(SystemLanguage.English);

        push("TAP_TO_CONTINUE", "TAP TO CONTINUE");
        push("PLAY", "PLAY");
        push("SELECT_LOCATION", "SELECT WORLD");
        push("TRACK", "SONG");
        push("SCORE", "SCORE");
        push("DANCE_STYLE", "DANCE STYLE");
        push("SELECT_CHARACTER", "SELECT CHARACTER");
        push("BREAKDANCE", "BREAKDANCE");
        push("SALSA", "SALSA");
        push("LINDY", "LINDY HOP");
        push("MARTIAL", "MARTIAL DANCE");
        push("INDIAN", "INDIAN DANCE");
        push("FREAK", "FREAK DANCE");
        push("MODERN", "MODERN DANCE");
        push("SAPHIRE_CHAN", "SAPPHIRE CHAN");
        push("TAICHI", "TAICHI");
        push("QUERY_CHAN", "QUERY CHAN");
        push("MAID", "MAID");
        push("UNITY_CHAN", "UNITY CHAN");
        push("TAICHI_CASUAL", "TAICHI CASUAL");
        push("QUERY_XMAS", "QUERY XMAS");
        push("REPLAY", "REPLAY");
        push("EXIT", "EXIT");
        push("PAUSE", "PAUSE");
        push("GAME_OVER", "GAME OVER");
        push("NEW_RECORD", "NEW RECORD");
        push("RANK", "RANK");
        push("COMBO_PH", "~0~ COMBO x1");
        push("COMBO", "COMBO");
        push("NICE", "NICE");
        push("COOL", "COOL");
        push("+ALPHA", "+ALPHA");
        push("SPECTAC", "SPECTAC");
        push("CAPACITY", "CAPACITY");
        push("SUPERIOR", "SUPERIOR");
        push("OVERHEAT", "OVERHEAT");
        push("FEVER", "FEVER");
        push("PERFECT", "PERFECT");
        push("WASTED", "WASTED");

        #endregion

        #region Russian
        init(SystemLanguage.Russian);

        push("TAP_TO_CONTINUE", "НАЖМИТЕ НА ЭКРАН");
        push("PLAY", "НАЧАТЬ ИГРУ");
        push("SELECT_LOCATION", "ВЫБЕРИТЕ ЛОКАЦИЮ");
        push("TRACK", "КОМПОЗИЦИЯ");
        push("SCORE", "ОЧКИ");
        push("DANCE_STYLE", "СТИЛЬ ТАНЦА");
        push("SELECT_CHARACTER", "ВЫБЕРИТЕ ПЕРСОНАЖА");
        push("BREAKDANCE", "Брейкдэнс");
        push("SALSA", "Сальса");
        push("LINDY", "Линди-Хоп");
        push("MARTIAL", "Танец боевых искусств");
        push("INDIAN", "Индийский танец");
        push("FREAK", "Фрикдэнс");
        push("MODERN", "Модерн Дэнс");
        push("SAPHIRE_CHAN", "САПФИР ТЯН");
        push("TAICHI", "ТАИЧИ");
        push("QUERY_CHAN", "КВЕРИ ТЯН");
        push("MAID", "ГОРНИЧНАЯ");
        push("UNITY_CHAN", "ЮНИТИ ТЯН");
        push("TAICHI_CASUAL", "ТАИЧИ НЕФОРМАЛ");
        push("QUERY_XMAS", "КВЕРИ РОЖ-ВО");
        push("REPLAY", "ПОВТОР");
        push("EXIT", "ВЫХОД");
        push("PAUSE", "ПАУЗА");
        push("GAME_OVER", "КОНЕЦ ИГРЫ");
        push("NEW_RECORD", "НОВЫЙ РЕКОРД");
        push("RANK", "РАНГ");
        push("COMBO_PH", "~0~ КОМБО x1");
        push("COMBO", "КОМБО");
        push("NICE", "ХОРОШО");
        push("COOL", "КРУТО");
        push("+ALPHA", "УМЕЛО");
        push("SPECTAC", "ТАЛАНТ");
        push("CAPACITY", "МОЩНО");
        push("SUPERIOR", "ВЫСШИЙ");
        push("OVERHEAT", "ПЕРЕГРЕВ");
        push("FEVER", "ЛИХАЧ");
        push("PERFECT", "ИДЕАЛЬНО");
        push("WASTED", "ПОТРАЧЕНО");

        #endregion

        #region Japanese
        init(SystemLanguage.Japanese);

        push("TAP_TO_CONTINUE", "タップして");
        push("PLAY", "スタート");
        push("SELECT_LOCATION", "セレクトワールド");
        push("TRACK", "ソング");
        push("SCORE", "スコア");
        push("DANCE_STYLE", "ダンススタイル");
        push("SELECT_CHARACTER", "セレクトキャラクター");
        push("BREAKDANCE", "ブレイクダンス");
        push("SALSA", "サルサ");
        push("LINDY", "リンディホップ");
        push("MARTIAL", "マラルダンス");
        push("INDIAN", "インディアンダンス");
        push("FREAK", "フリクションダンス");
        push("MODERN", "モダンダンス");
        push("SAPHIRE_CHAN", "サファイアチャン");
        push("TAICHI", "太一カ");
        push("QUERY_CHAN", "クエリチャン");
        push("MAID", "メイド");
        push("UNITY_CHAN", "ユニティチャン");
        push("TAICHI_CASUAL", "太一カジュアル");
        push("QUERY_XMAS", "クエリークリスマス");
        push("REPLAY", "リプレイ");
        push("EXIT", "出口");
        push("PAUSE", "一時停止する");
        push("GAME_OVER", "ゲームオーバー");
        push("NEW_RECORD", "新記録");
        push("RANK", "ランク");
        push("COMBO_PH", "~0~ コンボ x1");
        push("COMBO", "コンボ");
        push("NICE", "良い");
        push("COOL", "スゴイ");
        push("+ALPHA", "+アルファ");
        push("SPECTAC", "優れた");
        push("CAPACITY", "キャパシティ");
        push("SUPERIOR", "優れました");
        push("OVERHEAT", "オーバーヒート");
        push("FEVER", "フィーバー");
        push("PERFECT", "パーフェクト");
        push("WASTED", "悲惨な");

        #endregion

        #region Korean
        init(SystemLanguage.Korean);

        push("TAP_TO_CONTINUE", "여기를 누르세요.");
        push("PLAY", "스타트");
        push("SELECT_LOCATION", "세계 선택");
        push("TRACK", "노래");
        push("SCORE", "점수");
        push("DANCE_STYLE", "댄스 스타일");
        push("SELECT_CHARACTER", "문자 선택");
        push("BREAKDANCE", "브레이크 댄스");
        push("SALSA", "살사");
        push("LINDY", "린디 홉");
        push("MARTIAL", "무도회");
        push("INDIAN", "인디언 무용");
        push("FREAK", "괴물 댄스");
        push("MODERN", "현대 무용");
        push("SAPHIRE_CHAN", "사파이어 챈");
        push("TAICHI", "태극권");
        push("QUERY_CHAN", "질문 찬");
        push("MAID", "하녀");
        push("UNITY_CHAN", "유니티 챈");
        push("TAICHI_CASUAL", "타이치 캐주얼");
        push("QUERY_XMAS", "쿼리 크리스마스");
        push("REPLAY", "다시 하다");
        push("EXIT", "출구");
        push("PAUSE", "중지");
        push("GAME_OVER", "게임 끝");
        push("NEW_RECORD", "새로운 기록");
        push("RANK", "계급");
        push("COMBO_PH", "~0~ 콤보 x1");
        push("COMBO", "콤보");
        push("NICE", "좋은");
        push("COOL", "시원한");
        push("+ALPHA", "재능");
        push("SPECTAC", "장관의");
        push("CAPACITY", "생산 능력");
        push("SUPERIOR", "우수한");
        push("OVERHEAT", "과열하다");
        push("FEVER", "흥분상태");
        push("PERFECT", "완전한");
        push("WASTED", "살해된");

        #endregion

        #region Chinese
        init(SystemLanguage.Chinese);

        push("TAP_TO_CONTINUE", "點擊繼續");
        push("PLAY", "開始");
        push("SELECT_LOCATION", "選擇世界");
        push("TRACK", "歌曲");
        push("SCORE", "得分了");
        push("DANCE_STYLE", "舞蹈風格");
        push("SELECT_CHARACTER", "選擇字符");
        push("BREAKDANCE", "霹靂舞");
        push("SALSA", "薩爾薩");
        push("LINDY", "林迪跳");
        push("MARTIAL", "武術舞蹈");
        push("INDIAN", "印度舞");
        push("FREAK", "微笑舞蹈");
        push("MODERN", "現代舞");
        push("SAPHIRE_CHAN", "陳寶蓮");
        push("TAICHI", "太極");
        push("QUERY_CHAN", "查詢陳");
        push("MAID", "女傭");
        push("UNITY_CHAN", "統一蓮");
        push("TAICHI_CASUAL", "太極休閒");
        push("QUERY_XMAS", "查詢聖誕節");
        push("REPLAY", "重播");
        push("EXIT", "出口");
        push("PAUSE", "暫停");
        push("GAME_OVER", "遊戲結束");
        push("NEW_RECORD", "新紀錄");
        push("RANK", "秩");
        push("COMBO_PH", "~0~ 組合 x1");
        push("COMBO", "組合");
        push("NICE", "好");
        push("COOL", "涼");
        push("+ALPHA", "天才");
        push("SPECTAC", "壯觀");
        push("CAPACITY", "容量");
        push("SUPERIOR", "優越");
        push("OVERHEAT", "過熱");
        push("FEVER", "發熱");
        push("PERFECT", "完善");
        push("WASTED", "浪費");

        #endregion

        #region German
        init(SystemLanguage.German);

        push("TAP_TO_CONTINUE", "KLICK HIER");
        push("PLAY", "SPIELEN");
        push("SELECT_LOCATION", "WÄHLEN WELT");
        push("TRACK", "SANG");
        push("SCORE", "STAND");
        push("DANCE_STYLE", "TANZ STIL");
        push("SELECT_CHARACTER", "ZEICHEN AUSWÄHLEN");
        push("BREAKDANCE", "BREAKDANCE");
        push("SALSA", "SALSA");
        push("LINDY", "LINDY HOP");
        push("MARTIAL", "MARTIELLER TANZ");
        push("INDIAN", "INDISCHER TANZ");
        push("FREAK", "FREITANZ");
        push("MODERN", "MODERNER TANZ");
        push("SAPHIRE_CHAN", "SAPPHIRE CHAN");
        push("TAICHI", "TAICHI");
        push("QUERY_CHAN", "QUERY CHAN");
        push("MAID", "MAID");
        push("UNITY_CHAN", "UNITY CHAN");
        push("TAICHI_CASUAL", "TAICHI CASUAL");
        push("QUERY_XMAS", "QUERY XMAS");
        push("REPLAY", "WIEDERHOLUNG");
        push("EXIT", "AUSFAHRT");
        push("PAUSE", "PAUSE");
        push("GAME_OVER", "SPIEL IST AUS");
        push("NEW_RECORD", "NEUER EINTRAG");
        push("RANK", "RANG");
        push("COMBO_PH", "~0~ COMBO x1");
        push("COMBO", "COMBO");
        push("NICE", "GUT");
        push("COOL", "COOL");
        push("+ALPHA", "BEGABTES");
        push("SPECTAC", "SPEKTAK");
        push("CAPACITY", "KAPAZITÄT");
        push("SUPERIOR", "ÜBERLEGEN");
        push("OVERHEAT", "ÜBERHITZEN");
        push("FEVER", "FIEBER");
        push("PERFECT", "PERFEKT");
        push("WASTED", "VERSCHWENDET");

        #endregion

        #region French
        init(SystemLanguage.French);

        push("TAP_TO_CONTINUE", "CLIQUEZ ICI");
        push("PLAY", "DÉMARRER");
        push("SELECT_LOCATION", "SELECTIONNER LE MONDE");
        push("TRACK", "CHANSON");
        push("SCORE", "SCORE");
        push("DANCE_STYLE", "STYLE DE DANSE");
        push("SELECT_CHARACTER", "SÉLECTIONNER LE CARACTÈRE");
        push("BREAKDANCE", "BREAKDANCE");
        push("SALSA", "SALSA");
        push("LINDY", "LINDY HOP");
        push("MARTIAL", "DANSE MARTIALE");
        push("INDIAN", "DANSE INDIENNE");
        push("FREAK", "DANSE FOLLE");
        push("MODERN", "DANSE MODERNE");
        push("SAPHIRE_CHAN", "SAPPHIRE CHAN");
        push("TAICHI", "TAICHI");
        push("QUERY_CHAN", "QUERY CHAN");
        push("MAID", "MAID");
        push("UNITY_CHAN", "UNITY CHAN");
        push("TAICHI_CASUAL", "TAICHI CASUAL");
        push("QUERY_XMAS", "QUERY XMAS");
        push("REPLAY", "REJOUER");
        push("EXIT", "SORTIE");
        push("PAUSE", "PAUSE");
        push("GAME_OVER", "JEU TERMINÉ");
        push("NEW_RECORD", "NOUVEL ENREGISTREMENT");
        push("RANK", "RANG");
        push("COMBO_PH", "~0~ COMBO x1");
        push("COMBO", "COMBO");
        push("NICE", "AGRÉABLE");
        push("COOL", "APPRÉCIÉ");
        push("+ALPHA", "DOUÉ");
        push("SPECTAC", "SPECTAC");
        push("CAPACITY", "CAPACITÉ");
        push("SUPERIOR", "SUPÉRIEUR");
        push("OVERHEAT", "SURCHAUFFER");
        push("FEVER", "FIÈVRE");
        push("PERFECT", "PARFAIT");
        push("WASTED", "PERDU");

        #endregion

        #region Spanish
        init(SystemLanguage.Spanish);

        push("TAP_TO_CONTINUE", "PULSE AQUÍ");
        push("PLAY", "COMIENZO");
        push("SELECT_LOCATION", "SELECCIONA EL MUNDO");
        push("TRACK", "CANCIÓN");
        push("SCORE", "SCORE");
        push("DANCE_STYLE", "ESTILO DE DANZA");
        push("SELECT_CHARACTER", "SELECCIONE EL CARÁCTER");
        push("BREAKDANCE", "BREAKDANCE");
        push("SALSA", "SALSA");
        push("LINDY", "LINDY HOP");
        push("MARTIAL", "DANZA MARCIAL");
        push("INDIAN", "DANZA INDIA");
        push("FREAK", "DANZA ESPARITA");
        push("MODERN", "DANZA MODERNA");
        push("SAPHIRE_CHAN", "SAPPHIRE CHAN");
        push("TAICHI", "TAICHI");
        push("QUERY_CHAN", "QUERY CHAN");
        push("MAID", "MAID");
        push("UNITY_CHAN", "UNITY CHAN");
        push("TAICHI_CASUAL", "TAICHI CASUAL");
        push("QUERY_XMAS", "QUERY XMAS");
        push("REPLAY", "REPLAY");
        push("EXIT", "SALIDA");
        push("PAUSE", "PAUSA");
        push("GAME_OVER", "JUEGO TERMINADO");
        push("NEW_RECORD", "NUEVO RECORD");
        push("RANK", "RANGO");
        push("COMBO_PH", "~0~ COMBO x1");
        push("COMBO", "COMBO");
        push("NICE", "BONITO");
        push("COOL", "GUAY");
        push("+ALPHA", "DOTADO");
        push("SPECTAC", "APARATOSO");
        push("CAPACITY", "CAPACIDAD");
        push("SUPERIOR", "SUPERIOR");
        push("OVERHEAT", "SOBRECALENTAR");
        push("FEVER", "FIEBRE");
        push("PERFECT", "PERFECTO");
        push("WASTED", "VANO");

        #endregion
    }

    #region Singleton
    static SimpleLocalization instance;
    public static SimpleLocalization Instance
    {
        get { if (instance == null) { instance = new SimpleLocalization(); } return instance; }
    }
    #endregion

    #region Initizlization
    static bool isInitialized = false;

    static Dictionary<SystemLanguage, Dictionary<string, string>> s
        = new Dictionary<SystemLanguage, Dictionary<string, string>>();

    static SystemLanguage e, cc;

    delegate void DA(string key, string value); 
    delegate void PL(SystemLanguage lang, string term, int i); 
    delegate void IL(SystemLanguage lang);

    static DA push; static PL set; static IL init;

    static void initialize() {
        isInitialized = true;
        cc = Application.systemLanguage;
        push = delegate (string key, string value) { if (!s[e].ContainsKey(key)) s[e].Add(key, value); };
        init = delegate (SystemLanguage lang) { e = lang; s[e] = new Dictionary<string, string>(); };

        SetupLanguages();
        if (!s.ContainsKey(cc)) cc = SystemLanguage.English;
    }

    #endregion

    #region Methods
    /// <summary>
    /// Translates all the localization strings into the main language
    /// </summary>
    public static void TranslateAll()
    {
        var terms = Resources.FindObjectsOfTypeAll<Text>();
        if (!isInitialized) initialize();
        set = delegate (SystemLanguage lang, string term, int i)
        {
            string o; terms[i].text = (s[lang].TryGetValue(term, out o)) ? o : terms[i].text = term;
        };

        for (var i = 0; i < terms.Length; i++)
        {
            var t = Regex.Match(terms[i].text, "{(.*?)}");
            if (t.Success)
            {
                var term = t.Groups[1].Value;
                if (s[cc].ContainsKey(term)) set(cc, term, i); else set(fallbackLanguage, term, i);
            }
        }
    }

    /// <summary>
    /// Allows to get translation for the desired localization string
    /// </summary>
    /// <param name="key">Localization string</param>
    /// <returns></returns>
    public static string Translate(string key)
    {
        string o;
        if (!isInitialized) initialize();
        if (s[cc].TryGetValue(key, out o)) return o;
        else if (s[fallbackLanguage].TryGetValue(key, out o)) return o;
        else return key;
    }
    #endregion
}
