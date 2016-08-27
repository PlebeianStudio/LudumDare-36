using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Dialog : MonoBehaviour {


    public RectTransform dialogPanel;
    public Text nameField;
    public Text textField;
    public Image imageField;
    public bool PlayOnPlayerEnter;
    public bool Oneshoot = true;
    public dialogBox[] dialog;

    int dialogIndex = 0;
    bool nextDialog;
    string textToShow;

    bool played = false;
    bool inScene = false;
    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonUp(0))
            nextDialog = true;

        textField.text = textToShow;
    }

    IEnumerator ShowText() {
        bool transsition = false;
        CutsceneManager.Instance.IsActive = true;
        dialogPanel.gameObject.SetActive(true);

        while (dialogIndex < dialog.Length) {
            char[] txt = dialog[dialogIndex].text.ToCharArray();
            
            if (nextDialog) {
                nextDialog = false;
                for (int i = 0; i < txt.Length; i++) {
                    nameField.text = dialog[dialogIndex].name;

                    if (nextDialog) {
                        textToShow = dialog[dialogIndex].text;
                        nextDialog = false;
                        transsition = true;
                        break;
                    }

                    textToShow += txt[i];
                    yield return new WaitForSeconds(0.05f);
                }
                transsition = true;
            }


            
            while (transsition) {
                if (nextDialog) {
                    dialogPanel.localPosition = Vector3.Lerp(dialogPanel.localPosition, new Vector3(dialogPanel.localPosition.x, -250), Time.deltaTime * 5);
                }

                if (dialogPanel.localPosition.y <= -249) {
                    if (dialogIndex == dialog.Length-1) {
                        dialogPanel.gameObject.SetActive(false);
                        dialogPanel.localPosition = new Vector3(dialogPanel.localPosition.x, 0);
                    }
                    nameField.text = "";
                    textToShow = "";
                    break;
                }

                yield return null;
            }
            while (transsition) {
                if (nextDialog) {
                    dialogPanel.localPosition = Vector3.Lerp(dialogPanel.localPosition, new Vector3(dialogPanel.localPosition.x, 0), Time.deltaTime * 5);
                }

                if (dialogPanel.localPosition.y >= -0.1) {
                    transsition = false;
                    nextDialog = true;
                    dialogIndex++;
                    break;
                }
                yield return null;
            }

            yield return null;
        }

        played = true;
        CutsceneManager.Instance.IsActive = false;
        dialogIndex = 0;
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.tag);
        if (other.tag == "Player" && PlayOnPlayerEnter && !played) {
            nextDialog = true;
            StartCoroutine(ShowText());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            if (!Oneshoot)
                played = false;
        }
    }
}

 [System.Serializable]
public struct dialogBox {
    public string name;
    public string text;
    public Sprite image1;
    public Sprite image2;
}
