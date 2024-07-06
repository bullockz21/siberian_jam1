using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextOutput : MonoBehaviour
{
    public GameObject questPanel;
    public Text questText;
    public AudioSource source;
    public AudioClip tapSound;
    public Text textDisplay; // ������ �� ��������� ������ � ����� ����������
    public float letterDelay = 0.1f; // �������� ����� ���������
    public float wordDelay = 0.5f; // �������� ����� ������� �����

    private string[] texts = {
        /* 0 */ "�����.. �����... ������ �� ������� � ���� � �������� ����!!!", 
        /* 1 */ "���� �������!",
        /* 2 */ "������ ������� ���������!",
        /* 3 */ "�������� ����� �������!",
        /* 4 */ "��������� ���� ������ ��������.",
        /* 5 */ "���� ���������, ������ ������� ���������.",
        /* 6 */ "����� �����!",
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
        // ���� �� ��� ������� ������ ����������
        while (currentLetterIndex < textToDisplay.Length)
        {
            // �������� ������� ������ � �������� ������ ��� �����������
            currentText += textToDisplay[currentLetterIndex];
            // �������� ������������ �����
            textDisplay.text = currentText;
            source.PlayOneShot(tapSound);
            // ��������� ������ �������� �������
            currentLetterIndex++;
            // ��������� �������� ����� ����� ������������ ���������� �������
            yield return new WaitForSeconds(letterDelay);

            // ���� ������� ������ �������� �������� ��� ������ �����
            if (textToDisplay[currentLetterIndex - 1] == ' ' || textToDisplay[currentLetterIndex - 1] == '\n')
            {
                // ��������� ������� ����� ������� �����
                yield return new WaitForSeconds(wordDelay);
            }
        }
    }

    private void QuestText(string _text) {
        questText.text = _text;
    }

    private void ClearText() {
        // �������� ������� ����� � ������ �������
        currentText = "";
        currentLetterIndex = 0;
        textDisplay.text = currentText;
    }
}
