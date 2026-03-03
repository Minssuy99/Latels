using UnityEngine;
using DG.Tweening;

public class LobbyCameraController : MonoBehaviour
{
    [SerializeField] private Transform defaultPos;
    [SerializeField] private Transform panelPos;

    private Camera cam;
    private float defaultFOV;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void SetDefaultView()
    {
        cam.transform.position = defaultPos.position;
        float aspect = (float)Screen.width / Screen.height;
        cam.fieldOfView = Mathf.Lerp(35, 27, Mathf.InverseLerp(1.33f, 2.17f, aspect));
        defaultFOV = cam.fieldOfView;
    }

    public void MoveToPanelView()
    {
        cam.DOKill();
        cam.transform.DOKill();
        cam.transform.DOMove(panelPos.position, 0.25f);
        cam.transform.DORotate(panelPos.rotation.eulerAngles, 0.25f);
        cam.DOFieldOfView(defaultFOV + 3f, 0.25f);
    }

    public void MoveToDefaultView()
    {
        cam.DOKill();
        cam.transform.DOKill();
        cam.transform.DOMove(defaultPos.position, 0.25f);
        cam.transform.DORotate(defaultPos.rotation.eulerAngles, 0.25f);
        cam.DOFieldOfView(defaultFOV, 0.25f);
    }
}