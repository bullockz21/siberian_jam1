using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextOutput : MonoBehaviour
{
    public GameObject questPanel;
    public Text questText;
    public AudioSource source;
    public AudioClip tapSound;
    public Text textDisplay; // Ссылка на компонент текста в вашем интерфейсе
    public float letterDelay = 0.1f; // Задержка между символами
    public float wordDelay = 0.5f; // Задержка после каждого слова

    private string[] texts = {
        /* 0 */ "Прием.. прием... Летите по компасу к цели и заберите груз!!!", 
        /* 1 */ "Цель найдено!",
        /* 2 */ "Задача успешно выполнена!",
        /* 3 */ "Получено новое задание!",
        /* 4 */ "Доставьте груз остров рассвета.",
        /* 5 */ "Груз доставлен, Задача успешно выполнена.",
        /* 6 */ "Конец связи!",
    };

    private int currentLetterIndex = 0;
    private string currentText = "";

    private void Start()
    {
        StartCoroutine(PrintStartTexts());
    }

    public IEnumerator PrintQuest1Texts()
    {
        questPanel.SetActive(false);
        yield return StartCoroutine(DisplayTextCoroutine(texts[1]));

        // Message end
        yield return new WaitForSeconds(1.0f);
        ClearText();
        QuestText(texts[1]);
    }

    public IEnumerator PrintQuest2Texts()
    {
        questPanel.SetActive(false);
        yield return StartCoroutine(DisplayTextCoroutine(texts[2]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[3]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[4]));

        // Message end
        yield return new WaitForSeconds(1.0f);
        ClearText();
        QuestText(texts[4]);
    }

    public IEnumerator PrintQuest3Texts()
    {
        questPanel.SetActive(false);
        yield return StartCoroutine(DisplayTextCoroutine(texts[5]));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[6]));

        // Message end
        yield return new WaitForSeconds(1.0f);
        ClearText();
        QuestText(texts[6]);
    }

    private IEnumerator PrintStartTexts()
    {
        questPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(DisplayTextCoroutine(texts[0]));

        // Message end
        yield return new WaitForSeconds(1.0f);
        ClearText();
        QuestText(texts[0]);
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

    private void QuestText(string _text) {
        questText.text = _text;
    }

    private void ClearText() {
        // Сбросить текущий текст и индекс символа
        currentText = "";
        currentLetterIndex = 0;
        textDisplay.text = currentText;
    }
}
