using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cutscene : MonoBehaviour {

    public int id;
    public Transform canvas;
    public Transform CamStart;
    public Transform CamEnd;
    public bool PlayOnPlayerEnter;
    public string[] subtitles;

    bool played = false;
    Camera camera;
    Text subtitle;
    float timeToTween = 0;
    float timeToRead = 40;
    bool endOfcutscene = false;

    // Use this for initialization
    void Start() {
        camera = GetComponentInChildren<Camera>();
        subtitle = GetComponentInChildren<Text>();
        subtitle.text = "test";

        camera.transform.position = CamStart.position;
        camera.transform.rotation = CamStart.rotation;

        foreach (string s in subtitles) {
            timeToTween += s.ToCharArray().Length / timeToRead;
        }
        timeToTween = 1 / timeToTween;

        CutsceneManager.Instance.cutscenes.Add(id, this);
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        if (endOfcutscene)
            CutsceneManager.Instance.EndCutscene();
        else {
            ReOrient();
            updateText();
        }
    }

    public void StartScene() {
        camera.transform.position = CamStart.position;
        camera.transform.rotation = CamStart.rotation;
        endOfcutscene = false;
        txtIndex = -1;
    }

    void ReOrient() {
        camera.transform.position = Vector3.Lerp(camera.transform.position, CamEnd.transform.position, Time.deltaTime * timeToTween);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, CamEnd.transform.rotation, Time.deltaTime * timeToTween);
    }



    int txtIndex = -1;
    float txtTime = 0;
    float txtTimer = 1;

    void updateText() {
        if (txtTime < txtTimer) {
            txtIndex++;

            if (txtIndex >= subtitles.Length) {
                endOfcutscene = true;
                played = true;
            }
            else {

                txtTime = subtitles[txtIndex].ToCharArray().Length / timeToRead;
                txtTimer = 0;
                subtitle.text = subtitles[txtIndex];
            }
        }
        txtTimer += Time.deltaTime;

    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.tag);
        if (other.tag == "Player" && PlayOnPlayerEnter && !played) {
            CutsceneManager.Instance.TriggerCutscene(id);
        }
    }
}
