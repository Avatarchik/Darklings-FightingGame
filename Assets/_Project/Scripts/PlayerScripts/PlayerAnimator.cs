using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAnimator : DemonicsAnimator
{
    [SerializeField] private PlayerCollisionBoxes _playerCollisionBoxes = default;
    [SerializeField] private Player _player = default;
    [SerializeField] private PlayerMovement _playerMovement = default;
    [SerializeField] private Audio _audio = default;
    private Shadow _shadow;
    private int _celPrevious = -1;
    public PlayerStatsSO PlayerStats { get { return _player.playerStats; } set { } }


    void Awake()
    {
        _shadow = GetComponent<Shadow>();
    }

    void Start()
    {
        _animation = _player.playerStats._animation;
    }

    protected override void CheckEvents()
    {
        if (_celPrevious != _cel)
        {
            // _celPrevious = _cel;
            // base.CheckEvents();
            // if (GetEvent().projectile)
            // {
            //     _player.CreateEffect(GetEvent().projectilePoint, true);
            // }
            // if (GetEvent().jump)
            // {
            //     _playerMovement.TravelDistance(new DemonicsVector2((DemonicsFloat)GetEvent().jumpDirection.x * transform.root.localScale.x, (DemonicsFloat)GetEvent().jumpDirection.y));
            // }
            // if (GetEvent().footstep)
            // {
            //     _audio.SoundGroup("Footsteps").PlayInRandom();
            // }
            // if (GetEvent().throwEnd)
            // {
            //     _audio.Sound("Impact6").Play();
            //     // CameraShake.Instance.Shake(_animation.GetGroup(_group).cameraShake);
            // }
            // if (GetEvent().throwArcanaEnd)
            // {
            //     _audio.Sound("Impact6").Play();
            //     //  CameraShake.Instance.Shake(_animation.GetGroup(_group).cameraShake);
            // }
            // _player.Parrying = GetEvent().parry;
            // _player.Invincible = GetEvent().invisibile;
        }
    }
    public bool GetProjectile(string name, int frame)
    {
        return GetEvent(name, frame, out _).projectile;
    }
    public bool GetParrying(string name, int frame)
    {
        return GetEvent(name, frame, out _).parry;
    }
    public bool GetThrowArcanaEnd(string name, int frame)
    {
        return GetEvent(name, frame, out _).throwArcanaEnd;
    }
    public bool GetInvincible(string name, int frame)
    {
        return GetEvent(name, frame, out _).invisibile;
    }
    public bool GetFootstep(string name, int frame, out int cel)
    {
        return GetEvent(name, frame, out cel).footstep;
    }
    public DemonicsVector2 GetJump(string name, int frame)
    {
        DemonicsVector2 jumpDirection = DemonicsVector2.Zero;
        if (GetEvent(name, frame, out _).jump)
        {
            jumpDirection = new DemonicsVector2((DemonicsFloat)GetEvent(name, frame, out _).jumpDirection.x * transform.root.localScale.x, (DemonicsFloat)GetEvent(name, frame, out _).jumpDirection.y);
        }
        return jumpDirection;
    }
    public DemonicsVector2 GetGrabPoint(string name, int frame)
    {
        Vector2 grabPoint = GetEvent(name, frame, out _).grabPoint;
        return new DemonicsVector2((DemonicsFloat)grabPoint.x, (DemonicsFloat)grabPoint.y);
    }

    public override void SetAnimation(string name, int frame)
    {
        if (name == "Wallsplat")
        {
            transform.localPosition = new Vector2(10 * -transform.localScale.x, 0);
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            transform.localPosition = Vector2.zero;
            transform.localRotation = Quaternion.identity;
        }
        base.SetAnimation(name, frame);
    }

    protected override void CheckAnimationBoxes()
    {
        base.CheckAnimationBoxes();
        //_playerCollisionBoxes.SetHurtboxes(GetHurtboxes());
        //_playerCollisionBoxes.SetHitboxes(GetHitboxes());
    }

    public void SetInvinsible(bool state)
    {
        _spriteRenderer.enabled = !state;
        _shadow.SetInvinsible(state);
    }

    public bool InRecovery(string name, int frame)
    {
        _group = _animation.GetGroupId(name);
        _cel = GetCellByFrame(frame);
        for (int i = 0; i < _animation.animationCelsGroup[_group].animationCel.Count; i++)
        {
            if (i < _cel)
            {
                if (_animation.animationCelsGroup[_group].animationCel[i].hitboxes.Count > 0)
                {
                    return true;
                }
            }
        }
        return false;

        if (_animation.GetCel(_group, _cel).hitboxes.Count == 0)
        {
            return true;
        }
        return false;
    }

    public bool InActive()
    {
        // if (GetHitboxes().Length > 0)
        // {
        //     return true;
        // }
        return false;
    }

    public AttackSO GetFramedata(AttackSO attack)
    {
        int startUpFrames = 0;
        int activeFrames = 0;
        int recoveryFrames = 0;
        for (int i = 0; i < _animation.animationCelsGroup[_group].animationCel.Count; i++)
        {
            if (_animation.animationCelsGroup[_group].animationCel[i].hitboxes?.Count > 0)
            {
                activeFrames += _animation.animationCelsGroup[_group].animationCel[i].frames;
            }
            else
            {
                bool isPriorFrameActive = false;
                for (int j = 0; j < _animation.animationCelsGroup[_group].animationCel.Count; j++)
                {
                    if (_animation.animationCelsGroup[_group].animationCel[j].hitboxes?.Count > 0 && j < i)
                    {
                        isPriorFrameActive = true;
                    }
                }
                if (!isPriorFrameActive)
                {
                    startUpFrames += _animation.animationCelsGroup[_group].animationCel[i].frames;
                }
                else
                {
                    recoveryFrames += _animation.animationCelsGroup[_group].animationCel[i].frames;
                }
            }
        }
        attack.startUpFrames = startUpFrames;
        attack.activeFrames = activeFrames;
        attack.recoveryFrames = recoveryFrames;
        return attack;
    }

    public void ResetPosition()
    {
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;
    }

    public Sprite GetCurrentSprite()
    {
        return _spriteRenderer.sprite;
    }

    public void SpriteSuperArmorEffect()
    {
        _spriteRenderer.color = Color.red;
    }

    public void SpriteNormalEffect()
    {
        _spriteRenderer.color = Color.white;
    }

    public int SetSpriteLibraryAsset(int skinNumber)
    {
        _animation = _player.playerStats._animation;
        if (skinNumber > _animation.spriteAtlas.Length - 1)
        {
            _skin = 0;
        }
        else if (skinNumber < 0)
        {
            skinNumber = _animation.spriteAtlas.Length - 1;
        }
        _skin = skinNumber;
        return skinNumber;
    }

    public void SetSpriteOrder(int index)
    {
        _spriteRenderer.sortingOrder = index;
    }
}
