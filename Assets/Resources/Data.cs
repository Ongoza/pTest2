using System.Collections;
using System.Collections.Generic;


public class Data {

    private static readonly Dictionary<string, string> MessagesEng = new Dictionary<string, string>() {
        { "Intro", "Intro" }
        ,{ "Email", "Error Two" }
        ,{ "Test1", "Error Two" }
        ,{ "Test2", "Error Two" }
        ,{ "Result", "Error Two" }
    };

    private static readonly Dictionary<string, string> MessagesRus = new Dictionary<string, string>() {
        { "Intro", "Данные тест предназначен для определения вашего темперамента и состоит из 2 частей. Для начал теста нажмите кнопку 'Старт'" }
        ,{ "Email", "Введите адрес электронной почты\nдля получения результатов." }
        ,{ "Test1", "Найдите все желтые пирамиды вокруг вас." }
        ,{ "Test2", "Найдите все зеленые кубы вокруг вас в течение 2 минут." }
        ,{ "Result", "Вы успешно прошли тест. Ваши результаты: - Тест 1 вы нашли _ объектов из 5. - Тест 2 вы нашли х объектов из 5 в течение * минут." }
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

}
