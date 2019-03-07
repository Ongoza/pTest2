using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {

    private static readonly Dictionary<string, string> connectionData = new Dictionary<string, string>() {
        { "ServerIP", "127.0.0.1" }
       // { "ServerIP", "192.168.1.100" }
       // { "ServerIP", "91.212.177.22" }
        ,{ "ServerPort", "8888" }
    };

    private static readonly Dictionary<string, string> MessagesEng = new Dictionary<string, string>() {
        { "Intro", "Intro" }
        ,{ "Email", "Error Two" }
        ,{ "Test1", "Error Two" }
        ,{ "Test2", "Error Two" }
        ,{ "Result", "Error Two" }
    };

    private static readonly Dictionary<string, string> MessagesRus = new Dictionary<string, string>() {
        { "Intro", "<size=80><b>Тест:  определение темперамента</b></size>" }
        ,{ "selOneCol", "<b>Выберите самый приятный цвет</b>" }
        ,{ "selAllCol", "<b>Выберите один за одним цвета, которые нравятся больше всего</b>" }
        ,{ "Gyro","Извините\nВаше устройство не имеет гироскопа. " } // Sorry\nYour device has to have a Gyroscope
        ,{ "Email", "<b>Введите ваш email.</b>\n<size=40>На этот адрес Вам будет отправлена ссылка на окончание теста.</size>" }
        ,{ "Test1", "Найдите все <b>{0} {1}</b> вокруг вас.\n\n<size=40>После выполнения нажмите кнопку <Exit>,\nкоторая расположена внизу.</size>" }
        ,{ "Test_hint", "Найти <b>{0} {1}</b>. Выбрано <b>{2}/{3}</b>" }
        ,{ "Test2", "Найдите все <b>{0} {1}</b> вокруг вас\n\n\nв течение <b>{2}</b> секунд." }
        ,{ "Test_timer", "\n <b> Осталось {0} сек</b>" }
        ,{ "Result", "<size=80><b>Ваши результаты:</b></size>\n\n" +
            "<b>Тест 1</b>: вы нашли <b>{0}</b> объектов из {1}\n" +
            "<b>Тест 2</b>: вы нашли <b>{2}</b> объектов из {3} в течение <b>{4}</b> секунд.\n\n" +
            "<size=50>Завершающая часть теста доступна по ссылке www.ongoza.com/pTest</size>" }
        ,{ "color_0", "серые" }
        ,{ "color_1", "синие" }
        ,{ "color_2", "зеленые" }
        ,{ "color_3", "красные" }
        ,{ "color_4", "желтые" }
        ,{ "color_5", "пурпурные" }
        ,{ "color_6", "коричневые" }
        ,{ "color_7", "черные" }
        ,{ "obj_0", "кубы" }
        ,{ "obj_1", "сферы" }
        ,{ "obj_2", "капсулы" }
        ,{ "obj_3", "цилиндры" }
        ,{ "obj_4", "пирамиды" }

    };

    private static readonly Dictionary<string, string> KeyList = new Dictionary<string, string>() {
        { "1", "1" },{ "2", "2" },{ "3", "3" },{ "4", "4" },{ "5", "5" },{ "6", "6" },{ "7", "7" },{ "8", "8" },{ "9", "9" },{ "0", "0" },{ "*", "*" },{ "=", "=" }
        ,{ "!", "!" },{ "_", "_" },{ "-", "-" },{ "#", "#" },{ "^", "^" },{ "&", "&" },{ "?", "?" },{ "{", "{" },{ "}", "}" },{ "$", "$" },{ "%", "%" },{ "/", "/" }
        ,{ "Q", "Q" },{ "W", "W" },{ "E", "E" },{ "R", "R" },{ "T", "T" },{ "Y", "Y" },{ "U", "U" },{ "I", "I" },{ "O", "O" },{ "P", "P" },{ ".", "." },{ "|", "|" }
        ,{ "A", "A" },{ "S", "S" },{ "D", "D" },{ "F", "F" },{ "G", "G" },{ "H", "H" },{ "J", "J" },{ "K", "K" },{ "L", "L" },{ "+", "+" },{ "`", "`" },{ ",", "," }
        ,{ "Z", "Z" },{ "X", "X" },{ "C", "C" },{ "V", "V" },{ "B", "B" },{ "N", "N" },{ "M", "M" },{ "@", "@" },{ ".com", ".com" },{ "DEL", "DEL" }
         //,{ " ", "          " },{ "SHIFT", "SHIFT" }
    };

    public static string getMessage(string lang, string key)
    {string result = "";
        if (Messages[lang].ContainsKey(key)) { result = Messages[lang][key];}
        return result;
    }

    public static bool isLanguge(string key)
    {
        bool result = false;
        if (Messages.ContainsKey(key)) { result = true; }
        return result;
    }

    public static string getKey(string lang, string key)
    {string result = "";
        if (Messages[lang].ContainsKey(key)){ result =  KeyList[key];}
        return result;
    }

    public static Dictionary<string,string> getKeys(){return KeyList;}

    public static Dictionary<string, string> getConnectionData() { return connectionData; }

    private static readonly Dictionary<string, Dictionary<string, string>> Messages = new Dictionary<string, Dictionary<string, string>>() {
        { "Russian", MessagesRus},
        { "English", MessagesEng},
    };

    private static readonly Dictionary<string, string> QuestionsEng = new Dictionary<string, string>() {
        { "1", "" }
    };

    private static readonly Dictionary<string, string> QuestionsRus = new Dictionary<string, string>() {
        { "1", "" }
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Questions = new Dictionary<string, Dictionary<string, string>>() {
        { "Russian", QuestionsRus},
        { "English", QuestionsEng},
    };
}
