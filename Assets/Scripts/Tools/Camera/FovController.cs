using UnityEngine;
using DG.Tweening;

public static class FovController
{

    private const string FovTweenId = "FOV";



    public static void SetFov(float targetFov, float duration)
    {
        if (Camera.main == null) return;
        
        DOTween.Kill(FovTweenId);

        DOTween.To(
                () => Camera.main.fieldOfView,
                x => Camera.main.fieldOfView = x,
                targetFov,
                duration
            )
            .SetId(FovTweenId);
    }
}