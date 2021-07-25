﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperTexts
{
    private static readonly List<string> level_0 = new List<string>() 
    {
        "Приветствую! Теперь ты управляющий нашего предприятия. Тебе необходимо расчитать себестоимость продукции",
        "Звучит сложно? Ничего, сейчас мы начнём и всё станет понятнее",
        "Нажимай на первое здание чтобы войти"
    };

    private static readonly List<string> level_1 = new List<string>()
    {
        "Это офис учета оборудования. Здесь необходимо принять решение о закупке или продаже станков для выполнения программы",
        "Начнём! Нажми на компьютерный стол чтобы сесть за него.",
        "Нажми на папку с файлами чтобы посмотреть какие задания тебе нужно выполнить",
        "Тебе нужно получить исходные данные, чтобы это сделать необходимо пройти мини-игру. Жми на задание и пройди её",
        "Передвигай элементы так, чтобы они становились в ряд по 3 или больше штуки. После этого они уничтожаются. Если под элементами был фрагмент исходных данных, то ты его получишь. Собери все фрагменты!",
    };

    public static List<List<string>> allTexts = new List<List<string>>() { level_0, level_1 };
}
