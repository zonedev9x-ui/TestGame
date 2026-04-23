using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer hp;
    public SpriteRenderer visual;
    public float speedVisual = 0.3f;
    public Color32 colorTeamA;
    public Color32 colorTeamB;

    private BaseUnit unit;
    private float maxSizeBar;
    private const float VISUAL_HP_TIME_OUT_ANIMATE = 0.5f;
    private bool isHpBarActiveBeforePause;
    private bool flagAnimate;

    private void Update()
    {
        if (flagAnimate)
        {
            float visualHpValue = visual.size.x;
            float mainHpValue = hp.size.x;
            visualHpValue = Mathf.MoveTowards(visualHpValue, mainHpValue, Time.deltaTime * speedVisual);

            Vector2 v = visual.size;
            v.x = visualHpValue;
            visual.size = v;

            if (visualHpValue <= mainHpValue)
            {
                flagAnimate = false;
            }
        }
    }

    public void Init(BaseUnit unit)
    {
        this.unit = unit;
        maxSizeBar = hp.size.x;
        Deactive();
    }

    public void Reset()
    {
        hp.color = unit.isTeamA ? colorTeamA : colorTeamB;
        SetMainHp(1f);
        SetVisualHp(1f);
    }

    public void Pause()
    {
        isHpBarActiveBeforePause = gameObject.activeInHierarchy;
        gameObject.SetActive(false);
    }

    public void Resume()
    {
        if (unit.isDead == false && isHpBarActiveBeforePause)
        {
            gameObject.SetActive(true);
        }
    }

    public void Deactive()
    {
        SetMainHp(1f);
        SetVisualHp(1f);
        gameObject.SetActive(false);
    }

    public void UpdateHealthBar(float percent)
    {
        if (percent <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }

        SetMainHp(percent);

        float visualHpValue = visual.size.x;
        float mainHpValue = hp.size.x;
        float percentLost = Mathf.Clamp01((visualHpValue - mainHpValue) / maxSizeBar);
        if (percentLost >= 0.15f)
        {
            flagAnimate = true;
        }
        else
        {
            flagAnimate = false;
            Vector2 v = visual.size;
            v.x = hp.size.x;
            visual.size = v;
        }

        //gameObject.SetActive(DynamicValues.showHpBar);
    }

    private void SetMainHp(float percent)
    {
        float newSize = maxSizeBar * percent;
        Vector2 v = hp.size;
        v.x = newSize;
        hp.size = v;
    }

    private void SetVisualHp(float percent)
    {
        float newSize = maxSizeBar * percent;
        Vector2 v = visual.size;
        v.x = newSize;
        visual.size = v;
    }
}
