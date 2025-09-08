using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager_Shadow : MonoBehaviour
{
    public static GameManager_Shadow Instance;

    [Header("UI (TMP)")]
    public TMP_Text textTimer;
    public TMP_Text textHP;
    public TMP_Text textLevel;
    public GameObject panelEnd;
    public TMP_Text textEnd;
    public Button btnRetry, btnNext;


    [Header("Gameplay")]
    public List<ShadowTarget> statues = new List<ShadowTarget>();
    public HealthSystem health;
    public float startTime = 60f;     // مدة المرحلة
    public int level = 1;             
    public int wrongDamage = 1;

    [Header("Light")]
    public Light spot;                // InspectorLight
    public float cueScaleLevel1 = 1.0f;
    public float cueScaleLevel2 = 0.6f;

    [Header("Audio (Optional)")]
    public AudioSource music;
    public AudioSource sfx;
    public AudioClip sfxRight;
    public AudioClip sfxWrong;
    public AudioClip sfxWin;
    public AudioClip sfxLose;

    float t;
    bool over;
    int correctIndex = -1;

    public bool IsOver => over;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (statues.Count == 0) statues.AddRange(FindObjectsOfType<ShadowTarget>());

       
        if (health != null)
        {
            health.onChange += (cur, max) => textHP.text = $"HP: {cur}/{max}";
            health.onDeath += () => Lose("Out of HP!");
        }

       
        if (btnRetry) btnRetry.onClick.AddListener(() => { SetupLevel(level); RestartCore(); });
        if (btnNext) btnNext.onClick.AddListener(() => { level = Mathf.Min(level + 1, 2); SetupLevel(level); RestartCore(); });

        SetupLevel(level);
        RestartCore();
    }

    void Update()
    {
        if (over) return;

        t -= Time.deltaTime;
        if (t <= 0f)
        {
            t = 0f;
            Lose("Time’s up!");
        }
        UpdateTimerUI();
    }

    void RestartCore()
    {
        t = startTime;
        over = false;
        panelEnd.SetActive(false);
        if (health) health.ResetHP();
        UpdateTimerUI();
        UpdateLevelUI();
    }

    void UpdateTimerUI()
    {
        int tt = Mathf.CeilToInt(t);
        int m = tt / 60;
        int s = tt % 60;
        if (textTimer) textTimer.text = $"{m:00}:{s:00}";
    }

    void UpdateLevelUI()
    {
        if (textLevel) textLevel.text = $"Level {level}";
    }

    public void OnPick(ShadowTarget s)
    {
        if (over) return;

        bool isCorrect = (statues.IndexOf(s) == correctIndex);
        if (isCorrect)
        {
            if (sfx && sfxRight) sfx.PlayOneShot(sfxRight);
            Win();
        }
        else
        {
            if (sfx && sfxWrong) sfx.PlayOneShot(sfxWrong);
            if (health) health.Damage(wrongDamage);
        }
    }

    void SetupLevel(int L)
    {
        
        foreach (var st in statues)
        {
            st.isCorrect = false;
            st.EnableCue(false);
        }

        
        correctIndex = (statues.Count > 0) ? Random.Range(0, statues.Count) : -1;
        if (correctIndex >= 0)
        {
            statues[correctIndex].isCorrect = true;

           
            float cueScale = (L == 1) ? cueScaleLevel1 : cueScaleLevel2;
            statues[correctIndex].EnableCue(true);
        }

        
        if (spot)
        {
            if (L == 1) { spot.spotAngle = 34f; spot.transform.position = new Vector3(0f, 5f, 3f); }
            else { spot.spotAngle = 22f; spot.transform.position = new Vector3(1.2f, 6f, 2.0f); }
        }

        
        startTime = (L == 1) ? 60f : 45f;

       
        if (btnNext) btnNext.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quit pressed!");
        Application.Quit();         
    }


    void Win()
    {
        over = true;
        if (sfx && sfxWin) sfx.PlayOneShot(sfxWin);
        if (textEnd) textEnd.text = "You found the true statue!";
        if (panelEnd) panelEnd.SetActive(true);
        if (btnNext) btnNext.gameObject.SetActive(level < 2);
    }

    void Lose(string why)
    {
        over = true;
        if (sfx && sfxLose) sfx.PlayOneShot(sfxLose);
        if (textEnd) textEnd.text = $"Game Over - {why}";
        if (panelEnd) panelEnd.SetActive(true);
        if (btnNext) btnNext.gameObject.SetActive(false);
    }
}
