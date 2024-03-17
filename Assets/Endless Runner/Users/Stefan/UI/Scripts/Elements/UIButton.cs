using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ButtonStyle
{
    Default,
    IconLeft,
    IconRight,
    IconOnly,
}

public enum ButtonTextAlignment
{
    Left,
    Center,
    Right,
}

public class UIButton : UISelectable
{
    [SerializeField]
    private TextMeshProUGUI m_textField;

    public TextMeshProUGUI TextField
    {
        get
        {
            if ( m_textField == null )
                m_textField = transform.GetComponent<TextMeshProUGUI> ( );
            return m_textField;
        }
        set
        {
            m_textField = value;
        }
    }

    [SerializeField]
    private Image m_background;

    public Image Background
    {
        get
        {
            if ( m_background == null )
                m_background = transform.GetComponentInChildren<Image> ( );
            return m_background;
        }
        set
        {
            m_background = value;
        }
    }

    [SerializeField]
    private ColorBlock m_colorBlock;

    [SerializeField]
    private Vector3 hoveredScale = Vector3.one;

    [SerializeField]
    private float scaleSmoothTime = 0.2f;

    public Vector3 defaultScale = Vector3.one;

    private Vector3 scaleVelocity;
    private float maskVelocity;

    [SerializeField]
    private Image mask;

    [SerializeField]
    private ButtonStyle m_theme;

    public ButtonStyle Theme
    {
        get
        {
            return m_theme;
        }
        set
        {
            if ( value == m_theme )
                return;
            m_theme = value;

            OnThemeSwitch ( );
        }
    }

    public ColorBlock Colors
    {
        get
        {
            return m_colorBlock;
        }
        set
        {
            m_colorBlock = value;
        }
    }

    public string Text
    {
        get
        {
            return TextField.text;
        }
        set
        {
            if ( value == TextField.text )
                return;

            TextField.text = value;

            OnTextUpdate ( );
        }
    }

    [SerializeField]
    private ButtonTextAlignment m_textAlignment = ButtonTextAlignment.Center;

    public ButtonTextAlignment TextAlignment
    {
        get
        {
            return m_textAlignment;
        }
        set
        {
            if ( value == m_textAlignment )
                return;
            m_textAlignment = value;

            OnTextAlignmentChanged ( );
        }
    }

    public UnityEvent OnClickEvent;

    protected override void Start ( )
    {
        base.Start ( );

        defaultScale = transform.localScale;
    }

    protected override void OnDisable ( )
    {
        base.OnEnable ( );
    }

    public override void OnClick ( )
    {
        OnClickEvent.Invoke ( );
    }

    protected override void Awake ( )
    {
        m_colorBlock.onColorChangedEditor += SetColor;
    }

    private void Update ( )
    {
        SetColor ( );

        Vector3 targetScale = Hovered ? hoveredScale : defaultScale;
        transform.localScale = Vector3.SmoothDamp (transform.localScale, targetScale, ref scaleVelocity, scaleSmoothTime);

        float targetMask = Hovered ? 1 : 0;

        mask.fillAmount = Mathf.SmoothDamp (mask.fillAmount, targetMask, ref maskVelocity, scaleSmoothTime);
    }

    private void SetColor ( )
    {
        SetColor (Colors.GetColor (this));
    }

    private void SetColor ( Color color )
    {
        if ( TextField )
            TextField.color = color;
        if ( Background )
            Background.color = color;
    }

    private void OnTextUpdate ( )
    {
        TextField.text = Text;
    }

    private void OnThemeSwitch ( )
    {
        switch ( Theme )
        {
            case ButtonStyle.Default:

                break;

            case ButtonStyle.IconLeft:

                break;

            case ButtonStyle.IconRight:

                break;

            case ButtonStyle.IconOnly:

                break;

            default:
                break;
        }

        static void SwitchSiblingIndex ( Transform shouldbehigh, Transform shouldbelow )
        {
            shouldbehigh.SetAsFirstSibling ( );
            shouldbelow.SetAsLastSibling ( );
        }
    }

    private void OnTextAlignmentChanged ( )
    {
        switch ( TextAlignment )
        {
            case ButtonTextAlignment.Left:
                TextField.alignment = TextAlignmentOptions.Left;
                break;

            case ButtonTextAlignment.Center:
                TextField.alignment = TextAlignmentOptions.Center;
                break;

            case ButtonTextAlignment.Right:
                TextField.alignment = TextAlignmentOptions.Right;
                break;

            default:
                break;
        }
    }

    public new void Reset ( )
    {
        Text = "Button";
        Background = null;
        TextField = null;
    }
}