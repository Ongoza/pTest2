using System.Collections;
using System.Collections.Generic;


public class Data {

    private static readonly Dictionary<string, string> MessagesEng = new Dictionary<string, string>() {
        { "Intro", "Error One" },
        { "2", "Error Two" }
    };

    private static readonly Dictionary<string, string> MessagesRus = new Dictionary<string, string>() {
        { "Intro", "Старт программы" },
        { "2", "вторая часть" }
    };

    public static string getMessage(string key)
    {
        string result = "";
        result = MessagesRus[key];
        return result;
    }
}
