using System;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private AnimationClip _playPauseAnimationClip;
    private Animator _animator;
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        EventBus.OnCassetteStart += PlayPauseAnimation;
        EventBus.OnCassetteStop += PlayPauseAnimation;
    }
    
    private void OnDisable()
    {
        EventBus.OnCassetteStart -= PlayPauseAnimation;
        EventBus.OnCassetteStop -= PlayPauseAnimation;
    }

    private void PlayPauseAnimation()
    {
        _animator.Play(_playPauseAnimationClip.name);
    }
}
