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

    private static readonly Dictionary<string, string> listLng = new Dictionary<string, string>(){
        {"English", "English"}
        ,{"Russian", "Русский"}
        ,{"Spanish", "Española"}
    };

    public static Dictionary<string, string> getLanguages() { return listLng; }

    private static readonly Dictionary<string, string> MessagesEn = new Dictionary<string, string>() {
        {"Intro", "<size=80><b>Test: determination of temperament (4 parts)</b></size>"}
        , {"selOneCol", "<b>Choose the most pleasant color</b>"}
        , {"selAllCol", "<b>Choose one by one the colors you like most</b>"}
        , {"Gyro", "Sorry \nYour device does not have a gyroscope."} 
        , {"msgEmail", "<b>Enter your email.</b> \n <size=40> A link to the end of the test will be sent to this address.</size>"}
        , {"msgName", "<b>Enter your name.</b>"}
        , {"msgBirth", "<b>Enter your date of birth.</b>"}
        , {"msgGender", "<b>Specify your gender.</b>"}
        , {"Test1", "Test 1/4. Find all <b>{0} {1}</b> around you. \n <size=40>After the execution, press the <Exit> button below.</size> "}
        , {"Test_hint", "Find <b> {0} {1} </b>. <B> {2}/{3}</b>"}
        , {"Test2", "Test 2/4. Find all <b> {0} {1}</b> around you \n\n for <b>{2}</b> seconds." }
        , {"Test_timer", "\n <b> {0} sec left </b>"}
        , {"Result", "<size=80><b>Your results:</b></size>\n\n" +
        "<b>Test 1</b>: you found <b>{0}</b> objects from {1}\n" +
        "<b>Test 2</b>: you found <b>{2}</b> objects from {3} for <b>{4}</b> seconds.\n\n" +
        "<size=50> The final part of the test is available at www.ongoza.com</size>"}
        , {"color_0", "gray"}
        , {"color_1", "blue"}
        , {"color_2", "green"}
        , {"color_3", "red"}
        , {"color_4", "yellow"}
        , {"color_5", "magenta"}
        , {"color_6", "brown"}
        , {"color_7", "black"}
        , {"obj_0", "cubes"}
        , {"obj_1", "spheres"}
        , {"obj_2", "capsules"}
        , {"obj_3", "cylinders"}
        , {"obj_4", "pyramids"}
        , {"start", "Start"}
        , {"next", "Next"}
        , {"yes", "Yes"}
        , {"not", "No"}
        , {"IntroColTest", "Test 3/4. Choose the colors that you like one by one."}
        , {"IntroTextTest", "Test 4/4. Answer <Yes> or <No> to {0} questions."}
        , {"Res_C", "<b> Your current state:</b>"}
        , {"Res_C_S", "Stress Level"}
        , {"Res_C_E", "Concentration"}
        , {"Res_T", "<b> Your temperament:</b>"}
        , {"Phlegmatic", "Phlegmatic on"}
        , {"Sanguine", "Sanguine on"}
        , {"Melancholic", "Melancholic on"}
        , {"Choleric", "Choleric on"}
        , {"Power", "Stability"}
        , {"btnExit", "Exit"}
        , {"btnAbout", "About"}
        , {"btnRepeat", "Repeat"}
        , {"btnMore", "More ..."}
        , {"btnBack", "Return"}
        , {"btnNext", "Next"}
        , {"btnStart", "Start"}
        , {"btnLangSw","Change language" }
        , {"msgLangSw","Select language" }
        , {"msgAbout", "This program was developed in the project www.ongoza.com as psychological tests transfering to virtual reality."}
    };

    private static readonly Dictionary<string, string> MessagesSp = new Dictionary<string, string>() {
        { "Intro", "<size=80><b>Prueba: determinación del temperamento (4 partes)</b></size>" }
        ,{ "selOneCol", "<b>Elija el color más agradable</b>" }
        ,{ "selAllCol", "<b>Elija uno por uno los colores que más le gusten</b>" }
        ,{ "Gyro","Lo siento\nSu dispositivo no tiene un giroscopio" } // Sorry\nYour device has to have a Gyroscope
        ,{ "msgEmail", "<b>Ingrese su correo electrónico.</b>\n<size=40>Se enviará un enlace al final de la prueba a esta dirección</size>" }
        ,{ "msgName", "<b>Ingrese su nombre</b>" }
        ,{ "msgBirth", "<b>Ingrese su fecha de nacimiento</b>" }
        ,{ "msgGender", "<b>Especifique su género</b>" }
        ,{ "Test1", "Prueba 1/4. Encuentre todos <b>{0} {1}</b> a su alrededor.\n<size=40>Después de la ejecución, presione el botón <Exit>,\n que se encuentra abajo</size>" }
        ,{ "Test_hint", "Encontrar <b>{0} {1}</b>. Buscar <b>{2}/{3}</b>" }
        ,{ "Test2", "Prueba 2/4. Encuentre todos <b>{0} {1}</b> a su alrededor\n\n\npor <b>{2}</b> segundos" }
        ,{ "Test_timer", "\n <b>{0} segundos restantes</b>" }
        ,{ "Result", "<size=80><b>Sus resultados:</b></size>\n\n" +
            "<b>Prueba 1</b>: encontró <b>{0}</b> objetos de {1}\n" +
            "<b>Prueba 2</b>: encontró <b>{2}</b> objetos de {3} durante <b>{4}</b> segundos.\n\n" +
            "<size=50>La parte final de la prueba está disponible en www.ongoza.com</size>" }
        ,{ "color_0", "gris" }
        ,{ "color_1", "azul" }
        ,{ "color_2", "verde" }
        ,{ "color_3", "rojo" }
        ,{ "color_4", "amarillo" }
        ,{ "color_5", "magenta" }
        ,{ "color_6", "marrón" }
        ,{ "color_7", "negro" }
        ,{ "obj_0", "cubos" }
        ,{ "obj_1", "esferas" }
        ,{ "obj_2", "cápsulas" }
        ,{ "obj_3", "cilindros" }
        ,{ "obj_4", "pirámides" }
        ,{ "start", "Inicio" }
        ,{ "next", "Siguiente" }
        ,{ "yes", "Si" }
        ,{ "not", "No" }
        ,{"IntroColTest","Prueba 3/4. Elija los colores que le gusten uno por uno"}
        ,{"IntroTextTest","Prueba 4/4. Responda <Sí> o <No> a {0} preguntas."}
        ,{"Res_C","<b>Su estado actual:</b>"}
        ,{"Res_C_S","Nivel de estrés"}
        ,{"Res_C_E","Concentración"}
        ,{"Res_T","<b>Su temperamento:</b>"}
        ,{"Phlegmatic","Flemático en"}
        ,{"Sanguine","Sanguine en"}
        ,{"Melancholic","Mechcholic on"}
        ,{"Choleric","Choleric en"}
        ,{"Power","Estabilidad"}
        ,{"btnExit","Salida"}
        ,{"btnAbout","Acerca"}
        ,{"btnRepeat","Repetir"}
        ,{"btnMore","más ..."}
        ,{"btnBack","Volver"}
        ,{"btnNext","Siguiente"}
        ,{"btnStart","Comenzar"}
        , {"btnLangSw","Cambiar idioma" }
        , {"msgLangSw","Seleccione el idioma" }
        ,{"msgAbout","Este programa se desarrolló en el proyecto www.ongoza.com sobre la transferencia de pruebas psicológicas a la realidad virtual"}
    };

    private static readonly Dictionary<string, string> MessagesRu = new Dictionary<string, string>() {
        { "Intro", "<size=80><b>Тест:  определение темперамента (4 части)</b></size>" }
        ,{ "selOneCol", "<b>Выберите самый приятный цвет</b>" }
        ,{ "selAllCol", "<b>Выберите один за одним цвета, которые нравятся больше всего</b>" }
        ,{ "Gyro","Извините\nВаше устройство не имеет гироскопа. " } // Sorry\nYour device has to have a Gyroscope
        ,{ "msgEmail", "<b>Введите ваш email.</b>\n<size=40>На этот адрес Вам будет отправлена ссылка на окончание теста.</size>" }
        ,{ "msgName", "<b>Введите ваше имя.</b>" }
        ,{ "msgBirth", "<b>Укажите дату вашего рождения.</b>" }
        ,{ "msgGender", "<b>Укажите ваш пол.</b>" }
        ,{ "Test1", "Тест 1/4. Найдите все <b>{0} {1}</b> вокруг вас.\n\n<size=40>После выполнения нажмите кнопку <Exit>,\nкоторая расположена внизу.</size>" }
        ,{ "Test_hint", "Найти <b>{0} {1}</b>. Выбрано <b>{2}/{3}</b>" }
        ,{ "Test2", "Тест 2/4. Найдите все <b>{0} {1}</b> вокруг вас\n\n\nв течение <b>{2}</b> секунд." }
        ,{ "Test_timer", "\n <b> Осталось {0} сек</b>" }
        ,{ "Result", "<size=80><b>Ваши результаты:</b></size>\n\n" +
            "<b>Тест 1</b>: вы нашли <b>{0}</b> объектов из {1}\n" +
            "<b>Тест 2</b>: вы нашли <b>{2}</b> объектов из {3} в течение <b>{4}</b> секунд.\n\n" +
            "<size=50>Завершающая часть теста доступна по ссылке www.ongoza.com</size>" }
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
        ,{"IntroColTest","Тест 3/4. Выберите цвета которые вам нравятся один за одним."}
        ,{"IntroTextTest","Тест 4/4. Ответьте <Да> или <Нет> на {0} вопросов."}
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
        , {"btnLangSw","Сменить язык" }
        , {"msgLangSw","Выберите язык" }
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
        { "Russian", MessagesRu},
        { "English", MessagesEn},
        { "Spanish", MessagesSp},
    };

    public static readonly Dictionary<string, List<int>> Answers = new Dictionary<string, List<int>>(){
        ["+"] = new List<int>{1, 3, 8, 10, 13, 17, 22, 25, 27, 39, 44, 46, 49, 53, 56},
        ["-"] = new List<int>{ 5, 15, 20, 29, 32, 34, 37, 41, 51},
        ["n"] = new List<int>{ 2, 4, 7, 9, 11, 14, 16, 19, 21, 23, 26, 28, 31, 33, 35, 38, 40, 43, 45, 47, 50, 52, 55, 57}
    };

    private static readonly string[][] QuestionsRu = new string[][]{
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

    // http://www.iluguru.ee/test/eysencks-personality-inventory-epi-extroversionintroversion/
    private static readonly string[][] QuestionsEn = new string[][] {
        new string [] {"1", "Do you often long for excitement?"},
        new string [] {"2", "Do you often need understanding friends to cheer you up?"},
        new string [] {"3", "Are you usually carefree?"},
        new string [] {"4", "Do you find it very hard to take no for an answer?"},
        new string [] {"7", "Do your moods go up and down?"},
        new string [] {"9", "Do you ever feel ‘just miserable’ for no good reason?"},
        new string [] {"10", "Would you do almost anything for a dare?"},
        new string [] {"15", "Generally do you prefer reading to meeting people?"},
        new string [] {"16", "Are your feelings rather easily hurt?"},
        new string [] {"17", "Do you like going out a lot?"},
        new string [] {"19", "Are you sometimes bubbling over with energy and sometimes very sluggish?"},
        new string [] {"21", "Do you daydream a lot?"},
        new string [] {"22", "When people shout at you do you shout back?"},
        new string [] {"23", "Are you often troubled about feelings of guilt?"},
        new string [] {"26", "Would you call yourself tense or ‘highly strung’?"},
        new string [] {"27", "Do other people think of you as being very lively?"},
           // new string [] {"32", "If there is something you want to know about, would you rather look it up in a book than talk to someone about it?"},
          // new string [] {"33", "Do you get palpitations or thumping in your heart?"},
        new string [] {"34", "Do you like the kind of work that you need to pay close attention to?"},
        new string [] {"38", "Are you an irritable person?"},
        new string [] {"39", "Do you like doing things in which you have to act quickly?"},
        new string [] {"41", "Are you slow and unhurried in the way you move?"},
        new string [] {"43", "Do you have many nightmares?"},
        new string [] {"44", "Do you like talking to people so much that you never miss a chance of talking to a stranger?"},
        new string [] {"46", "Would you be very unhappy if you could not see lots of people most of the time?"},
        new string [] {"50", "Are you easily hurt when people find fault with you or your work?"},
        new string [] {"51", "Do you find it hard to really enjoy yourself at a lively party?"},
        new string [] {"52", "Are you troubled by feelings of inferiority?"},
        new string [] {"53", "Can you easily get some life into a dull party?"},
        new string [] {"55", "Do you worry about your health?" }
    };

    // https://www.sicologiahoy.com/test/test-eysenck-extroversionintroversion/
    private static readonly string[][] QuestionsSp = new string[][] {
        new string [] {"1", "A menudo buscas tuaciones emocionantes?"},
        new string [] {"2", "Necetas a menudo que tus amigos te levanten el ánimo?"},
        new string [] {"3", "Eres usualmente relajado?"},
        new string [] {"4", "Te es difícil aceptar  como una respuesta?"},
        //new string [] {"5", "Te detienes y piensas antes de hacer algo?"},
        //new string [] {"6", " dices que vas a hacer algo, Lo cumples n importar lo inconveniente que sea?"},
        new string [] {"7", "Tu ánimo sube y baja repentinamente?"},
        //new string [] {"8", "Generalmente haces y dices cosas n pensar antes?"},
        new string [] {"9", "Te entes a veces miserable n razón aparente?"},
        new string [] {"10", "Harías ca cualquier cosa  alguien te reta?"},
        //new string [] {"11", "Te entes tímido/a  de repente hablas con un extraño/a atractivo/a?"},
        //new string [] {"12", "Pierdes tu temperamento de vez en cuando?"},
        //new string [] {"13", "Habitualmente haces cosas de forma espontánea?"},
        //new string [] {"14", "Habitualmente te preocupas de las cosas que debiste haber hecho o dicho?"},
        new string [] {"15", "Generalmente prefieres leer o cocer gente nueva?"},
        new string [] {"16", "Tus sentimientos se hieren facilmente?"},
        new string [] {"17", "Te gusta salir a menudo?"},
        //new string [] {"18", "A veces tienes ideas o pensamientos que  te gustaría que otros supieran?"},
        new string [] {"19", "Te entes a veces muy energético y otras veces demaado flojo?"},
        //new string [] {"20", "Prefieres tener pocos, pero especiales amigos?"},
        new string [] {"22", "Cuándo la gente te grita, les gritas de vuelta?"},
        new string [] {"21", "Sueñas mucho de día?"},
        new string [] {"23", "Te entes culpable a menudo?"},
        //new string [] {"24", "Son todos tus hábitos deseables?"},
        //new string [] {"25", "Usualmente te puedes relajar fácilmente y disfrutar de la fiesta?"},        
        new string [] {"26", "Te conderas una persona tensa?"},
        new string [] {"27", "Otras personas te han dicho que eres muy animado?"},
        //new string [] {"28", "Después de hacer algo importante", " entes que pudiste haber hecho mejor?"},        
        //new string [] {"29", "Eres mayormente callado cuando estás con otra gente?"},
        //new string [] {"30", "Te gustan los rumores?"},        
        //new string [] {"31", "A veces las ideas en tu cabeza  te dejan dormir?"},
        //new string [] {"32", " es que hay algo que quieres saber, Lo buscas en línea o le preguntas a alguien?"},
        //new string [] {"33", "A veces entes palpitaciones de nerviosmo?"},
        new string [] {"34", "Te gusta la clase de trabajo dónde te tienes que enfocar?"},
        //new string [] {"35", "Sufres ataques que te dejan temblando o sacudido?"},
        //new string [] {"36", "empre declaras tus pertenencias en aduanas, incluso cuando es poco probable que lo puedan encontrar?"},
        //new string [] {"37", "Te desagrada estar con gente que constantemente se hacen bromas?"},
        new string [] {"38", "Eres una persona irritable?"},
        new string [] {"39", "Te gusta hacer cosas dónde tienes que actuar rápidamente?"},
        //new string [] {"40", "Te preocupan muchos cosas malas que podrían pasar?"},
        new string [] {"41", "Eres lento y relajado en cómo te mueves?"},
        //new string [] {"42", "Alguna vez has llegado tarde a una reunión?"},
        new string [] {"43", "Tienes muchas pesadillas?"},
        new string [] {"44", "Te gusta hablar tanto con la gente, que nunca pierdes la oportunidad de hablar con un extraño?"},
        //new string [] {"45", "Tienes muchos dolores y preocupaciones?"},
        new string [] {"46", "Estarías molesto   pudieras ver gente, la mayoría del tiempo?"},
        //new string [] {"47", "Te conderas una persona nerviosa?"},
        //new string [] {"48", "De las personas que coces Hay alguien que  te agrada para nada?"},
        //new string [] {"49", "Dirías que eres relativamente confiado?"},
        new string [] {"50", "Te entes herido cuándo gente encuentra errores en tu trabajo?"},
        new string [] {"51", "Encuentras difícil relajarte y disfrutar de una fiesta?"},
        new string [] {"52", "Te perturban sentimientos de inferioridad?"},
        new string [] {"53", "Podrías fácilmente animar una fiesta aburrida?"},
        //new string [] {"54", "A veces hablas de cosas de las cuáles  sabes nada?"},
        new string [] {"55", "Te preocupa tu salud?"},
        //new string [] {"56", "Te gusta hacerle bromas a otros?"},
        //new string [] {"57", "Sufres de insomnio?"},
    };

    private static readonly Dictionary<string, string[][]> Questions = new Dictionary<string, string[][]>() {
         ["Russian"]= QuestionsRu,
         ["English"]= QuestionsEn,
         ["Spanish"] = QuestionsSp,
    };

    public static string[] getQuestionIndex(string lang, int index){return Questions[lang][index];}
    public static int getQuestionsCount(string lang) { return Questions[lang].Length; }
}
