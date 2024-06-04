using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillCootimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _cooltimeText;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = Color.white;
    }
    public void StartText(int cooltime)
    {
        _image.color = new Color(165, 0, 0);
        StartCoroutine(WaitCooltimeCoroutine(cooltime));
    }
    private IEnumerator WaitCooltimeCoroutine(int cooltime)
    {
        int time = cooltime;
        for(int i = 0; i < cooltime; i++)
        {
            _cooltimeText.text = time.ToString();

            yield return new WaitForSeconds(1f);

            time -= 1;
        }
        _cooltimeText.text = "";
        _image.color = Color.white;
    }
}
