using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {
    public static string getVersion() {
        return "0.8";
    }

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
        ,{ "msgEmail", "<b>Введите ваш email.</b>\n<size=40>На этот адрес Вам будет отправлена ссылка на окончание теста.</size>" }
        ,{ "msgName", "<b>Введите ваше имя.</b>" }
        ,{ "msgBirth", "<b>Укажите дату вашего рождения.</b>" }
        ,{ "msgGender", "<b>Укажите ваш пол.</b>" }
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
        ,{ "start", "Начать" }
        ,{ "next", "Следующий" }
        ,{ "yes", "Да" }
        ,{ "not", "Нет" }
        ,{"IntroColTest","Выберите цвета которые вам нравятся один за одним."}
        ,{"IntroTextTest","Ответьте \"Да\" или \"Нет\" на 30 вопросов."}
        ,{"Res_C","<b>Ваше текущее состояние:</b>"}
        ,{"Res_C_S","Уровень стресса"}
        ,{"Res_C_E","Концентрация"}
        ,{"Res_T","<b>Ваш темперамент:</b>"}
        ,{"Phlegmatic","Флегматик на"}
        ,{"Sanguine","Сангвиник на"}
        ,{"Melancholic","Меланхолик на"}
        ,{"Choleric","Холерик на"}
        ,{"Power","Стабильность"}
        ,{"btnExit","Выход"}
        ,{"btnAbout","О программе"}        
        ,{"btnRepeat","Повторить"}
        ,{"btnMore","больше..."}
        ,{"btnBack","Вернуться"}
        ,{"btnNext","Далее"}
        ,{"btnStart","начать"}
        ,{"msgAbout","Эта программа разработана в ракмах проекта www.ongoza.com по переносу психологических тестов в виртуальную реальность."}
    };

    private static readonly Dictionary<string, string> KeyList = new Dictionary<string, string>() {
        { "1", "1" },{ "2", "2" },{ "3", "3" },{ "4", "4" },{ "5", "5" },{ "6", "6" },{ "7", "7" },{ "8", "8" },{ "9", "9" },{ "0", "0" },{ "*", "*" },{ "=", "=" }
        ,{ "!", "!" },{ "_", "_" },{ "-", "-" },{ "#", "#" },{ "^", "^" },{ "&", "&" },{ "?", "?" },{ "{", "{" },{ "}", "}" },{ "$", "$" },{ "%", "%" },{ "/", "/" }
        ,{ "Q", "Q" },{ "W", "W" },{ "E", "E" },{ "R", "R" },{ "T", "T" },{ "Y", "Y" },{ "U", "U" },{ "I", "I" },{ "O", "O" },{ "P", "P" },{ ".", "." },{ "|", "|" }
        ,{ "A", "A" },{ "S", "S" },{ "D", "D" },{ "F", "F" },{ "G", "G" },{ "H", "H" },{ "J", "J" },{ "K", "K" },{ "L", "L" },{ "+", "+" },{ "`", "`" },{ ",", "," }
        ,{ "Z", "Z" },{ "X", "X" },{ "C", "C" },{ "V", "V" },{ "B", "B" },{ "N", "N" },{ "M", "M" },{ "@", "@" },{ ".com", ".com" },{ "DEL", "DEL" }
         //,{ " ", "          " },{ "SHIFT", "SHIFT" }
    };

    public static string getMessage(string lang, string key){ string result = "";
        if (Messages[lang].ContainsKey(key)) { result = Messages[lang][key]; }
        return result;
    }

    public static bool isLanguge(string key){
        bool result = false;
        if (Messages.ContainsKey(key)) { result = true; }
        return result;
    }

    public static string getKey(string lang, string key){ string result = "";
        if (Messages[lang].ContainsKey(key)) { result = KeyList[key]; }
        return result;
    }

    public static Dictionary<string, string> getKeys() { return KeyList; }

    public static Dictionary<string, string> getConnectionData() { return connectionData; }

    private static readonly Dictionary<string, Dictionary<string, string>> Messages = new Dictionary<string, Dictionary<string, string>>() {
        { "Russian", MessagesRus},
        { "English", MessagesEng},
    };

    public static readonly Dictionary<string, List<int>> Answers = new Dictionary<string, List<int>>(){
        ["+"] = new List<int>{1, 3, 8, 10, 13, 17, 22, 25, 27, 39, 44, 46, 49, 53, 56},
        ["-"] = new List<int>{ 5, 15, 20, 29, 32, 34, 37, 41, 51},
        ["n"] = new List<int>{ 2, 4, 7, 9, 11, 14, 16, 19, 21, 23, 26, 28, 31, 33, 35, 38, 40, 43, 45, 47, 50, 52, 55, 57}
    };

    private static readonly string[][] QuestionsRus = new string[][]{
      new string[]{"1","Часто ли Вы чувствуете тягу к новым впечатлениям?"},
      new string[]{"2","Часто ли Вы чувствуете, что нуждаетесь в друзьях, которые могут Вас понять?"},
      new string[]{"3","Считаете ли Вы себя беззаботным человеком?"},
      new string[]{"4","Очень ли трудно Вы отказываетесь от своих намерений?"},
      new string[]{"7","Часты ли у Вас спады и подъемы настроения?"},
      new string[]{"9","Чуствовали ли вы себя несчастным, без серьезной причины для этого?"},
      new string[]{"10","Верно ли, что на спор Вы способны решиться на все?"},
      new string[]{"15","Предпочитаете ли вы работать в одиночестве?"},
      new string[]{"16","Вас легко задеть?"},
      new string[] {"17","Любите ли Вы часто бывать в компании?"},
      new string[]{"19","Вы иногда полны энергии, а иногда чувствуете сильную вялость?"},
      new string[]{"21","Много ли Вы мечтаете?"},
      new string[]{"22","Можете ли вы быстро выразить ваши мысли словами?"},
      new string[]{"23","Часто ли Вас терзает чувство вины?"},
      new string[]{"26","У Вас часто нервы бывают напряжены до предела?"},
      new string[]{"27","Считают ли Вас человеком живым и веселым?"},
   //   new string[]{"32","Предпочли бы вы остаться в одиночестве дома, чем пойти на скучную вечеринку?"},
  //    new string[]{"33","Бываете ли вы иногда беспокойными настолько, что не можете долго усидеть на месте?"},
      new string[]{"34","Нравится ли Вам работа, которая требует пристального внимания?"},
      new string[]{"38","Вы легко раздражетесь?"},
      new string[]{"39","Нравится ли Вам работа, которая требует быстроты действий?"},
      new string[]{"41","Предпочитаете ли вы больше строить планы, чем действовать?"},
      new string[]{"43","Нервничаете ли вы в местах, подобных лифту, метро, туннелю?"},
      new string[]{"44","При знакомстве вы обычно первыми проявляете инициативу?"},
      new string[]{"46","Огорчились бы Вы, если бы долго не могли видеться со своими друзьями?"},
      new string[]{"50","Вы уверенный в себе человек?"},
      new string[]{"51","Замкнуты ли вы обычно со всеми, кроме близких друзей?"},
      new string[]{"52","Часто ли с вами случаются неприятности?"},
      new string[]{"53","Сумели бы Вы внести оживление в скучную компанию?"},
      new string[]{"55","Беспокоитесь ли Вы о своем здоровье?"}
    };

    private static readonly string[][] QuestionsEng = new string[][] {
        new string[]{ "1", "" }
    };

    private static readonly Dictionary<string, string[][]> Questions = new Dictionary<string, string[][]>() {
         ["Russian"]= QuestionsRus,
         ["English"]= QuestionsEng,
    };

    public static string[] getQuestionIndex(string lang, int index){return Questions[lang][index];}
    public static int getQuestionsCount(string lang) { return Questions[lang].Length; }
}
