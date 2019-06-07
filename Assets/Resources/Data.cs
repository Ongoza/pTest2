using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {
    public static string getVersion() {
        return "0.99";
    }

    private static readonly Dictionary<string, string> connectionData = new Dictionary<string, string>() {
       // { "ServerIP", "127.0.0.1" }
       // { "ServerIP", "192.168.43.253" }
        { "ServerIP", "91.212.177.22" }
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
        ,{ "gyroWarn", "Your device does not have a gyroscope.\nTry selecting the <Next> button."}
        ,{"selOneCol", "<b>Choose the most pleasant color</b>"}
        ,{"selAllCol", "<b>Choose one by one the colors you like most</b>"}
        ,{"Gyro", "Sorry \nYour device does not have a gyroscope."} 
        ,{"msgEmail", "<b>Enter your email.</b> \n <size=40> A link to the end of the test will be sent to this address.</size>"}
        ,{"msgName", "<b>Enter your name.</b>"}
        ,{"msgBirth", "<b>Enter your date of birth.</b>"}
        ,{"msgGender", "<b>Specify your gender.</b>"}
        ,{"Test1", "Test 1/4. Find all <b>{0} {1}</b> around you. \n <size=40>After the execution, press the <Exit> button below.</size> "}
        ,{"Test_hint", "Find <b> {0} {1} </b>. <B> {2}/{3}</b>"}
        ,{"Test2", "Test 2/4. Find all <b> {0} {1}</b> around you \n\n for <b>{2}</b> seconds." }
        ,{"Test_timer", "\n <b> {0} sec left </b>"}
        ,{"Result", "<size=80><b>Your results:</b></size>\n\n" +
        "<b>Test 1</b>: you found <b>{0}</b> objects from {1}\n" +
        "<b>Test 2</b>: you found <b>{2}</b> objects from {3} for <b>{4}</b> seconds.\n\n" +
        "<size=50> The final part of the test is available at www.ongoza.com</size>"}
        ,{"color_0", "gray"}
        ,{"color_1", "blue"}
        ,{"color_2", "green"}
        ,{"color_3", "red"}
        ,{"color_4", "yellow"}
        ,{"color_5", "magenta"}
        ,{"color_6", "brown"}
        ,{"color_7", "black"}
        ,{"obj_0", "cubes"}
        ,{"obj_1", "spheres"}
        ,{"obj_2", "capsules"}
        ,{"obj_3", "cylinders"}
        ,{"obj_4", "pyramids"}
        ,{"start", "Start"}
        ,{"next", "Next"}
        ,{"yes", "Yes"}
        ,{"not", "No"}
        ,{"IntroColTest", "Test 3/4. Choose the colors that you like one by one."}
        ,{"IntroTextTest", "Test 4/4. Answer <Yes> or <No> to {0} questions."}
        ,{"Res_C", "<b> Your current state:</b>"}
        ,{"Res_C_S", "Stress Level"}
        ,{"Res_C_E", "Concentration"}
        ,{"Res_T", "<b> Your temperament:</b>"}
        ,{"Phlegmatic", "Phlegmatic on"}
        ,{"Sanguine", "Sanguine on"}
        ,{"Melancholic", "Melancholic on"}
        ,{"Choleric", "Choleric on"}
        ,{"Power", "Stability"}
        ,{"btnExit", "Exit"}
        ,{"btnAbout", "About"}
        ,{"btnRepeat", "Repeat"}
        ,{"btnMore", "More ..."}
        ,{"btnBack", "Return"}
        ,{"btnNext", "Next"}
        ,{"btnStart", "Start"}
        ,{"btnLangSw","Change language" }
        ,{"msgLangSw","Select language" }
        ,{"msgAbout", "This program was developed in the project www.ongoza.com as psychological tests transfering to virtual reality."}
        ,{"btnMale","Male" }
        ,{"btnFemale","Female" }
        ,{"btnUnknown","Prefer not to answer" }
        ,{"msgYear","Year" }
        ,{"msgMonth","Month" }
        ,{"msgDay","Day" }
        ,{"btnDelete","Delete" }
        ,{"btnDescription","Description" }
        ,{"dscPhlegmatic", "<b>Phlegmatic ({0}%)</b> - relaxed, peaceful, quiet, and easy-going. They are sympathetic and care about others, yet they try to hide their emotions. Phlegmatic individuals also are good at generalizing ideas or problems to the world and making compromises."}
        ,{"dscSanguine", "<b>Sanguine ({0}%)</b> - highly talkative, enthusiastic, active, and enjoy being part of a crowd; they find that being social, outgoing, and charismatic is easy to accomplish, have a hard time doing nothing and engage in more risk seeking behaviour."}
        ,{"dscMelancholic", "<b>Melancholic ({0}%)</b> - analytical and detail-oriented, and they are deep thinkers and feelers, avoid being singled out in a crowd. They often strive for perfection within themselves and their surroundings, which leads to tidy and detail oriented behavior."}
        ,{"dscCholeric", "<b>Choleric ({0}%)</b> - independent, decisive, and goal-oriented, and ambitious. These combined with their dominant, result-oriented outlook make them natural leaders. In Greek, Medieval and Renaissance thought, they were also violent, vengeful, and short-tempered."}
        ,{"dscPower", "<b> ({0}%)</b>. It reflects how pronounced the characteristics of temperament are."}
        ,{"dscStress", "<b>Stress level ({0}%)</b>. Reflects your stress level."}
        ,{"dscEffi", "<b>Concentration ({0}%)</b>. Reflects your level of concentration."}
        ,{"btnClose", "Close"}
    };

    private static readonly Dictionary<string, string> MessagesSp = new Dictionary<string, string>() {
        { "Intro", "<size=80><b>Prueba: determinación del temperamento (4 partes)</b></size>" }
        ,{ "gyroWarn", "Su dispositivo no tiene un giroscopio.\nIntente seleccionar el botón <Siguiente>"}
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
        ,{"btnLangSw","Change language" }
        ,{"msgLangSw","Select language" }
        ,{"msgAbout","Este programa se desarrolló en el proyecto www.ongoza.com sobre la transferencia de pruebas psicológicas a la realidad virtual"}
        ,{"btnMale","Masculina" }
        ,{"btnFemale","Hembra" }
        ,{"btnUnknown","Prefiero no responder" }
        ,{"msgYear","Año" }
        ,{"msgMonth","Mes" }
        ,{"msgDay","Día" }
        ,{"btnDelete","Сancelar" }
        ,{"btnDescription","Descripción" }
        ,{"dscPhlegmatic", "<b>Flemático ({0}):</b> relajado, pacífico, tranquilo y sencillo. Son simpatizantes de sus emociones. Los individuos flemáticos también están interesados en."}
        ,{"dscSanguine", "<b>Sanguíneo ({0}):</b> muy hablador, entusiasta, activo y disfruta de ser parte de una multitud; encuentran que ser social, extrovertido y carismático es fácil de lograr, les cuesta mucho no hacer nada y se involucran en un comportamiento de búsqueda de riesgos."}
        ,{"dscMelancholic", "<b>Melancholic ({0}):</b> analítico y orientado a los detalles, y son pensadores y analistas profundos, evitan ser señalados entre la multitud. A menudo luchan por la perfección dentro de sí mismos y en su entorno, lo que conduce a un comportamiento ordenado y orientado a los detalles."}
        ,{"dscCholeric", "<b>Colérico ({0}):</b> independiente, decisivo, orientado a objetivos y ambicioso. Estos combinados con su perspectiva dominante orientada hacia los resultados los convierten en líderes naturales."}
        ,{"dscPower", "<b>Pronunciadas ({0}%)</b>. Refleja cuán pronunciadas son las características del temperamento."}
        ,{"dscStress", "<b>Estrés ({0}%)</b>. Refleja tu nivel de estrés."}
        ,{"dscEffi", "<b>Concentración ({0}%)</b>. Refleja tu nivel de concentración."}
        ,{"btnClose", "Apagar"}
    };

    private static readonly Dictionary<string, string> MessagesRu = new Dictionary<string, string>() {
        { "Intro", "<size=80><b>Тест:  определение темперамента (4 части)</b></size>" }
        ,{ "gyroWarn", "Ваше устройство не имеет гироскопа.\n Попробуйте выбрать кнопку <Далее>"}
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
        ,{"btnLangSw","Change language" }
        ,{"msgLangSw","Select language" }
        ,{"msgAbout","Эта программа разработана в ракмах проекта www.ongoza.com по переносу психологических тестов в виртуальную реальность."}
        ,{"btnMale","Мужской" }
        ,{"btnFemale","Женский" }
        ,{"btnUnknown","Предпочитаю не ответить" }
        ,{"msgYear","Год" }
        ,{"msgMonth","Месяц" }
        ,{"msgDay","День" }
        ,{"btnDelete","Удалить" }
        ,{"btnDescription","Description" }
        ,{"dscPhlegmatic", "<b>Флегматик ({0})</b> — неспешен, невозмутим, имеет устойчивые стремления и настроение, внешне скуп на проявление эмоций и чувств. Он проявляет упорство и настойчивость в работе, оставаясь спокойным и уравновешенным. В работе он производителен, компенсируя свою неспешность прилежанием."}
        ,{"dscSanguine", "<b>Сангвиник ({0})</b> — живой, горячий, подвижный человек, с частой сменой впечатлений, с быстрой реакцией на все события, происходящие вокруг него, довольно легко примиряющийся со своими неудачами и неприятностями. Он очень продуктивен в работе, когда ему интересно."}
        ,{"dscMelancholic", "<b>Меланхолик ({0})</b> — склонный к постоянному переживанию различных событий, он остро реагирует на внешние факторы. Свои переживания он зачастую не может сдерживать усилием воли, он повышено впечатлителен, эмоционально раним."}
        ,{"dscCholeric", "<b>Холерик ({0})</b> — быстрый, порывистый, с резко меняющимся настроением с эмоциональными вспышками, быстро истощаемый. Холерик обладает огромной работоспособностью, однако, увлекаясь, безалаберно растрачивает свои силы и быстро истощается."}
        ,{"dscPower", "<b>Стабильность ({0}%)</b>. Отображает насколько ярко выражены характеристики темперамента."}
        ,{"dscStress", "<b>Стрессс ({0}%)</b>. Отражает уровень вашего стресса."}
        ,{"dscEffi", "<b>Концентрация ({0}%)</b>. Отражает уровень вашей концентрации."}
        ,{"btnClose", "Закрыть"}
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

    // true (+)6,24,36 (-) 12,18,30,42,48,54
    public static readonly Dictionary<string, List<int>> Answers = new Dictionary<string, List<int>>(){
        ["+"] = new List<int>{1, 3, 8, 10, 13, 17, 22, 25, 27, 39, 44, 46, 49, 53, 56},
        ["-"] = new List<int>{ 5, 15, 20, 29, 32, 34, 37, 41, 51},
        ["n"] = new List<int>{ 2, 4, 7, 9, 11, 14, 16, 19, 21, 23, 26, 28, 31, 33, 35, 38, 40, 43, 45, 47, 50, 52, 55, 57}
    };
    // https://drive.google.com/file/d/1hz5Y4wtpGmex_oj8ItYpSel9j4mReP0p/view?usp=sharing
    //https://books.google.com.ua/books?id=bY3hDgAAQBAJ&pg=PA366&lpg=PA366&dq=%D0%A7%D0%B0%D1%81%D1%82%D0%BE+%D0%BB%D0%B8+%D0%92%D1%8B+%D1%87%D1%83%D0%B2%D1%81%D1%82%D0%B2%D1%83%D0%B5%D1%82%D0%B5,+%D1%87%D1%82%D0%BE+%D0%BD%D1%83%D0%B6%D0%B4%D0%B0%D0%B5%D1%82%D0%B5%D1%81%D1%8C+%D0%B2+%D0%B4%D1%80%D1%83%D0%B7%D1%8C%D1%8F%D1%85,+%D0%BA%D0%BE%D1%82%D0%BE%D1%80%D1%8B%D0%B5+%D0%BC%D0%BE%D0%B3%D1%83%D1%82+%D0%92%D0%B0%D1%81+%D0%BF%D0%BE%D0%BD%D1%8F%D1%82%D1%8C?&source=bl&ots=9fdgXYAdUC&sig=ACfU3U3TbreiMWKOojG_cpw2uKZh5SK31w&hl=en&sa=X&ved=2ahUKEwivi7TdlsXiAhWJIZoKHYmWC_gQ6AEwAHoECAEQAQ#v=onepage&q&f=false
    private static readonly string[][] QuestionsRu = new string[][]{
      new string[]{"1","Часто ли Вы чувствуете тягу к новым впечатлениям?"}, //e+
      new string[]{"2","Часто ли Вы чувствуете, что нуждаетесь в друзьях, которые могут Вас понять?"}, //n+
      new string[]{"3","Считаете ли Вы себя беззаботным человеком?"}, //e+
      new string[]{"4","Очень ли трудно Вы отказываетесь от своих намерений?"}, //n+
      //new string [] {"5", "Обдумываете ли вы свои дела не спеша и предпочитаете ли подождать, прежде чем действовать?"}, //e-
      //new string [] {"6", "Всегда ли вы сдерживаете свои обещания, даже если это вам невыгодно?"}, // true+
      new string[]{"7","Часты ли у Вас спады и подъемы настроения?"}, //n+
      //new string [] {"8", "Быстро ли вы обычно действуете и говорите, не тратите ли много времени на обдумывание?"}, //e+
      new string[]{"9","Чуствовали ли вы себя несчастным, без серьезной причины для этого?"}, //n+
      new string[]{"10","Верно ли, что на спор Вы способны решиться на все?"}, //e+
      //new string [] {"11", "Смущаетесь ли вы, когда хотите познакомиться с человеком противоположного пола, который вам симпатичен?"}, //n+
      //new string [] {"12", "Бывает ли когда-нибудь, что, разозлившись, вы выходите из себя?"}, //true-
      //new string [] {"13", "Часто ли бывает, что вы действуете необдуманно, под влиянием момента?"}, //e+
      //new string [] {"14", "Часто ли вас беспокоят мысли о том, что вам не следовало чего-либо делать или говорить?"}, //n+
      new string[]{"15","Предпочитаете ли вы работать в одиночестве?"}, //e- Предпочитаете ли вы чтение книг встречам с людьми? 
      new string[]{"16","Вас легко задеть?"}, //n+
      new string[]{"17","Любите ли Вы часто бывать в компании?"}, //e+
      //new string [] {"18", "Бывают ли иногда у вас такие мысли, которыми вам не хотелось бы делиться с другими людьми?"}, // true-
      new string[]{"19","Вы иногда полны энергии, а иногда чувствуете сильную вялость?"}, //n+
      //new string [] {"20", "Стараетесь ли вы ограничить круг своих знакомых небольшим числом самых близких друзей?"}, //e-
      new string[]{"21","Много ли Вы мечтаете?"}, //n+
      new string[]{"22","Можете ли вы быстро выразить ваши мысли словами?"}, //e+ Когда на вас кричат, отвечаете ли вы тем же?
      new string[]{"23","Считаете ли вы все свои привычки хорошими?"}, //n+ Часто ли Вас терзает чувство вины?
      //new string [] {"24", "Часто ли у вас появляется чувство, что вы в чем-то виноваты?"}, //true+
      //new string [] {"25", "Способны ли вы иногда дать волю своим чувствам и беззаботно развлечься с веселой компанией?"}, //e+
      new string[]{"26","У Вас часто нервы бывают напряжены до предела?"}, //n+
      new string[]{"27","Считают ли Вас человеком живым и веселым?"}, //e+
      //new string [] {"28", "После того как дело сделано, часто ли вы мысленно возвращаетесь к нему и думаете, что могли бы сделать лучше?"}, //n+
      //new string [] {"29", "Чувствуете ли вы себя неспокойно, находясь в большой компании?"}, //e-
      //new string [] {"30", "Бывает ли, что вы передаете слухи?"}, //true-
      //new string [] {"31", "Бывает ли, что вам не спится из-за того, что в голову лезут разные мысли?"}, //n+
   //   new string[]{"32","Предпочли бы вы остаться в одиночестве дома, чем пойти на скучную вечеринку?"}, //e-
  //    new string[]{"33","Бывают ли у вас сильные сердцебиения?"}, //n+ Бываете ли вы иногда беспокойными настолько, что не можете долго усидеть на месте?
      new string[]{"34","Нравится ли Вам работа, которая требует пристального внимания?"}, //e-
      //new string [] {"35", "Бывают ли у вас приступы дрожи?"}, //n+
      //new string [] {"36", "Всегда ли вы говорите только правду?"}, //true+
      //new string [] {"37", "Бывает ли вам неприятно находиться в компании, где все подшучивают друг над другом?"}, //e-
      new string[]{"38","Вы легко раздражетесь?"}, //n+
      new string[]{"39","Нравится ли Вам работа, которая требует быстроты действий?"}, //e+
      //new string [] {"40", "Верно ли, что вам часто не дают покоя мысли о разных неприятностях и «ужасах», которые могли бы произойти, хотя все кончилось благополучно?"}, //n+
      new string[]{"41","Предпочитаете ли вы больше строить планы, чем действовать?"}, //e- Верно ли, что вы неторопливы в движениях и несколько медлительны?
      //new string [] {"42", "Опаздывали ли вы когда-нибудь на работу или на встречу с кем-либо?"}, //true-
      new string[]{"43","Часто ли вам снятся кошмары?"}, //n+ Нервничаете ли вы в местах, подобных лифту, метро, туннелю?
      new string[]{"44","При знакомстве вы обычно первыми проявляете инициативу?"}, //e+
      //new string [] {"45", "Беспокоят ли вас какие-нибудь боли?"}, //n+
      new string[]{"46","Огорчились бы Вы, если бы долго не могли видеться со своими друзьями?"}, //e+
      //new string [] {"47", "Можете ли вы назвать себя нервным человеком?"}, //n+
      //new string [] {"48", "Есть ли среди ваших знакомых такие, которые вам явно не нравятся?"}, //true-
      //new string [] {"49", "Могли бы вы сказать, что вы уверенный в себе человек?"}, //e+ Говорите ли вы иногда первое, что придет в голову?
      new string[]{"50","Долго ли вы переживаете после случившегося конфуза?"}, //n+Легко ли вас задевает критика ваших недостатков или вашей работы?
      new string[]{"51","Замкнуты ли вы обычно со всеми, кроме близких друзей?"}, //e- Трудно ли вам получить настоящее удовольствие от мероприятий, в которых участвует много народа?
      new string[]{"52","Беспокоит ли вас чувство, что вы чем-то хуже других?"}, //n+ Часто ли с вами случаются неприятности?
      new string[]{"53","Сумели бы Вы внести оживление в скучную компанию?"}, //e+
      //,new string [] {"54", "Бывает ли, что вы говорите о вещах, в которых совсем не разбираетесь?"}, //true-
      new string[]{"55","Беспокоитесь ли Вы о своем здоровье?"} //n+
      //,new string [] {"56", "Любите ли вы подшутить над другими?"}, //e+
      //new string [] {"57", "Страдаете ли вы бессонницей?"}, //n+
    };

    // http://www.iluguru.ee/test/eysencks-personality-inventory-epi-extroversionintroversion/
    private static readonly string[][] QuestionsEn = new string[][] {
        new string [] {"1", "Do you often long for excitement?"},
        new string [] {"2", "Do you often need understanding friends to cheer you up?"},
        new string [] {"3", "Are you usually carefree?"},
        new string [] {"4", "Do you find it very hard to take no for an answer?"},
        //new string [] {"5", "Do you stop and think things over before doing anything?"},
        //new string [] {"6", "If you say you will do something do you always keep your promise, no matter how inconvenient it might be to do so?"},
        new string [] {"7", "Do your moods go up and down?"},
        //new string [] {"8", "Do you generally do and say things quickly without stopping to think?"},
        new string [] {"9", "Do you ever feel ‘just miserable’ for no good reason?"},
        new string [] {"10", "Would you do almost anything for a dare?"},
        //new string [] {"11", "Do you suddenly feel shy when you want to talk to an attractive stranger?"},
        //new string [] {"12", "Once in a while do you lose your temper and get angry?"},
        //new string [] {"13", "Do you often do things on the spur of the moment?"},
        //new string [] {"14", "Do you often worry about things you should have done or said?"},
        new string [] {"15", "Generally do you prefer reading to meeting people?"},
        new string [] {"16", "Are your feelings rather easily hurt?"},
        new string [] {"17", "Do you like going out a lot?"},
        //new string [] {"18", "Do you occasionally have thoughts and ideas that you would not like other people to know about?"},
        new string [] {"19", "Are you sometimes bubbling over with energy and sometimes very sluggish?"},
        //new string [] {"20", "Do you prefer to have few but special friends?"},
        new string [] {"21", "Do you daydream a lot?"},
        new string [] {"22", "When people shout at you do you shout back?"},
        new string [] {"23", "Are you often troubled about feelings of guilt?"},
        //new string [] {"24", "Are all your habits good and desirable ones?"},
        //new string [] {"25", "Can you usually let yourself go and enjoy yourself a lot at a lively party?"},
        new string [] {"26", "Would you call yourself tense or ‘highly strung’?"},
        new string [] {"27", "Do other people think of you as being very lively?"},
        //new string [] {"28", "After you have done something important, do you come away feeling you could have done better?"},
        //new string [] {"29", "Are you mostly quiet when you are with other people?"},
        //new string [] {"30", "Do you sometimes gossip?"},
        //new string [] {"31", "Do ideas run through your head so that you cannot sleep?"},
        // new string [] {"32", "If there is something you want to know about, would you rather look it up in a book than talk to someone about it?"},
        // new string [] {"33", "Do you get palpitations or thumping in your heart?"},
        new string [] {"34", "Do you like the kind of work that you need to pay close attention to?"},
        //new string [] {"35", "Do you get attacks of shaking or trembling?"},
        //new string [] {"36", "Would you always declare everything at customs, even if you knewyou could never be found out?"},
        //new string [] {"37", "Do you hate being with a crowd who play jokes on one another?"},
        new string [] {"38", "Are you an irritable person?"},
        new string [] {"39", "Do you like doing things in which you have to act quickly?"},
        //new string [] {"40", "Do you worry about awful things that might happen?"},
        new string [] {"41", "Are you slow and unhurried in the way you move?"},
        //new string [] {"42", "Have you ever been late for an appointment or work?"},
        new string [] {"43", "Do you have many nightmares?"},
        new string [] {"44", "Do you like talking to people so much that you never miss a chance of talking to a stranger?"},
        //new string [] {"45", "Are you troubled by aches and pains?"},
        new string [] {"46", "Would you be very unhappy if you could not see lots of people most of the time?"},
        //new string [] {"47", "Would you call yourself a nervous person?"},
        //new string [] {"48", "Of all the people you know, are there some whom you definitely do not like?"},
        //new string [] {"49", "Would you say that you were fairly self-confident?"},
        new string [] {"50", "Are you easily hurt when people find fault with you or your work?"},
        new string [] {"51", "Do you find it hard to really enjoy yourself at a lively party?"},
        new string [] {"52", "Are you troubled by feelings of inferiority?"},
        new string [] {"53", "Can you easily get some life into a dull party?"},
        //new string [] {"54", "Do you sometimes talk about things you know nothing about?"},
        new string [] {"55", "Do you worry about your health?" }
        //,new string [] {"56", "Do you like playing pranks on others?"},
        //new string [] {"57", "Do you suffer from sleeplessness?"},
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
        new string [] {"55", "Te preocupa tu salud?"}
        //,new string [] {"56", "Te gusta hacerle bromas a otros?"},
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
