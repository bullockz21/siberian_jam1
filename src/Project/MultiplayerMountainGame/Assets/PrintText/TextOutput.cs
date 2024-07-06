using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextOutput : MonoBehaviour
{
    public AudioSource source;
    public AudioClip tapSound;
    public Text textDisplay; // Ссылка на компонент текста в вашем интерфейсе
    public float letterDelay = 0.1f; // Задержка между символами
    public float wordDelay = 0.5f; // Задержка после каждого слова

    private string[] texts = {
        /* 0 */ "Прием.. прием... Воздух вызывает Землю!!!", 
        /* 1 */ "Прием. Земля на связи!",
        /* 2 */ "Земля. Уточните задачу.",
        /* 3 */ "Воздух. Отыщите пострадавшего и доставьте в ближайший населенный пункт.",
        /* 4 */ "Воздух. Летите к цели на компасе.",
        /* 5 */ "Земля. Вас понял, начинаю выполнение задачи.",
        /* 6 */ "Земля. Конец связи!",
        /* 7 */ "Прием.. Земля. Цель найдено!",
        /* 8 */ "Воздух. Начинайте спасение!",
        /* 9 */ "Земля. Приступаю к выполнению задачи! (Пролететь сквозь цель)",
        /* 10 */ "Прием. Земля, задача успешно выполнена",
        /* 11 */ "Воздух. Хорошая работа! Доставьте пострадавшего как можно быстрее!!!",
        /* 12 */ "Земля. Вас понял! Конец связи!",
    };

    private int currentLetterIndex = 0;
    private string currentText = "";

    private void Start()
    {
        StartCoroutine(PrintStartTexts());
    }

    public IEnumerator PrintQuest1Texts()
    {
        yield return StartCoroutine(DisplayTextCoroutine(texts[7]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[8]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[9]));

        // Message end
        yield return new WaitForSeconds(2.0f);
        ClearText();
    }

    public IEnumerator PrintQuest2Texts()
    {
        yield return StartCoroutine(DisplayTextCoroutine(texts[10]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[11]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[12]));

        // Message end
        yield return new WaitForSeconds(2.0f);
        ClearText();
    }

    private IEnumerator PrintStartTexts()
    {
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[0]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[1]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[2]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[3]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[4]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[5]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[6]));

        // Message end
        yield return new WaitForSeconds(2.0f);
        ClearText();
    }

    private IEnumerator DisplayTextCoroutine(string textToDisplay)
    {
        ClearText();
        // Пока не все символы текста отображены
        while (currentLetterIndex < textToDisplay.Length)
        {
            // Добавить текущий символ к текущему тексту для отображения
            currentText += textToDisplay[currentLetterIndex];
            // Обновить отображаемый текст
            textDisplay.text = currentText;
            source.PlayOneShot(tapSound);
            // Увеличить индекс текущего символа
            currentLetterIndex++;
            // Подождать заданное время перед отображением следующего символа
            yield return new WaitForSeconds(letterDelay);

            // Если текущий символ является пробелом или концом слова
            if (textToDisplay[currentLetterIndex - 1] == ' ' || textToDisplay[currentLetterIndex - 1] == '\n')
            {
                // Подождать немного после полного слова
                yield return new WaitForSeconds(wordDelay);
            }
        }
    }

    private void ClearText() {
        // Сбросить текущий текст и индекс символа
        currentText = "";
        currentLetterIndex = 0;
        textDisplay.text = currentText;
    }
}
