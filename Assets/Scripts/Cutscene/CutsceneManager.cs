using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour {
    public static CutsceneManager Instance;

    Image fadeImg;
    public Dictionary<int, Cutscene> cutscenes = new Dictionary<int, Cutscene>();

    public bool IsActive = false;

    bool fadeToCutscene = false;
    bool fadeFromCutscene = false;

    // Use this for initialization
    void Awake() {
        Instance = this;
        fadeImg = GetComponentInChildren<Image>();

        /*if (fadeToCutscene)
            FadeToCutscene();*/

    }


    // Update is called once per frame
    void Update() {
       if(Input.GetKeyDown(KeyCode.Space))
            TriggerCutscene(0);

        if (fadeToCutscene)
            FadeToCutscene();

        if (fadeFromCutscene)
            FadeFromCutscene();

    }

    public void TriggerCutscene(int id) {      
        transIn = true;
        fadeToCutscene = true;
        IsActive = true;
        _id = id;
    }

    public void EndCutscene() {
        transIn = true;
        fadeFromCutscene = true;
        fadeToCutscene = false;
    }


    Color textureColor;
    bool transIn = false;
    bool transOut = false;
    int _id;

    void FadeToCutscene() {
        if (transIn) {
            if (textureColor.a > 1) {
                transIn = false;
                transOut = true;
                cutscenes[_id].canvas.gameObject.SetActive(true);
                cutscenes[_id].StartScene();
            }
            else {
                textureColor = fadeImg.color;
                textureColor.a = Mathf.Lerp(textureColor.a, 1.2f, Time.deltaTime * 5);
                fadeImg.color = textureColor;
            }
        }

        if (transOut) {
            if (textureColor.a < 0) {
                transOut = false;
                fadeToCutscene = false;
            }
            else {
                textureColor = fadeImg.color;
                textureColor.a = Mathf.Lerp(textureColor.a, -0.2f, Time.deltaTime * 5);
                fadeImg.color = textureColor;
            }
        }
    }

    void FadeFromCutscene() {
        if (transIn) {
            if (textureColor.a > 1) {
                transIn = false;
                transOut = true;
                cutscenes[_id].canvas.gameObject.SetActive(false);
            }
            else {
                textureColor = fadeImg.color;
                textureColor.a = Mathf.Lerp(textureColor.a, 1.2f, Time.deltaTime * 5);
                fadeImg.color = textureColor;
            }
        }

        if (transOut) {
            if (textureColor.a <= 0) {
                transOut = false;
                fadeFromCutscene = false;
                IsActive = false;
            }
            else {
                textureColor = fadeImg.color;
                textureColor.a = Mathf.Lerp(textureColor.a, -1.2f, Time.deltaTime * 5);
                fadeImg.color = textureColor;
            }
        }
    }
}
