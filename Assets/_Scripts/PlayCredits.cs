using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class PlayCredits : MonoBehaviour
{
    public static bool creditsHaveEnded = false;
    public Credit[] credits;
    public TextMeshProUGUI text;
    public TextMeshProUGUI thankYou;
    public float speed;
    public float startingLineY,
        finishingLineY;

    private void Start()
    {
        thankYou.gameObject.SetActive(false);
        startingLineY = - Camera.main.scaledPixelHeight;
        text.GetComponent<RectTransform>().anchoredPosition += Vector2.up * startingLineY;
        text.text = "";
        foreach(Credit credit in credits)
        {
            text.text += "<font=\"FranchiseFilled SDF\">";
            text.text += credit.descriptor;
            text.text += "</font>\n\n";
            text.text += "<font=\"American Captain SDF\">";
            foreach (string mention in credit.mentions)
            {
                text.text += mention + "\n";
            }
            text.text += "</font>\n\n\n";
        }
        Debug.Log(text.text);
        finishingLineY = Camera.main.scaledPixelHeight * .5f + text.GetPreferredValues().y;
    }

    private void Update()
    {
        if (creditsHaveEnded)
        {
            thankYou.gameObject.SetActive(true);
        }
        else
        {
            if (text.GetComponent<RectTransform>().anchoredPosition.y < finishingLineY)
            {
                if (Input.touchCount > 0) text.GetComponent<RectTransform>().anchoredPosition += Vector2.up * 20F * speed;
                else text.GetComponent<RectTransform>().anchoredPosition += Vector2.up * speed;
            }
            else if (!creditsHaveEnded)
            {
                creditsHaveEnded = true;
            }
        }

    }
}
