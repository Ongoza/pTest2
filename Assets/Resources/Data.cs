using System.Collections;
using System.Collections.Generic;


public class Data {

    private static readonly Dictionary<string, string> connectionData = new Dictionary<string, string>() {
        { "ServerIP", "127.0.0.1" }
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
        { "Intro", "Тест:  определение темперамента." }
        ,{ "Email", "Введите адрес вашей электронной почты." }
        ,{ "Test1", "Найдите все <b>{0} {1}</b> вокруг вас.\nПосле выполнения нажмите кнопку <Exit>." }
        ,{ "Test_hint", "Найти <b>{0} {1}</b>. Выбрано <b>{2}/{3}</b>" }
        ,{ "Test2", "Найдите все <b>{0} {1}</b> вокруг вас\nв течение <b>{2}</b> секунд." }
        ,{ "Test_timer", "\n <b> Осталось {0} сек</b>" }
        ,{ "Result", "Ваши результаты:\nТест 1: вы нашли <b>{0}</b> объектов из {1}\nТест 2: вы нашли <b>{2}</b> объектов из {3} в течение <b>{4}</b> секунд." }
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

    public static string getMessage(string key)
    {string result = "";
        if (MessagesRus.ContainsKey(key)) { result = MessagesRus[key];}
        return result;
    }

    public static string getKey(string key)
    {string result = "";
        if (MessagesRus.ContainsKey(key)){ result =  KeyList[key];}
        return result;
    }

    public static Dictionary<string,string> getKeys(){return KeyList;}

    public static Dictionary<string, string> getConnectionData() { return connectionData; }

}
