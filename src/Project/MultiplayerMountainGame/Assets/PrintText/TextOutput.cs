using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextOutput : MonoBehaviour
{
    public AudioSource source;
    public AudioClip tapSound;
    public Text textDisplay; // ������ �� ��������� ������ � ����� ����������
    public float letterDelay = 0.1f; // �������� ����� ���������
    public float wordDelay = 0.5f; // �������� ����� ������� �����

    private string[] texts = {
        /* 0 */ "�����.. �����... ������ �������� �����!!!", 
        /* 1 */ "�����. ����� �� �����!",
        /* 2 */ "�����. �������� ������.",
        /* 3 */ "������. ������� ������������� � ��������� � ��������� ���������� �����.",
        /* 4 */ "������. ������ � ���� �� �������.",
        /* 5 */ "�����. ��� �����, ������� ���������� ������.",
        /* 6 */ "�����. ����� �����!",
        /* 7 */ "�����.. �����. ���� �������!",
        /* 8 */ "������. ��������� ��������!",
        /* 9 */ "�����. ��������� � ���������� ������! (��������� ������ ����)",
        /* 10 */ "�����. �����, ������ ������� ���������",
        /* 11 */ "������. ������� ������! ��������� ������������� ��� ����� �������!!!",
        /* 12 */ "�����. ��� �����! ����� �����!",
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

    private void ClearText() {
        // �������� ������� ����� � ������ �������
        currentText = "";
        currentLetterIndex = 0;
        textDisplay.text = currentText;
    }
}
