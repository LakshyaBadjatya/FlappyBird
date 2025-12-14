using UnityEngine;
using System;

public class MainMenuButtons : MonoBehaviour
{
    [Header("Rate Us (Email)")]
    public string emailTo = "lakshyabadjatya@gmail.com";
    public string emailSubject = "Feedback for Flappy Bird";
    [TextArea]
    public string emailBody =
        "Hi,\n\nI want to share my feedback about your game:\n";

    [Header("GitHub")]
    public string githubUrl = "https://github.com/LakshyaBadjatya";

    // ⭐ RATE US BUTTON
    public void RateUsByEmail()
    {
        string mailUrl =
            "mailto:" + emailTo +
            "?subject=" + Uri.EscapeDataString(emailSubject) +
            "&body=" + Uri.EscapeDataString(emailBody);

        Application.OpenURL(mailUrl);
    }

    // 🐙 GITHUB BUTTON
    public void OpenGithub()
    {
        Application.OpenURL(githubUrl);
    }
}
