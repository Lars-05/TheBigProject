using UnityEngine;
using DG.Tweening;

public static class FovController
{
    private static Camera targetCamera;
    private const string FovTweenId = "FOV";

    public static void Register(Camera cam)
    {
        targetCamera = cam;
    }

    public static void SetFov(float targetFov, float duration)
    {
        if (targetCamera == null) return;
        
        DOTween.Kill(FovTweenId);

        DOTween.To(
                () => targetCamera.fieldOfView,
                x => targetCamera.fieldOfView = x,
                targetFov,
                duration
            )
            .SetId(FovTweenId);
    }
}