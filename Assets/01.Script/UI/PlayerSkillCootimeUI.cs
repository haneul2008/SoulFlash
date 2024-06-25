using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SkillType
{
    AirDash,
    Rolls,
    HeavyAttack,
    AirAttack,
    Block
}
public class PlayerSkillCootimeUI : MonoBehaviour
{
    public NotifyValue<bool> OnSwitchSkillGuide = new NotifyValue<bool>();

    [SerializeField] private TMP_Text _cooltimeText;
    [SerializeField] private SkillType _skilltype;
    private Image _image;
    private GameObject _guideText;

    private AnimationPlayer _animationPlayer;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = Color.white;
        _guideText = transform.Find("KeyBinding").gameObject;

        OnSwitchSkillGuide.OnValueChanged += SwitchSkillGuideText;
    }
    private void Start()
    {
        SwitchSkillGuideText(true, true);

        switch (_skilltype)
        {
            case SkillType.AirDash:
                PlayerAirDash airdash = GameManager.instance.Player.gameObject.GetComponent<PlayerAirDash>();
                airdash.OnEndAitDashAction += StartText;

                _animationPlayer = airdash;
                break;

            case SkillType.Block:
                PlayerBlock block = GameManager.instance.Player.gameObject.GetComponent<PlayerBlock>();
                block.OnEndBlockAction += StartText;

                _animationPlayer = block;
                break;

            case SkillType.AirAttack:
                PlayerAirAttack airAttack = GameManager.instance.Player.gameObject.GetComponent<PlayerAirAttack>();
                airAttack.OnEndAirAttackAction += StartText;

                _animationPlayer = airAttack;
                break;

            case SkillType.HeavyAttack:
                PlayerHeavyAttack heavyAttack = GameManager.instance.Player.gameObject.GetComponent<PlayerHeavyAttack>();
                heavyAttack.OnEndHeavyAttackAction += StartText;

                _animationPlayer = heavyAttack;
                break;

            case SkillType.Rolls:
                PlayerRolls rolls = GameManager.instance.Player.gameObject.GetComponent<PlayerRolls>();
                rolls.OnEndRolls += StartText;

                _animationPlayer = rolls;
                break;
        }
    }
    private void OnDisable()
    {
        OnSwitchSkillGuide.OnValueChanged -= SwitchSkillGuideText;

        switch (_skilltype)
        {
            case SkillType.AirDash:
                PlayerAirDash airdash = _animationPlayer as PlayerAirDash;
                airdash.OnEndAitDashAction -= StartText;
                break;

            case SkillType.Block:
                PlayerBlock block = _animationPlayer as PlayerBlock;
                block.OnEndBlockAction -= StartText;
                break;

            case SkillType.AirAttack:
                PlayerAirAttack airAttack = _animationPlayer as PlayerAirAttack;
                airAttack.OnEndAirAttackAction -= StartText;
                break;

            case SkillType.HeavyAttack:
                PlayerHeavyAttack heavyAttack = _animationPlayer as PlayerHeavyAttack;
                heavyAttack.OnEndHeavyAttackAction -= StartText;
                break;

            case SkillType.Rolls:
                PlayerRolls rolls = _animationPlayer as PlayerRolls;
                rolls.OnEndRolls -= StartText;
                break;
        }
    }
    private void Update()
    {
        OnSwitchSkillGuide.Value = GameManager.instance.SkillGuideText;
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
    private void SwitchSkillGuideText(bool prev, bool next)
    {
        _guideText.SetActive(GameManager.instance.SkillGuideText);
    }
}
