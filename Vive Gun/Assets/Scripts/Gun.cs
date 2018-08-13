using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewGun", menuName = "Weapon/Gun", order = 1)]
public class Gun : ScriptableObject{

    public enum rarity { Common, Rare, Epic, GodForged};
    public enum damageType { True, Fire, Ice, Solar, Void};


    public string Name;
    public GameObject gunPrefab;
    public rarity gunRarity;

    [Header("Physical Variables")]
    public Vector3 holdRotationOffset;
    public bool isAutomatic;
    public float verticalRecoil;
    public float horizontalRecoil;


    [Header("Gun Stats")]
    public float bulletDamage;
    public float bulletSpeed;
    public damageType bulletDamageType;
    public float fireRate;
    public int clipSize;



}
