using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using Pooling;
using UnityEngine;

public enum Vfx
{
    PlayerTakeDamage,
    ZombieTakeDamage,
    ZombieSpawn,
    ZombieDespawn,
    BulletDestroy,
    WalkDust,
}

public enum Sfx
{
    Pickup,
    ZombieDeath,
    GunShot,
}

public class FxManager : Singleton<FxManager>
{
    [SerializeField] private PoolAfterSeconds playerTakeDamage;
    [SerializeField] private PoolAfterSeconds zombieTakeDamage;
    [SerializeField] private PoolAfterSeconds zombieSpawn;
    [SerializeField] private PoolAfterSeconds zombieDespawn;
    [SerializeField] private PoolAfterSeconds bulletDestroy;
    [SerializeField] private PoolAfterSeconds walkDust;

    [SerializeField] private AudioSource pickup;
    [SerializeField] private AudioSource gunshot;
    [SerializeField] private AudioSource zombieDeath;

    [SerializeField] private AudioClip[] gunshots;

    private Dictionary<Vfx, PoolAfterSeconds> _listedVfx;
    private Dictionary<Sfx, AudioSource> _listedSfx;

    protected override void SingletonAwake()
    {
        DontDestroyOnLoad(gameObject);

        _listedVfx = new Dictionary<Vfx, PoolAfterSeconds>
        {
            { Vfx.PlayerTakeDamage, playerTakeDamage },
            { Vfx.ZombieTakeDamage, zombieTakeDamage },
            { Vfx.ZombieSpawn, zombieSpawn },
            { Vfx.ZombieDespawn, zombieDespawn },
            { Vfx.BulletDestroy, bulletDestroy },
            { Vfx.WalkDust, walkDust },
        };

        _listedSfx = new Dictionary<Sfx, AudioSource>
        {
            { Sfx.Pickup, pickup },
            { Sfx.GunShot, gunshot },
            { Sfx.ZombieDeath, zombieDeath },
        };
    }

    public void PlayVfx(Vfx fx, Vector3 position, float scale = 1f)
    {
        var vfx = _listedVfx[fx].Get<PoolAfterSeconds>(position, Quaternion.identity);
        vfx.transform.localScale = Vector3.one * scale;
    }

    public void PlaySfx(Sfx fx)
    {
        var sfx = _listedSfx[fx];
        sfx.Stop();
        sfx.Play();
    }

    public void PlayGunShot()
    {
        int index = Random.Range(0, gunshots.Length);
        _listedSfx[Sfx.GunShot].PlayOneShot(gunshots[index]);
    }
}