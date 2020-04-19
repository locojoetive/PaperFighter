using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class PlayCredits : MonoBehaviour
{
    public static bool creditsHaveEnded = false;
    public Credit[] credits;

    public GameObject creditPrefab;
    public List<GameObject> creditObjects;
    public RectTransform backgroundImage;

    public GameObject thankYou;
    public float speed;
    public float spaceBetweenCredits;
    public float startingLineY,
        finishingLineY;
    private Animator animator;
    public bool initialize = false;

    private void Start()
    {
        InitializeCredits();
    }

    private void Update()
    {
        if (initialize)
        {
            InitializeCredits();
            initialize = false;
        }

        if (!runInEditMode)
        {
            if (!creditsHaveEnded)
            {
                if (animator == null)
                {
                    PlayCreditsFallBack();
                } else
                {
                    PlayCreditsAnimation();
                }
            }
        }
    }

    private void InitializeCredits()
    {
        if (creditObjects.Count > 0)
        {
            foreach (GameObject go in creditObjects)
            {
                Destroy(go);
            }
            creditObjects = new List<GameObject>();
        }

        animator = GetComponent<Animator>();
        creditObjects = new List<GameObject>();

        startingLineY = -Camera.main.scaledPixelHeight;


        Vector2 previousPosition = Vector2.up * startingLineY;
        foreach (Credit credit in credits)
        {
            GameObject currentCredit = Instantiate(creditPrefab, transform);
            TextMeshProUGUI currentText = currentCredit.GetComponentInChildren<TextMeshProUGUI>();
            RectTransform currentImage = currentCredit.GetComponentInChildren<RectTransform>();

            currentText.text = "";
            currentText.text += "<font=\"FranchiseFilled SDF\">";
            currentText.text += credit.descriptor;
            currentText.text += "</font>\n\n";
            currentText.text += "<font=\"American Captain SDF\">";
            foreach (string mention in credit.mentions)
            {
                currentText.text += mention + "\n";
            }
            currentText.text += "</font>";
            
            currentImage.anchoredPosition = previousPosition;

            creditObjects.Add(currentCredit);
            previousPosition -= Vector2.up * (currentText.preferredHeight + spaceBetweenCredits);
        }

        finishingLineY = -previousPosition.y;
        
        thankYou.GetComponent<RectTransform>().anchoredPosition = previousPosition;
        backgroundImage.offsetMin = new Vector2(backgroundImage.offsetMin.x, previousPosition.y - speed);
    }

    private void PlayCreditsAnimation()
    {
        throw new NotImplementedException();
    }

    private void PlayCreditsFallBack()
    {
        if (GetComponent<RectTransform>().anchoredPosition.y < finishingLineY)
        {
            if (Input.touchCount > 0) GetComponent<RectTransform>().anchoredPosition += Vector2.up * 20F * speed;
            else GetComponent<RectTransform>().anchoredPosition += Vector2.up * speed;
        }
        else if (!creditsHaveEnded)
        {
            thankYou.GetComponent<RectTransform>().anchoredPosition = - Vector2.up * GetComponent<RectTransform>().anchoredPosition.y;
            creditsHaveEnded = true;
        }
    }
}
