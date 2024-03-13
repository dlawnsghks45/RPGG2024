using System.Collections;
using UnityEngine;
using UnityEngine.UI;

    public class MultiGage : MonoBehaviour
    {
      
        //�̱��游���.
        private static MultiGage _instance = null;
        public static MultiGage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(MultiGage)) as MultiGage;
                    if (_instance == null)
                    {
                        //Debug.Log("Player script Error");
                    }
                }
                return _instance;
            }
        }


        [Tooltip("������ Ÿ��")]
        public Image.FillMethod fillMethod = Image.FillMethod.Horizontal;
        [Tooltip("MultiGageColor�� �����ϰ� 0�� ���ϰ� �Ǿ��� �� �׸� ��")]
        public Color nonValueColor = Color.black;
        [Tooltip("������� �׷��� ��")]
        public Color[] multiGageColor = { Color.red, Color.yellow };
        [Tooltip("�� ���� ������ ��")]
        public float gageLineValue = 10;
        [Tooltip("�ǰ� �� �� �������� �پ��� �����ϴ� �ð�")]
        public float dimWaitTime = 0.1f;
        [Tooltip("�� �������� �پ��� ���� �� �� �پ�鶧���� �ɸ��� �ð�")]
        public float dimDeleteTime = 0.3f;
        [Tooltip("������ �ܻ� ����")]
        public bool dimEffectOn = true;


     [SerializeField]private GameObject uiCanvasObject;  //SetActive �뵵
     [SerializeField] private RectTransform gage1RectTransform = null;
     [SerializeField]  private RectTransform gage2RectTransform = null;
     [SerializeField]  private RectTransform gageDim1RectTransform = null;
     [SerializeField]  private RectTransform gageDim2RectTransform = null;
     [SerializeField]  private Canvas gage1Canvas;         //SortOrder ���� �뵵
     [SerializeField]  private Canvas gage2Canvas;
     [SerializeField]    private Canvas gageDim1Canvas;
     [SerializeField]  private Canvas gageDim2Canvas;
        [SerializeField]  private Image gage1Image;           //fillAmount ���� �뵵
        [SerializeField]   private Image gage2Image;
        [SerializeField]  private Image gageDim1Image;
        [SerializeField]  private Image gageDim2Image;
        private IEnumerator gageEffectCor;
        private IEnumerator gageDimEffectCor;
        private decimal targetGageValue;
        private decimal prevGageValue;
        private decimal dimGageValue;
        private int colorIndex = 0;

        private bool isInit = false;

        private void Awake()
        {
            InitProperty();
        }

        public void ObserveStart(decimal target)
        {
            targetGageValue = target;
            InitProperty();
            InitSetting();
            CalcGage();
            uiCanvasObject.SetActive(true);
            dimGageValue = prevGageValue = targetGageValue;

            if (gageEffectCor != null)
                StopCoroutine(gageEffectCor);
            gageEffectCor = ObserveCor();
            StartCoroutine(gageEffectCor);
        }

        public void ObserveEnd()
        {
            if (gageDimEffectCor != null)
                StopCoroutine(gageDimEffectCor);
            if (gageEffectCor != null)
                StopCoroutine(gageEffectCor);
            uiCanvasObject.SetActive(false);
        }

        IEnumerator ObserveCor()
        {
            while (true)
            {
                if (prevGageValue != targetGageValue)
                {
                    CalcGage();
                    if (dimEffectOn)
                    {
                        if (prevGageValue < targetGageValue)
                        {
                            dimGageValue = targetGageValue;
                            gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
                            gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
                            gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
                            gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
                            gageDim1Image.fillAmount = (float)targetGageValue % gageLineValue / gageLineValue;
                        }
                        if (gageDimEffectCor != null)
                            StopCoroutine(gageDimEffectCor);
                        gageDimEffectCor = GageEffectCor();
                        StartCoroutine(gageDimEffectCor);
                    }
                }
                prevGageValue = targetGageValue;
                yield return null;
            }
        }

        IEnumerator GageEffectCor()
        {
            yield return new WaitForSeconds(dimWaitTime);

            float timer = dimDeleteTime;
            int dimColorIndex;
            while (0 < timer)
            {
                dimGageValue = (decimal)Mathf.Lerp((float)targetGageValue, (float)dimGageValue, timer / dimDeleteTime);
                dimColorIndex = Mathf.FloorToInt((float)dimGageValue / gageLineValue);

                gageDim1Canvas.sortingOrder = 10001 + dimColorIndex * 2;
                gageDim2Canvas.sortingOrder = 10001 + dimColorIndex * 2 - 2;
                gageDim1Image.color = 0 <= dimColorIndex ? multiGageColor[dimColorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
                gageDim2Image.color = 1 <= dimColorIndex ? multiGageColor[(dimColorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
                gageDim1Image.fillAmount = (float)dimGageValue % gageLineValue / gageLineValue;

                yield return null;
                timer -= Time.deltaTime;
            }

            gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
            gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
            gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim1Image.fillAmount = (float)targetGageValue % gageLineValue / gageLineValue;
        }
        private void InitProperty()
        {
            if (isInit) return;


            isInit = true;
        }
        private void InitSetting()
        {
            gage1Canvas.overrideSorting = gage2Canvas.overrideSorting = gageDim1Canvas.overrideSorting = gageDim2Canvas.overrideSorting = true;
            gage1Image.fillMethod = gage2Image.fillMethod = gageDim1Image.fillMethod = gageDim2Image.fillMethod = fillMethod;

            gageDim1Canvas.sortingOrder = gageDim2Canvas.sortingOrder = 10000;
            gageDim1Image.color = gageDim2Image.color = nonValueColor;
            gageDim1Image.fillAmount = gageDim2Image.fillAmount = 0;
            gage2Image.fillAmount = gageDim2Image.fillAmount = 1;
        }

        private void CalcGage()
        {
            colorIndex = Mathf.FloorToInt((float)targetGageValue / gageLineValue);

            gage1Canvas.sortingOrder = 10002 + colorIndex * 2;
            gage2Canvas.sortingOrder = 10002 + colorIndex * 2 - 2;
            gage1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] : nonValueColor;
            gage2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] : nonValueColor;
            gage1Image.fillAmount = (float)targetGageValue % gageLineValue / gageLineValue;
        }

        private void CalcGageDim()
        {
            gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
            gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
            gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim1Image.fillAmount = (float)targetGageValue % gageLineValue / gageLineValue;
        }
    }
