using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using Pooling;
using UnityEngine;

public enum Vfx
{
    PlayerTakeDamage,
    ZombieTakeDamage,
    BulletDestroy,
    WalkDust,
}

public class VfxManager : Singleton<VfxManager>
{
    [SerializeField] private PoolAfterSeconds playerTakeDamage;
    [SerializeField] private PoolAfterSeconds zombieTakeDamage;
    [SerializeField] private PoolAfterSeconds bulletDestroy;
    [SerializeField] private PoolAfterSeconds walkDust;

    private Dictionary<Vfx, PoolAfterSeconds> _listedVfx;

    protected override void SingletonAwake()
    {
        DontDestroyOnLoad(gameObject);

        _listedVfx = new Dictionary<Vfx, PoolAfterSeconds>
        {
            { Vfx.PlayerTakeDamage, playerTakeDamage },
            { Vfx.ZombieTakeDamage, zombieTakeDamage },
            { Vfx.BulletDestroy, bulletDestroy },
            { Vfx.WalkDust, walkDust },
        };
    }

    public void PlayFx(Vfx fx, Vector3 position, float scale = 1f)
    {
        var vfx = _listedVfx[fx].Get<PoolAfterSeconds>(position, Quaternion.identity);
        vfx.transform.localScale = Vector3.one * scale;
    }
}